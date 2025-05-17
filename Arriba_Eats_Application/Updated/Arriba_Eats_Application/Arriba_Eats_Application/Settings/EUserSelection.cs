using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents the types of food styles available for restaurants
    /// </summary>
    public enum FoodStyle
    {
        Italian = 1,
        French = 2,
        Chinese = 3,
        Japanese = 4,
        American = 5,
        Australian = 6
    }

    /// <summary>
    /// Represents the possible statuses of an order in the system
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>Order has been placed but not yet processed</summary>
        Ordered,

        /// <summary>Order is currently being cooked</summary>
        Cooking,

        /// <summary>Order has been cooked but not yet picked up</summary>
        Cooked,

        /// <summary>Order is on its way to the customer</summary>
        BeingDelivered,

        /// <summary>Order has been delivered to the customer</summary>
        Delivered
    }

    /// <summary>
    /// Represents the type of user in the system
    /// </summary>
    public enum UserType
    {
        /// <summary>User is a customer who orders food</summary>
        Customer,

        /// <summary>User is a deliverer who delivers food</summary>
        Deliverer,

        /// <summary>User is a client who owns a restaurant</summary>
        Client
    }
}