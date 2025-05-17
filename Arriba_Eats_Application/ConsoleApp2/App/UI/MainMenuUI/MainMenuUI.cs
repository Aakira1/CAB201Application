using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.UI.MenuUI.MainMenuUI
{
	/// <summary>
	/// MainMenuUI class is responsible for displaying the main menu of the application.
	/// </summary>
	class MainMenuUI : MenuUI
	{
		public override void ShowMenu(bool IsActive, EUserType userType)
		{
			while (IsActive)
			{
                //ClearScreen(); // uncomment this line if you want to clear the screen before displaying the menu
                DisplayOutput(WelcomeMessage() + "\n");
				DisplayOutput("1. Login");
				DisplayOutput("2. Register");
				DisplayOutput("3. Exit");
				DisplayOutput("Please Enter a choice between 1 and 3:");

				// debugging only option
				UserService userService = new UserService();
				userService.DisplayAllUsers();
				string input = GetInput();
                

                if (string.IsNullOrWhiteSpace(input) && string.IsNullOrEmpty(input))
                {
                    DisplayError("Input cannot be empty. Please try again.");
                    continue;
                }
                HandleEmptyInput(input);
                HandleIncorrectInput(input);

                IsActive = SelectionMenu(input, EUserType.None);
			}

			DisplayOutput("Thankyou for using Arriba Eats!");
		}

		public override bool SelectionMenu(string Input, EUserType userType)
		{
			switch (Input)
			{
				case "1":
					LoginUI login = new LoginUI();
					login.ShowMenu(true, userType);
					break;
				case "2":
					RegisterAccountUI register = new RegisterAccountUI();
					register.ShowMenu(true, userType);
					break;
				case "3":
					DisplayOutput("Exiting the application...");
					return false;
				default:
					DisplayError("Invalid selection. Please try again.\n");
					break;
			}
			return true;
		}
	}
}
