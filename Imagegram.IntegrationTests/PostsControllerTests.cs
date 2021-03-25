using Imagegram.Application.Accounts.Commands.CreateAccount;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using Imagegram.Application.Common.Models;

namespace Imagegram.IntegrationTests
{
    [TestFixture]
    public class PostsControllerTests
    {
        private ApiApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void GivenARequestToTheController()
        {
            _factory = new ApiApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task WhenUsersPostAPost_ThenTheResultIsCreated()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "testpost@demo.com",
                Name = "demo",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Register", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            string responseBody = await result.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            AuthResult authResult = JsonSerializer.Deserialize<AuthResult>(responseBody, options);
            using (var formContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
                formContent.Add(new StreamContent(new MemoryStream(bytes)), "ImageFile", "image.jpg");
                formContent.Add(new StringContent("demo"), "Comments");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
                using (var postResult = await _client.PostAsync("api/Posts", formContent))
                {
                    string postResultResponseBody = await postResult.Content.ReadAsStringAsync();
                    Assert.That(postResult.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                    long postId = long.Parse(await postResult.Content.ReadAsStringAsync());
                    Assert.That(postId, Is.GreaterThan(0));
                }
            }
        }

        [Test]
        public async Task WhenUsersCommentOnAPost_ThenTheResultIsCreated()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "testcomment@demo.com",
                Name = "demo",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Register", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            string responseBody = await result.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            AuthResult authResult = JsonSerializer.Deserialize<AuthResult>(responseBody, options);

            long postId;
            using (var formContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
                formContent.Add(new StreamContent(new MemoryStream(bytes)), "ImageFile", "image.jpg");
                formContent.Add(new StringContent("demo"), "Comments");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
                using (var postResult = await _client.PostAsync("api/Posts", formContent))
                {
                    string postResultResponseBody = await postResult.Content.ReadAsStringAsync();
                    Assert.That(postResult.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                    postId = long.Parse(await postResult.Content.ReadAsStringAsync());
                    Assert.That(postId, Is.GreaterThan(0));
                }
            }

            string comments = "test";
            var commentResult = await _client.PostAsync($"api/Posts/{postId}/Comments?content={comments}", null);
            Assert.That(commentResult.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            long commentId = long.Parse(await commentResult.Content.ReadAsStringAsync());
            Assert.That(commentId, Is.GreaterThan(0));
        }

        [Test]
        public async Task WhenUsersGetCommentsOnAPost_ThenTheResultIsOk()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "getcomment@demo.com",
                Name = "demo",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Register", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            string responseBody = await result.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            AuthResult authResult = JsonSerializer.Deserialize<AuthResult>(responseBody, options);

            long postId;
            using (var formContent = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var bytes = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");
                formContent.Add(new StreamContent(new MemoryStream(bytes)), "ImageFile", "image.jpg");
                formContent.Add(new StringContent("demo"), "Comments");

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.Token);
                using (var postResult = await _client.PostAsync("api/Posts", formContent))
                {
                    string postResultResponseBody = await postResult.Content.ReadAsStringAsync();
                    Assert.That(postResult.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                    postId = long.Parse(await postResult.Content.ReadAsStringAsync());
                    Assert.That(postId, Is.GreaterThan(0));
                }
            }

            var commentsResult = await _client.GetAsync($"api/Posts/{postId}/Comments?PageNumber=1&PageSize=20");
            string commentsJSONString = await commentsResult.Content.ReadAsStringAsync();
            Assert.That(commentsResult.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(commentsJSONString, Is.Not.Null);
        }
    }
}
