using Imagegram.Application.Comments.Commands.CreateComment;
using Imagegram.Application.Comments.Queries.GetCommentsFromPost;
using Imagegram.Application.Common.Interfaces;
using Imagegram.Application.Posts.Commands.CreatePost;
using Imagegram.Application.Posts.Queries.GetAllPosts;
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
        /// <returns>Generated Post Id</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> Post([FromForm] CreatePostCommand createPostCommand /*,[FromHeader(Name = "X-Account-Id")] string accountId*/)
        {
            long postId = await Mediator.Send(createPostCommand);
            return postId > 0 ? Created("", postId) : (ActionResult)BadRequest();
        }

        /// <summary>
        /// Create a comment on a post
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <param name="content"></param>
        /// <returns>Generated Commend Id</returns>
        [Route("{id}/Comments")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> CommentOnPost(long id, string content /*,[FromHeader(Name = "X-Account-Id")] string accountId*/)
        {
            long postId = await Mediator.Send(new CreateCommentCommand()
            {
                PostId = id,
                Content = content,
            });
            return postId > 0 ? Created("", postId) : (ActionResult)BadRequest();
        }

        /// <summary>
        /// Get a list of comments on a post
        /// </summary>
        /// <param name="getCommentsFromPostQuery"></param>
        /// <param name="id">Post Id</param>
        /// <returns></returns>
        [Route("{id}/Comments")]
        [Produces("application/json")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> GetCommentsOnPost([FromQuery] GetCommentsFromPostQuery getCommentsFromPostQuery, [FromRoute] long id)
        {
            var comments = await Mediator.Send(new RetrieveCommentsFromPostQuery()
            {
                PageNumber = getCommentsFromPostQuery.PageNumber,
                PageSize = getCommentsFromPostQuery.PageSize,
                PostId = id,
            });
            return Ok(comments);
        }

        /// <summary>
        /// Get all posts along with last 3 comments to each post
        /// </summary>
        /// <param name="getAllPostsQuery"></param>
        /// <returns></returns>
        [Produces("application/json")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<long>> GetPosts([FromQuery] GetAllPostsQuery getAllPostsQuery)
        {
            var posts = await Mediator.Send(getAllPostsQuery);
            return Ok(posts);
        }
    }
}
