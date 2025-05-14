using System;
using System.Collections.Generic;
using System.Linq;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data;
using Arriba_Eats_App.UI.MenuUI;
using System.Runtime.CompilerServices;

namespace Arriba_Eats_App.Services.UserServices
{
	/// <summary>
	/// Service for managing user accounts in the system
	/// </summary>
	public class UserService
	{
		// private properties for user data
		private MenuUI _menuUI = new MenuUI();

		#region User services methods (Registering, Update, Delete - CRUD)
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
				user.DeliveryLocation == null)
			{
				Console.WriteLine("User details are incomplete");
				return false;
			}

			// Check if username already exists
			//if (InMemoryDataStore.GetUserByUsername(user.Username) != null)
			//{
				//Console.WriteLine("Username already exists");
				//return false;
			//}

			// Set user type and ID if not set
			user.UserType = EUserType.Customer;
			if (user.Id == Guid.Empty)
				user.Id = Guid.NewGuid();

			// Add user to data store
			InMemoryDataStore.AddUser(user);
			Console.WriteLine($"You have been successfully registered as a customer, {user.Name}!\n");
			return true;
		}
		public bool RegisterDeliveryPerson(DeliveryPerson deliveryPerson)
		{
			if (deliveryPerson == null)
			{
				Console.WriteLine("Delivery person cannot be null");
				return false;
			}

			if (string.IsNullOrEmpty(deliveryPerson.Name) ||
				string.IsNullOrEmpty(deliveryPerson.Email) ||
				string.IsNullOrEmpty(deliveryPerson.MobileNumber) ||
				string.IsNullOrEmpty(deliveryPerson.Age) ||
				string.IsNullOrEmpty(deliveryPerson.Password) ||
				string.IsNullOrEmpty(deliveryPerson.VehicleType) ||
				string.IsNullOrEmpty(deliveryPerson.LicensePlate) ||
				string.IsNullOrEmpty(deliveryPerson.VehicleColor))
			{
				Console.WriteLine("Delivery person details are incomplete");
				return false;
			}
			// Check if username already exists
			//if (InMemoryDataStore.GetUserByUsername(deliveryPerson.Username) != null)
			//{
			//	Console.WriteLine("Username already exists");
			//	return false;
			//}
			// Set user type and ID if not set
			deliveryPerson.UserType = EUserType.DeliveryPerson;
			if (deliveryPerson.Id == Guid.Empty)
				deliveryPerson.Id = Guid.NewGuid();

			// Add delivery person to data store
			InMemoryDataStore.AddDeliveryPerson(deliveryPerson);
			Console.WriteLine($"You have been successfully registered as a delivery person, {deliveryPerson.Name}!\n");
			return true;
		}
		public bool RegisterClient(Restaurant restaurant)
		{
			if (restaurant == null)
			{
				Console.WriteLine("Restaurant cannot be null");
				return false;
			}

			if (string.IsNullOrEmpty(restaurant.Name) ||
				string.IsNullOrEmpty(restaurant.CuisineType) ||
				restaurant.Location == null) // add more
			{
				Console.WriteLine("Restaurant details are incomplete");
				return false;
			}

			// Check if email already exists
			if (InMemoryDataStore.GetUserByEmail(restaurant.Email) != null)
			{
				Console.WriteLine("Email already exists");
				return false;
			}

			// Set user type and ID if not set
			restaurant.UserType = EUserType.RestaurantOwner;

			if (restaurant.Id == Guid.Empty) restaurant.Id = Guid.NewGuid();

			// Add restaurant to data store
			InMemoryDataStore.AddRestaurantOwner(restaurant); // Add restaurant owner to the list
			//InMemoryDataStore.AddRestaurant(restaurant.Restaurant); // Add restaurant to the list
			Console.WriteLine($"You have been successfully registered as a restaurant owner, {restaurant.Name}!\n");
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
		/// Validates user login credentials
		/// </summary>
		/// <param name="username">Username</param>
		/// <param name="password">Password</param>
		/// <returns>True if credentials are valid</returns>
		public bool ValidateLogin(string email, string password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				Console.WriteLine("Username or password cannot be empty");
				return false;
			}

			var user = InMemoryDataStore.GetUserByEmail(email);
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
		#endregion

		#region Debugging Functions
		/// <summary>
		/// Displays all users in the system in a formatted table
		/// </summary>
		public void DisplayAllUsers()
		{
			// Get all users from the data store
			var users = InMemoryDataStore.Users;
			var deliveryPersons = InMemoryDataStore.DeliveryPersons;
			var restaurants = InMemoryDataStore.Restaurants;

			// Check if there are any users at all
			if (users.Count == 0 && deliveryPersons.Count == 0 && restaurants.Count == 0)
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
			const int emailWidth = 25;
			const int phoneWidth = 15;
			const int balanceWidth = 12;

			// Display table header
			Console.WriteLine(
				$"{"ID".PadRight(idWidth)} | " +
				$"{"Name".PadRight(nameWidth)} | " +
				$"{"Email".PadRight(emailWidth)} | " +
				$"{"Phone".PadRight(phoneWidth)} | " +
				$"{"Balance".PadRight(balanceWidth)} | " +
				$"User Type");
			Console.WriteLine(new string(
				'-',
				idWidth +
				nameWidth +
				emailWidth +
				phoneWidth +
				balanceWidth + 10));

			// Display each user
			foreach (var user in users)
			{
				Console.WriteLine(
					$"{user.Id.ToString().PadRight(idWidth)} | " +
					$"{TruncateString(user.Name, nameWidth).PadRight(nameWidth)} | " +
					$"{TruncateString(user.Email, emailWidth).PadRight(emailWidth)} | " +
					$"{TruncateString(user.MobileNumber, phoneWidth).PadRight(phoneWidth)} | " +
					$"{user.WalletBalance.ToString("C2").PadRight(balanceWidth)} | " +
					$"{user.UserType}"
				);
			}

			// Display each delivery person
			foreach (var deliverer in deliveryPersons)
			{
				Console.WriteLine(
					$"{deliverer.Id.ToString().PadRight(idWidth)} | " +
					$"{TruncateString(deliverer.Name, nameWidth).PadRight(nameWidth)} | " +
					$"{TruncateString(deliverer.Email, emailWidth).PadRight(emailWidth)} | " +
					$"{TruncateString(deliverer.MobileNumber, phoneWidth).PadRight(phoneWidth)} | " +
					$"{"N/A".PadRight(balanceWidth)} | " +
					$"{deliverer.UserType}"
				);
			}

			// Display each restaurant
			foreach (var restaurant in restaurants)
			{
				Console.WriteLine(
					$"{restaurant.Id.ToString().PadRight(idWidth)} | " +
					$"{TruncateString(restaurant.Name, nameWidth).PadRight(nameWidth)} | " +
					$"{TruncateString(restaurant.Email, emailWidth).PadRight(emailWidth)} | " +
					$"{TruncateString(restaurant.MobileNumber, phoneWidth).PadRight(phoneWidth)} | " +
					$"{"N/A".PadRight(balanceWidth)} | " +
					$"{restaurant.UserType}"
				);
				// Also display the restaurant info
				Console.WriteLine($"\n   Restaurant: {restaurant.RestaurantName} ({restaurant.CuisineType})");
				Console.WriteLine($"   Location: {restaurant.Location}");
			}

			// Display footer with count
			Console.WriteLine();
			Console.WriteLine($"Total Users: {users.Count + deliveryPersons.Count + restaurants.Count}");
		}
		public void DisplayDelivererDetails(Guid userId)
		{
			// Get user from data store
			var Deliverer = InMemoryDataStore.GetDeliveryPersonById(userId);

			// Check if user exists
			if (Deliverer == null)
			{
				Console.WriteLine($"User with ID {userId} not found.");
				return;
			}

			// Display detailed user information
			Console.WriteLine($"=== User Details: {Deliverer.Name} ===");
			Console.WriteLine();
			Console.WriteLine($"ID:               {Deliverer.Id}");
			Console.WriteLine($"Name:             {Deliverer.Name}");
			//Console.WriteLine($"Username:         {Deliverer.Username}");
			Console.WriteLine($"Email:            {Deliverer.Email}");
			Console.WriteLine($"Phone:            {Deliverer.MobileNumber}");
			Console.WriteLine($"Address:          {Deliverer.Address}");
			Console.WriteLine($"Age:              {Deliverer.Age}");
			Console.WriteLine($"User Type:        {Deliverer.UserType}");
			Console.WriteLine($"Vehicle Type:     {Deliverer.VehicleType}");
			Console.WriteLine($"License Plate:    {Deliverer.LicensePlate}");
			Console.WriteLine($"Vehicle Color:    {Deliverer.VehicleColor}");
			Console.WriteLine($"Current Location: {Deliverer.CurrentLocation}");
			Console.WriteLine($"User Type:		  {Deliverer.UserType}");
		}


		//public bool CheckUsernameExists(string username)
		//{
		//	if (string.IsNullOrEmpty(username))
		//	{
		//		Console.WriteLine("Username cannot be empty");
		//		return false;
		//	}
		//	return InMemoryDataStore.CheckUsernameExists(username);
		//}

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

		#endregion

		#region Validation Handling
		/// <summary>
		/// Validates a name input from the user
		/// </summary>
		/// <param name="Input"></param>
		/// <returns></returns>
		public string ValidName(string Input)
		{
			// Make sure name is only letters
			while (string.IsNullOrEmpty(Input) || Input.All(char.IsLetter) || !Input.Contains(" "))
			{
				_menuUI.DisplayError("Must consist of at least one letter and only letters, spaces, " +
					"apostrophes and hyphens.");
				Input = _menuUI.GetInput();
			}
			return Input;
		}
		public string ValidAge(string Input)
		{
			while (string.IsNullOrEmpty(Input) || !int.TryParse(Input, out int age) || age < 18 || age > 100)
			{
				_menuUI.DisplayError("Age must be a number between 18 and 100. " +
					"Please try again.");
				Input = _menuUI.GetInput();
			}
			return Input;
		}
		public string ValidMobileNo(string Input)
		{
			while (string.IsNullOrEmpty(Input) || !Input.All(char.IsDigit) 
				|| Input.Length != 10 || Input[0] != '0')
			{
				_menuUI.DisplayError("Must consist of only numbers, " +
					"be exactly 10 characters long, and have a leading zero.");
				Input = _menuUI.GetInput();
			}
			return Input;
		}
		public string ValidEmail(string Input)
		{
			while (string.IsNullOrEmpty(Input) || !Input.Contains("@") || !Input.Contains("."))
			{
				_menuUI.DisplayError("Must include exactly one @ character and have at " +
					"least one other character on either side.");
				Input = _menuUI.GetInput();
			}
			CheckEmailExists(Input);

			return Input;
		}
		public string ValidPassword(string Input, bool isRecheck, User user)
		{
			while (!isRecheck)
			{
				if (string.IsNullOrEmpty(Input))
				{
					_menuUI.DisplayError("Password cannot be empty.");
					Input = _menuUI.GetInput();
					continue;
				}

				if (Input.Length < 8)
				{
					_menuUI.DisplayError("Password must be at least 8 characters long.");
					Input = _menuUI.GetInput();
					continue;
				}

				if (!Input.Any(char.IsDigit))
				{
					_menuUI.DisplayError("Password must contain at least one number.");
					Input = _menuUI.GetInput();
					continue;
				}

				if (!Input.Any(char.IsLower))
				{
					_menuUI.DisplayError("Password must contain at least one lowercase letter.");
					Input = _menuUI.GetInput();
					continue;
				}

				if (!Input.Any(char.IsUpper))
				{
					_menuUI.DisplayError("Password must contain at least one uppercase letter.");
					Input = _menuUI.GetInput();
					continue;
				}
				//if (!Input.Any(ch => "!@#$%^&*()_+-=[]{}|;':\",.<>?/`~".Contains(ch)))
				//{
					//_menuUI.DisplayError("Password must contain at least one special character.");
					//Input = _menuUI.GetInput();
					//continue;
				//}
				break;
			}

			while (isRecheck)
			{
				if (string.IsNullOrEmpty(Input))
				{
					_menuUI.DisplayError("Password confirmation cannot be empty.");
					Input = _menuUI.GetInput();
					continue;
				}

				if (user.Password != Input)
				{
					_menuUI.DisplayError("Passwords do not match. Please try again.");
					Input = _menuUI.GetInput();
					continue;
				}

				break; // Only break when passwords match
			}
			return Input;
		}
		public int ValidLocation(int Input)
		{
			while (Input < 0 || Input > 100)
			{
				_menuUI.DisplayError("Location must be between 0 and 100.");
				Input = int.Parse(_menuUI.GetInput());
			}
			// check input is a digit
			while (string.IsNullOrEmpty(Input.ToString()) || !Input.ToString().All(char.IsDigit))
			{
				_menuUI.DisplayError("Must be of the format X,Y " +
					"where X and Y are both integer values");
				Input = int.Parse(_menuUI.GetInput());
			}
			return Input;
		}
		public string ValidLicensePlate(string Input)
		{
			while (string.IsNullOrEmpty(Input) || !Input.All(char.IsLetterOrDigit)
				|| string.IsNullOrWhiteSpace(Input) || Input.Contains(Input.ToUpper()))
			{
				_menuUI.DisplayError("Must be between 1 and 8 characters long, " +
					"may only contain uppercase letters, " +
					"numbers and spaces, and may not consist entirely of spaces.");
				Input = _menuUI.GetInput();
			}
			return Input.ToUpper();
		}
		public string ValidCuisineType(int cuisineChoiceInt, string cuisinetype)
		{
			if (cuisineChoiceInt == 1)
			{
				cuisinetype = "Italian";
			}
			else if (cuisineChoiceInt == 2)
			{
				cuisinetype = "French";
			}
			else if (cuisineChoiceInt == 3)
			{
				cuisinetype = "Chinese";
			}
			else if (cuisineChoiceInt == 4)
			{
				cuisinetype = "Japanese";
			}
			else if (cuisineChoiceInt == 5)
			{
				cuisinetype = "American";
			}
			else if (cuisineChoiceInt == 6)
			{
				cuisinetype = "Australian";
			}

			while(cuisineChoiceInt.ToString() == null || cuisineChoiceInt.ToString() == string.Empty ||
				cuisineChoiceInt < 1 || cuisineChoiceInt > 6)
			{
				_menuUI.DisplayError("Italian, French, Chinese, Japanese, American or Australian. Note, this will be entered via a menu option so " +
					"you will need to enter a numeric value between 1 -6 inclusive.");
				cuisinetype = _menuUI.GetInput();
			}

			return cuisinetype;
		}

		#endregion

		#region Private Methods
		/// <summary>
		/// Gets a property value from a user by property name
		/// </summary>
		private List<User> FindUsers(string name = "", string mobile = "", string email = "")
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
		private static string TruncateString(string str, int maxLength)
		{
			if (string.IsNullOrEmpty(str)) return string.Empty;
			return str.Length <= maxLength ? str : str.Substring(0, maxLength - 3) + "...";
		}
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
				//case "username":
				//	return user.Username;
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
				//case "username":
				//	// Check if username already exists
				//	var existingUser = InMemoryDataStore.GetUserByUsername(newValue);
				//	if (existingUser != null && existingUser.Id != user.Id)
				//	{
				//		Console.WriteLine("Username already exists");
				//		return false;
				//	}
				//	user.Username = newValue;
				//	break;
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

		internal EUserType GetUserType(string emailInput, string password)
		{
			if (emailInput == null)
			{
				throw new ArgumentNullException(nameof(emailInput));
			}
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
			var user = InMemoryDataStore.GetUserByEmail(emailInput);

			if (user == null)
			{
				throw new ArgumentException("User not found");
			}
			if (user.Password != password)
			{
				throw new ArgumentException("Invalid password");
			}
			return user.UserType;
		}
		#endregion
	}
}