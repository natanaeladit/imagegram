using Imagegram.Application.Common.Interfaces;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Imagegram.Domain.Entities;

namespace Imagegram.Application.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<long>
    {
        [Required]
        public long PostId { get; set; }
        [Required]
        public string Content { get; set; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, long>
    {
        private readonly IImagegramDbContext _context;
        public readonly IUserService _userService;
        public CreateCommentCommandHandler(IImagegramDbContext context,
            IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        public async Task<long> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var post = await _context.Posts.FindAsync(request.PostId);
            if (post != null)
            {
                var comment = new Comment()
                {
                    Content = request.Content,
                    CreatedAt = DateTime.Now,
                    Creator = _userService.CurrentUserId,
                    PostId = request.PostId,
                };
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync(cancellationToken);
                return comment.Id;
            }
            return 0;
        }
    }
}
