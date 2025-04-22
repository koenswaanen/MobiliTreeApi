using System;
using System.Collections.Generic;
using MobiliTreeApi.Domain;

namespace MobiliTreeApi.Repositories
{
    public static class FakeData
    {
        public static List<Session> GetSeedSessions() =>
        [
            new Session {CustomerId = "c001", ParkingFacilityId = "pf001", StartDateTime = new DateTime(2023, 10, 1, 8, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 9, 0, 0)},
            new Session {CustomerId = "c002", ParkingFacilityId = "pf001", StartDateTime = new DateTime(2023, 10, 1, 9, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 10, 0, 0)},
            new Session {CustomerId = "c003", ParkingFacilityId = "pf002", StartDateTime = new DateTime(2023, 10, 1, 10, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 11, 0, 0)},
            new Session {CustomerId = "c004", ParkingFacilityId = "pf001", StartDateTime = new DateTime(2023, 10, 1, 11, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 12, 0, 0)},
            new Session {CustomerId = "geen idee", ParkingFacilityId = "pf001", StartDateTime = new DateTime(2023, 10, 1, 12, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 13, 0, 0)},
            new Session {CustomerId = "c001", ParkingFacilityId = "pf001", StartDateTime = new DateTime(2023, 10, 1, 13, 0, 0), EndDateTime = new DateTime(2023, 10, 1, 14, 0, 0)},
        ];

        public static Dictionary<string, Customer> GetSeedCustomers() =>
            new()
            {
                {
                    "c001", new Customer("c001", "John", "pf001")
                },
                {
                    "c002", new Customer("c002", "Sarah", "pf001", "pf002")
                },
                {
                    "c003", new Customer("c003", "Andrea", "pf002")
                },
                {
                    "c004", new Customer("c004", "Peter")
                }
            };

        public static Dictionary<string, ServiceProfile> GetSeedServiceProfiles() =>
            new Dictionary<string, ServiceProfile>
            {
                {
                    "pf001",
                    new ServiceProfile
                    {
                        OverrunWeekDaysPrices =
                        [
                            new TimeslotPrice(0, 7, 1.5m), // betweeen midnight and 7 AM price is 1.5 eur/hour
                            new TimeslotPrice(7, 18, 3.5m), // betweeen 7 AM and 6 PM price is 3.5 eur/hour
                            new TimeslotPrice(18, 24, 2.5m) // betweeen 6 PM and midnight price is 2.5 eur/hour
                        ],
                        OverrunWeekendPrices =
                        [
                            new TimeslotPrice(0, 7, 1.8m),
                            new TimeslotPrice(7, 18, 3.8m),
                            new TimeslotPrice(18, 24, 2.8m)
                        ],
                        WeekDaysPrices =
                        [
                            new TimeslotPrice(0, 7, 0.5m),
                            new TimeslotPrice(7, 18, 2.5m),
                            new TimeslotPrice(18, 24, 1.5m)
                        ],
                        WeekendPrices =
                        [
                            new TimeslotPrice(0, 7, 0.8m),
                            new TimeslotPrice(7, 18, 2.8m),
                            new TimeslotPrice(18, 24, 1.8m)
                        ],
                    }
                },
                {
                    "pf002",
                    new ServiceProfile
                    {
                        OverrunWeekDaysPrices =
                        [
                            new TimeslotPrice(0, 8, 1.5m),
                            new TimeslotPrice(8, 17, 3.5m),
                            new TimeslotPrice(17, 24, 2.5m)
                        ],
                        OverrunWeekendPrices =
                        [
                            new TimeslotPrice(0, 8, 1.8m),
                            new TimeslotPrice(8, 17, 3.8m),
                            new TimeslotPrice(17, 24, 2.8m)
                        ],
                        WeekDaysPrices =
                        [
                            new TimeslotPrice(0, 8, 0.5m),
                            new TimeslotPrice(8, 17, 2.5m),
                            new TimeslotPrice(17, 24, 1.5m)
                        ],
                        WeekendPrices =
                        [
                            new TimeslotPrice(0, 8, 0.8m),
                            new TimeslotPrice(8, 17, 2.8m),
                            new TimeslotPrice(17, 24, 1.8m)
                        ],
                    }
                }
            };
    }
}
