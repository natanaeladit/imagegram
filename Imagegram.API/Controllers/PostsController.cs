using Imagegram.Application.Common.Interfaces;
using Imagegram.Application.Posts.Commands.CreatePost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Imagegram.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ApiControllerBase
    {
        /// <summary>
        /// Create new post with an image
        /// </summary>
        /// <param name="createPostCommand"></param>
        /// <returns>Post Id</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Post([FromForm] CreatePostCommand createPostCommand /*,[FromHeader(Name = "X-Account-Id")] string accountId*/)
        {
            long postId = await Mediator.Send(createPostCommand);
            return postId > 0 ? Created("", postId) : (ActionResult)BadRequest();
        }
    }
}
