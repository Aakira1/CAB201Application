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
		public UserService() { }

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

		public string SearchUserDetails(User user, UserType userType, string DataType)
		{
			if(user == null)
			{
				return "User cannot be null";
			}

			if (userType == UserType.Customer)
			{
				switch(DataType)
				{
					case "Name":
						Console.WriteLine(user.Name);
						return user.Name;
					case "Email":
						return user.Email;
					case "MobileNumber":
						return user.MobileNumber;
					case "Address":
						return user.Address;
					default:
						Console.WriteLine("Invalid data type");
						return null;
				}
			}

			if (userType == UserType.DeliveryService)
			{
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
					default:
						Console.WriteLine("Invalid data type");
						return null;
				}
			}

			if (userType == UserType.Client)
			{
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
					default:
						Console.WriteLine("Invalid data type");
						return null;
				}
			}

			return "Invalid Data Type";
		}
	}
}
