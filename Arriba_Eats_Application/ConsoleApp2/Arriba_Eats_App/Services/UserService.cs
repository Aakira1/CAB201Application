using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data.Repositories;


namespace Arriba_Eats_App.Arriba_Eats_App.Services
{
	class UserService : CustomerRepo
	{
		#region Public Methods
		public UserService() { }

		/// <summary>
		/// This method registers a new user in the system.
		/// </summary>
		/// <param name="Data"></param>
		public void RegisterUser(User Data)
		{
			if (Data == null)
			{
				Console.WriteLine("Customer cannot be null");
				return;
			}
			if (string.IsNullOrEmpty(Data.Name) || string.IsNullOrEmpty(Data.Address) || string.IsNullOrEmpty(Data.MobileNumber))
			{
				Console.WriteLine("Customer details are incomplete");
				return;
			}
			Add(Data);
			Console.WriteLine("Customer added successfully");
		}

		/// <summary>
		/// This method updates an existing user in the system.
		/// </summary>
		/// <param name="Data"></param>
		public void UpdateUser(User Data)
		{
			if (Data == null)
			{
				Console.WriteLine("Customer cannot be null");
				return;
			}
			if (string.IsNullOrEmpty(Data.Name) || string.IsNullOrEmpty(Data.Address) || string.IsNullOrEmpty(Data.MobileNumber))
			{
				Console.WriteLine("Customer details are incomplete");
				return;
			}
			Update(Data);
			Console.WriteLine("Customer updated successfully");
		}


		/// <summary>
		/// This method deletes a user from the system.
		/// </summary>
		/// <param name="UUID"></param>
		public void DeleteUser(Guid UUID)
		{
			if (UUID == Guid.Empty)
			{
				Console.WriteLine("Customer ID cannot be empty");
				return;
			}
			Delete(UUID);
			Console.WriteLine("Customer deleted successfully");
		}

		/// <summary>
		/// This method retrieves a user from the system by their UUID.
		/// </summary>
		/// <param name="UUID"></param>
		/// <returns></returns>
		public User GetUserData(Guid UUID)
		{
			if (UUID == Guid.Empty)
			{
				Console.WriteLine("Customer ID cannot be empty");
				return null;
			}
			var user = GetById(UUID);
			if (user == null)
			{
				Console.WriteLine("Customer not found");
				return null;
			}
			Console.WriteLine("Customer found");
			return user;
		}

		/// <summary>
		/// This method retrieves all users from the system.
		/// </summary>
		/// <param name="Name"></param>
		/// <param name="Mobile"></param>
		/// <param name="Email"></param>
		/// <returns></returns>
		public List<User> GetAllUsers(string Name, string Mobile, string Email)
		{
			var users = GetAll().ToList();
			if (users.Count == 0)
			{
				Console.WriteLine("No customers found");
				return null;
			}
			Console.WriteLine("Customers found");
			return users;
		}

		/// <summary>
		/// This method retrieves user details based on the specified data type.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="DataType"></param>
		/// <returns></returns>
		public string GetUserDetails(User user, string DataType)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return "User cannot be null";
			}
			return SearchUserDetails(user, DataType);
		}

		/// <summary>
		/// This method retrieves user password based on the specified authorization status.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="DataType"></param>
		/// <param name="newValue"></param>
		public void UpdateUserDetails(User user, string DataType, string newValue)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return;
			}
			UpdateDetails(user, DataType, newValue);
		}
		public string ReturnLoginValidation(string username, string password)
		{
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
			{
				return "Username or password cannot be empty";
			}

			foreach(UserData u in GetAll())
			{
				if (u.Username == username && u.Password == password)
				{
					Console.WriteLine("Login successful");
					return "Login successful";
				}
			}
			return "Login Failed";
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// This method retrieves user password based on the specified authorization status.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="DataType"></param>
		/// <returns></returns>
		// refactored* - function handles user search details.
		private string SearchUserDetails(User user, string DataType)
		{
			if(user == null)
			{
				return "User cannot be null";
			}

			switch (DataType)
			{
				case "Name":
					return user.Name;
				case "Email":
					return user.Email;
				case "MobileNumber":
					return user.MobileNumber;
				case "Address":
					return user.Address;
				case "UUID":
					return user.UUID.ToString();
				case "Age":
					return user.Age;
				case "Username":
					return user.Username;
				case "UserType":
					return user.UserType.ToString();
				default:
					Console.WriteLine("Invalid data type");
					return "Invalid Data Type";
			}
		}

		/// <summary>
		///	This method retrieves the user password based on the specified authorization status.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="isAuthorized"></param>
		/// <returns></returns>
		private string GetUserPassword(User user, bool isAuthorized)
		{
			if(!isAuthorized)
			{
				return "Unauthorized access";
			}

			return user.Password;
		}

		/// <summary>
		/// This method updates user details based on the specified data type and new value.
		/// </summary>
		/// <param name="user"></param>
		/// <param name="DataType"></param>
		/// <param name="newValue"></param>
		/// <returns></returns>
		private string UpdateDetails(User user, string DataType, string newValue)
		{
			if (user == null)
			{
				Console.WriteLine("User cannot be null");
				return "User cannot be null";
			}
			switch (DataType)
			{
				case "Name":
					user.Name = newValue;
					break;
				case "Email":
					user.Email = newValue;
					break;
				case "MobileNumber":
					user.MobileNumber = newValue;
					break;
				case "Address":
					user.Address = newValue;
					break;
				case "UUID":
					Console.WriteLine("Invalid Option.");
					break;
				case "Age":
					user.Age = newValue;
					break;
				case "Username":
					user.Username = newValue;
					break;
				default:
					Console.WriteLine("Invalid data type");
					return "Invalid Data Type";
			}
			Update(user);
			Console.WriteLine("Customer updated successfully");
			return "Customer updated successfully";
		}
		#endregion
	}
}
