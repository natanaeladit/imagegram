using Imagegram.Application.Accounts.Commands.CreateAccount;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Imagegram.Tests.Accounts.Commands
{
    using static TestingFixture;
    public class CreateAccountTests
    {
        [Test]
        public async Task ShouldCreateNewAccount()
        {
            var authResult = await SendAsync(new CreateAccountCommand()
            {
                Email = "demo@test.com",
                Name = "demo user",
                Password = "demo666&&&TEST",
            });

            Assert.IsNotNull(authResult);
            Assert.IsNotNull(authResult.Token);
            Assert.IsTrue(authResult.Result);
        }
    }
}