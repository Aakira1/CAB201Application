using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Arriba_Eats_App.Services;
using Arriba_Eats_App.Arriba_Eats_App.UI;
using Arriba_Eats_App.UX;

// Personal note
/*
	
 */


namespace Arriba_Eats_App.Arriba_Eats_App.UI
{
	/// <summary>
	/// MenuUI class is the base class for all menu-related user interfaces in the application.
	/// </summary>
	class MenuUI : IUserInterface
	{
		public void ClearScreen()
		{
			return;
		}

		public void DisplayError(string message)
		{
			return;
		}

		public void DisplayOutput(string message)
		{
			return;
		}

		public string GetInput()
		{
			string input = Console.ReadLine()!;
			if (string.IsNullOrEmpty(input))
			{
				HandleEmptyInput(input);
				return string.Empty;
			}
			return input;
		}

		public string GetSecuredInput(string input)
		{
			return input;
		}

		// This class is a placeholder for the sub menu functionality.


		public void WaitForKeyPress()
		{
			return;
		}
		public string WelcomeMessage()
		{
			return "Welcome to Arriba Eats!";
		}

		public string HandleEmptyInput(string input)
		{
			if (string.IsNullOrWhiteSpace(input) && string.IsNullOrEmpty(input))
			{
				return "Input cannot be empty. Please try again.";
			}
			return input;
		}
		public virtual void ShowMenu(bool isActive)
		{
			Console.WriteLine("Default Menu");
		}
		public virtual bool SelectionMenu(string input)
		{
			return false;
		}

	}

	/// <summary>
	/// MainMenuUI class is responsible for displaying the main menu of the application.
	/// </summary>
	class MainMenuUI : MenuUI
	{
		public override void ShowMenu(bool IsActive)
		{
			while (IsActive)
			{
				//Console.Clear();
				Console.WriteLine(WelcomeMessage()+"\n");
				Console.WriteLine("1. Login");
				Console.WriteLine("2. Register");
				Console.WriteLine("3. Exit");
				Console.WriteLine("Please Enter a choice between 1 and 3:");

				string Input = GetInput();

				if (string.IsNullOrEmpty(Input))
				{
					HandleEmptyInput(Input);
					continue;
				}
				IsActive = SelectionMenu(Input);
			}

			Console.WriteLine("Thankyou for using Arriba Eats!");
		}

		public override bool SelectionMenu(string Input)
		{
			UserService userService = new UserService();

			switch (Input)
			{
				case "1":
					LoginUI login = new LoginUI();
					login.ShowMenu(true);
					break;
				case "2":
					RegisterAccountUI register = new RegisterAccountUI();
					register.ShowMenu(true);
					break;
				case "3":
					Console.WriteLine("Exiting the application...");
					return false;
				default:
					Console.WriteLine("Invalid selection. Please try again.\n");
					break;
			}

			return true;
		}
	}

	/// <summary>
	/// RegisterAccountUI class is responsible for displaying the registration menu of the application.
	/// </summary>
	class RegisterAccountUI : MainMenuUI
	{
		public override void ShowMenu(bool IsActive)
		{
			Console.WriteLine("Register Account Menu\n");
			Console.WriteLine("1. Customer");
			Console.WriteLine("2. Deliverer");
			Console.WriteLine("3. Client");
			Console.WriteLine("4. Return to the previous menu");
			Console.WriteLine("Please Enter a choice between 1 and 4:");

			string Input = GetInput();

			if (string.IsNullOrEmpty(Input))
			{
				HandleEmptyInput(Input);
				return;
			}
			IsActive = SelectionMenu(Input);
		}

		public override bool SelectionMenu(string Input)
		{
		

			switch (Input)
			{
				case "1":
					Console.WriteLine("Registering as a Customer...");
					break;
				case "2":
					Console.WriteLine("Registering as a Deliverer...");
					break;
				case "3":
					Console.WriteLine("Registering as a Client...");
					break;
				case "4":
					MainMenuUI mainMenu = new MainMenuUI();
					mainMenu.ShowMenu(true);
					break;
				default:
					Console.WriteLine("Invalid selection. Please try again.\n");
					break;
			}
			return true;
		}
	}

	/// <summary>
	/// LoginUI class is responsible for displaying the login menu of the application.
	/// </summary>
	class LoginUI : MainMenuUI
	{
		// This class is a placeholder for the sub menu functionality.
		// It can be implemented later as needed.
		// For now, it inherits from MainMenuUI to access its methods and properties.
		public override void ShowMenu(bool IsActive)
		{
			while (IsActive)
			{
				//Console.Clear();
				Console.WriteLine("Enter Your Username");
				string Input = GetInput();
				if (string.IsNullOrEmpty(Input))
				{
					HandleEmptyInput(Input);
					continue;
				}
				IsActive = SelectionMenu(Input);

				if (IsActive)
				{
					Console.WriteLine("Login successful!");
					break;
				}
				else
				{
					Console.WriteLine("Login failed. Please try again.");
				}
				Console.WriteLine("Do you want to try again? (y/n)");
				string retryInput = GetInput();
				if (retryInput.ToLower() != "y")
				{
					Console.WriteLine("returning to Selection Menu...");
					break;
				}
				ShowMenu(true);
				continue;
			}
		}
		public override bool SelectionMenu(string Input)
		{
			return false;
		}

	}

}