using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;

namespace ArribaEats.Services
{
    /// <summary>
    /// Implementation of the ArribaEats service
    /// </summary>
    public class ArribaEatsService : IArribaEatsService
    {
        private List<User> _users;
        private List<Restaurant> _restaurants;

        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// Gets the list of all restaurants in the system
        /// </summary>
        public List<Restaurant> Restaurants => _restaurants;

        /// <summary>
        /// Creates a new instance of the ArribaEats service
        /// </summary>
        public ArribaEatsService()
        {
            _users = new List<User>();
            _restaurants = new List<Restaurant>();
            CurrentUser = null;
        }

        /// <summary>
        /// Registers a new customer in the system
        /// </summary>
        /// <param name="name">The name of the customer</param>
        /// <param name="age">The age of the customer</param>
        /// <param name="email">The email address of the customer</param>
        /// <param name="mobile">The mobile number of the customer</param>
        /// <param name="password">The password of the customer</param>
        /// <param name="location">The location of the customer</param>
        /// <returns>True if registration was successful, false otherwise</returns>
        public bool RegisterCustomer(string name, int age, string email, string mobile, string password, Location location)
        {
            if (IsEmailInUse(email))
                return false;

            var customer = new Customer(name, age, email, mobile, password, location);
            _users.Add(customer);
            return true;
        }

        /// <summary>
        /// Registers a new deliverer in the system
        /// </summary>
        /// <param name="name">The name of the deliverer</param>
        /// <param name="age">The age of the deliverer</param>
        /// <param name="email">The email address of the deliverer</param>
        /// <param name="mobile">The mobile number of the deliverer</param>
        /// <param name="password">The password of the deliverer</param>
        /// <param name="licencePlate">The licence plate of the deliverer's vehicle</param>
        /// <returns>True if registration was successful, false otherwise</returns>
        public bool RegisterDeliverer(string name, int age, string email, string mobile, string password, string licencePlate)
        {
            if (IsEmailInUse(email))
                return false;

            var deliverer = new Deliverer(name, age, email, mobile, password, licencePlate);
            _users.Add(deliverer);
            return true;
        }

        /// <summary>
        /// Registers a new client in the system
        /// </summary>
        /// <param name="name">The name of the client</param>
        /// <param name="age">The age of the client</param>
        /// <param name="email">The email address of the client</param>
        /// <param name="mobile">The mobile number of the client</param>
        /// <param name="password">The password of the client</param>
        /// <param name="restaurantName">The name of the restaurant</param>
        /// <param name="style">The food style of the restaurant</param>
        /// <param name="location">The location of the restaurant</param>
        /// <returns>True if registration was successful, false otherwise</returns>
        public bool RegisterClient(string name, int age, string email, string mobile, string password,
                                 string restaurantName, FoodStyle style, Location location)
        {
            if (IsEmailInUse(email))
                return false;

            var client = new Client(name, age, email, mobile, password, restaurantName, style, location);
            _users.Add(client);
            _restaurants.Add(client.Restaurant);
            return true;
        }

        /// <summary>
        /// Logs in a user with the specified email and password
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>True if login was successful, false otherwise</returns>
        public bool Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            if (user != null && user.ValidatePassword(password))
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
        }

