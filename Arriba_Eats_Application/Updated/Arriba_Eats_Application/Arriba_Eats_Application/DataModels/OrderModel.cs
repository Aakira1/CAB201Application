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
        #region Properties

        /// <summary>
        /// Gets or sets the order ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the customer who placed the order
        /// </summary>
        public Customer Customer { get; set; }
        /// <summary>
        /// Gets or sets the restaurant from which the order was placed
        /// </summary>
        public Restaurant Restaurant { get; set; }
        /// <summary>
        /// Gets or sets the deliverer assigned to the order
        /// </summary>
        public Deliverer? Deliverer { get; set; }
        /// <summary>
        /// Gets or sets the status of the order
        /// </summary>
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        /// <summary>
        /// Gets or sets the items in the order
        /// </summary>
        public Dictionary<MenuItem, int> Items { get; set; } = new();
        /// <summary>
        /// Gets or sets the list of menu items available in the restaurant
        /// </summary>
        public List<MenuItem> items { get; set; } = new();

        /// <summary>
        /// Gets the order details for the visible menu
        /// </summary>
        public Order VisibleMenu => new Order(Customer, Restaurant, Id)
        {
            Id = Id,
            Customer = Customer,
            Restaurant = Restaurant,
            Deliverer = Deliverer,
            Status = Status,
            Items = Items.ToDictionary(item => item.Key, item => item.Value)
        };

        /// <summary>
        /// Gets the total price of the order
        /// </summary>
        public decimal TotalPrice => Items.Sum(item => item.Value * item.Key.Price);

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new order with the specified customer and restaurant
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="restaurant"></param>
        /// <param name="id"></param>
        public Order(Customer customer, Restaurant restaurant, int id)
        {
            Id = id;
            Customer = customer;
            Restaurant = restaurant;
        }

        #endregion

        #region Order Actions

        /// <summary>
        /// Adds an item to the order
        /// </summary>
        /// <param name="item"></param>
        /// <param name="quantity"></param>
        public void AddItem(MenuItem item, int quantity)
        {
            if (quantity <= 0) return;

            if (Items.ContainsKey(item))
                Items[item] += quantity;
            else
                Items[item] = quantity;
        }
        /// <summary>
        /// Removes an item from the order
        /// </summary>
        /// <param name="newStatus"></param>
        public void SetStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void AssignDeliverer(Deliverer deliverer)
        {
            Deliverer = deliverer;
        }
        #endregion

        #region Utility
        /// <summary>
        /// Gets the order information as a dictionary of property names and values
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetOrderInfo()
        {
            return new Dictionary<string, string>
            {
                { "Order ID", Id.ToString() },
                { "Customer", Customer.Name },
                { "Restaurant", Restaurant.Name },
                { "Status", Status.ToString() },
                { "Total Price", $"${TotalPrice:F2}" }
            };
        }

        /// <summary>
        /// Gets the order items as a formatted string
        /// </summary>
        /// <returns></returns>
        public string GetItemsAsString()
        {
            return string.Join("\n", Items.Select(i => $"{i.Value}x {i.Key.Name} - ${i.Key.Price:F2} each = ${i.Value * i.Key.Price:F2}"));
        }

        #endregion
    }
}