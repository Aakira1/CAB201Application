using System;
using System.Collections.Generic;
using System.Linq;

namespace Arriba_Eats_App.Data.Models
{
	#region Base Classes
	/// <summary>
	/// Base class for all user-related data in the application
	/// </summary>
	public abstract class UserData
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public required string Name { get; set; }
		public required string Age { get; set; }
		public required string Address { get; set; }
		public required string Email { get; set; }
		public required string MobileNumber { get; set; }
		public required string Username { get; set; }
		public required string Password { get; set; } // Should be stored as hash in production
		public UserType UserType { get; set; }
	}

	/// <summary>
	/// Base class for all entities with unique identifier
	/// </summary>
	public abstract class EntityBase
	{
		public Guid Id { get; set; } = Guid.NewGuid();
	}
	#endregion

	#region User Types
	/// <summary>
	/// Standard user/customer account
	/// </summary>
	public class User : UserData
	{
		public required Location DeliveryLocation { get; set; }
		public List<Order> OrderHistory { get; set; } = new();
		public decimal WalletBalance { get; set; }
		public decimal TotalSpent { get; set; }
	}

	/// <summary>
	/// Delivery person account with vehicle details
	/// </summary>
	public class DeliveryPerson : UserData
	{
		public required string VehicleType { get; set; }
		public required string LicensePlate { get; set; }
		public required string VehicleColor { get; set; }
		public required Location CurrentLocation { get; set; }
		public Order? CurrentOrder { get; set; }
		public List<Order> CompletedDeliveries { get; set; } = new();
	}

	/// <summary>
	/// Restaurant owner/manager account
	/// </summary>
	public class RestaurantOwner : UserData
	{
		public required Restaurant Restaurant { get; set; }
	}
	#endregion

	#region Business Entities
	/// <summary>
	/// Restaurant with menu and location information
	/// </summary>
	public class Restaurant : EntityBase
	{
		public required string Name { get; set; }
		public required string Address { get; set; }
		public required string PhoneNumber { get; set; }
		public required Location Location { get; set; }
		public List<MenuItem> MenuItems { get; set; } = new();
		public List<Rating> Ratings { get; set; } = new();

		// Calculated property for average rating
		public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Stars) : 0;
	}

	/// <summary>
	/// Menu item with name, description and price
	/// </summary>
	public class MenuItem : EntityBase
	{
		public required string Name { get; set; }
		public string? Description { get; set; }
		public required decimal Price { get; set; }
		public string? ImageUrl { get; set; }
		public List<Rating> Ratings { get; set; } = new();

		// Calculated property for average rating
		public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Stars) : 0;
	}

	/// <summary>
	/// Customer order with items, status and tracking
	/// </summary>
	public class Order : EntityBase
	{
		public required User Customer { get; set; }
		public required Restaurant Restaurant { get; set; }
		public DeliveryPerson? DeliveryPerson { get; set; }
		public List<OrderItem> Items { get; set; } = new();
		public OrderStatus Status { get; set; } = OrderStatus.Placed;
		public DateTime OrderDate { get; set; } = DateTime.Now;
		public DateTime? DeliveredDate { get; set; }

		// Calculated properties
		public decimal Subtotal => Items.Sum(item => item.MenuItem.Price * item.Quantity);
		public decimal DeliveryFee { get; set; } = 2.99m;
		public decimal Tax => Subtotal * 0.0875m; // Example 8.75% tax
		public decimal TotalAmount => Subtotal + DeliveryFee + Tax;
	}

	/// <summary>
	/// Item in an order with quantity and special instructions
	/// </summary>
	public class OrderItem : EntityBase
	{
		public required MenuItem MenuItem { get; set; }
		public int Quantity { get; set; } = 1;
		public string? SpecialInstructions { get; set; }
		public decimal ItemTotal => MenuItem.Price * Quantity;
	}

	/// <summary>
	/// Rating for restaurant or menu item
	/// </summary>
	public class Rating : EntityBase
	{
		public required User Customer { get; set; }
		public int Stars { get; set; } // 1-5 star rating
		public string? Review { get; set; }
		public DateTime DatePosted { get; set; } = DateTime.Now;
	}

	/// <summary>
	/// Geographic location with latitude and longitude
	/// </summary>
	public class Location
	{
		public Location(double latitude, double longitude)
		{
			Latitude = latitude;
			Longitude = longitude;
		}

		public double Latitude { get; set; }
		public double Longitude { get; set; }

		/// <summary>
		/// Calculate simple Euclidean distance to another location
		/// </summary>
		/// <remarks>
		/// This is not accurate for real-world distances and should be replaced
		/// with actual geolocation calculations in production
		/// </remarks>
		public double DistanceTo(Location other)
		{
			return Math.Sqrt(Math.Pow(Latitude - other.Latitude, 2) +
						   Math.Pow(Longitude - other.Longitude, 2));
		}

		public override string ToString() => $"({Latitude}, {Longitude})";
	}
	#endregion

	#region Enums
	/// <summary>
	/// Status of an order in the system
	/// </summary>
	public enum OrderStatus
	{
		Placed,
		Confirmed,
		Preparing,
		ReadyForPickup,
		OutForDelivery,
		Delivered,
		Cancelled
	}

	/// <summary>
	/// Type of user in the system
	/// </summary>
	public enum UserType
	{
		Customer,
		DeliveryPerson,
		RestaurantOwner,
		Administrator
	}
	#endregion
}