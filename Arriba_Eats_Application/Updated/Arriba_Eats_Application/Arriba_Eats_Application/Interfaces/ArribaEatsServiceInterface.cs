using System;
using System.Collections.Generic;
using ArribaEats.Models;

namespace ArribaEats.Services
{
    /// <summary>
    /// Interface for the ArribaEats service
    /// </summary>
    public interface IArribaEatsService
    {
        /// <summary>
        /// Gets the currently logged in user
        /// </summary>
        User CurrentUser { get; }

        /// <summary>
        /// Gets the list of all restaurants in the system
        /// </summary>
        List<Restaurant> Restaurants { get; }

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
        bool RegisterCustomer(string name, int age, string email, string mobile, string password, Location location);

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
        bool RegisterDeliverer(string name, int age, string email, string mobile, string password, string licencePlate);

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
        bool RegisterClient(string name, int age, string email, string mobile, string password,
                           string restaurantName, FoodStyle style, Location location);

        /// <summary>
        /// Logs in a user with the specified email and password
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>True if login was successful, false otherwise</returns>
        bool Login(string email, string password);

        /// <summary>
        /// Logs out the current user
        /// </summary>
        void Logout();

        /// <summary>
        /// Adds a menu item to a restaurant
        /// </summary>
        /// <param name="name">The name of the menu item</param>
        /// <param name="price">The price of the menu item</param>
        /// <returns>True if the item was added successfully, false otherwise</returns>
        bool AddMenuItem(string name, decimal price);

        /// <summary>
        /// Gets the restaurants sorted by the specified criteria
        /// </summary>
        /// <param name="customer">The customer to calculate distances for</param>
        /// <param name="sortBy">The criteria to sort by</param>
        /// <returns>A list of restaurants sorted by the specified criteria</returns>
        List<Restaurant> GetSortedRestaurants(Customer customer, string sortBy);

        /// <summary>
        /// Creates an order for a customer at a restaurant
        /// </summary>
        /// <param name="customer">The customer placing the order</param>
        /// <param name="restaurant">The restaurant the order is from</param>
        /// <returns>The newly created order</returns>
        Order CreateOrder(Customer customer, Restaurant restaurant);

        /// <summary>
        /// Adds an item to an order
        /// </summary>
        /// <param name="order">The order to add the item to</param>
        /// <param name="item">The menu item to add</param>
        /// <param name="quantity">The quantity of the item to add</param>
        void AddItemToOrder(Order order, MenuItem item, int quantity);

        /// <summary>
        /// Gets the available orders for a deliverer
        /// </summary>
        /// <returns>A list of available orders</returns>
        List<Order> GetAvailableOrders();

        /// <summary>
        /// Accepts an order for delivery
        /// </summary>
        /// <param name="deliverer">The deliverer accepting the order</param>
        /// <param name="order">The order to accept</param>
        /// <returns>True if the order was accepted successfully, false otherwise</returns>
        bool AcceptOrder(Deliverer deliverer, Order order);

        /// <summary>
        /// Updates the status of an order
        /// </summary>
        /// <param name="order">The order to update</param>
        /// <param name="status">The new status</param>
        /// <returns>True if the status was updated successfully, false otherwise</returns>
        bool UpdateOrderStatus(Order order, OrderStatus status);

        /// <summary>
        /// Creates a review for a restaurant
        /// </summary>
        /// <param name="customer">The customer writing the review</param>
        /// <param name="restaurant">The restaurant being reviewed</param>
        /// <param name="rating">The rating (1-5 stars)</param>
        /// <param name="comment">The comment for the review</param>
        /// <returns>The newly created review</returns>
        Review CreateReview(Customer customer, Restaurant restaurant, int rating, string comment);

        /// <summary>
        /// Checks if an email is already in use
        /// </summary>
        /// <param name="email">The email to check</param>
        /// <returns>True if the email is already in use, false otherwise</returns>
        bool IsEmailInUse(string email);

        public bool DebugPrintUsers();
    }
}