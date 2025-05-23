using System;
using System.Linq;
using MobiliTreeApi.Exceptions;
using MobiliTreeApi.Repositories;
using MobiliTreeApi.Services;
using Xunit;

namespace MobiliTreeApi.Tests
{
    public class InvoiceServiceTest
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IParkingFacilityRepository _parkingFacilityRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceServiceTest()
        {
            _sessionsRepository = new SessionsRepositoryFake(FakeData.GetSeedSessions());
            _parkingFacilityRepository = new ParkingFacilityRepositoryFake(FakeData.GetSeedServiceProfiles());
            _customerRepository = new CustomerRepositoryFake(FakeData.GetSeedCustomers());
        }

        [Fact]
        public void GivenSessionsService_WhenQueriedForInexistentParkingFacility_ThenThrowException()
        {
            var parkingFacilityId = "nonExistingParkingFacilityId";
            var invoicesResult = GetSut().GetInvoices(parkingFacilityId);
            invoicesResult.Match(
                invoices =>
                {
                    Assert.Null(invoices);
                    return invoices;
                },
                exception =>
                {
                    Assert.IsType<ParkingFacilityNotFoundException>(exception);
                    Assert.Equal("Invalid parking facility id 'nonExistingParkingFacilityId'", exception.Message);
                    return null;
                });           
        }

        [Fact]
        public void GivenEmptySessionsStore_WhenQueriedForUnknownParkingFacility_ThenReturnEmptyInvoiceList()
        {
            var result = GetSut().GetInvoices("pf001").Match(
                invoices => invoices,
                exception => null);

            Assert.Empty(result);
        }

        [Fact]
        public void GivenOneSessionInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnInvoiceListWithOneElement()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            var customerId = "some customer";
            var parkingFacilityId = "pf001";

            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = customerId,
                ParkingFacilityId = parkingFacilityId,
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001").Match(invoices => invoices, exception => null);

            var invoice = Assert.Single(result);
            Assert.NotNull(invoice);
            Assert.Equal(parkingFacilityId, invoice.ParkingFacilityId);
            Assert.Equal(customerId, invoice.CustomerId);
        }

        [Fact]
        public void GivenMultipleSessionsInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnOneInvoicePerCustomer()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c001",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c001",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c002",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001").Match(invoices => invoices, exception => null);

            Assert.Equal(2, result.Count);
            var invoiceCust1 = result.SingleOrDefault(x => x.CustomerId == "c001");
            var invoiceCust2 = result.SingleOrDefault(x => x.CustomerId == "c002");
            Assert.NotNull(invoiceCust1);
            Assert.NotNull(invoiceCust2);
            Assert.Equal("pf001", invoiceCust1.ParkingFacilityId);
            Assert.Equal("pf001", invoiceCust2.ParkingFacilityId);
            Assert.Equal("c001", invoiceCust1.CustomerId);
            Assert.Equal("c002", invoiceCust2.CustomerId);
        }

        [Fact]
        public void GivenMultipleSessionsForMultipleFacilitiesInTheStore_WhenQueriedForExistingParkingFacility_ThenReturnInvoicesOnlyForQueriedFacility()
        {
            var startDateTime = new DateTime(2018, 12, 15, 12, 25, 0);
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c001",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c001",
                ParkingFacilityId = "pf002",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });
            _sessionsRepository.AddSession(new Domain.Session
            {
                CustomerId = "c002",
                ParkingFacilityId = "pf001",
                StartDateTime = startDateTime,
                EndDateTime = startDateTime.AddHours(1)
            });

            var result = GetSut().GetInvoices("pf001").Match(invoices => invoices, exception => null);

            Assert.Equal(2, result.Count);
            var invoiceCust1 = result.SingleOrDefault(x => x.CustomerId == "c001");
            var invoiceCust2 = result.SingleOrDefault(x => x.CustomerId == "c002");
            Assert.NotNull(invoiceCust1);
            Assert.NotNull(invoiceCust2);
            Assert.Equal("pf001", invoiceCust1.ParkingFacilityId);
            Assert.Equal("pf001", invoiceCust2.ParkingFacilityId);
            Assert.Equal("c001", invoiceCust1.CustomerId);
            Assert.Equal("c002", invoiceCust2.CustomerId);
        }

        private IInvoiceService GetSut()
        {
            return new InvoiceService(
                _sessionsRepository, 
                _parkingFacilityRepository,
                _customerRepository);
        }
    }
}
