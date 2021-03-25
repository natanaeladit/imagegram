using Imagegram.Application.Comments.Commands.CreateComment;
using Imagegram.Tests.Posts.Commands;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Imagegram.Tests.Comments.Commands
{
    using static TestingFixture;
    public class CreateCommentTests
    {
        [Test]
        public async Task ShouldCreateComment()
        {
            (long, long) postIdAndCommentId = await CreateComment();
            Assert.IsTrue(postIdAndCommentId.Item2 > 0);
        }

        [Test]
        public async Task ShouldNotCreateComment()
        {
            var createCommentCmd = new CreateCommentCommand()
            {
                Content = "Test",
                PostId = 999999,
            };

            var commentId = await SendAsync(createCommentCmd);
            Assert.IsTrue(commentId == 0);
        }

        public async Task<(long, long)> CreateComment()
        {
            CreatePostTests postTest = new CreatePostTests();
            long postId = await postTest.CreatePost();
            Assert.IsTrue(postId > 0);

            var createCommentCmd = new CreateCommentCommand()
            {
                Content = "Test",
                PostId = postId,
            };

            var commentId = await SendAsync(createCommentCmd);
            return (postId, commentId);
        }
    }
}
