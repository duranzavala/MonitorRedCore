using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MonitorRedCore.Infraestructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage).AsEnumerable();

                var json = new
                {
                    detail = errors,
                    success = false,
                    status = 400,
                    title = "Bad request",
                };
                context.Result = new BadRequestObjectResult(json);
                return;
            }
            await next();
        }
    }
}
