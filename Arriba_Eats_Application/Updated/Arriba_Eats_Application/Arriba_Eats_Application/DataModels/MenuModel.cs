using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a menu item in a restaurant
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// Gets or sets the name of the menu item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price of the menu item
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Creates a new menu item with the specified details
        /// </summary>
        /// <param name="name">The name of the menu item</param>
        /// <param name="price">The price of the menu item</param>
        public MenuItem(string name, decimal price)
        {
            Name = name;
            Price = price;
        }

        /// <summary>
        /// Returns a string representation of this menu item
        /// </summary>
        /// <returns>A string representation of this menu item</returns>
        public override string ToString()
        {
            return $"{Name} - ${Price:F2}";
        }
    }
}