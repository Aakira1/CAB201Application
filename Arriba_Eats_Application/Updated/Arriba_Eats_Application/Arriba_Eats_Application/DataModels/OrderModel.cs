using System;
using System.Collections.Generic;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents an order in the system
    /// </summary>
    /// <remarks>
    /// Creates a new order with the specified details
    /// </remarks>
    /// <param name="customer">The customer who placed the order</param>
    /// <param name="restaurant">The restaurant the order is from</param>
    public class Order(Customer customer,
                 Restaurant restaurant, 
                 int id)
    {
        private static int _nextId = 1; // Static variable to keep track of the next order ID
        /// <summary>
        /// Gets the unique identifier for this order
        /// </summary>
        public int Id { get; } = id;


        /// <summary>
        /// Gets the customer who placed the order
        /// </summary>
        public Customer Customer { get; set; } = customer;

        /// <summary>
        /// Gets the restaurant the order is from
        /// </summary>
        public Restaurant Restaurant { get; set; } = restaurant;

        /// <summary>
        /// Gets the deliverer assigned to the order
        /// </summary>
        public Deliverer? Deliverer { get; set; }

        /// <summary>
        /// Gets the current status of the order
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;

        /// <summary>
        /// Gets the items in the order with their quantities
        /// </summary>
        public Dictionary<MenuItem, int> Items { get; set; } = new Dictionary<MenuItem, int>();

        public List<MenuItem> items { get; set; } = new List<MenuItem>();

        /// <summary>
        /// Gets the total price of the order
        /// </summary>
        public decimal TotalPrice => Items.Sum(item => item.Value * item.Key.Price);


        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="item">The menu item to add</param>
        /// <param name="quantity">The quantity of the item to add</param>
        public void AddItem(MenuItem item, int quantity)
        {
            if (Items.ContainsKey(item))
            {
                Items[item] += quantity;
            }
            else if (quantity > 0)
            {
                Items.Add(item, quantity);
            }
            else
                Items[item] = quantity;
        }

        /// <summary>
        /// Sets the status of the order
        /// </summary>
        /// <param name="status">The new status</param>
        public void SetStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        /// <summary>
        /// Assigns a deliverer to the order
        /// </summary>
        /// <param name="deliverer">The deliverer to assign</param>
        public void AssignDeliverer(Deliverer deliverer)
        {
            Deliverer = deliverer;
        }

        /// <summary>
        /// Gets the order information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of order information</returns>
        public Dictionary<string, string> GetOrderInfo()
        {
            var info = new Dictionary<string, string>
            {
                { "Order ID", Id.ToString() },
                { "Customer", Customer.Name },
                { "Restaurant", Restaurant.Name },
                { "Status", Status.ToString() },
                { "Total Price", $"${TotalPrice:F2}" }
            };
            return info;
        }

        /// <summary>
        /// Gets a string listing all items in the order with their quantities
        /// </summary>
        /// <returns>A string representation of the order items</returns>
        public string GetItemsAsString()
        {
            return string.Join("\n", Items.Select(i => $"{i.Value}x {i.Key.Name} - ${i.Key.Price:F2} each = ${i.Value * i.Key.Price:F2}"));
        }
    }
}