        /// <summary>
        /// Adds a menu item to a restaurant
        /// </summary>
        /// <param name="name">The name of the menu item</param>
        /// <param name="price">The price of the menu item</param>
        /// <returns>True if the item was added successfully, false otherwise</returns>
        public bool AddMenuItem(string name, decimal price)
        {
            if (CurrentUser is Client client)
            {
                client.AddMenuItem(name, price);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the restaurants sorted by the specified criteria
        /// </summary>
        /// <param name="customer">The customer to calculate distances for</param>
        /// <param name="sortBy">The criteria to sort by</param>
        /// <returns>A list of restaurants sorted by the specified criteria</returns>
        public List<Restaurant> GetSortedRestaurants(Customer customer, string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case "name":
                    return _restaurants.OrderBy(r => r.Name, StringComparer.OrdinalIgnoreCase).ToList();
                case "distance":
                    return _restaurants.OrderBy(r => customer != null ? r.Location.DistanceTo(customer.Location) : 0).ToList();
                case "style":
                    return _restaurants.OrderBy(r => r.Style.ToString()).ToList();
                case "rating":
                    return _restaurants.OrderByDescending(r => r.AverageRating).ToList();
                default:
                    return _restaurants.ToList();
            }
        }

        /// <summary>
        /// Creates an order for a customer at a restaurant
        /// </summary>
        /// <param name="customer">The customer placing the order</param>
        /// <param name="restaurant">The restaurant the order is from</param>
        /// <returns>The newly created order</returns>
        public Order CreateOrder(Customer customer, Restaurant restaurant)
        {
            if (customer == null || restaurant == null)
                return null;

            var order = new Order(customer, restaurant);
            return order;
        }

        /// <summary>
        /// Adds an item to an order
        /// </summary>
        /// <param name="order">The order to add the item to</param>
        /// <param name="item">The menu item to add</param>
        /// <param name="quantity">The quantity of the item to add</param>
        public void AddItemToOrder(Order order, MenuItem item, int quantity)
        {
            order.AddItem(item, quantity);
        }

        /// <summary>
        /// Gets the available orders for a deliverer
        /// </summary>
        /// <returns>A list of available orders</returns>
        public List<Order> GetAvailableOrders()
        {
            return _restaurants
                .SelectMany(r => r.Orders)
                .Where(o => o.Status == OrderStatus.Cooked && o.Deliverer == null)
                .ToList();
        }

        /// <summary>
        /// Accepts an order for delivery
        /// </summary>
        /// <param name="deliverer">The deliverer accepting the order</param>
        /// <param name="order">The order to accept</param>
        /// <returns>True if the order was accepted successfully, false otherwise</returns>
        public bool AcceptOrder(Deliverer deliverer, Order order)
        {
            if (order.Status != OrderStatus.Cooked || order.Deliverer != null)
                return false;

            deliverer.AcceptOrder(order);
            return true;
        }

        /// <summary>
        /// Updates the status of an order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="status">The new status</param>
        /// <returns>True if the status was updated successfully, false otherwise</returns>
        public bool UpdateOrderStatus(Order order, OrderStatus status)
        {
            // Validate the status transition
            bool isValidTransition = false;

            switch (order.Status)
            {
                case OrderStatus.Ordered:
                    isValidTransition = status == OrderStatus.Cooking;
                    break;
                case OrderStatus.Cooking:
                    isValidTransition = status == OrderStatus.Cooked;
                    break;
                case OrderStatus.Cooked:
                    isValidTransition = status == OrderStatus.BeingDelivered;
                    break;
                case OrderStatus.BeingDelivered:
                    isValidTransition = status == OrderStatus.Delivered;
                    break;
            }

            if (!isValidTransition)
                return false;

            order.SetStatus(status);
            return true;
        }

        /// <summary>
        /// Creates a review for a restaurant
        /// </summary>
        /// <param name="customer">The customer writing the review</param>
        /// <param name="restaurant">The restaurant being reviewed</param>
        /// <param name="rating">The rating (1-5 stars)</param>
        /// <param name="comment">The comment for the review</param>
        /// <returns>The newly created review</returns>
        public Review CreateReview(Customer customer, Restaurant restaurant, int rating, string comment)
        {
            return new Review(customer, restaurant, rating, comment);
        }

        /// <summary>
        /// Checks if an email is already in use
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True if the email is already in use, false otherwise</returns>
        public bool IsEmailInUse(string email)
        {
            return _users.Any(u => u.Email == email);
        }

        bool IArribaEatsService.DebugPrintUsers()
        {
            Console.WriteLine("=== Registered Users ===\n");

            var customers = _users.OfType<Customer>().ToList();
            var clients = _users.OfType<Client>().ToList();
            var deliverers = _users.OfType<Deliverer>().ToList();

            if (customers.Any())
            {
                Console.WriteLine(">> Customers:");
                foreach (var c in customers)
                {
                    Console.WriteLine($"- {c.Name} | Email: {c.Email}, Location: {c.Location.X},{c.Location.Y}");
                }
                Console.WriteLine();
            }

            if (clients.Any())
            {
                Console.WriteLine(">> Clients:");
                foreach (var c in clients)
                {
                    Console.WriteLine($"- {c.Name} | Restaurant: {c.Restaurant?.Name ?? "None"} | Style: {c.Restaurant?.Style}");
                }
                Console.WriteLine();
            }

            if (deliverers.Any())
            {
                Console.WriteLine(">> Deliverers:");
                foreach (var d in deliverers)
                {
                    Console.WriteLine($"- {d.Name} | Plate: {d.LicencePlate}");
                }
                Console.WriteLine();
            }

            if (!customers.Any() && !clients.Any() && !deliverers.Any())
            {
                Console.WriteLine("No users are currently registered.");
            }

            Console.WriteLine("==========================\n");
            return true;
        }
    }
}