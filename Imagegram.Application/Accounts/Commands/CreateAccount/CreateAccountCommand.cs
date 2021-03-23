using Imagegram.Application.Common.Interfaces;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Imagegram.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<CreateAccountVm>
    {
        [Required]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountVm>
    {
        private readonly IIdentityService _identityService;
        public CreateAccountCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        public async Task<CreateAccountVm> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            CreateAccountVm vm = new CreateAccountVm();
            bool emailExists = await _identityService.AccountExistsByEmailAsync(request.Email);
            if (emailExists)
            {
                vm.Errors = new List<string>() { "Account with this email already exists" };
                return vm;
            }
            var auth = await _identityService.CreateAccountAsync(request.Email, request.Password, request.Name);
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
