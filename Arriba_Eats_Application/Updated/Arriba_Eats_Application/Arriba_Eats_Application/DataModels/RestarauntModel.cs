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
        #region Properties
        /// <summary>
        /// Gets or sets the restaurant's unique identifier
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's food style
        /// </summary>
        public FoodStyle Style { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's location
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's menu
        /// </summary>
        public List<MenuItem> Menu { get; set; } = new();
        /// <summary>
        /// Gets or sets the restaurant's reviews
        /// </summary>
        public List<Review> Reviews { get; set; } = new();
        /// <summary>
        /// Gets or sets the restaurant's orders
        /// </summary>
        public List<Order> Orders { get; set; } = new();
        /// <summary>
        /// Gets or sets the restaurant's customer
        /// </summary>
        public Customer Customer { get  ; set; }
        /// <summary>
        /// Gets or sets the restaurant's deliverer
        /// </summary>
        public Deliverer Deliverer { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's location
        /// </summary>
        public Location Locations { get; set; }
        /// <summary>
        /// Gets or sets the restaurant's orders
        /// </summary>
        Restaurant restaurant { get; set; } // Not needed unless used for something specific

        /// <summary>
        /// Gets or sets the restaurant's owner
        /// </summary>
        public Client Owner { get; set; }
        /// <summary>
        /// Gets the average rating of the restaurant based on its reviews
        /// </summary>
        public double AverageRating
        {
            get
            {
                if (Reviews.Count == 0)
                    return 0;
                return Reviews.Average(r => r.Rating);
            }
            set { }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new restaurant with the specified details
        /// </summary>
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
        /// Creates a new restaurant with the specified details
        /// </summary>
        List<Restaurant> _restaurants = new List<Restaurant>();


        #endregion

        #region Menu Management

        /// <summary>
        /// Adds a menu item to the restaurant's menu
        /// </summary>
        public MenuItem AddMenuItem(string name, decimal price)
        {
            var item = new MenuItem(name, price);
            Menu.Add(item);
            return item;
        }

        #endregion

        #region Order Management

        /// <summary>Adds an order to the restaurant</summary>
        public void AddOrder(Order order) => Orders.Add(order);

        /// <summary>Removes an order from the restaurant</summary>
        public void RemoveOrder(Order order) => Orders.Remove(order);

        #endregion

        #region Review Management

        /// <summary>Adds a review to the restaurant</summary>
        public void AddReview(Review review) => Reviews.Add(review);

        #endregion

        #region Utility

        /// <summary>
        /// Gets the restaurant information as a dictionary of property names and values
        /// </summary>
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
        #endregion
    }
}