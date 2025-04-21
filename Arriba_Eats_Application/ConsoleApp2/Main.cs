using Arriba_Eats_App.Arriba_Eats_App.UI;
using System;
using System.ComponentModel.DataAnnotations;
using Arriba_Eats_App.Arriba_Eats_App.Services;
using Arriba_Eats_App.Arriba_Eats_App.Data.Models;

namespace Main
{
	class InitiateProgram
	{
		static void Main(string[] args)
		{
			//var Menu = new MainMenuUI();
			//Menu.ShowMenu();
			var User = new UserService();
			var Customer = new User
			{
				Name = "John Doe",
				Address = "123 Main St",
				MobileNumber = "123-456-7890",
				UserType = UserType.Customer
			};
			User.Add(Customer);
			User.SearchUserDetails(Customer, UserType.Customer, "Name");
		}
	}
}