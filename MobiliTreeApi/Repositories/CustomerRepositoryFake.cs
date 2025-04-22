using System.Collections.Generic;
using LanguageExt;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public class CustomerRepositoryFake : ICustomerRepository
    {
        private readonly Dictionary<string, Customer> _customers;

        public CustomerRepositoryFake(Dictionary<string, Customer> customers)
        {
            _customers = customers;
        }

        public Option<Customer> GetCustomer(string customerId)
        {
            //if (_customers.TryGetValue(customerId, out var customer))
            //{
            //    return customer;    
            //}

            //return null;
            return _customers.TryGetValue(customerId, out var customer)
                ? customer
                : Option<Customer>.None;
        }
    }
}
