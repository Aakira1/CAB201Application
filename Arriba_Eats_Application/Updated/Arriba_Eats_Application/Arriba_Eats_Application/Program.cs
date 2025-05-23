using System;
using System.Runtime.CompilerServices;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.UI;
using ArribaEats.UI.Settings;

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
            //Create some dummy data
            //CreateDummyData(service);


            var ui = new ArribaEatsUI(service); // Create the UI that depends on the service
            ui.Run();

        }
        /// <summary>
        /// Seeds the test users into the service.
        /// </summary>
        /// <param name="service"></param>
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


            string password1 = "admi";
            string password2 = "admin";


            // Register them into the service
            service.RegisterCustomer(customer1.Name, customer1.Age, customer1.Email, customer1.Mobile, password1, customer1.Location);
            service.RegisterCustomer(customer2.Name, customer2.Age, customer2.Email, customer2.Mobile, password2, customer2.Location);
        }


        /// <summary>
        /// Creates some dummy data for testing purposes.
        /// </summary>
        /// <param name="_service"></param>
        private static void CreateDummyData(ArribaEatsService _service)
        {
            // Clear existing data if needed
            _service.Restaurants.Clear();

            // Create some dummy customers
            var customer1 = new Customer("Alice", 28, "alice@example.com", "0412345678", "Password1!", new Location(2, 3));
            var customer2 = new Customer("Bob", 35, "bob@example.com", "0498765432", "Password2!", new Location(8, 1));

            // Create some dummy clients/restaurants
            var client1 = new Client("Chef Mike", 45, "mike@restaurant.com", "0411122233", "Password3!", "Tasty Tacos", FoodStyle.Australian, new Location(5, 5));
            var client2 = new Client("Chef Anna", 38, "anna@restaurant.com", "0411223344", "Password4!", "Sushi Spot", FoodStyle.Japanese, new Location(6, 2));

            _service.Restaurants.Add(client1.Restaurant);
            _service.Restaurants.Add(client2.Restaurant);

            // Create some dummy orders
            var order1 = new Order(customer1, client1.Restaurant, 1);
            var order2 = new Order(customer2, client2.Restaurant, 2);

            var deliverer1 = new Deliverer(
                name: "Charlie Brown",
                age: 30,
                email: "a@a",
                mobile: "0412345680",
                password: "pass3",
                licencePlate: "ABC123"
                );

            _service.RegisterDeliverer(deliverer1.Name, deliverer1.Age, deliverer1.Email, deliverer1.Mobile, "test", deliverer1.LicencePlate);

            // Add dummy menu items
            var taco = client1.AddMenuItem("Beef Taco", 9.99m);
            var sushi = client2.AddMenuItem("Salmon Roll", 12.50m);

            // Add items to orders
            order1.AddItem(taco, 2);
            order2.AddItem(sushi, 3);

            // Set order statuses to Cooked so they appear in available orders
            order1.SetStatus(OrderStatus.Cooked);
            order2.SetStatus(OrderStatus.Cooked);

            // Add orders to restaurants and customers
            client1.Restaurant.AddOrder(order1);
            client2.Restaurant.AddOrder(order2);

            customer1.AddOrder(order1);
            customer2.AddOrder(order2);

            //DebugMenu(_service, deliverer1, client1.Restaurant, customer2);
        }
        /// <summary>
        /// Debugs the menu for a specific deliverer, restaurant, and customer.
        /// </summary>
        /// <param name="_service"></param>
        /// <param name="deliverer"></param>
        /// <param name="restaurant"></param>
        /// <param name="customer"></param>
        static void DebugMenu(ArribaEatsService _service, Deliverer deliverer, Restaurant restaurant, Customer customer)
        {
            InputChecks inputChecks = new InputChecks();
            var r = restaurant;
            Location loc = inputChecks.GetValidLocation();

            int distance = (int)Math.Sqrt(Math.Pow(r.Location.X - customer.Location.X, 2) + Math.Pow(r.Location.Y - customer.Location.Y, 2));
            int total = (int)Math.Sqrt(Math.Pow(loc.X + customer.Location.X, restaurant.Location.X) + Math.Pow(loc.Y - customer.Location.Y, restaurant.Location.Y));

            Console.WriteLine($"Distance from restaurant to customer: {total} units");

            Console.WriteLine("The following orders are available for delivery. Select an order to accept it:");
            Console.WriteLine("Order  Restaurant Name       Loc    Customer Name    Loc    Dist");

            for (int i = 0; i < _service.GetAvailableOrders(restaurant).Count; i++)
            {
                // make it match Console.WriteLine("Order  Restaurant Name       Loc    Customer Name    Loc    Dist");
                var order = _service.GetAvailableOrders(restaurant)[i];
                Console.WriteLine($"{i + 1}      {order.Restaurant.Name}       {order.Restaurant.Location.X},{order.Restaurant.Location.Y}    {order.Customer.Name}       {order.Customer.Location.X},{order.Customer.Location.Y}    {distance}");

            }

        }
    }
}