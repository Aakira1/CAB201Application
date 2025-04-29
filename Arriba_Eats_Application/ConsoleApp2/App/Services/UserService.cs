using System;
using System.Collections.Generic;
using System.Linq;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data;

namespace Arriba_Eats_App.Services
{
	/// <summary>
	/// Service for managing user accounts in the system
	/// </summary>
	public class UserService
	{
		/// <summary>
		/// Registers a new user in the system
		/// </summary>
		/// <param name="user">User data to register</param>
		/// <returns>True if registration was successful</returns>
		public bool RegisterUser(User user)
		{
			// Validate user data
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return false;
			}

			if (string.IsNullOrEmpty(user.Name) ||
				string.IsNullOrEmpty(user.Email) ||
				string.IsNullOrEmpty(user.MobileNumber) ||
				string.IsNullOrEmpty(user.Age) ||
				string.IsNullOrEmpty(user.Password) ||
				string.IsNullOrEmpty(user.Username) ||
				user.DeliveryLocation == null)
			{
				Console.WriteLine("User details are incomplete");
				return false;
			}

			// Check if username already exists
			if (InMemoryDataStore.GetUserByUsername(user.Username) != null)
			{
				Console.WriteLine("Username already exists");
				return false;
			}

			// Set user type and ID if not set
			user.UserType = UserType.Customer;
			if (user.Id == Guid.Empty)
				user.Id = Guid.NewGuid();

			// Add user to data store
			InMemoryDataStore.AddUser(user);
			Console.WriteLine($"You have been successfully registered as a customer, {user.Name}!\n");
			return true;
		}

