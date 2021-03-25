using Imagegram.Application.Accounts.Commands.CreateAccount;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Imagegram.Application.Accounts.Commands.Login;

namespace Imagegram.IntegrationTests
{
    [TestFixture]
    public class AccountControllerTests
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
        public async Task WhenUsersRegisterAccount_ThenTheResultIsCreated()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "test@demo.com",
                Name = "demo",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Register", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task WhenUsersRegisterAccountWithWeakPassword_ThenTheResultIsBadRequest()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "testweak@demo.com",
                Name = "demo",
                Password = "strong",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Register", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task WhenUsersLogin_ThenTheResultIsCreated()
        {
            CreateAccountCommand createAccountJSON = new CreateAccountCommand()
            {
                Email = "testlogin@demo.com",
                Name = "demo",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(createAccountJSON), Encoding.UTF8, "application/json");
            var createAccountResult = await _client.PostAsync("api/Account/Register", content);
            Assert.That(createAccountResult.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            LoginCommand loginJSON = new LoginCommand()
            {
                Email = createAccountJSON.Email,
                Password = createAccountJSON.Password,
            };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Login", loginContent);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task WhenUsersLoginWithInvalidCredentials_ThenTheResultIsBadRequest()
        {
            LoginCommand loginJSON = new LoginCommand()
            {
                Email = "unknown@demo.com",
                Password = "strong$$$555SSS",
            };
            var content = new StringContent(JsonSerializer.Serialize(loginJSON), Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/Account/Login", content);
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
