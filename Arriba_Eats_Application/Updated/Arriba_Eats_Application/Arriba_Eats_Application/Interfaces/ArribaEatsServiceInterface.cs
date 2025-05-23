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
        #region User Management

        /// <summary>Gets the currently logged-in user.</summary>
        User CurrentUser { get; }
        Customer CurrentCustomer { get; }
        Deliverer CurrentDeliverer { get; }
        Restaurant CurrentRestaurant { get; }

        Order Order { get; }
        Client CurrentClient { get; }

        /// <summary>Attempts to log in a user with the specified credentials.</summary>
        bool Login(string email, string password);

        /// <summary>Logs out the current user.</summary>
        void Logout();

        /// <summary>Checks whether an email is already registered.</summary>
        bool IsEmailInUse(string email);

        #endregion

        #region Customer

        /// <summary>Registers a new customer.</summary>
        bool RegisterCustomer(string name, int age, string email, string mobile, string password, Location location);

        /// <summary>Creates a new order for a customer at a specific restaurant.</summary>
        Order CreateOrder(Customer customer, Restaurant restaurant);

        /// <summary>Adds a menu item to an existing order.</summary>
        void AddItemToOrder(Order order, MenuItem item, int quantity);

        /// <summary>Creates a new review for a restaurant.</summary>
        Review CreateReview(Customer customer, Restaurant restaurant, int rating, string comment);

        #endregion

        #region Deliverer

        /// <summary>Registers a new deliverer.</summary>
        bool RegisterDeliverer(string name, int age, string email, string mobile, string password, string licencePlate);

        /// <summary>Gets all available orders that can be accepted by deliverers.</summary>
        List<Order> GetAvailableOrders(Restaurant restaurant);

        /// <summary>Accepts an order for delivery.</summary>
        bool AcceptOrder(Deliverer deliverer, Order order);

        #endregion

        #region Client

        /// <summary>Registers a new client and their restaurant.</summary>
        bool RegisterClient(string name, int age, string email, string mobile, string password,
                            string restaurantName, FoodStyle style, Location location);

        /// <summary>Adds a new menu item to the current client's restaurant.</summary>
        bool AddMenuItem(string name, decimal price);

        #endregion

        #region Restaurant

        /// <summary>Gets the list of all restaurants in the system.</summary>
        List<Restaurant> Restaurants { get; }
        /// <summary>Gets the list of all restaurants owned by the current client.</summary>
        ///     
            


        /// <summary>Gets restaurants sorted based on the provided criteria.</summary>
        List<Restaurant> GetSortedRestaurants(Customer customer, string sortBy);

        #endregion

        #region Order Management

        /// <summary>Updates the status of an existing order.</summary>
        bool UpdateOrderStatus(Order order, OrderStatus status);

        /// <summary>Finalizes a customer's order once it is placed and complete.</summary>
        void FinalizeOrder(Order order);

        #endregion

        #region Debug/Utility

        /// <summary>Prints debug information about registered users (for development/testing only).</summary>
        bool DebugPrintUsers();

        #endregion
    }
}
