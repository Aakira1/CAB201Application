using System;

namespace ArribaEats.Models 
{

    /// <summary>
    /// Represents a customer in the system
    /// </summary>
    public class Customer : User
    {
        /// <summary>
        /// Gets or sets the location of the customer
        /// </summary>
        public Location Location { get; set; }

        /// <summary>
        /// Gets the list of orders made by the customer
        /// </summary>
        public List<Order> Orders { get; }

        /// <summary>
        /// Gets the total spending of the customer across all orders
        /// </summary>
        public decimal TotalSpending => Orders.Sum(o => o.TotalPrice);

        /// <summary>
        /// Creates a new customer with the specified details
        /// </summary>
        /// <param name="name">The name of the customer</param>
        /// <param name="age">The age of the customer</param>
        /// <param name="email">The email address of the customer</param>
        /// <param name="mobile">The mobile number of the customer</param>
        /// <param name="password">The password of the customer</param>
        /// <param name="location">The location of the customer</param>
        public Customer(string name, int age, string email, string mobile, string password, Location location)
            : base(name, age, email, mobile, password)
        {
            Location = location;
            Orders = new List<Order>();
        }

        /// <summary>
        /// Adds an order to the customer's order history
        /// </summary>
        /// <param name="order">The order to add</param>
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }

        /// <summary>
        /// Gets the customer information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of customer information</returns>
        public override Dictionary<string, string> GetUserInfo()
        {
            var info = base.GetUserInfo();
            info.Add("Location", Location.ToString());
            info.Add("Number of orders", Orders.Count.ToString());
            info.Add("Total spent on food", $"${TotalSpending:F2}");
            return info;
        }
    }
}