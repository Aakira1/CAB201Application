using System;
using System.Collections.Generic;
using System.Linq;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a customer in the system
    /// </summary>
    public class Customer : User
    {
        #region Properties
        /// <summary>
        /// Gets or sets the customer's location
        /// </summary>
        public Location Location { get; set; }
        /// <summary>
        /// Gets or sets the list of orders made by the customer
        /// </summary>
        public List<Order> Orders { get; set; } = new();
        /// <summary>
        /// Gets or sets the total amount spent by the customer
        /// </summary>
        public decimal TotalSpent { get; set; } = 0;

        /// <summary>
        /// Gets the total spending of the customer across all orders
        /// </summary>
        public decimal TotalSpending => Orders.Sum(o => o.TotalPrice);

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new customer with the specified details
        /// </summary>
        public Customer(string name, int age, string email, string mobile, string password, Location location)
            : base(name, age, email, mobile, password)
        {
            Location = location;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an order to the customer's order history
        /// </summary>
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        /// <summary>
        /// Gets the customer information as a dictionary of property names and values
        /// </summary>
        public override Dictionary<string, string> GetUserInfo()
        {
            var info = base.GetUserInfo();
            info.Add("Location", Location.ToString());
            info.Add("Spending", $"You've made {Orders.Count} order(s) and spent a total of ${TotalSpending:F2}");
            return info;
        }

        #endregion
    }
}
