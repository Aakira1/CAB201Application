using System;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;

namespace Arriba_Eats_App.UI.MenuUI
{
	/// <summary>
	/// RegisterAccountUI class is responsible for displaying the registration menu of the application.
	/// </summary>
	class RegisterAccountUI : MenuUI
	{
		private UserService userService = new UserService();

		public override void ShowMenu(bool IsActive)
		{
			ClearScreen();
			DisplayOutput("Register Account Menu\n");
			DisplayOutput("1. Customer");
			DisplayOutput("2. Deliverer");
			DisplayOutput("3. Client");
			DisplayOutput("4. Return to the previous menu");
			DisplayOutput("Please Enter a choice between 1 and 4:");

			string Input = GetInput();

			if (string.IsNullOrEmpty(Input))
			{
				DisplayError(HandleEmptyInput(Input));
				WaitForKeyPress();
				return;
			}
			IsActive = SelectionMenu(Input);
		}
		public override bool SelectionMenu(string Input)
		{
			switch (Input)
			{
				case "1":
					try
					{
						ClearScreen();
						DisplayOutput("Registering User - Customer");
						DisplayOutput("Please enter your details:");

						DisplayOutput("Name: ");
						User.Name = GetInput();
						if (string.IsNullOrEmpty(User.Name))
						{
							DisplayError("Name cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Email: ");
						User.Email = GetInput();
						if (string.IsNullOrEmpty(User.Email))
						{
							DisplayError("Email cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Mobile Number: ");
						User.MobileNumber = GetInput();
						if (string.IsNullOrEmpty(User.MobileNumber))
						{
							DisplayError("Mobile Number cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Username: ");
						User.Username = GetInput();
						if (string.IsNullOrEmpty(User.Username))
						{
							DisplayError("Username cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Password: ");
						User.Password = GetSecuredInput(string.Empty);
						if (string.IsNullOrEmpty(User.Password))
						{
							DisplayError("Password cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Age: ");
						User.Age = GetInput();
						if (string.IsNullOrEmpty(User.Age))
						{
							DisplayError("Age cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Address: ");
						User.Address = GetInput();
						if (string.IsNullOrEmpty(User.Address))
						{
							DisplayError("Address cannot be empty");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Delivery Location: ");
						DisplayOutput("X Coordinate: ");
						string xInput = GetInput();
						if (!double.TryParse(xInput, out double x))
						{
							DisplayError("Invalid X coordinate");
							WaitForKeyPress();
							return true;
						}

						DisplayOutput("Y Coordinate: ");
						string yInput = GetInput();
						if (!double.TryParse(yInput, out double y))
						{
							DisplayError("Invalid Y coordinate");
							WaitForKeyPress();
							return true;
						}

						User.DeliveryLocation = new Location(x, y);
						User.UserType = UserType.Customer;
						userService.RegisterUser(User);
						WaitForKeyPress();

						// Return to main menu after registration
						return false; // Return to main menu
					}
					catch (Exception ex)
					{
						DisplayError($"Error during registration: {ex.Message}");
						WaitForKeyPress();
						return true;
					}
				case "2":
					DisplayOutput("Registering as a Deliverer...");
					// Implement deliverer registration
					WaitForKeyPress();
					break;
				case "3":
					DisplayOutput("Registering as a Client...");
					// Implement client registration
					WaitForKeyPress();
					break;
				case "4":
					return false; // Return to main menu
				default:
					DisplayError("Invalid selection. Please try again.\n");
					WaitForKeyPress();
					break;
			}
			return true;
		}
	}
}