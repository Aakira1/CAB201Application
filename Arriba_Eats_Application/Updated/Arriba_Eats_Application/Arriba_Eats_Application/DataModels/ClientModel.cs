using System;
using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a client (restaurant owner) in the system
    /// </summary>
    public class Client : User
    {
        /// <summary>
        /// Gets or sets the restaurant owned by the client
        /// </summary>
        public Restaurant Restaurant { get; private set; }

        /// <summary>
        /// Creates a new client with the specified details
        /// </summary>
        /// <param name="name">The name of the client</param>
        /// <param name="age">The age of the client</param>
        /// <param name="email">The email address of the client</param>
        /// <param name="mobile">The mobile number of the client</param>
        /// <param name="password">The password of the client</param>
        /// <param name="restaurantName">The name of the restaurant</param>
        /// <param name="style">The food style of the restaurant</param>
        /// <param name="location">The location of the restaurant</param>
        public Client(string name, int age, string email, string mobile, string password,
                     string restaurantName, FoodStyle style, Location location)
            : base(name, age, email, mobile, password)
        {
            Restaurant = new Restaurant(restaurantName, style, location, this);
        }

        /// <summary>
        /// Adds a menu item to the restaurant's menu
        /// </summary>
        /// <param name="name">The name of the menu item</param>
        /// <param name="price">The price of the menu item</param>
        /// <returns>The newly created menu item</returns>
        public MenuItem AddMenuItem(string name, decimal price)
        {
            return Restaurant.AddMenuItem(name, price);
        }

        /// <summary>
        /// Starts cooking an order
        /// </summary>
        /// <param name="order">The order to start cooking</param>
        public void StartCooking(Order order)
        {
            if (order.Status != OrderStatus.Ordered)
            {
                //Console.WriteLine("Cannot start cooking. Order must be in 'Ordered' status.");
                return;
            }
            order.SetStatus(OrderStatus.Cooking);
        }

        /// <summary>
        /// Finishes cooking an order
        /// </summary>
        /// <param name="order">The order that is finished cooking</param>
        public void FinishCooking(Order order)
        {
            if (order.Status != OrderStatus.Cooking)
            {
                //Console.WriteLine("Cannot finish cooking. Order must be in 'Cooking' status.");
                return;
            }
            order.SetStatus(OrderStatus.Cooked);
        }

        /// <summary>
        /// Sets an order as being delivered
        /// </summary>
        /// <param name="order">The order that is being delivered</param>
        public void SetOrderBeingDelivered(Order order)
        {
            if (order.Status != OrderStatus.Cooked)
            {
                //Console.WriteLine("Cannot mark as being delivered. Order must be 'Cooked'.");
                return;
            }
            order.SetStatus(OrderStatus.BeingDelivered);
        }

        /// <summary>
        /// Gets the client information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of client information</returns>
        public override Dictionary<string, string> GetUserInfo()
        {
            var info = base.GetUserInfo();
            info.Add("Restaurant Name", Restaurant.Name);
            info.Add("Restaurant Style", Restaurant.Style.ToString());
            info.Add("Restaurant Location", Restaurant.Location.ToString());
            return info;
        }
    }
}