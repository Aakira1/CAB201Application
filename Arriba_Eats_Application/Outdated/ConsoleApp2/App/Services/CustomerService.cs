using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data;
using Arriba_Eats_App.UI.MenuUI;
using Arriba_Eats_App.UI.MenuUI.MainMenuUI;



namespace Arriba_Eats_App.App.Services.Customer
{
	class CustomerService
	{
		public CustomerService customerService(User user)
		{
			DisplayUserDetails(user);

			return this;
		}

		/// <summary>
		/// Displays detailed information for a single user
		/// </summary>
		/// <param name="user">User object to display</param>
		private void DisplayUserDetails(User user)
		{
			// Get user from data store
			var fetchedUser = InMemoryDataStore.GetUserById(id: user.Id);

			// Check if user exists
			if (fetchedUser == null)
			{
				Console.WriteLine($"User with ID {user.Id} not found.");
				return;
			}

			// Display detailed user information
			Console.WriteLine($"Your user details are as follows: ");
			Console.WriteLine();
			//Console.WriteLine($"ID:               {fetchedUser.Id}");
			Console.WriteLine($"Name:             {fetchedUser.Name}");
			Console.WriteLine($"Age:              {fetchedUser.Age}");
			Console.WriteLine($"Email:            {fetchedUser.Email}");
			Console.WriteLine($"Phone:            {fetchedUser.MobileNumber}");
			Console.WriteLine($"Delivery Location: {fetchedUser.DeliveryLocation}");
			//Console.WriteLine($"Address:          {fetchedUser.Address}");
			//Console.WriteLine($"User Type:        {fetchedUser.UserType}");
			//Console.WriteLine($"Wallet Balance:   {fetchedUser.WalletBalance:C2}");
			//Console.WriteLine($"Total Spent:      {fetchedUser.TotalSpent:C2}");
			//Console.WriteLine($"User Type:        {fetchedUser.UserType}");

			// Display order history if any
			if (fetchedUser.OrderHistory.Count > 0)
			{
				Console.WriteLine();
				Console.WriteLine("Order History:");
				foreach (var order in fetchedUser.OrderHistory)
				{
					Console.WriteLine($"  - Order {order.Id.ToString().Substring(0, 8)} | {order.OrderDate.ToShortDateString()} | {order.TotalAmount:C2} | Status: {order.Status}");
				}
			}
		}

		// log out method
		public void LogOut()
		{
			Console.WriteLine("Logging out...");
			MainMenuUI mainMenuUI = new MainMenuUI();
			mainMenuUI.ShowMenu(true, EUserType.None);
		}
	}

}
