using Application.Exercises.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class ExerciseErrorFactory
{
    public static ObjectResult ToObjectResult(this ExerciseException error)
    {
        return error switch
        {
            ExerciseAlreadyExistsException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status409Conflict
            },
            ExerciseNotFoundException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status404NotFound
            },
            ExerciseGroupsNotFoundException groupsNotFound => new ObjectResult(new
            {
                message = groupsNotFound.Message,
                missingGroupIds = groupsNotFound.MissingGroupIds.Select(id => id.Value)
            })
            {
                StatusCode = StatusCodes.Status404NotFound
            },
            ExerciseUnhandledException => new ObjectResult(error.Message)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => throw new NotImplementedException("Exercise error handler is not implemented.")
        };
    }
}
