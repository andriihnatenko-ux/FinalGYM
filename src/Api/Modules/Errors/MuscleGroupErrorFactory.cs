using Application.MuscleGroups.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class MuscleGroupErrorFactory
{
    public static ObjectResult ToObjectResult(this MuscleGroupException error)
    {
        return error switch
        {
            MuscleGroupAlreadyExistsException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status409Conflict
            },
            MuscleGroupNotFoundException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status404NotFound
            },
            MuscleGroupUnhandledException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => throw new NotImplementedException("Muscle group error handler is not implemented.")
        };
    }
}
