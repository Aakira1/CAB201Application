using System;
using System.Collections.Generic;
using System.Linq;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents an order in the system
    /// </summary>
    public class Order
    {
        private static int nextId = 1;

        /// <summary>
        /// Gets the unique identifier for this order
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the customer who placed the order
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// Gets the restaurant the order is from
        /// </summary>
        public Restaurant Restaurant { get; }

        /// <summary>
        /// Gets the deliverer assigned to the order
        /// </summary>
        public Deliverer Deliverer { get; private set; }

        /// <summary>
        /// Gets the current status of the order
        /// </summary>
        public OrderStatus Status { get; private set; }

        /// <summary>
        /// Gets the items in the order with their quantities
        /// </summary>
        public Dictionary<MenuItem, int> Items { get; }

        public List<MenuItem> items { get; set; } = new();

        /// <summary>
        /// Gets the total price of the order
        /// </summary>
        public decimal TotalPrice => Items.Sum(i => i.Key.Price * i.Value);

        /// <summary>
        /// Creates a new order with the specified details
        /// </summary>
        /// <param name="customer">The customer who placed the order</param>
        /// <param name="restaurant">The restaurant the order is from</param>
        public Order(Customer customer, Restaurant restaurant)
        {
            Id = nextId++;
            Customer = customer;
            Restaurant = restaurant;
            Status = OrderStatus.Ordered;
            Items = new Dictionary<MenuItem, int>();

            customer.AddOrder(this);
            restaurant.AddOrder(this);
        }

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
            else
            {
                Items[item] = quantity;
            }
        }

        /// <summary>
        /// Sets the status of the order
        /// </summary>
        /// <param name="status">The new status</param>
        public void SetStatus(OrderStatus status)
        {
            Status = status;

            if (status == OrderStatus.Delivered)
            {
                Restaurant.RemoveOrder(this);
            }
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
                { "Status", Status.ToString() },
                { "Restaurant", Restaurant.Name },
                { "Customer", Customer.Name },
                { "Total Price", $"${TotalPrice:F2}" }
            };

            if (Deliverer != null)
            {
                info.Add("Deliverer", Deliverer.Name);
                info.Add("Licence Plate", Deliverer.LicencePlate);
            }

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