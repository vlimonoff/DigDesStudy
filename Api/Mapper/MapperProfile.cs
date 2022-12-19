using Api.Mapper.MapperActions;
using Api.Models.Attach;
using Api.Models.Comment;
using Api.Models.Like;
using Api.Models.Post;
using Api.Models.User;
using AutoMapper;
using Common;
using DAL.Entities;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                ;

            CreateMap<User, UserModel>();
            CreateMap<User, UserAvatarModel>()
                .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
                .AfterMap<UserAvatarMapperAction>();
            CreateMap<Avatar, AttachModel>();
            CreateMap<Post, PostModel>()
                .ForMember(d => d.Contents, m => m.MapFrom(d => d.PostContents))
                .ForMember(d => d.Likes, m => m.MapFrom(d => d.Likes))
                .ForMember(d => d.Comments, m => m.MapFrom(d => d.PostComments))
                .ForMember(d => d.LikesCount, m => m.MapFrom(d => d.Likes!.Count))
                .ForMember(d => d.CommentsCount, m => m.MapFrom(d => d.PostComments!.Count));
            CreateMap<PostContent, AttachExternalModel>().AfterMap<PostContentMapperAction>();
            CreateMap<PostContent, AttachModel>();

            CreateMap<CreatePostRequest, CreatePostModel>();
            CreateMap<MetadataModel, MetadataLinkModel>();
            CreateMap<MetadataLinkModel, PostContent>();
            CreateMap<CreatePostModel, Post>()
                .ForMember(d => d.PostContents, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.Created, m => m.MapFrom(s => DateTime.UtcNow));

            CreateMap<CreateCommentRequest, CreateCommentModel>();
            CreateMap<CreateCommentModel, Comment>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<Comment, CommentModel>()
                .ForMember(d => d.Likes, m => m.MapFrom(d => d.Likes))
                .ForMember(d => d.LikesCount, m => m.MapFrom(d => d.Likes!.Count));

            CreateMap<CreateCommentReplyRequest, CreateCommentReplyModel>();
            CreateMap<CreateCommentReplyModel, CommentReply>()
                .ForMember(d => d.CreatedDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<CommentReply, CommentReplyModel>()
                .ForMember(d => d.Likes, m => m.MapFrom(d => d.Likes))
                .ForMember(d => d.LikesCount, m => m.MapFrom(d => d.Likes!.Count));

            CreateMap<CreateLikeRequest, CreateLikeModel>();
            CreateMap<CreateLikeModel, Like>()
                .ForMember(d => d.LikeDate, m => m.MapFrom(s => DateTimeOffset.UtcNow));
            CreateMap<Like, LikeModel>();
        }
    }
}
