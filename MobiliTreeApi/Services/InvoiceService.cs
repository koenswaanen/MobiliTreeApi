using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using LanguageExt.Common;
using LanguageExt.UnsafeValueAccess;
using MobiliTreeApi.Domain;
using MobiliTreeApi.Exceptions;
using MobiliTreeApi.Repositories;

namespace MobiliTreeApi.Services
{
    public interface IInvoiceService
    {
        Result<List<Invoice>> GetInvoices(string parkingFacilityId);
        Invoice GetInvoice(string parkingFacilityId, string customerId);
    }

    public class InvoiceService: IInvoiceService
    {
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IParkingFacilityRepository _parkingFacilityRepository;
        private readonly ICustomerRepository _customerRepository;

        public InvoiceService(ISessionsRepository sessionsRepository, IParkingFacilityRepository parkingFacilityRepository, ICustomerRepository customerRepository)
        {
            _sessionsRepository = sessionsRepository;
            _parkingFacilityRepository = parkingFacilityRepository;
            _customerRepository = customerRepository;
        }

        public Result<List<Invoice>> GetInvoices(string parkingFacilityId)
        {
            var serviceProfileOption = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
            if (serviceProfileOption.IsNone)
            {
                return new Result<List<Invoice>>(new ParkingFacilityNotFoundException("Invalid Parking Facility Id", $"Invalid parking facility id '{parkingFacilityId}'"));
            }
            var serviceProfile = serviceProfileOption.ValueUnsafe();
            
            var sessions = _sessionsRepository.GetSessions(parkingFacilityId);

            var sessionsByCustomer = sessions.GroupBy(x => x.CustomerId);

            var invoices = sessionsByCustomer.Select(session =>
            {
                var customer = _customerRepository.GetCustomer(session.Key);
                var customerSessions = session.ToList();
                var customerHasContract = customer.Match(
                    c => c.ContractedParkingFacilityIds.Contains(parkingFacilityId),
                    () => false
                );

                return new Invoice
                {
                    CustomerId = session.Key,
                    ParkingFacilityId = parkingFacilityId,
                    Amount = CalculateAmount(customerSessions, serviceProfile, customerHasContract)
                };
            }).ToList();
            return invoices;
        }

        public Option<Invoice> GetInvoice(string parkingFacilityId, string customerId)
        {
            var serviceProfileOption = _parkingFacilityRepository.GetServiceProfile(parkingFacilityId);
            if (serviceProfileOption.IsNone)
            {
                throw new ParkingFacilityNotFoundException("Invalid Parking Facility Id", $"Invalid parking facility id '{parkingFacilityId}'");
            }
            var serviceProfile = serviceProfileOption.ValueUnsafe();

            var sessions = _sessionsRepository.GetSessions(parkingFacilityId)
                .Where(s => s.CustomerId == customerId)
                .ToList();
            if (sessions.Count == 0)
            {
                return Option<Invoice>.None;
            }

                var customer = _customerRepository.GetCustomer(customerId);
            var customerHasContract = customer.Match(
                c => c.ContractedParkingFacilityIds.Contains(parkingFacilityId),
                () => false
            );

            return new Invoice
            {
                CustomerId = customerId,
                ParkingFacilityId = parkingFacilityId,
                Amount = CalculateAmount(sessions, serviceProfile, customerHasContract)
            };
        }

        private static decimal CalculateAmount(List<Session> customerSessions, ServiceProfile serviceProfile, bool customerHasContract)
        {
            decimal totalAmount = 0;
            const int minutesInAnHour = 60;
            foreach (var session in customerSessions)
            {
                var isWeekend = session.StartDateTime.Value.DayOfWeek == DayOfWeek.Saturday || session.StartDateTime.Value.DayOfWeek == DayOfWeek.Sunday;

                IList<TimeslotPrice> timeslotPrices;
                if (customerHasContract)
                {
                    timeslotPrices = isWeekend ? serviceProfile.WeekendPrices : serviceProfile.WeekDaysPrices;
                }
                else
                {
                    timeslotPrices = isWeekend ? serviceProfile.OverrunWeekendPrices : serviceProfile.OverrunWeekDaysPrices;
                }

                TimeSpan diff = session.EndDateTime.Value - session.StartDateTime.Value;
                var minutes = diff.TotalMinutes;

                var timeslotPrice = timeslotPrices.Single(t => session.StartDateTime.Value.Hour >= t.StartHour && session.StartDateTime.Value.Hour < t.EndHour);
                totalAmount += (decimal)minutes * (timeslotPrice.PricePerHour / minutesInAnHour);
            }
            return decimal.Round(totalAmount, 2);
        }
    }
}
