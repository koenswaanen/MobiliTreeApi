using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MobiliTreeApi.Domain
{
    public class Customer
    {
        public Customer()
        {
        }

        public Customer(string id, string name, params string[] contractedParkingFacilityIds)
        {
            Id = id;
            Name = name;
            ContractedParkingFacilityIds = contractedParkingFacilityIds.ToList();
        }

        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<string> ContractedParkingFacilityIds { get; set; }
    }
}