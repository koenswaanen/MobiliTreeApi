using System.Collections.Generic;
using LanguageExt;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public class ParkingFacilityRepositoryFake : IParkingFacilityRepository
    {
        private readonly Dictionary<string, ServiceProfile> _serviceProfiles;

        public ParkingFacilityRepositoryFake(Dictionary<string, ServiceProfile> serviceProfiles)
        {
            _serviceProfiles = serviceProfiles;
        }

        public Option<ServiceProfile> GetServiceProfile(string parkingFacilityId)
        {    
            return _serviceProfiles.TryGetValue(parkingFacilityId, out var serviceProfile)
                ? serviceProfile
                : Option<ServiceProfile>.None;
        }
    }
}
