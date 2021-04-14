using Imagegram.Application.Posts.Queries.GetAllPosts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagegram.Tests.Posts.Queries
{
    using static TestingFixture;
    public class GetAllPostsTests
    {
        [Test]
        public async Task ShouldReturnPosts()
        {
            var getAllPostsQuery = new GetAllPostsQuery()
            {
                PageNumber = 1,
                PageSize = 20,
            };

            var result = await SendAsync(getAllPostsQuery);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
        }
    }
}
