using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimplifyStreaming.API.App.Common.Filters
{
    public class ValidationCheckFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);

        }

        public void OnActionExecuted(ActionExecutedContext context) {}
    }
}
