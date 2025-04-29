using System;
using System.Reflection.Metadata.Ecma335;
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
			return true;
			/*
				rewrite this to use the new user registration method
				switch on enum - if 1 = customer, 2 = deliverer, 3 = client
			    remove the switch statement and use the enum to call the correct method
			    this will be to assign additional field variables 
				- if customer - address, delivery location

			 */

			//switch (Input)
			//{
			//case "1":
			//try
			//{
			//bool isValid = false;

			//ClearScreen();
			//DisplayOutput("Registering User - Customer");
			//DisplayOutput("Please enter your details -\n");

			//while (!isValid)
			//{
			//// Check if the name is valid
			//DisplayOutput("Please enter your name: ");
			//User.Name = GetInput();

			//if (string.IsNullOrEmpty(User.Name))
			//{
			//DisplayError("Name cannot be empty");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}

			//// Check if the name is valid
			//DisplayOutput("Please enter your age (18-100): ");
			//User.Age = GetInput();

			//if (string.IsNullOrEmpty(User.Age))
			//{
			//DisplayError("Age cannot be empty");
			//}
			//if (!int.TryParse(User.Age, out int age) || age < 18 || age > 100)
			//{
			//DisplayError("Invalid age. Age must be between 18 and 100.");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}

			//// Check if the user already exists
			//DisplayOutput("Please enter your email address: ");
			//User.Email = GetInput();

			//if (string.IsNullOrEmpty(User.Email))
			//{
			//DisplayError("Email cannot be empty");
			//}
			//if (userService.CheckEmailExists(User.Email))
			//{
			//DisplayError("Email already registered. Please use a different email.");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}
			//// Check if the email is already registered
			//DisplayOutput("Please enter your mobile phone number: ");
			//User.MobileNumber = GetInput();

			//if (string.IsNullOrEmpty(User.MobileNumber))
			//{
			//DisplayError("Mobile Number cannot be empty");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}

			//// Check if the mobile number is already registered
			//DisplayOutput("Username: ");
			//User.Username = GetInput();

			//if (string.IsNullOrEmpty(User.Username))
			//{
			//DisplayError("Username cannot be empty");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}
			//// Check if the username is already registered
			//if (userService.CheckUsernameExists(User.Username))
			//{
			//DisplayError("Username already taken. Please choose a different one.");
			//}

			//// check if the password is valid
			//DisplayOutput(
			//"Your password must:" +
			//"\n- Be at least 8 characters long" +
			//"\n- contain a number" +
			//"\n- contain a lowercase letter" +
			//"\n- contain an uppercase letter\n" +
			//"\nPlease enter a password:"
			//);

			//// Check if the password is valid
			//DisplayOutput("Password: ");
			//User.Password = GetInput();
			//if (string.IsNullOrEmpty(User.Password))
			//{
			//DisplayError("Password cannot be empty");
			//}
			//if (User.Password.Length < 8 || !User.Password.Any(char.IsDigit) ||
			//!User.Password.Any(char.IsLower) || !User.Password.Any(char.IsUpper))
			//{
			//DisplayError("Password must be at least 8 characters long, contain a number, " +
			//"and include both uppercase and lowercase letters.");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}
			//// Check if the password is valid 
			//DisplayOutput($"Confirm Password: {User.Password}\n" +
			//"Type Y-Yes or N-No... ");
			//string confirmPassword = GetInput();
			//if (confirmPassword == "Y" && confirmPassword == "y")
			//{
			//isValid = true; // Continue to the next input
			//}
			//if (confirmPassword == "N" && confirmPassword == "n")
			//{
			//Console.WriteLine("Please re-enter your password: ");
			//}
			//else
			//{
			//isValid = true; // Continue to the next input
			//}





			////// Check if the address is valid
			////DisplayOutput("Address: ");
			////User.Address = GetInput();
			////if (string.IsNullOrEmpty(User.Address))
			////{
			////DisplayError("Address cannot be empty");
			////WaitForKeyPress();
			////return true;
			////}

			////// Check if the delivery location is valid
			////DisplayOutput("Delivery Location: ");
			////DisplayOutput("X Coordinate: ");
			////string xInput = GetInput();
			////if (!double.TryParse(xInput, out double x))
			////{
			////DisplayError("Invalid X coordinate");
			////WaitForKeyPress();
			////return true;
			////}

			////// Check if the delivery location is valid
			////DisplayOutput("Y Coordinate: ");
			////string yInput = GetInput();
			////if (!double.TryParse(yInput, out double y))
			////{
			////DisplayError("Invalid Y coordinate");
			////WaitForKeyPress();
			////return true;
			////}

			////User.DeliveryLocation = new Location(x, y);
			////User.UserType = UserType.Customer;
			////userService.RegisterUser(User);
			////WaitForKeyPress();

			////// Return to main menu after registration
			////return false; // Return to main menu
			//}
			//}
			//catch (Exception ex)
			//{
			//DisplayError($"Error during registration: {ex.Message}");
			//WaitForKeyPress();
			//return true; // Return to the registration menu
			//}
			//return false; // Return to main menu
			//case "2":
			//DisplayOutput("Registering as a Deliverer...");
			//// Implement deliverer registration
			//WaitForKeyPress();
			//break;
			//case "3":
			//DisplayOutput("Registering as a Client...");
			//// Implement client registration
			//WaitForKeyPress();
			//break;
			//case "4":
			//return false; // Return to main menu
			//default:
			//DisplayError("Invalid selection. Please try again.\n");
			//WaitForKeyPress();
			//break;
			//}
			//return true;
		}
	}
}