using System;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;

namespace Arriba_Eats_App.UI.MenuUI.MainMenuUI
{
	/// <summary>
	/// LoginUI class is responsible for displaying the login menu of the application.
	/// </summary>
	class LoginUI : MenuUI
	{
		private UserService userService = new UserService();

		public override void ShowMenu(bool IsActive, EUserType userType)
		{
			MainMenuUI mainMenuUI = new MainMenuUI();
			ConsoleKey consoleKey;
			var keyinfo = Console.ReadKey(true);
			consoleKey = keyinfo.Key;

			while (IsActive)
			{
				ClearScreen();
				DisplayOutput("---------- Login Screen ----------\n");
				DisplayOutput("Enter Your Email:");
				string EmailInput = GetInput();

				DisplayOutput("Enter Your Password");
				string Password = GetSecuredInput(string.Empty, true);

				if (string.IsNullOrEmpty(EmailInput) || string.IsNullOrEmpty(Password))
				{
					if (string.IsNullOrEmpty(EmailInput)) DisplayError(HandleEmptyInput(EmailInput));

					if (string.IsNullOrEmpty(Password)) DisplayError(HandleEmptyInput(Password));
					
					//WaitForKeyPress();
					continue;
				}

				try
				{
					if (userService.ValidateLogin(EmailInput, Password))
					{
						DisplayOutput("Login successful!");
						// Navigate to user menu based on user type
						// You would implement this part
						//WaitForKeyPress();
						break; // add additional logic here
					}
					else
					{
						DisplayError("Login failed. Invalid username or password.");
					}
				}
				catch (Exception ex)
				{
					DisplayError($"Error during login: {ex.Message}");
				}

				DisplayOutput("Do you want to try again? (y/n) or enter to return to the main menu.");
				string retryInput = GetInput();

				// Check if the user wants to retry (if enter is press defaults to main menu)
				if (retryInput.ToLower() != "y" || keyinfo.Key == ConsoleKey.Enter)
				{
					DisplayOutput("Returning to Selection Menu...");
					mainMenuUI.ShowMenu(true, userType);
					continue;
				}

			}
		}

		public override bool SelectionMenu(string Input, EUserType userType)
		{
			switch (Input)
			{
				case "1":
					// Implement logic for option 1
					break;
				case "2":
					// Implement logic for option 2
					break;
				case "3":
					DisplayOutput("Exiting the application...");
					return false;
				default:
					DisplayError("Invalid selection. Please try again.");
					return true;
			}
			return true; // Keep the menu active
		}
	}
}