using System;
using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a client (restaurant owner) in the system
    /// </summary>
    public class Client : User
    {
        #region Properties

        /// <summary>Gets or sets the restaurant owned by the client</summary>
        public Restaurant Restaurant { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new client with the specified details
        /// </summary>
        public Client(string name, int age, string email, string mobile, string password,
                      string restaurantName, FoodStyle style, Location location)
            : base(name, age, email, mobile, password)
        {
            Restaurant = new Restaurant(restaurantName, style, location, this);
        }

        #endregion

        #region Menu Management

        /// <summary>Adds a menu item to the restaurant's menu</summary
        public MenuItem AddMenuItem(string name, decimal price)
        {
            return Restaurant.AddMenuItem(name, price);
        }

        #endregion

        #region Order Management

        /// <summary>Starts cooking an order</summary>
        public void StartCooking(Order order)
        {
            if (order.Status == OrderStatus.Ordered)
                order.SetStatus(OrderStatus.Cooking);
        }

        /// <summary>Finishes cooking an order</summary>
        public void FinishCooking(Order order)
        {
            if (order.Status == OrderStatus.Cooking)
                order.SetStatus(OrderStatus.Cooked);
        }

        /// <summary>Marks an order as being delivered</summary>
        public void SetOrderBeingDelivered(Order order)
        {
            if (order.Status == OrderStatus.Cooked)
                order.SetStatus(OrderStatus.BeingDelivered);
        }

        #endregion

        #region Utility

        /// <summary>Gets the client information as a dictionary</summary>
        public override Dictionary<string, string> GetUserInfo()
        {
            var info = base.GetUserInfo();
            info.Add("Restaurant Name", Restaurant.Name);
            info.Add("Restaurant Style", Restaurant.Style.ToString());
            info.Add("Restaurant Location", Restaurant.Location.ToString());
            return info;
        }

        #endregion
    }
}
