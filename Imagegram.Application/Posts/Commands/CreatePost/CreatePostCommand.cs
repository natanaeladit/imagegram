using Imagegram.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Imagegram.Domain.Entities;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Drawing;

namespace Imagegram.Application.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<long>
    {
        public IFormFile ImageFile { get; set; }
        public string Comments { get; set; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, long>
    {
        private readonly IImagegramDbContext _context;
        private readonly IConfigurationService _configurationService;
        public readonly IUserService _userService;

        public CreatePostCommandHandler(IImagegramDbContext context,
            IConfigurationService configurationService,
            IUserService userService)
        {
            _context = context;
            _configurationService = configurationService;
            _userService = userService;
        }

        public async Task<long> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), _configurationService.GetConfig("ImagesFolder"));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            string key = Guid.NewGuid().ToString();
            string filePath = Path.Combine(folder, $"{key}.jpg");
            using (var ms = new MemoryStream())
            {
                await request?.ImageFile?.OpenReadStream()?.CopyToAsync(ms, cancellationToken);
                using (var image = new Bitmap(ms))
                {
                    image.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            var post = new Post()
            {
                Comments = request.Comments,
                CreatedAt = DateTime.Now,
                Creator = _userService.CurrentUserId,
                ImageUrl = filePath,
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync(cancellationToken);
            return post.Id;
        }
    }
}
