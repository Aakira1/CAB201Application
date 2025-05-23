using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Interfaces;
using ArribaEats.Models;

namespace ArribaEats.Services
{
    /// <summary>
    /// Implementation of the ArribaEats service
    /// </summary>
    public class ArribaEatsService : IArribaEatsService
    {
        private List<User> _users = new();
        private List<Restaurant> _restaurants = new();
        private List<Order> _orders = new();
        private List<Deliverer> _deliverers = new();
        private int _nextOrderId = 1;

        public User CurrentUser { get; set; }

        public List<Restaurant> Restaurants => _restaurants;

        public Customer CurrentCustomer { get; }

        public Deliverer CurrentDeliverer { get;}

        public Restaurant CurrentRestaurant { get; }

        public List<Restaurant> Restaurant => _restaurants;

        public Client CurrentClient { get; }

        public Order Order { get; set; }

        #region User Management

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

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool IsEmailInUse(string email)
        {
            return _users.Any(u => u.Email == email);
        }

        #endregion

        #region Registration

        public bool RegisterCustomer(string name, int age, string email, string mobile, string password, Location location)
        {
            if (IsEmailInUse(email)) return false;
            _users.Add(new Customer(name, age, email, mobile, password, location));
            return true;
        }

        public bool RegisterDeliverer(string name, int age, string email, string mobile, string password, string licencePlate)
        {
            if (IsEmailInUse(email)) return false;
            _users.Add(new Deliverer(name, age, email, mobile, password, licencePlate));
            return true;
        }

        public bool RegisterClient(string name, int age, string email, string mobile, string password,
            string restaurantName, FoodStyle style, Location location)
        {
            if (IsEmailInUse(email)) return false;
            var client = new Client(name, age, email, mobile, password, restaurantName, style, location);
            _users.Add(client);
            _restaurants.Add(client.Restaurant);
            return true;
        }

        #endregion

        #region Menu Management

        public bool AddMenuItem(string name, decimal price)
        {
            if (CurrentUser is Client client)
            {
                client.AddMenuItem(name, price);
                return true;
            }
            return false;
        }

        #endregion

        #region Restaurant
        /// <summary>
        /// Sorts the restaurants based on the given criteria
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<Restaurant> GetSortedRestaurants(Customer customer, string sortBy)
        {
            IRestaurantSorter sorter = sortBy.ToLower() switch
            {
                "name" => new NameSorter(),
                "rating" => new RatingSorter(),
                "distance" => new DistanceSorter(),
                "style" => new StyleSorter(),
                _ => throw new ArgumentException("Invalid sorting criteria")
            };

            return sorter.SortRestaurants(_restaurants, customer);
        }

        #endregion

        #region Orders
        /// <summary>
        /// Creates a new order for the customer at the specified restaurant
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        public Order CreateOrder(Customer customer, Restaurant restaurant)
        {
            return new Order(customer, restaurant, _nextOrderId);
        }
        /// <summary>
        /// Adds a menu item to the order with the specified quantity
        /// </summary>
        /// <param name="order"></param>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItemToOrder(Order order, MenuItem item, int quantity)
        {
            order.AddItem(item, quantity);
        }
        /// <summary>
        /// Finalizes the order and links it to the customer and restaurant
        /// </summary>
        /// <param name="order"></param>
        public void FinalizeOrder(Order order)
        {
            if (order != null && !_orders.Contains(order))
            {
                order.Customer.Orders.Add(order);
                order.Restaurant.Orders.Add(order);
                _orders.Add(order);
                _nextOrderId++;
            }
        }
        /// <summary>
        /// Updates the status of the order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateOrderStatus(Order order, OrderStatus status)
        {
            if (order == null) return false;
            order.Status = status;
            return true;
        }
        /// <summary>
        /// Gets all available orders that can be accepted by deliverers
        /// </summary>
        /// <param name="restaurant"></param>
        /// <returns></returns>
        public List<Order> GetAvailableOrders(Restaurant restaurant)
        {
            return _restaurants
                .SelectMany(r => r.Orders)
                .Where(o => o.Status == OrderStatus.Cooked && o.Deliverer == null)
                .ToList();
        }
        /// <summary>
        /// Accepts an order for delivery by a deliverer 
        /// </summary>
        /// <param name="deliverer"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AcceptOrder(Deliverer deliverer, Order order)
        {
            if (order.Status != OrderStatus.Cooked || order.Deliverer != null) return false;
            deliverer.AcceptOrder(order, deliverer);
            return true;
        }

        #endregion

        #region Reviews

        /// <summary>
        /// Creates a review and links it properly to restaurant and customer
        /// </summary>
        public Review CreateReview(Customer customer, Restaurant restaurant, int rating, string comment)
        {
            var review = new Review(customer, restaurant, rating, comment);
            restaurant.AddReview(review);
            return review;
        }


        #endregion

        #region Debug
        /// <summary>
        /// Prints the details of all registered users in the system
        /// </summary>
        /// <returns></returns>
        bool IArribaEatsService.DebugPrintUsers()
        {
            Console.WriteLine("=== Registered Users ===");

            var customers = _users.OfType<Customer>().ToList();
            var clients = _users.OfType<Client>().ToList();
            var deliverers = _users.OfType<Deliverer>().ToList();

            if (customers.Any())
            {
                Console.WriteLine(">> Customers:");
                foreach (var c in customers)
                    Console.WriteLine($"- {c.Name} | Email: {c.Email}, Location: {c.Location.X},{c.Location.Y}");
                Console.WriteLine();
            }

            if (clients.Any())
            {
                Console.WriteLine(">> Clients:");
                foreach (var c in clients)
                    Console.WriteLine($"- {c.Name} | Restaurant: {c.Restaurant?.Name ?? "None"} | Style: {c.Restaurant?.Style}");
                Console.WriteLine();
            }

            if (deliverers.Any())
            {
                Console.WriteLine(">> Deliverers:");
                foreach (var d in deliverers)
                    Console.WriteLine($"- {d.Name} | Plate: {d.LicencePlate}");
                Console.WriteLine();
            }

            if (!customers.Any() && !clients.Any() && !deliverers.Any())
                Console.WriteLine("No users are currently registered.");

            Console.WriteLine("==========================");
            return true;
        }

        #endregion
    }
}