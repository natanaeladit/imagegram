using Imagegram.Application.Common.Interfaces;
using Imagegram.Application.Common.Models;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Application.Comments.Queries.GetCommentsFromPost
{
    public class GetCommentsFromPostQuery
    {
        [Required, Range(1, int.MaxValue)]
        public int PageNumber { get; set; }
        [Required, Range(1, 100)]
        public int PageSize { get; set; }
    }

    public class RetrieveCommentsFromPostQuery : GetCommentsFromPostQuery, IRequest<PaginatedList<CommentVm>>
    {
        [Required]
        public long PostId { get; set; }
    }
    public class RetrieveCommentsFromPostQueryHandler : IRequestHandler<RetrieveCommentsFromPostQuery, PaginatedList<CommentVm>>
    {
        private readonly IImagegramDbContext _context;
        public RetrieveCommentsFromPostQueryHandler(IImagegramDbContext context)
        {
            _context = context;
        }
        public async Task<PaginatedList<CommentVm>> Handle(RetrieveCommentsFromPostQuery request, CancellationToken cancellationToken)
        {
            var comments = _context.Comments
                .Where(c => c.PostId == request.PostId)
                .Select(c => new CommentVm()
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = DateTime.Now,
                    Creator = c.Creator,
                })
                .OrderByDescending(c => c.CreatedAt);
            var paginatedComments = await PaginatedList<CommentVm>.CreateAsync(comments, request.PageNumber, request.PageSize);
            return paginatedComments;
        }
    }
}
