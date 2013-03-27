namespace Quad.Berm.Tests.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Quad.Berm.Data;
    using Quad.Berm.Persistence;
    using Quad.Berm.Tests.Common;

    using QuickGenerate.Primitives;

    public abstract class PersistenseTestsBase : TestBase
    {
        protected static readonly StringGenerator FlightNoGenerator = new StringGenerator(2, 10);

        protected IRepository Repository
        {
            get
            {
                return this.Locator.GetInstance<IRepository>();
            }
        }

        internal static CustomerEntity CreateCustomer()
        {
            var instance = new CustomerEntity
                               {
                                   ChannelAddress = "https://example.com/" + Guid.NewGuid() + "/post.aspx",
                                   DeviceId = Guid.NewGuid()
                               };
            return instance;
        }

        internal static SubscriptionEntity CreateSubscription(long customerId)
        {
            var instance = new SubscriptionEntity
            {
                CustomerId = customerId,
                FlightId = FlightNoGenerator.GetRandomValue(),
                Language = MetadataInfo.DefaultLanguage
            };

            return instance;
        }

        internal static FlightContainerEntity CreateFlightContainer(int flightsCount = 2)
        {
            var c = new FlightContainerEntity
            {
                ActualDate = DateTime.Now,
                Flights = CreateFlights(flightsCount)
            };

            return c;
        }

        internal static FlightEntity[] CreateFlights(int count = 2)
        {
            count = (int)Math.Floor((double)count / 2);
            var list = new List<FlightEntity>(count * 2);
            for (var i = 0; i < count; i++)
            {
                var id = 101123 + (2 * i);
                var f1 = new FlightEntity
                {
                    Id = id.ToString(CultureInfo.InvariantCulture),
                    Direction = FlightDirectionType.Arrival,
                    Category = FlightCategoryType.International,
                    Airline = "AFL",
                    Number = 7519,
                    Terminal = "D",
                    ScheduledDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    EstimatedDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    ActualDate = new DateTimeOffset(DateTime.Now.Date.AddDays(1).Ticks, new TimeSpan(0, 4, 0, 0)),
                    Airport = "HTA",
                    ConnectionAirport1 = "HRK",
                    ConnectionAirport2 = "IFO",
                    CraftType = "AN24",
                    CancelState = 0,
                    Status = FlightStatusType.Expected
                };

                var f2 = new FlightEntity
                {
                    Id = (id + 1).ToString(CultureInfo.InvariantCulture),
                    Direction = FlightDirectionType.Departure,
                    Category = FlightCategoryType.International,
                    Airline = "JAF",
                    Number = 161,
                    Terminal = "F",
                    ScheduledDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    EstimatedDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    ActualDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    Airport = "IAQ",
                    ConnectionAirport1 = "HRK",
                    ConnectionAirport2 = "IKT",
                    CraftType = "YK50",
                    Gate = "F5",
                    RegistrationDeskRangeFrom = "11",
                    RegistrationDeskRangeTo = "24",
                    RegistrationDeskSeparate1 = "1",
                    RegistrationDeskSeparate2 = "35",
                    RegistrationStartDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)).AddHours(-4),
                    RegistrationEndDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)).AddHours(-1),
                    BoardingStartDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)).AddHours(-1),
                    BoardingEndDate = new DateTimeOffset(DateTime.Now.Ticks, new TimeSpan(0, 4, 0, 0)),
                    CancelState = 0,
                    Status = FlightStatusType.CheckIn
                };

                list.Add(f1);
                list.Add(f2);
            }           

            return list.ToArray();
        }

        internal static HotelContainerEntity CreateHotelContainer()
        {
            var list = new[]
                           {
                               new HotelEntity
                                   {
                                       Code = "bspl",
                                       Language = "en",
                                       Name = "borispil",
                                       Description = "main hotel <test>",
                                       OrderingEmail = "bspl.hotel@kbp.aero",
                                       Phone = "+380677363232",
                                       OrderIndex = 1
                                   },
                               new HotelEntity
                                   {
                                       Code = "aero",
                                       Language = "en",
                                       Name = "aeroport",
                                       Description = "secondary hotel <test>",
                                       OrderingEmail = "aero.hotel@kbp.aero",
                                       Phone = "+380677363232",
                                       OrderIndex = 2
                                   }
                           };

            return new HotelContainerEntity(list);
        }

        internal static TaxyContainerEntity CreateTaxyContainer()
        {
            var list = new[]
                           {
                               new TaxyEntity
                                   {
                                       Code = "sky",
                                       Language = "en",
                                       Name = "Sky Taxi",
                                       Description = "Exclusive comfort from the official airport taxi",
                                       OrderingEmail = "sky.taxi@kbp.aero",
                                       Phone = "+380677363232",
                                       OrderIndex = 1
                                   }
                           };

            return new TaxyContainerEntity(list);
        }

        internal static LocalizationContainerEntity CreateLocalizationContainer()
        {
            var airlines = new Dictionary<string, LocalizationMapping>
                               {
                                   {
                                       "AFL",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "Aeroflot"
                                               }
                                           }
                                   },
                                   {
                                       "JAF",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "JAFJAF"
                                               }
                                           }
                                   }
                               };
            var airports = new Dictionary<string, LocalizationMapping>
                               {
                                   {
                                       "KBP",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "Borispol"
                                               }
                                           }
                                   },
                                   {
                                       "HTA",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "HTAHTA"
                                               }
                                           }
                                   },
                                   {
                                       "HRK",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "HRKHRK"
                                               }
                                           }
                                   },
                                   {
                                       "IFO",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "IFOIFO"
                                               }
                                           }
                                   },
                                   {
                                       "IAQ",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "IAQIAQ"
                                               }
                                           }
                                   },
                                   {
                                       "IKT",
                                       new LocalizationMapping
                                           {
                                               {
                                                   MetadataInfo
                                                   .DefaultLanguage,
                                                   "IKTIKT"
                                               }
                                           }
                                   }
                               };
            var craftTypes = new Dictionary<string, LocalizationMapping>
                                 {
                                     {
                                         "AN24",
                                         new LocalizationMapping
                                             {
                                                 {
                                                     MetadataInfo
                                                     .DefaultLanguage,
                                                     "AN24AN24"
                                                 }
                                             }
                                     },
                                     {
                                         "YK50",
                                         new LocalizationMapping
                                             {
                                                 {
                                                     MetadataInfo
                                                     .DefaultLanguage,
                                                     "YK50YK50"
                                                 }
                                             }
                                     }
                                 };

            var c = new LocalizationContainerEntity();
            c.Add(MetadataInfo.DictionaryAirline, new LocalizationDictionary(airlines));
            c.Add(MetadataInfo.DictionaryAirport, new LocalizationDictionary(airports));
            c.Add(MetadataInfo.DictionaryCraftType, new LocalizationDictionary(craftTypes));

            c.ActualDate = DateTime.Now;
            return c;
        }
    }
}