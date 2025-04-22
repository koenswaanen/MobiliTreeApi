using LanguageExt;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public interface IParkingFacilityRepository
    {
        Option<ServiceProfile> GetServiceProfile(string parkingFacilityId);
    }
}