using Api.Exceptions;
using Api.Models.Comment;
using Api.Models.Like;
using AutoMapper;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class LikeService
    {
        private readonly IMapper _mapper;
        private readonly DAL.DataContext _context;

        public LikeService(IMapper mapper, DAL.DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task AddLike(CreateLikeRequest request)
        {
            var model = _mapper.Map<CreateLikeModel>(request);
            var dbModel = _mapper.Map<Like>(model);
            await _context.Likes.AddAsync(dbModel);
            await _context.SaveChangesAsync();
        }

      
        public async Task<List<LikeModel>> GetLikes(Guid targetId)
        {
            var post = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == targetId);
            var comment = await _context.Comments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == targetId);
            var reply = await _context.CommentReplies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == targetId);
            
            if((post == null || post == default) & (comment == null || comment == default) & (reply == null || reply == default)) 
            {
                throw new TargetNotFoundException();
            }

            var likes = await _context.Likes
                .AsNoTracking()
                .Include(c => c.Author).ThenInclude(u => u.Avatar)
                .ToListAsync();

            var likesModels = _mapper.Map<List<LikeModel>>(likes);
            return likesModels;
        }
    }
}
