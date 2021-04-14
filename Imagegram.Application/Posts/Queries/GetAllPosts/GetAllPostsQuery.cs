using Imagegram.Application.Comments.Queries.GetCommentsFromPost;
using Imagegram.Application.Common.Interfaces;
using Imagegram.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Imagegram.Application.Posts.Queries.GetAllPosts
{
    public class GetAllPostsQuery : IRequest<PaginatedList<PostVm>>
    {
        [Required, Range(1, int.MaxValue)]
        public int PageNumber { get; set; }
        [Required, Range(1, 100)]
        public int PageSize { get; set; }
    }

    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PaginatedList<PostVm>>
    {
        private readonly IImagegramDbContext _context;

        public GetAllPostsQueryHandler(IImagegramDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<PostVm>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await
                        _context.Posts
                                .Select(p => new PostVm()
                                {
                                    Id = p.Id,
                                    Comments = p.Comments,
                                    CreatedAt = p.CreatedAt,
                                    Creator = p.Creator,
                                    ImageUrl = p.ImageUrl,
                                    CommentsCount = p.CommentList.Count(),
                                })
                                .OrderByDescending(p => p.CommentsCount)
                                .Skip((request.PageNumber - 1) * request.PageSize)
                                .Take(request.PageSize)
                                .ToListAsync();

            var models =
                (from p in posts
                 select new PostVm()
                 {
                     Id = p.Id,
                     Comments = p.Comments,
                     CreatedAt = p.CreatedAt,
                     Creator = p.Creator,
                     ImageUrl = p.ImageUrl,
                     CommentsCount = p.CommentsCount,
                     CommentList = (from c in _context.Comments
                                    where c.PostId == p.Id
                                    orderby c.CreatedAt descending
                                    select new CommentVm()
                                    {
                                        Content = c.Content,
                                        CreatedAt = c.CreatedAt,
                                        Creator = c.Creator,
                                        Id = c.Id,
                                        PostId = c.PostId,
                                    })
                                    .Take(3),
                 }).ToList();

            PaginatedList<PostVm> paginatedPosts = new PaginatedList<PostVm>(models, _context.Posts.Count(), request.PageNumber, request.PageSize);

            return paginatedPosts;
        }
    }

}
