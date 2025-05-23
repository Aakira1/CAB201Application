using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a menu item in a restaurant
    /// </summary>
    public class MenuItem
    {
        #region Properties

        /// <summary>Gets or sets the name of the menu item</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the price of the menu item</summary>
        public decimal Price { get; set; }

        #endregion

        #region Constructor

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

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string representation of this menu item
        /// </summary>
        public override string ToString()
        {
            return $"{Name} - ${Price:F2}";
        }

        #endregion
    }
}