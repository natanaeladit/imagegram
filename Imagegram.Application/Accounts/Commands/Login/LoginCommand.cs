using Imagegram.Application.Common.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Application.Accounts.Commands.Login
{
    public class LoginCommand : IRequest<LoginVm>
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginVm>
    {
        private readonly IIdentityService _identityService;
        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<LoginVm> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            LoginVm vm = new LoginVm();
            var auth = await _identityService.LoginAsync(request.Email, request.Password);
            if (auth.Result)
            {
                vm.Result = true;
                vm.Token = auth.Token;
            }
            else
            {
                vm.Errors = auth.Errors;
            }
            return vm;
        }
    }
}
