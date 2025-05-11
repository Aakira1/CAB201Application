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

		//public required string Username { get; set; } // optional
		public required string Password { get; set; } // Should be stored as hash in production
		public EUserType UserType { get; set; }
	}

	/// <summary>
	/// Base class for all entities with unique identifier
	/// </summary>
	public abstract class EntityBase : UserData
	{
		private Guid id = Guid.NewGuid();

		public Guid Id { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
	}
	#endregion

	#region User Types
	/// <summary>
	/// Standard user/customer account
	/// </summary>
	public class User : EntityBase
	{
		public required Location DeliveryLocation { get; set; }
		public List<Order> OrderHistory { get; set; } = new();
		public decimal WalletBalance { get; set; }
		public decimal TotalSpent { get; set; }
	}

	/// <summary>
	/// Delivery person account with vehicle details
	/// </summary>
	public class DeliveryPerson : EntityBase
	{
		public required string VehicleType { get; set; }
		public required string LicensePlate { get; set; }
		public required string VehicleColor { get; set; }
		public required Location CurrentLocation { get; set; }
		public List<Order> CurrentOrder { get; set; } = new();
		public List<Order> CompletedDeliveries { get; set; } = new();
	}

	/// <summary>
	/// Restaurant owner/manager account
	/// </summary>
	/// <summary>
	/// Restaurant with menu and location information
	/// </summary>
	public class Restaurant : EntityBase
	{
		public required string RestaurantName { get; set; } = string.Empty;
		public required string PhoneNumber { get; set; }
		public required string CuisineType { get; set; }
		public required Location Location { get; set; } = new(0, 0);
		public List<MenuItem> MenuItems { get; set; } = new();
		public List<Order> Orders { get; set; } = new();
		public List<Rating> Ratings { get; set; } = new();
		public decimal AverageRating { get; set; } = 0;
		public int TotalRatings { get; set; } = 0;
		public int TotalOrders { get; set; } = 0;
		public int TotalDeliveries { get; set; } = 0;
		public int TotalEarnings { get; set; } = 0;
		public int TotalCustomers { get; set; } = 0;
		public int TotalMenuItems { get; set; } = 0;
	}
	#endregion

	#region Business Entities
	public class MenuItem : EntityBase
	{
		public required User Customer { get; set; }
		public required string Category { get; set; }
		public required string Ingredients { get; set; }
		public required string PreparationTime { get; set; }
		public required string ServingSize { get; set; }
		public required bool IsAvailable { get; set; } = true;
		public required int Stars { get; set; } = 0;
		public List<Rating>? Ratings { get; internal set; }
	}

	/// <summary>
	/// Customer order with items, status and tracking
	/// </summary>
	public class Order : EntityBase
	{
		public required User Customer { get; set; }
		public required DeliveryPerson DeliveryPerson { get; set; }
		public required Guid OrderId { get; set; } = Guid.NewGuid();
		public required string OrderNumber { get; set; } = string.Empty;
		public required string DeliveryAddress { get; set; } = string.Empty;
		public required Location DeliveryLocation { get; set; } = new(0, 0);
		public required string SpecialInstructions { get; set; } = string.Empty;
		public required string PaymentMethod { get; set; } = "Credit Card";
		public required string OrderStatus { get; set; } = "Pending";
		public required DateTime EstimatedDeliveryTime { get; set; }
		public required DateTime ActualDeliveryTime { get; set; }
		public required Restaurant Restaurant { get; set; }
		public required EOrderStatus Status { get; set; }
		public DateTime OrderDate { get; internal set; }
		public float TotalAmount { get; internal set; }
	}

	/// <summary>
	/// Rating for restaurant or menu item
	/// </summary>
	public class Rating : EntityBase
	{
		public required Restaurant Restaurant { get; set; }
		public required MenuItem MenuItem { get; set; }
		public required DeliveryPerson DeliveryPerson { get; set; }
		public required Order Order { get; set; }
		public required User Customer { get; set; }
		public required Location DeliveryLocation { get; set; } = new(0, 0);
		public required string SpecialInstructions { get; set; } = string.Empty;
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
	public enum EOrderStatus
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
	public enum EUserType
	{
		None = 0,
		Customer = 1,
		DeliveryPerson = 2,
		RestaurantOwner = 3,
		Administrator = 4
	}
	#endregion
}