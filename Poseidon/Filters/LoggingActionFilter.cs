using Microsoft.AspNetCore.Mvc.Filters;

namespace Poseidon.Filters
{
    public class LoggingActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Log request details before action executes
            Console.WriteLine($"Executing action: {context.ActionDescriptor.DisplayName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Log response details after action executes
            Console.WriteLine($"Action executed: {context.ActionDescriptor.DisplayName}");
        }
    }
}
