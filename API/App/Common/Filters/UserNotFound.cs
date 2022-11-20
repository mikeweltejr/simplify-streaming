using DynamoDB.DAL.App.Models;
using DynamoDB.DAL.App.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SimplifyStreaming.API.App.Common.Services;

namespace SimplifyStreaming.API.App.Common.Filters
{
    public class UserNotFound : TypeFilterAttribute
    {
        public UserNotFound(string idName) : base(typeof(UserNotFoundImplementation))
        {
            Arguments = new object[] { idName };
        }

        public class UserNotFoundImplementation : IAsyncActionFilter
        {
            private readonly IUserService _userService;
            private readonly IScopedService<User> _scopedService;
            private readonly string _idName;

            public UserNotFoundImplementation(
                IUserService userService,
                IScopedService<User> scopedService,
                string idName)
            {
                _userService = userService;
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

                var user = await _userService.GetUser(id);

                if(user == null)
                {
                    context.Result = new NotFoundObjectResult("User not found");
                    return;
                }

                _scopedService.Set(user);

                await next();
            }
        }
    }
}
