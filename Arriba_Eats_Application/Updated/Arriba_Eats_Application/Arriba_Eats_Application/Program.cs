
using System;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.UI;

namespace ArribaEats
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new ArribaEatsService();
            // Seed test users
            //SeedTestUsers(service);
            //service.DebugPrintUsers();

            //AvailableOrdersMenu();


            var ui = new ArribaEatsUI(service); // Create the UI that depends on the service
            ui.Run();

        }

        static void SeedTestUsers(IArribaEatsService service)
        {
            // Customers
            var customer1 = new Customer(
            name: "Alice Carter",
            age: 28,
            email: "alice@example.com",
            mobile: "0412345678",
            password: "pass1",
            location: new Location { X = 4, Y = 9 }
            );

            var customer2 = new Customer(
              name: "Bob Smith",
              age: 35,
              email: "bob@mail.com",
              mobile: "0412345679",
              password: "pass2",
              location: new Location { X = 5, Y = 10 }
              );

            // Deliverers
            var deliverer1 = new Deliverer(
            name: "Charlie Brown",
            age: 30,
            email: "charlie@mail.com",
            mobile: "0412345680",
            password: "pass3",
            licencePlate: "ABC123"
            );

            var deliverer2 = new Deliverer(
              name: "David Wilson",
              age: 32,
              email: "david@mail.com",
              mobile: "0412345681",
              password: "pass4",
              licencePlate: "XYZ789"
              );

            var client1 = new Client(
              name: "Eve Adams",
              age: 40,
              email: "eve@mail.com",
              mobile: "0412345682",
              password: "pass5",
              restaurantName: "Eve's Diner",
              style: FoodStyle.American,
              location: new Location { X = 6, Y = 11 }
              );

            var client2 = new Client(
            name: "Frank Johnson",
            age: 45,
            email: "admin@a.c",
            mobile: "0412345683",
            password: "pass6",
            restaurantName: "Frank's Pizza",
            style: FoodStyle.Italian,
            location: new Location { X = 7, Y = 12 }
          );

            string password1 = "admin#";
            string password2 = "admin1";
            string password3 = "admin2";
            string password4 = "admin3";
            string password5 = "admin4";
            string password6 = "admin5";

            // Register them into the service
            service.RegisterCustomer(customer1.Name, customer1.Age, customer1.Email, customer1.Mobile, password1, customer1.Location);
            service.RegisterCustomer(customer2.Name, customer2.Age, customer2.Email, customer2.Mobile, password2, customer2.Location);
            service.RegisterDeliverer(deliverer1.Name, deliverer1.Age, deliverer1.Email, deliverer1.Mobile, password3, deliverer1.LicencePlate);
            service.RegisterDeliverer(deliverer2.Name, deliverer2.Age, deliverer2.Email, deliverer2.Mobile, password4, deliverer2.LicencePlate);
            service.RegisterClient(client1.Name, client1.Age, client1.Email, client1.Mobile, password5, client1.Restaurant.Name, client1.Restaurant.Style, client1.Restaurant.Location);
            service.RegisterClient(client2.Name, client2.Age, client2.Email, client2.Mobile, password6, client2.Restaurant.Name, client2.Restaurant.Style, client2.Restaurant.Location);
        }



        public static void AvailableOrdersMenu()
        {
            // Dummy hardcoded orders
            var dummyOrders = new List<(string Restaurant, string RestLoc, string Customer, string CustLoc, int Distance)>
            {
                ("Tim's Tex-Mex", "6,2", "Sam Bowes-Lyon", "9,5", 12),
                ("Sushi Palace", "3,4", "Lily Evans", "5,7", 8),
                ("Pasta House", "1,1", "Harry Potter", "2,3", 15),
                ("Burger Joint", "4,5", "Ron Weasley", "6,8", 10),
                ("Pizza Place", "2,2", "Hermione Granger", "3,6", 20)
            };

            Console.WriteLine("The following orders are available for delivery. Select an order to accept it:");
            Console.WriteLine("Order  Restaurant Name       Loc    Customer Name    Loc    Dist");

            for (int i = 0; i < dummyOrders.Count; i++)
            {
                var o = dummyOrders[i];
                Console.WriteLine($"{i + 1}: {o.Restaurant.PadRight(20)} {o.RestLoc.PadRight(6)} {o.Customer.PadRight(16)} {o.CustLoc.PadRight(6)} {o.Distance}");
            }

            int returnOption = dummyOrders.Count + 1;
            Console.WriteLine($"{returnOption}: Return to the previous menu");

            Console.Write($"Please enter a choice between 1 and {returnOption}: ");
            string input = Console.ReadLine()?.Trim();

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > returnOption)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            if (choice == returnOption)
            {
                return;
            }

            var selected = dummyOrders[choice - 1];

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Thanks for accepting the order. Please head to {selected.Restaurant} at {selected.RestLoc} to pick it up.");
            Console.ResetColor();

            Console.WriteLine("Please make a choice from the menu below:");
            // Add dummy follow-up menu here if desired
        }

    }
}