using System;
using System.ComponentModel.DataAnnotations;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;
using Arriba_Eats_App.UI;
using Arriba_Eats_App.UX;

namespace Arriba_Eats_App.UI.MenuUI
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
			UserType = EUserType.Customer,
			DeliveryLocation = new Location(0, 0)
		};

		public DeliveryPerson deliveryPerson { get; set; } = new DeliveryPerson()
		{
			Name = string.Empty,
			Age = string.Empty,
			Address = string.Empty,
			Email = string.Empty,
			MobileNumber = string.Empty,
			Username = string.Empty,
			Password = string.Empty,
			VehicleType = string.Empty,
			LicensePlate = string.Empty,
			VehicleColor = string.Empty,
			CurrentLocation = new Location(0, 0),
			CurrentOrder = null,
			CompletedDeliveries = new List<Order>(),
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
		public string GetSecuredInput(string prompt, bool isActive)
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
				if (isActive)
				{
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
				}
				else
				{
					if (key == ConsoleKey.Backspace && password.Length > 0)
					{
						Console.Write("\b \b");
						password = password[0..^1];
					}
					else if (!char.IsControl(keyInfo.KeyChar))
					{
						Console.Write(keyInfo.KeyChar);
						password += keyInfo.KeyChar;
					}
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
		public virtual void ShowMenu(bool isActive, EUserType userType)
		{
			Console.WriteLine("Default Menu");
			userType = EUserType.None;
		}
		public virtual bool SelectionMenu(string input, EUserType userType)
		{
			return false;
		}
	}
}