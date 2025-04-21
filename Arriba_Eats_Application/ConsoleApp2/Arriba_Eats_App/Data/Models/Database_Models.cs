using Arriba_Eats_App.Arriba_Eats_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Arriba_Eats_App.Data.Models
{
	/// <summary>
	/// This namespace contains the data models for the Arriba Eats application.
	/// </summary>
	/// <remarks>
	/// The data models are used to represent the data structures used in the application.
	/// </remarks>
	/// <author>Ayden Beggs</author> 
	/// <date>19/04/2025</date>
	/// <version>1.0</version>

	#region Data Models

	/// <summary>
	/// UserData class is the base class for all user-related data in the application.
	/// </summary>
	public abstract class UserData
	{
		public Guid UUID { get; set; }
		public string Name { get; set; }
		public string Age { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string MobileNumber { get; set; }
		public string Password { get; set; }
	}

	/// <summary>
	/// Admin class represents an administrator in the system, including their name, email, and mobile number.
	/// </summary>
	public class Customer : UserData
	{
		public Location DeliveryLocation { get; set; }
		public List<Order> OrderHistory { get; set; } = new List<Order>();
		public decimal WalletBalance { get; set; }
		public decimal TotalSpent { get; set; }
	}

	/// <summary>
	/// DeliveryService class represents a delivery person in the system, including their vehicle type, license plate, and current location.
	/// </summary>
	public class DeliveryService : UserData
	{
		public string VehicleType { get; set; }
		public string LicensePlate { get; set; }
		public string VehicleColor { get; set; }
		public Location CurrentLocation { get; set; }
		public Order CurrentOrderStatus { get; set; }
	}

	public class Client : UserData
	{
		public Restaraunt Restaraunt { get; set; }
	}


	#endregion

	#region Class Definitions & Enums
	// ----------------------------------------------------------- Object class for additional user data ----------------------------------------------------------- //
	/// <summary>
	/// MenuItem class represents a menu item in a restaurant, including its name, price, and UUID.
	/// </summary>
	public class MenuItem
	{
		public Guid UUID { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
	}

	/// <summary>
	/// Restaraunt class represents a restaurant, including its name, address, phone number, location, menu items, and ratings.
	/// </summary>
	public class Restaraunt
	{
		public Guid UUID { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public Location Location { get; set; }
		public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
		public List<Ratings> ItemRatings { get; set; } = new List<Ratings>();
		//public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Stars) : 0;
	}

	/// <summary>
	/// Order class represents a customer's order, including the customer, restaurant, delivery person, and order items.
	/// </summary>
	public class Order
	{
		public Guid UUID { get; set; }
		public Customer Customer { get; set; }
		public Restaraunt Restaraunt { get; set; }
		public DeliveryService DeliveryPerson { get; set; }
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
		public OrderStatus Status { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal TotalAmount => OrderItems.Sum(item => item.MenuItem.Price * item.Quantity); // Calculate total amount based on order items and their quantities

	}

	/// <summary>
	/// OrderItem class represents an item in an order, including the menu item and its quantity.
	/// </summary>
	public class OrderItem
	{
		public MenuItem MenuItem { get; set; }
		public int Quantity { get; set; }
	}

	/// <summary>
	/// Ratings class represents a customer's rating and review for a menu item.
	/// </summary>
	public class Ratings
	{
		public Guid CustomerID { get; set; }
		public string RatingStars { get; set; }
		public int Stars { get; set; }
		public string Review { get; set; }
		public DateTime DatePosted { get; set; }
	}

	/// <summary>
	/// Location class represents a geographical location with latitude and longitude.
	/// </summary>
	public class Location
	{
		public double X { get; set; } // latitude
		public double Y { get; set; } // longitude


		public double DistanceTo(Location other)
		{
			return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2)); // Euclidean distance, not accurate for real-world distances      --*note: likely to be restructed but currently fits the projects requirements.*--
		}
	}

	/// <summary>
	/// OrderStatus enum represents the status of an order in the system.
	/// </summary>
	public enum OrderStatus
	{
		Ordered,
		Preparing,
		Cooking,
		OutForDelivery,
		Delivered
	}

	#endregion
}

