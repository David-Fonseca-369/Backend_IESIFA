using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend_IESIFA.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<Exception> logger;

        public ExceptionFilter(ILogger<Exception> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {

            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
