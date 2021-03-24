using FluentValidation;
using Imagegram.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Application.Posts.Commands.CreatePost
{
    public class CreatePostValidator : AbstractValidator<CreatePostCommand>
    {
        private readonly IConfigurationService _configurationService;
        public CreatePostValidator(IConfigurationService configurationService)
        {
            _configurationService = configurationService;

            RuleFor(v => v.ImageFile)
                .NotNull().WithMessage("Image file is required."); ;

            RuleFor(v => v.ImageFile.FileName)
                .Must(BeImageType).WithMessage("Invalid image file format.")
                .When(v => v.ImageFile != null);

            RuleFor(v => v.Comments).NotEmpty();
        }

        public bool BeImageType(string filename)
        {
            string[] allowedExtensions = _configurationService.GetConfig("AllowedImageExtensions").Split(",");
            string fileExt = Path.GetExtension(filename);
            return allowedExtensions.Any(e => e == fileExt);
        }
    }
}
