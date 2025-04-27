using System;
using Arriba_Eats_App.Services;

namespace Arriba_Eats_App.Arriba_Eats_App.UI.MainMenuUI
{
	/// <summary>
	/// LoginUI class is responsible for displaying the login menu of the application.
	/// </summary>
	class LoginUI : MainMenuUI
	{
		private UserService userService = new UserService();

		public override void ShowMenu(bool IsActive)
		{
			while (IsActive)
			{
				ClearScreen();
				DisplayOutput("Login Screen");
				DisplayOutput("Enter Your Username");
				string UserName = GetInput();

				DisplayOutput("Enter Your Password");
				string Password = GetSecuredInput(string.Empty);

				if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
				{
					if (string.IsNullOrEmpty(UserName))
						DisplayError(HandleEmptyInput(UserName));
					if (string.IsNullOrEmpty(Password))
						DisplayError(HandleEmptyInput(Password));
					WaitForKeyPress();
					continue;
				}

				try
				{
					if (userService.ValidateLogin(UserName, Password))
					{
						DisplayOutput("Login successful!");
						// Navigate to user menu based on user type
						// You would implement this part
						WaitForKeyPress();
						break;
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

				DisplayOutput("Do you want to try again? (y/n)");
				string retryInput = GetInput();
				if (retryInput.ToLower() != "y")
				{
					DisplayOutput("Returning to Selection Menu...");
					break;
				}
			}
		}

		public override bool SelectionMenu(string Input)
		{
			// This method is not needed for LoginUI as we handle inputs directly in ShowMenu
			return false;
		}
	}
}