using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace XCoreAssignment.Models
{
    public class ModelStateException : Exception
    {
        public ModelStateDictionary ModelState { get; }
        public object? Model { get; }

        public ModelStateException(ModelStateDictionary modelState, object? model)
        {
            ModelState = modelState;
            Model = model;
        }
    }
}
