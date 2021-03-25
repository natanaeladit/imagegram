using Imagegram.Application.Comments.Queries.GetCommentsFromPost;
using Imagegram.Tests.Comments.Commands;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Imagegram.Tests.Comments.Queries
{
    using static TestingFixture;
    public class GetCommentsFromPostTests
    {
        [Test]
        public async Task ShouldGetComment()
        {
            CreateCommentTests createCommentTests = new CreateCommentTests();
            (long, long) postIdAndCommentId = await createCommentTests.CreateComment();
            Assert.IsTrue(postIdAndCommentId.Item2 > 0);

            var getComments = new RetrieveCommentsFromPostQuery()
            {
                PostId = postIdAndCommentId.Item1,
                PageNumber = 1,
                PageSize = 20,
            };

            var comments = await SendAsync(getComments);
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Items.Count == 1);
            Assert.IsTrue(comments.Items[0].Id == postIdAndCommentId.Item2);
        }
    }
}
