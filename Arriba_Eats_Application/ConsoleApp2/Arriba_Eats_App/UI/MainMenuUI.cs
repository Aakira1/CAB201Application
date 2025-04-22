using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Arriba_Eats_App.Services;
// Personal note
/*
	
 */


namespace Arriba_Eats_App.Arriba_Eats_App.UI
{
	/// <summary>
	/// MenuUI class is the base class for all menu-related user interfaces in the application.
	/// </summary>
	class MenuUI
	{
		public virtual void ShowMenu()
		{
			Console.WriteLine("Default Menu");
		}

		public string GetPasswordInput()
		{
			Console.Write("Please enter your password: ");
			string password = Console.ReadLine()!;
			return password;
		}

		public string GetUserInput()
		{
			Console.Write("Please enter your username: ");
			string username = Console.ReadLine()!;
			return username;
		}

		public void DisplayErrorMessage(string message)
		{
			Console.WriteLine(message);
		}

		public void HandleEmptyInput(string Input)
		{
			if (string.IsNullOrEmpty(Input))
			{
				DisplayErrorMessage("Input cannot be empty. Please try again.");
				Console.ReadKey();
				Console.Clear();
			}
		}

		public string WelcomeMessage()
		{
			return "Welcome to Arriba Eats!";
		}

		public void HiddenPassword(string Password)
		{
			Password = string.Empty;

			ConsoleKey key;
			do
			{
				var KeyInfo = Console.ReadKey(intercept: true);
				key = KeyInfo.Key;

				if (key == ConsoleKey.Backspace && Password.Length > 0)
				{
					Password = Password.Substring(0, (Password.Length - 1));
					Console.Write("\b \b");
					Password = Password[0..^1];
				}
				else if (!char.IsControl(KeyInfo.KeyChar))
				{
					Console.Write("*");
					Password += KeyInfo.KeyChar;
				}
			} while (key != ConsoleKey.Enter);
		}
	}

	/// <summary>
	/// MainMenuUI class is responsible for displaying the main menu of the application.
	/// </summary>
	class MainMenuUI : MenuUI
	{
		public override void ShowMenu()
		{
			bool IsActive = true;

			while (IsActive)
			{
				//Console.Clear();
				Console.WriteLine(WelcomeMessage()+"\n");
				Console.WriteLine("1. Login");
				Console.WriteLine("2. Register");
				Console.WriteLine("3. Exit"); 

				string Input = GetUserInput();
				if (string.IsNullOrEmpty(Input))
				{
					HandleEmptyInput(Input);
					continue;
				}
				IsActive = SelectionMenu(Input);
			}

			Console.WriteLine("Thankyou for using Arriba Eats!");
		}

		private bool SelectionMenu(string Input)
		{
			switch (Input)
			{
				case "1":
					LoginUI login = new LoginUI();
					login.ShowMenu();
					break;
				case "2":
					RegisterAccountUI register = new RegisterAccountUI();
					register.ShowMenu();
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
		public override void ShowMenu()
		{
			
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
		public override void ShowMenu()
		{
			bool IsActive = true;

			while (IsActive)
			{
				//Console.Clear();
				Console.WriteLine("Enter Your Username");
				string Input = GetUserInput();
				if (string.IsNullOrEmpty(Input))
				{
					HandleEmptyInput(Input);
					continue;
				}
				IsActive = SelectionMenu(Input);
			}
		}
		private bool SelectionMenu(string Input)
		{
			UserService _userService = new UserService();

			switch (Input)
			{
				case "1":
					LoginUI login = new LoginUI();
					login.ShowMenu();
					break;
				case "2":
					RegisterAccountUI register = new RegisterAccountUI();
					register.ShowMenu();
					break;
				default:
					Console.WriteLine("Invalid selection. Please try again.\n");
					break;
			}

			return true;
		}

	}


}