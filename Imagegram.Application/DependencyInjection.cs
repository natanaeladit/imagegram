using FluentValidation;
using Imagegram.Application.Posts.Commands.CreatePost;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Imagegram.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
