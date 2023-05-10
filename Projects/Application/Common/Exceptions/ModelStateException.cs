using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Application.Common.Exceptions;

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
