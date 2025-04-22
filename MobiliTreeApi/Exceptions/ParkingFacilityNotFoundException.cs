using System;

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
}