		/// <summary>
		/// Updates an existing user in the system
		/// </summary>
		/// <param name="user">User data with updates</param>
		/// <returns>True if update was successful</returns>
		public bool UpdateUser(User user)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return false;
			}

			if (string.IsNullOrEmpty(user.Name) ||
				string.IsNullOrEmpty(user.Email) ||
				string.IsNullOrEmpty(user.MobileNumber))
			{
				Console.WriteLine("User details are incomplete");
				return false;
			}

			// Check if user exists
			var existingUser = InMemoryDataStore.GetUserById(user.Id);
			if (existingUser == null)
			{
				Console.WriteLine("User not found");
				return false;
			}

			// Update user
			InMemoryDataStore.UpdateUser(user);
			Console.WriteLine("User updated successfully");
			return true;
		}

		/// <summary>
		/// Deletes a user from the system
		/// </summary>
		/// <param name="id">ID of the user to delete</param>
		/// <returns>True if deletion was successful</returns>
		public bool DeleteUser(Guid id)
		{
			if (id == Guid.Empty)
			{
				Console.WriteLine("User ID cannot be empty");
				return false;
			}

			// Check if user exists
			var user = InMemoryDataStore.GetUserById(id);
			if (user == null)
			{
				Console.WriteLine("User not found");
				return false;
			}

			// Delete user
			bool result = InMemoryDataStore.RemoveUser(id);
			if (result)
				Console.WriteLine("User deleted successfully");
			else
				Console.WriteLine("Failed to delete user");

			return result;
		}

		/// <summary>
		/// Retrieves a user from the system by ID
		/// </summary>
		/// <param name="id">ID of the user to retrieve</param>
		/// <returns>User data or null if not found</returns>
		public User GetUserById(Guid id)
		{
			if (id == Guid.Empty)
			{
				Console.WriteLine("User ID cannot be empty");
				return null;
			}

			var user = InMemoryDataStore.GetUserById(id);
			if (user == null)
			{
				Console.WriteLine("User not found");
				return null;
			}

			Console.WriteLine("User found");
			return user;
		}

		/// <summary>
		/// Retrieves all users from the system
		/// </summary>
		/// <returns>List of all users</returns>
		public List<User> GetAllUsers()
		{
			var users = InMemoryDataStore.Users.ToList();
			if (users.Count == 0)
			{
				Console.WriteLine("No users found");
			}
			else
			{
				Console.WriteLine($"Found {users.Count} users");
			}

			return users;
		}

		/// <summary>
		/// Finds users by name, mobile or email
		/// </summary>
		/// <param name="name">Optional name filter</param>
		/// <param name="mobile">Optional mobile filter</param>
		/// <param name="email">Optional email filter</param>
		/// <returns>List of matching users</returns>
		public List<User> FindUsers(string name = null, string mobile = null, string email = null)
		{
			var users = InMemoryDataStore.Users.ToList();

			// Apply filters if provided
			if (!string.IsNullOrEmpty(name))
				users = users.Where(u => u.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

			if (!string.IsNullOrEmpty(mobile))
				users = users.Where(u => u.MobileNumber.Contains(mobile)).ToList();

			if (!string.IsNullOrEmpty(email))
				users = users.Where(u => u.Email.Contains(email, StringComparison.OrdinalIgnoreCase)).ToList();

			if (users.Count == 0)
			{
				Console.WriteLine("No matching users found");
			}
			else
			{
				Console.WriteLine($"Found {users.Count} matching users");
			}

			return users;
		}

		/// <summary>
		/// Validates user login credentials
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>True if credentials are valid</returns>
		public bool ValidateLogin(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				Console.WriteLine("Username or password cannot be empty");
				return false;
			}

			var user = InMemoryDataStore.GetUserByUsername(username);
			if (user == null)
			{
				Console.WriteLine("Invalid username or password");
				return false;
			}

			// In a real application, you would use a secure password comparison
			// method like PasswordHasher or similar, not plain text comparison
			if (user.Password == password)
			{
				Console.WriteLine("Login successful");
				return true;
			}

			Console.WriteLine("Invalid username or password");
			return false;
		}

		/// <summary>
		/// Gets specific user details by property name
		/// </summary>
		/// <param name="user">User to get details from</param>
		/// <param name="propertyName">Name of the property to retrieve</param>
		/// <returns>The property value as a string</returns>
		public string GetUserDetail(User user, string propertyName)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return "User cannot be null";
			}

			return GetUserPropertyValue(user, propertyName);
		}

		/// <summary>
		/// Updates a specific user property
		/// </summary>
		/// <param name="user">User to update</param>
		/// <param name="propertyName">Name of the property to update</param>
		/// <param name="newValue">New value for the property</param>
		/// <returns>True if update was successful</returns>
		public bool UpdateUserDetail(User user, string propertyName, string newValue)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return false;
			}

			bool result = SetUserPropertyValue(user, propertyName, newValue);
			if (result)
			{
				InMemoryDataStore.UpdateUser(user);
			}

			return result;
		}

		#region Private Methods
		/// <summary>
		/// Gets a property value from a user by property name
		/// </summary>
		private string GetUserPropertyValue(User user, string propertyName)
		{
			if (user == null)
			{
				return "User cannot be null";
			}

			switch (propertyName.ToLower())
			{
				case "name":
					return user.Name;
				case "email":
					return user.Email;
				case "mobilenumber":
					return user.MobileNumber;
				case "address":
					return user.Address;
				case "id":
					return user.Id.ToString();
				case "age":
					return user.Age;
				case "username":
					return user.Username;
				case "usertype":
					return user.UserType.ToString();
				case "walletbalance":
					return user.WalletBalance.ToString("C2");
				case "totalspent":
					return user.TotalSpent.ToString("C2");
				case "deliverylocation":
					return user.DeliveryLocation?.ToString() ?? "No location set";
				default:
					Console.WriteLine("Invalid property name");
					return "Invalid property name";
			}
		}

		/// <summary>
		/// Sets a property value on a user by property name
		/// </summary>
		private bool SetUserPropertyValue(User user, string propertyName, string newValue)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return false;
			}

			switch (propertyName.ToLower())
			{
				case "name":
					user.Name = newValue;
					break;
				case "email":
					user.Email = newValue;
					break;
				case "mobilenumber":
					user.MobileNumber = newValue;
					break;
				case "address":
					user.Address = newValue;
					break;
				case "id":
					Console.WriteLine("Cannot update user ID");
					return false;
				case "age":
					user.Age = newValue;
					break;
				case "username":
					// Check if username already exists
					var existingUser = InMemoryDataStore.GetUserByUsername(newValue);
					if (existingUser != null && existingUser.Id != user.Id)
					{
						Console.WriteLine("Username already exists");
						return false;
					}
					user.Username = newValue;
					break;
				case "password":
					// In a real application, you should hash the password before storing
					user.Password = newValue;
					break;
				default:
					Console.WriteLine("Invalid property name");
					return false;
			}

			Console.WriteLine("User updated successfully");
			return true;
		}

		#endregion

		#region Debugging Functions
		/// <summary>
		/// Displays all users in the system in a formatted table
		/// </summary>
		public void DisplayAllUsers()
		{
			// Get all users from the data store
			var users = InMemoryDataStore.Users;

			// Check if there are any users
			if (users.Count == 0)
			{
				Console.WriteLine("No users found in the system.");
				return;
			}

			// Display header
			Console.WriteLine("=== All Users ===");
			Console.WriteLine();

			// Define column widths
			const int idWidth = 36;
			const int nameWidth = 20;
			const int usernameWidth = 15;
			const int emailWidth = 25;
			const int phoneWidth = 15;
			const int balanceWidth = 12;

			// Display table header
			Console.WriteLine(
				$"{"ID".PadRight(idWidth)} | " +
				$"{"Name".PadRight(nameWidth)} | " +
				$"{"Username".PadRight(usernameWidth)} | " +
				$"{"Email".PadRight(emailWidth)} | " +
				$"{"Phone".PadRight(phoneWidth)} | " +
				$"{"Balance".PadRight(balanceWidth)} | " +
				$"User Type");

			Console.WriteLine(new string(
				'-', 
				idWidth + 
				nameWidth + 
				usernameWidth + 
				emailWidth + 
				phoneWidth + 
				balanceWidth + 10));

			// Display each user
			foreach (var user in users)
			{
				Console.WriteLine(
					$"{user.Id.ToString().PadRight(idWidth)} | " +
					$"{TruncateString(user.Name, nameWidth).PadRight(nameWidth)} | " +
					$"{TruncateString(user.Username, usernameWidth).PadRight(usernameWidth)} | " +
					$"{TruncateString(user.Email, emailWidth).PadRight(emailWidth)} | " +
					$"{TruncateString(user.MobileNumber, phoneWidth).PadRight(phoneWidth)} | " +
					$"{user.WalletBalance.ToString("C2").PadRight(balanceWidth)}" +
					$" | {user.UserType}"
				);
			}

			// Display footer with count
			Console.WriteLine();
			Console.WriteLine($"Total Users: {users.Count}");
		}

		/// <summary>
		/// Displays detailed information for a single user
		/// </summary>
		/// <param name="userId">ID of the user to display</param>
		public void DisplayUserDetails(Guid userId)
		{
			// Get user from data store
			var user = InMemoryDataStore.GetUserById(userId);

			// Check if user exists
			if (user == null)
			{
				Console.WriteLine($"User with ID {userId} not found.");
				return;
			}

			// Display detailed user information
			Console.WriteLine($"=== User Details: {user.Name} ===");
			Console.WriteLine();
			Console.WriteLine($"ID:               {user.Id}");
			Console.WriteLine($"Name:             {user.Name}");
			Console.WriteLine($"Username:         {user.Username}");
			Console.WriteLine($"Email:            {user.Email}");
			Console.WriteLine($"Phone:            {user.MobileNumber}");
			Console.WriteLine($"Address:          {user.Address}");
			Console.WriteLine($"Age:              {user.Age}");
			Console.WriteLine($"User Type:        {user.UserType}");
			Console.WriteLine($"Wallet Balance:   {user.WalletBalance:C2}");
			Console.WriteLine($"Total Spent:      {user.TotalSpent:C2}");
			Console.WriteLine($"Delivery Location: {user.DeliveryLocation}");
			Console.WriteLine($"User Type:		  {user.UserType}");

			// Display order history if any
			if (user.OrderHistory.Count > 0)
			{
				Console.WriteLine();
				Console.WriteLine("Order History:");
				foreach (var order in user.OrderHistory)
				{
					Console.WriteLine($"  - Order {order.Id.ToString().Substring(0, 8)} | {order.OrderDate.ToShortDateString()} | {order.TotalAmount:C2} | Status: {order.Status}");
				}
			}
		}

		public bool CheckUsernameExists(string username)
		{
			if (string.IsNullOrEmpty(username))
			{
				Console.WriteLine("Username cannot be empty");
				return false;
			}
			return InMemoryDataStore.CheckUsernameExists(username);
		}

		public bool CheckEmailExists(string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				Console.WriteLine("Email cannot be empty");
				return false;
			}
			return InMemoryDataStore.CheckEmailExists(email);
		}

		/// <summary>
		/// Truncates a string to a specified maximum length, adding ellipsis if truncated
		/// </summary>
		/// <param name="str"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		private static string TruncateString(string str, int maxLength)
		{
			if (string.IsNullOrEmpty(str)) return string.Empty;
			return str.Length <= maxLength ? str : str.Substring(0, maxLength - 3) + "...";
		}

		#endregion
	}
}