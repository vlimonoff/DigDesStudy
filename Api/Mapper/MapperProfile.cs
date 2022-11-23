﻿using Api.Mapper.MapperActions;
using Api.Models.Attach;
using Api.Models.Post;
using Api.Models.User;
using AutoMapper;
using Common;

namespace Api.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<CreateUserModel, DAL.Entities.User>()
                .ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
                .ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)))
                .ForMember(d => d.BirthDate, m => m.MapFrom(s => s.BirthDate.UtcDateTime))
                ;

            CreateMap<DAL.Entities.User, UserModel>();
            CreateMap<DAL.Entities.User, UserAvatarModel>()
                .ForMember(d => d.PostsCount, m => m.MapFrom(s => s.Posts!.Count))
                .AfterMap<UserAvatarMapperAction>();
            CreateMap<DAL.Entities.Avatar, AttachModel>();
            CreateMap<DAL.Entities.Post, PostModel>()
                .ForMember(d => d.Contents, m => m.MapFrom(d => d.PostContents));
            CreateMap<DAL.Entities.PostContent, AttachExternalModel>().AfterMap<PostContentMapperAction>();
            CreateMap<DAL.Entities.PostContent, AttachModel>();

            CreateMap<CreatePostRequest, CreatePostModel>();
            CreateMap<MetadataModel, MetadataLinkModel>();
            CreateMap<MetadataLinkModel, DAL.Entities.PostContent>();
            CreateMap<CreatePostModel, DAL.Entities.Post>()
                .ForMember(d => d.PostContents, m => m.MapFrom(s => s.Contents))
                .ForMember(d => d.Created, m => m.MapFrom(s => DateTime.UtcNow));
        }
    }
}