using System;
using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a deliverer in the system
    /// </summary>
    public class Deliverer : User
    {
        #region Properties
        /// <summary>
        /// Gets or sets the deliverer's licence plate
        /// </summary>
        public string LicencePlate { get; set; }
        public Order CurrentOrder { get; set; }
        public Location CurrentLocation { get; set; } = new(0, 0);
        public Deliverer deliverer { get; set; } // Not needed unless used for something specific

        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new deliverer with the specified details
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="password"></param>
        /// <param name="licencePlate"></param>
        public Deliverer(string name, int age, string email, string mobile, string password, string licencePlate)
            : base(name, age, email, mobile, password)
        {
            LicencePlate = licencePlate;
        }

        #endregion

        #region Order Interaction
        /// <summary>
        /// Accepts an order for delivery
        /// </summary>
        /// <param name="order"></param>
        /// <param name="deliverer"></param>
        public void AcceptOrder(Order order, Deliverer deliverer)
        {
            if (order.Status == OrderStatus.Cooked && order.Deliverer == null)
            {
                order.Deliverer = deliverer;
                deliverer.CurrentOrder = order;
                // Optionally, update order status here or leave it as Cooked until client sets BeingDelivered
                order.SetStatus(OrderStatus.BeingDelivered);
            }
        }

        /// <summary>
        /// Sets the current location of the deliverer
        /// </summary>
        public void ArriveAtRestaurant()
        {
            if (CurrentOrder != null)
            {
                CurrentLocation = CurrentOrder.Restaurant.Location;
            }
        }

        /// <summary>
        /// Completes the delivery of the current order
        /// </summary>
        public void CompleteDelivery()
        {
            if (CurrentOrder != null)
            {
                CurrentLocation = CurrentOrder.Customer.Location;
                CurrentOrder.SetStatus(OrderStatus.Delivered);
                CurrentOrder = null;
            }
        }

        /// <summary>
        /// Calculates the total distance for the order delivery
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public int CalculateTotalDistance(Order order)
        {
            int toRestaurant = CurrentLocation.DistanceTo(order.Restaurant.Location);
            int toCustomer = order.Restaurant.Location.DistanceTo(order.Customer.Location);
            return toRestaurant + toCustomer;
        }

        #endregion

        #region Display Methods

        public void GetOrderDetails()
        {
            if (CurrentOrder != null)
            {
                Console.WriteLine($"Order ID: {CurrentOrder.Id}");
                Console.WriteLine($"Restaurant: {CurrentOrder.Restaurant.Name}");
                Console.WriteLine($"Customer: {CurrentOrder.Customer.Name}");
                Console.WriteLine($"Total Price: {CurrentOrder.TotalPrice:C}");
            }
            else
            {
                Console.WriteLine("No current order.");
            }
        }

        public override Dictionary<string, string> GetUserInfo()
        {
            var info = base.GetUserInfo();
            info.Add("Licence plate", LicencePlate);

            if (CurrentOrder != null)
            {
                info.Add("Current Order ID", CurrentOrder.Id.ToString());
                info.Add("Restaurant", $"{CurrentOrder.Restaurant.Name} at {CurrentOrder.Restaurant.Location}");
                info.Add("Customer", $"{CurrentOrder.Customer.Name} at {CurrentOrder.Customer.Location}");
            }
            else
            {
                info.Add("Current Status", "Not currently delivering an order");
            }

            return info;
        }

        #endregion
    }
}