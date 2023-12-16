using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TheaterCashRegister.BLL.Exception;

public class DefaultExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var status = HttpStatusCode.InternalServerError;
        var message = string.Empty;

        var exceptionType = context.Exception.GetType();
        if (MatchExceptionType(exceptionType, typeof(EntityNotFoundException)))
        {
            message = context.Exception.Message;
            status = HttpStatusCode.NotFound;
        }
        else if (MatchExceptionType(exceptionType, typeof(EntityDuplicateException),
                     typeof(EntityIllegalStateException)))
        {
            message = context.Exception.Message;
            status = HttpStatusCode.BadRequest;
        }
        else
        {
            message = context.Exception.Message;
        }

        context.ExceptionHandled = true;
        context.HttpContext.Response.StatusCode = (int)status;
        context.Result = new JsonResult(new { error = new { message = message, status_code = status } });
    }

    private static bool MatchExceptionType(Type exceptionType, params Type[] targetTypes)
    {
        return targetTypes.Any(targetType => exceptionType == targetType);
    }
}