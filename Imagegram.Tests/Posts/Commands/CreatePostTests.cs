using FluentAssertions;
using Imagegram.Application.Posts.Commands.CreatePost;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Tests.Posts.Commands
{
    using static TestingFixture;
    public class CreatePostTests
    {
        [Test]
        public async Task ShouldCreatePost()
        {
            long postId = await CreatePost();
            Assert.IsTrue(postId > 0);
        }

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreatePostCommand();

            FluentActions.Invoking(async () =>
                await SendAsync(command)).Should().Throw<Exception>();
        }

        public async Task<long> CreatePost()
        {
            var imageFileMock = new Mock<IFormFile>();
            var bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
            MemoryStream memoryStream = new MemoryStream(bytes);
            memoryStream.Position = 0;
            var fileName = "test.jpg";
            imageFileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            imageFileMock.Setup(f => f.FileName).Returns(fileName);
            imageFileMock.Setup(f => f.Length).Returns(memoryStream.Length);

            var createPostCmd = new CreatePostCommand()
            {
                Comments = "Test",
                ImageFile = imageFileMock.Object,
            };

            var result = await SendAsync(createPostCmd);
            return result;
        }
    }
}
