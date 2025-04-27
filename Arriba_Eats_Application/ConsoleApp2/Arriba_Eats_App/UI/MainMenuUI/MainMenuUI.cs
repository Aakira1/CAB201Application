using System;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;
using Arriba_Eats_App.UX;

namespace Arriba_Eats_App.Arriba_Eats_App.UI.MainMenuUI
{
	/// <summary>
	/// MenuUI class is the base class for all menu-related user interfaces in the application.
	/// </summary>
	class MenuUI : IUserInterface
	{
		public User User { get; set; } = new User()
		{
			Username = string.Empty,
			Address = string.Empty,
			Age = string.Empty,
			Email = string.Empty,
			MobileNumber = string.Empty,
			Name = string.Empty,
			Password = string.Empty,
			UserType = UserType.Customer,
			DeliveryLocation = new Location(0, 0)
		};

		public void ClearScreen()
		{
			Console.Clear();
		}

		public void DisplayError(string message)
		{
			var originalColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ForegroundColor = originalColor;
		}

		public void DisplayOutput(string message)
		{
			Console.WriteLine(message);
		}

		public string GetInput()
		{
			string input = Console.ReadLine() ?? string.Empty;
			return input;
		}

		// Fixed implementation of secure input (password masking)
		public string GetSecuredInput(string prompt)
		{
			if (!string.IsNullOrEmpty(prompt))
			{
				Console.Write(prompt);
			}

			string password = string.Empty;
			ConsoleKey key;

			do
			{
				var keyInfo = Console.ReadKey(intercept: true);
				key = keyInfo.Key;

				if (key == ConsoleKey.Backspace && password.Length > 0)
				{
					Console.Write("\b \b");
					password = password[0..^1];
				}
				else if (!char.IsControl(keyInfo.KeyChar))
				{
					Console.Write("*");
					password += keyInfo.KeyChar;
				}
			} while (key != ConsoleKey.Enter);

			Console.WriteLine();
			return password;
		}

		public void WaitForKeyPress()
		{
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey(true);
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
				IsActive = SelectionMenu(Input);
			}

			DisplayOutput("Thankyou for using Arriba Eats!");
		}

		public override bool SelectionMenu(string Input)
		{
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