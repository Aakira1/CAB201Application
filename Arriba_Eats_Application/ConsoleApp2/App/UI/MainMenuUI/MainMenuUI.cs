using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;
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
				ClearScreen();
				DisplayOutput(WelcomeMessage() + "\n");
				DisplayOutput("1. Login");
				DisplayOutput("2. Register");
				DisplayOutput("3. Exit");
				DisplayOutput("Please Enter a choice between 1 and 3:");

				// debugging only option
				UserService userService = new UserService();
				userService.DisplayAllUsers();


				string Input = GetInput();

				if (string.IsNullOrEmpty(Input))
				{
					DisplayError(HandleEmptyInput(Input));
					WaitForKeyPress();
					continue;
				}
				IsActive = SelectionMenu(Input, EUserType.None);
			}

			DisplayOutput("Thankyou for using Arriba Eats!");
		}

		public override bool SelectionMenu(string Input, EUserType userType)
		{
			switch (Input)
			{
				case "1":
					LoginUI login = new LoginUI();
					login.ShowMenu(true, EUserType.None);
					break;
				case "2":
					RegisterAccountUI register = new RegisterAccountUI();
					register.ShowMenu(true, EUserType.None);
					break;
				case "3":
					DisplayOutput("Exiting the application...");
					return false;
				default:
					DisplayError("Invalid selection. Please try again.\n");
					WaitForKeyPress();
					break;
			}

			return true;
		}

	}
}
