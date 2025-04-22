using LanguageExt;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public interface ICustomerRepository
    {
        Option<Customer> GetCustomer(string customerId);
    }
}