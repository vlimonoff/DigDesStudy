using Api.Exceptions;
using Api.Models.Comment;
using Api.Models.Post;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Api.Services
{
    public class CommentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CommentService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateComment(CreateCommentRequest request)
        {
            var model = _mapper.Map<CreateCommentModel>(request);
            var dbModel = _mapper.Map<Comment>(model);
            await _context.Comments.AddAsync(dbModel);
            await _context.SaveChangesAsync();
        }

        public async Task ReplyComment(CreateCommentReplyRequest request)
        {
            var model = _mapper.Map<CreateCommentReplyModel>(request);
            var dbModel = _mapper.Map<CommentReply>(model);
            await _context.CommentReplies.AddAsync(dbModel);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommentModel>> GetPostComments(Guid postId)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == postId);
            if (post == default || post == null)
            {
                throw new PostNotFoundException();
            }

            var comments = await _context.Comments
                .AsNoTracking()
                .Include(c => c.Author).ThenInclude(u => u.Avatar)
                .Include(x => x.Likes!).ThenInclude(x => x.Author).ThenInclude(x => x.Avatar)
                .Include(x => x.CommentReplies!).ThenInclude(x => x.Author)
                .ToListAsync();

            var commentModels = _mapper.Map<List<CommentModel>>(comments);
            return commentModels;
        }
    }
}
