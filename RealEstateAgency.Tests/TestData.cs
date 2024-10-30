using RealEstateAgency.Domain;

namespace RealEstateAgency.Tests;

public static class TestData
{
    public static List<Client> Clients { get; private set; }
    public static List<Order> Orders { get; private set; }
    public static List<RealEstate> RealEstates { get; private set; }

    static TestData()
    {
        RealEstates =
        [
            new RealEstate
            {
                Type = RealEstate.PropertyType.Residential,
                Address = "45 Kuybysheva St, Samara",
                Square = 85.5,
                NumberOfRooms = 3
            },
            new RealEstate
            {
                Type = RealEstate.PropertyType.Uninhabitable,
                Address = "12 Sovetskaya St, Samara",
                Square = 200.0,
                NumberOfRooms = 5
            },
            new RealEstate
            {
                Type = RealEstate.PropertyType.Residential,
                Address = "99 Gagarina Ave, Samara",
                Square = 110.0,
                NumberOfRooms = 4
            },
            new RealEstate
            {
                Type = RealEstate.PropertyType.Residential,
                Address = "67 Sovetskaya St, Samara",
                Square = 90.0,
                NumberOfRooms = 3
            },
            new RealEstate
            {
                Type = RealEstate.PropertyType.Residential,
                Address = "22 Aerodromnaya St, Samara",
                Square = 120.0,
                NumberOfRooms = 4
            },
            new RealEstate
            {
                Type = RealEstate.PropertyType.Residential,
                Address = "13 Krasnoarmeyskaya St, Samara",
                Square = 100.0,
                NumberOfRooms = 3
            }
        ];

        Clients =
        [
            new Client
            {
                Id = 1,
                FirstAndLastName = "Chukarev Michail",
                Pasport = "1234 567890",
                NumberPhone = "+7 123 456 7890",
                Address = "st. Lenina, 123, Samara",
                Email = "misha@gmail.com"
            },
            new Client
            {
                Id = 2,
                FirstAndLastName = "Stepanov Dima",
                Pasport = "4321 987654",
                NumberPhone = "+7 987 654 3210",
                Address = "st. Pushkina, 34, Samara",
                Email = "dima@gmail.com"
            },
            new Client
            {
                Id = 3,
                FirstAndLastName = "Ivanov Ivan",
                Pasport = "3456 789012",
                NumberPhone = "+7 654 321 0987",
                Address = "st. Molodogvardeyskaya, 5, Samara",
                Email = "ivanov@gmail.com"
            },
            new Client
            {
                Id = 4,
                FirstAndLastName = "Petrov Petr",
                Pasport = "6789 012345",
                NumberPhone = "+7 321 654 7890",
                Address = "st. Komsomolskaya, 89, Samara",
                Email = "petrov@gmail.com"
            }
        ];

        Orders =
        [
            new Order
            {
                Id = 1,
                Time = DateTime.Now.AddMonths(-2),
                Type = Order.PurchaseOrSale.Purchase,
                Price = 950000,
                RealEstate = RealEstates[0],
                Client = Clients[0]
            },
            new Order
            {
                Id = 2,
                Time = DateTime.Now.AddMonths(-1),
                Type = Order.PurchaseOrSale.Sale,
                Price = 1200000,
                RealEstate = RealEstates[1],
                Client = Clients[0]
            },
            new Order
            {
                Id = 3,
                Time = DateTime.Now.AddMonths(-4),
                Type = Order.PurchaseOrSale.Sale,
                Price = 950000,
                RealEstate = RealEstates[2],
                Client = Clients[1]
            },
            new Order
            {
                Id = 4,
                Time = DateTime.Now.AddMonths(-3),
                Type = Order.PurchaseOrSale.Purchase,
                Price = 500000,
                RealEstate = RealEstates[3],
                Client = Clients[2]
            },
            new Order
            {
                Id = 5,
                Time = DateTime.Now.AddMonths(-5),
                Type = Order.PurchaseOrSale.Sale,
                Price = 700000,
                RealEstate = RealEstates[4],
                Client = Clients[3]
            },
            new Order
            {
                Id = 6,
                Time = DateTime.Now.AddMonths(-2),
                Type = Order.PurchaseOrSale.Purchase,
                Price = 850000,
                RealEstate = RealEstates[5],
                Client = Clients[3]
            }
        ];
    }
}
