using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Maintainable.Api.Common
{
    public class ExceptionHandlerFilter : IAsyncExceptionFilter, IAsyncActionFilter
    {
        private const string ErrorMessageTemplate = "Error executing {ControllerName} {ActionName} with arguments {@Arguments}. ErrorId: {ErrorId}";

        private const string NotFoundMessageTemplate = "Not found: {ControllerName} {ActionName} with arguments {@Arguments}.";

        private IDictionary<string, object> _actionArguments;

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                controllerName = descriptor.ControllerName;
                actionName = descriptor.ActionName;
            }

            if (context.Exception is NotFoundException)
            {
                Log.ForContext<ExceptionHandlerFilter>()
                    .Debug(
                        context.Exception,
                        NotFoundMessageTemplate,
                        controllerName,
                        actionName,
                        _actionArguments);

                context.Result = new NotFoundResult();
            }
            else
            {
                var errorId = Guid.NewGuid().ToString();
                Log.ForContext<ExceptionHandlerFilter>().Error(
                    context.Exception,
                    ErrorMessageTemplate,
                    controllerName,
                    actionName,
                    _actionArguments,
                    errorId);

                var problemDetails = new ServerErrorProblemDetails(
                    errorId,
                    context.Exception.Message,
                    context.Exception.StackTrace);
                var result = new ServerErrorResult(problemDetails);
                context.Result = result;
            }

            context.ExceptionHandled = true;

            return Task.CompletedTask;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            this._actionArguments = context.ActionArguments;
            await next().ConfigureAwait(false);
        }
    }
}
