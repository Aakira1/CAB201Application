using System;
using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a deliverer in the system
    /// </summary>
    public class Deliverer : User
    {
        /// <summary>
        /// Gets or sets the licence plate of the deliverer's vehicle
        /// </summary>
        public string LicencePlate { get; set; }

        /// <summary>
        /// Gets or sets the current order being delivered
        /// </summary>
        public Order CurrentOrder { get; private set; }

        /// <summary>
        /// Gets or sets the current location of the deliverer
        /// </summary>
        public Location CurrentLocation { get; set; }

        /// <summary>
        /// Creates a new deliverer with the specified details
        /// </summary>
        /// <param name="name">The name of the deliverer</param>
        /// <param name="age">The age of the deliverer</param>
        /// <param name="email">The email address of the deliverer</param>
        /// <param name="mobile">The mobile number of the deliverer</param>
        /// <param name="password">The password of the deliverer</param>
        /// <param name="licencePlate">The licence plate of the deliverer's vehicle</param>
        public Deliverer(string name, int age, string email, string mobile, string password, string licencePlate)
            : base(name, age, email, mobile, password)
        {
            LicencePlate = licencePlate;
            CurrentOrder = null;
            // Initialize at default location (0,0)
            CurrentLocation = new Location(0, 0);
        }

        /// <summary>
        /// Calculates the total distance for delivering an order
        /// </summary>
        /// <param name="order">The order to deliver</param>
        /// <returns>The total distance to deliver the order</returns>
        public int CalculateTotalDistance(Order order)
        {
            int toRestaurant = CurrentLocation.DistanceTo(order.Restaurant.Location);
            int toCustomer = order.Restaurant.Location.DistanceTo(order.Customer.Location);
            return toRestaurant + toCustomer;
        }

        /// <summary>
        /// Accepts an order for delivery
        /// </summary>
        /// <param name="order">The order to accept</param>
        public void AcceptOrder(Order order)
        {
            CurrentOrder = order;
            order.AssignDeliverer(this);
        }

        /// <summary>
        /// Sets the deliverer's status as at the restaurant
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
        /// Gets the deliverer information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of deliverer information</returns>
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
    }
}