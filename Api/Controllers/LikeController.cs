using Api.Models.Comment;
using Api.Models.Like;
using Api.Services;
using Common.Consts;
using Common.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost]
        [Authorize]
        public async Task AddLike(CreateLikeRequest request)
        {
            if (!request.AuthorId.HasValue)
            {
                var userId = User.GetClaimValue<Guid>(ClaimNames.Id);
                if (userId == default)
                    throw new Exception("not authorize");
                request.AuthorId = userId;
            }

            await _likeService.AddLike(request);
        }

        [HttpGet]
        public async Task<List<LikeModel>> GetLikes(Guid targetId)
        {
            return await _likeService.GetLikes(targetId);
        }
    }
}
