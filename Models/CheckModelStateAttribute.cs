using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XCoreAssignment.Models;

[AttributeUsage(AttributeTargets.Method)]
public class CheckModelStateAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {

        if (!context.ModelState.IsValid)
        {
            throw new ModelStateException(context.ModelState, context.ActionArguments.FirstOrDefault().Value);
        }
    }
}
