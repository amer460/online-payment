using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using XCoreAssignment.Models;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace XCoreAssignment.Exceptions
{
    public class ModelStateExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ModelStateException)
            {
                var exception = (ModelStateException)context.Exception;

                context.ExceptionHandled = true;

                // Create a new ViewResult and pass the original model to it
                var viewResult = new ViewResult
                {
                    ViewName = context.RouteData.Values["action"].ToString(),
                    ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState)
                    {
                        Model = exception.Model
                    }
                };

                context.Result = viewResult;
            }
        }
    }
}
