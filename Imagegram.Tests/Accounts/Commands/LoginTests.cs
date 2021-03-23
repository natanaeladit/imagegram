using Imagegram.Application.Accounts.Commands.CreateAccount;
using Imagegram.Application.Accounts.Commands.Login;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Imagegram.Tests.Accounts.Commands
{
    using static TestingFixture;
    public class LoginTests
    {
        [Test]
        public async Task ShouldLogin()
        {
            var createAccountCmd = new CreateAccountCommand()
            {
                Email = "demo@test.com",
                Name = "demo user",
                Password = "demo666&&&TEST",
            };
            var authResult = await SendAsync(createAccountCmd);

            var loginAuthResult = await SendAsync(new LoginCommand()
            {
                Email = createAccountCmd.Email,
                Password = createAccountCmd.Password,
            });

            Assert.IsNotNull(loginAuthResult);
            Assert.IsNotNull(loginAuthResult.Token);
            Assert.IsTrue(loginAuthResult.Result);
        }
    }
}
