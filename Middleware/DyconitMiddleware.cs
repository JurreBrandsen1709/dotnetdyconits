using System.Diagnostics;
using DotnetDyconits.Attributes;
using Microsoft.AspNetCore.Http;

namespace DotnetDyconits.Middleware
{
    public class DyconitMiddleware
    {
        private readonly ILogger<DyconitMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly DyconitOptions _options;

        public DyconitMiddleware(
            ILogger<DyconitMiddleware> logger,
            RequestDelegate next,
            DyconitOptions options = new DyconitOptions())
        {
            _logger = logger;
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var attribute = context.GetEndpoint()?.Metadata.GetMetadata<Conit>();

            if (attribute == null)
            {
                await _next(context);
            }

            double _numericalErrorThreshold = attribute.numericalErrorThreshold;
            int _orderErrorThreshold = attribute.orderErrorThreshold;
            int _stalenessThreshold = attribute.stalenessThreshold;

            var numericalError = CalculateNumericalError();
            if (numericalError > _numericalErrorThreshold)
            {
                // log error
                _logger.LogError($"Numerical error threshold exceeded: {numericalError}");
            }

            // Check for order error
            var orderError = CalculateOrderError();
            if (orderError > _orderErrorThreshold)
            {
                // log error
                _logger.LogError($"Order error threshold exceeded: {orderError}");
            }

            // Check for staleness
            var staleness = CalculateStaleness();
            if (staleness > _stalenessThreshold)
            {
                // log error
                _logger.LogError($"Staleness threshold exceeded: {staleness}");
            }

            await _next(context);
        }

        private int CalculateStaleness()
        {
            return 1;
        }

        private int CalculateOrderError()
        {
            return 1;
        }

        private int CalculateNumericalError()
        {
            return 1;
        }
    }

    public static class DyconitMiddlewareExtensions
    {
        public static IApplicationBuilder UseDyconitsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DyconitMiddleware>();
        }
    }
}
