using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MobiliTreeApi.Exceptions
{
    public class ParkingFacilityNotFoundException : Exception
    {
        public string Error { get; }
        public override string Message { get; }
        public ParkingFacilityNotFoundException(string error, string message) : base(message)
        {
            Error = error;
            Message = message;
        }
    }
    public class ParkingFacilityExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;
        public ParkingFacilityExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            if(exception is not ParkingFacilityNotFoundException parkingFacilityNotFoundException)
            {
                return true;
            }

            var problemDetails = new ProblemDetails
            {
                Title = parkingFacilityNotFoundException.Error,
                Detail = parkingFacilityNotFoundException.Message,
                Status = StatusCodes.Status404NotFound,
                Type = "Not Found",
            };

            return await _problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext { HttpContext = httpContext, ProblemDetails = problemDetails });
        }
    }
}
