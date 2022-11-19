using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.App.Common.Filters
{
    public class TitleNotFound : TypeFilterAttribute
    {
        public TitleNotFound(string idName) : base(typeof(TitleNotFoundImplementation))
        {
            Arguments = new object[] { idName };
        }

        public class TitleNotFoundImplementation : IAsyncActionFilter
        {
            private readonly ITitleRepository _titleRepository;
            private readonly IScopedService<Title> _scopedService;
            private readonly string _idName;

            public TitleNotFoundImplementation(
                ITitleRepository titleRepository,
                IScopedService<Title> scopedService,
                string idName)
            {
                _titleRepository = titleRepository;
                _scopedService = scopedService;
                _idName = idName;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var id = (string?)context.RouteData.Values[_idName];

                if (id == null)
                {
                    context.Result = new BadRequestObjectResult($"{_idName} cannot be null");
                    return;
                }

                var title = await _titleRepository.Get(id);

                if(title == null)
                {
                    context.Result = new NotFoundObjectResult("Title not found");
                    return;
                }

                _scopedService.Set(title);

                await next();
            }
        }
    }
}
