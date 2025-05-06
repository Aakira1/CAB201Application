using Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Arriba_Eats_App.Data
{
	/// <summary>
	/// Static in-memory data store for the Arriba Eats application
	/// </summary>
	public static class InMemoryDataStore
	{
		#region Data Collections
		private static readonly List<User> _users = new();
		private static readonly List<DeliveryPerson> _deliveryPersons = new();
		private static readonly List<RestaurantOwner> _restaurantOwners = new();
		private static readonly List<Restaurant> _restaurants = new();
		private static readonly List<MenuItem> _menuItems = new();
		private static readonly List<Order> _orders = new();
		private static readonly List<Rating> _ratings = new();
		#endregion

		#region User Management
		public static IReadOnlyList<User> Users => _users.AsReadOnly();

		public static void AddUser(User user)
		{
			if (user.Id == Guid.Empty)
				user.Id = Guid.NewGuid();
			_users.Add(user);
		}

		public static User? GetUserById(Guid id) =>
			_users.FirstOrDefault(u => u.Id == id);

		public static User? GetUserByUsername(string username) =>
			_users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

		public static void UpdateUser(User user)
		{
			var index = _users.FindIndex(u => u.Id == user.Id);
			if (index >= 0)
				_users[index] = user;
		}

		public static bool RemoveUser(Guid id)
		{
			var user = GetUserById(id);
			return user != null && _users.Remove(user);
		}

		public static bool CheckUsernameExists(string username)
		{
			return _users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
		}

		public static bool CheckEmailExists(string email)
		{
			return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
		}
		#endregion

		#region Delivery Person Management
		public static IReadOnlyList<DeliveryPerson> DeliveryPersons => _deliveryPersons.AsReadOnly();

		public static void AddDeliveryPerson(DeliveryPerson deliveryPerson)
		{
			if (deliveryPerson.Id == Guid.Empty)
				deliveryPerson.Id = Guid.NewGuid();
			_deliveryPersons.Add(deliveryPerson);
		}

		public static DeliveryPerson? GetDeliveryPersonById(Guid id) =>
			_deliveryPersons.FirstOrDefault(d => d.Id == id);

		public static List<DeliveryPerson> GetAvailableDeliveryPersons() =>
			_deliveryPersons.Where(d => d.CurrentOrder == null).ToList();

		public static void UpdateDeliveryPerson(DeliveryPerson deliveryPerson)
		{
			var index = _deliveryPersons.FindIndex(d => d.Id == deliveryPerson.Id);
			if (index >= 0)
				_deliveryPersons[index] = deliveryPerson;
		}

		public static bool RemoveDeliveryPerson(Guid id)
		{
			var deliveryPerson = GetDeliveryPersonById(id);
			return deliveryPerson != null && _deliveryPersons.Remove(deliveryPerson);
		}

		#endregion

		#region Restaurant Management
		public static IReadOnlyList<Restaurant> Restaurants => _restaurants.AsReadOnly();
		public static IReadOnlyList<RestaurantOwner> RestaurantOwners => _restaurantOwners.AsReadOnly();

		public static void AddRestaurant(Restaurant restaurant)
		{
			if (restaurant.Id == Guid.Empty)
				restaurant.Id = Guid.NewGuid();
			_restaurants.Add(restaurant);
		}

		public static void AddRestaurantOwner(RestaurantOwner owner)
		{
			if (owner.Id == Guid.Empty)
				owner.Id = Guid.NewGuid();
			_restaurantOwners.Add(owner);
		}

		public static Restaurant? GetRestaurantById(Guid id) =>
			_restaurants.FirstOrDefault(r => r.Id == id);

		public static RestaurantOwner? GetRestaurantOwnerById(Guid id) =>
			_restaurantOwners.FirstOrDefault(r => r.Id == id);

		public static RestaurantOwner? GetRestaurantOwnerByRestaurantId(Guid restaurantId) =>
			_restaurantOwners.FirstOrDefault(r => r.Restaurant.Id == restaurantId);

		public static void UpdateRestaurant(Restaurant restaurant)
		{
			var index = _restaurants.FindIndex(r => r.Id == restaurant.Id);
			if (index >= 0)
				_restaurants[index] = restaurant;
		}

		public static bool RemoveRestaurant(Guid id)
		{
			var restaurant = GetRestaurantById(id);
			return restaurant != null && _restaurants.Remove(restaurant);
		}
		#endregion

		#region Menu Item Management
		public static IReadOnlyList<MenuItem> MenuItems => _menuItems.AsReadOnly();

		public static void AddMenuItem(MenuItem menuItem)
		{
			if (menuItem.Id == Guid.Empty)
				menuItem.Id = Guid.NewGuid();
			_menuItems.Add(menuItem);
		}

		public static MenuItem? GetMenuItemById(Guid id) =>
			_menuItems.FirstOrDefault(m => m.Id == id);

		public static List<MenuItem> GetMenuItemsByRestaurant(Guid restaurantId)
		{
			var restaurant = GetRestaurantById(restaurantId);
			return restaurant?.MenuItems ?? new List<MenuItem>();
		}

		public static void UpdateMenuItem(MenuItem menuItem)
		{
			var index = _menuItems.FindIndex(m => m.Id == menuItem.Id);
			if (index >= 0)
				_menuItems[index] = menuItem;
		}

		public static bool RemoveMenuItem(Guid id)
		{
			var menuItem = GetMenuItemById(id);
			return menuItem != null && _menuItems.Remove(menuItem);
		}
		#endregion

		#region Order Management
		public static IReadOnlyList<Order> Orders => _orders.AsReadOnly();

		public static void AddOrder(Order order)
		{
			if (order.Id == Guid.Empty)
				order.Id = Guid.NewGuid();
			_orders.Add(order);

			// Update customer's order history
			order.Customer.OrderHistory.Add(order);
		}

		public static Order? GetOrderById(Guid id) =>
			_orders.FirstOrDefault(o => o.Id == id);

		public static List<Order> GetOrdersByCustomer(Guid customerId) =>
			_orders.Where(o => o.Customer.Id == customerId).ToList();

		public static List<Order> GetOrdersByRestaurant(Guid restaurantId) =>
			_orders.Where(o => o.Restaurant.Id == restaurantId).ToList();

		public static List<Order> GetOrdersByDeliveryPerson(Guid deliveryPersonId) =>
			_orders.Where(o => o.DeliveryPerson?.Id == deliveryPersonId).ToList();

		public static List<Order> GetActiveOrders() =>
			_orders.Where(o => o.Status != EOrderStatus.Delivered &&
							  o.Status != EOrderStatus.Cancelled).ToList();

		public static void UpdateOrder(Order order)
		{
			var index = _orders.FindIndex(o => o.Id == order.Id);
			if (index >= 0)
				_orders[index] = order;
		}

		public static bool RemoveOrder(Guid id)
		{
			var order = GetOrderById(id);
			return order != null && _orders.Remove(order);
		}

		#endregion

		#region Rating Management
		public static IReadOnlyList<Rating> Ratings => _ratings.AsReadOnly();

		public static void AddRating(Rating rating)
		{
			if (rating.Id == Guid.Empty)
				rating.Id = Guid.NewGuid();
			_ratings.Add(rating);
		}

		public static List<Rating> GetRatingsByRestaurant(Guid restaurantId)
		{
			var restaurant = GetRestaurantById(restaurantId);
			return restaurant?.Ratings ?? new List<Rating>();
		}

		public static List<Rating> GetRatingsByMenuItem(Guid menuItemId)
		{
			var menuItem = GetMenuItemById(menuItemId);
			return menuItem?.Ratings ?? new List<Rating>();
		}

		public static List<Rating> GetRatingsByUser(Guid userId) =>
			_ratings.Where(r => r.Customer.Id == userId).ToList();
		#endregion

		#region Global Operations
		/// <summary>
		/// Clears all data from memory
		/// </summary>
		public static void ClearAllData()
		{
			_users.Clear();
			_deliveryPersons.Clear();
			_restaurantOwners.Clear();
			_restaurants.Clear();
			_menuItems.Clear();
			_orders.Clear();
			_ratings.Clear();
		}

		/// <summary>
		/// Seeds the data store with sample data
		/// </summary>
		public static void SeedSampleData()
		{
			// Implementation for seeding sample data would go here
			// This would create test users, restaurants, menu items, etc.

		}
		#endregion
	}
}