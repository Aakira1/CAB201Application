using System;
using System.Collections.Generic;
using System.Linq;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a restaurant in the system
    /// </summary>
    public class Restaurant
    {
        public string Name { get; set; }
        public FoodStyle Style { get; set; }
        public Location Location { get; set; }
        public List<MenuItem> Menu { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public Client Owner { get; set; }

        /// <summary>
        /// Gets the average rating of the restaurant
        /// </summary>
        public double AverageRating
        {
            get
            {
                if (Reviews.Count == 0)
                    return 0;
                return Reviews.Average(r => r.Rating);
            }
        }

        /// <summary>
        /// Creates a new restaurant with the specified details
        /// </summary>
        /// <param name="name">The name of the restaurant</param>
        /// <param name="style">The food style of the restaurant</param>
        /// <param name="location">The location of the restaurant</param>
        /// <param name="owner">The client who owns the restaurant</param>
        public Restaurant(string name, FoodStyle style, Location location, Client owner)
        {
            Name = name;
            Style = style;
            Location = location;
            Owner = owner;
            Menu = new List<MenuItem>();
            Reviews = new List<Review>();
            Orders = new List<Order>();
        }

        /// <summary>
        /// Adds a menu item to the restaurant's menu
        /// </summary>
        /// <param name="name">The name of the menu item</param>
        /// <param name="price">The price of the menu item</param>
        /// <returns>The newly created menu item</returns>
        public MenuItem AddMenuItem(string name, decimal price)
        {
            var item = new MenuItem(name, price);
            Menu.Add(item);
            return item;
        }

        /// <summary>
        /// Adds an order to the restaurant's order list
        /// </summary>
        /// <param name="order">The order to add</param>
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        /// <summary>
        /// Adds a review to the restaurant
        /// </summary>
        /// <param name="review">The review to add</param>
        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        /// <summary>
        /// Removes an order from the restaurant's order list
        /// </summary>
        /// <param name="order">The order to remove</param>
        public void RemoveOrder(Order order)
        {
            Orders.Remove(order);
        }

        /// <summary>
        /// Gets the restaurant information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of restaurant information</returns>
        public Dictionary<string, string> GetRestaurantInfo()
        {
            return new Dictionary<string, string>
            {
                { "Name", Name },
                { "Style", Style.ToString() },
                { "Location", Location.ToString() },
                { "Average Rating", $"{AverageRating:F1} stars" },
                { "Menu Items", Menu.Count.ToString() }
            };
        }
    }
}
