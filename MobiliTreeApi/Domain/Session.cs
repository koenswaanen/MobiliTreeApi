using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MobiliTreeApi.Domain
{
    public class Session : IValidatableObject
    {
        [Required]
        public string ParkingFacilityId { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public DateTime? StartDateTime { get; set; }
        [Required]
        public DateTime? EndDateTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDateTime >= EndDateTime)
            {
                yield return new ValidationResult("StartDateTime must be earlier than EndDateTime.", [nameof(StartDateTime), nameof(EndDateTime)]);
            }
        }
    }
}