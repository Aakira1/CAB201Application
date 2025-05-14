using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.App.Services.Customer;
using Arriba_Eats_App.UI.MenuUI.MainMenuUI;
using Arriba_Eats_App.UI.MenuUI;



namespace Arriba_Eats_App.UI.MenuUI.CustomerMenuUI
{
	class CustomersUI : MenuUI
	{
		public override void ShowMenu(bool isActive, EUserType userType)
		{
			while (isActive)
			{
				ClearScreen();
				DisplayOutput(WelcomeMessage() + "\n");
				DisplayOutput("1. Display your user information");
				DisplayOutput("2. Select a list of restaurants to order from");
				DisplayOutput("3. See the status of your orders");
				DisplayOutput("4. Rate a restaurant you've ordered from");
				DisplayOutput("5. Logout");
				DisplayOutput("Please Enter a choice between 1 and 5:");
				string input = GetInput();
				if (string.IsNullOrEmpty(input))
				{
					DisplayError(HandleEmptyInput(input));
					WaitForKeyPress();
					continue;
				}
				isActive = SelectionMenu(input, userType);
			}
			DisplayOutput("Thank you for using Arriba Eats!");
		}

		public override bool SelectionMenu(string input, EUserType userType)
		{
			switch(input)
			{
				case "1":
					Console.WriteLine("Display your user information");
					return true;
				case "2":
					Console.WriteLine("Select a list of restaurants to order from");
					return true;
				case "3":
					Console.WriteLine("See the status of your orders");
					return true;
				case "4":
					Console.WriteLine("Rate a restaurant you've ordered from");
					return true;
				case "5":
					Console.WriteLine("Logout");
					// return to previous menu
					CustomerService customerService = new CustomerService();
					customerService.LogOut();
					return false;
			}
			return false;
		}

	}
}
