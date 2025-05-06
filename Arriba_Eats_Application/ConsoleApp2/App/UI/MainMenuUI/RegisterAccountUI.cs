using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services;
using Arriba_Eats_App.UI.MenuUI.MainMenuUI;

namespace Arriba_Eats_App.UI.MenuUI
{
	/// <summary>
	/// RegisterAccountUI class is responsible for displaying the registration menu of the application.
	/// </summary>
	class RegisterAccountUI : MenuUI
	{
		private UserService userService = new UserService();

		public override void ShowMenu(bool IsActive, EUserType userType)
		{
			ClearScreen();
			DisplayOutput("Register Account Menu\n");
			DisplayOutput("1. Customer");
			DisplayOutput("2. Deliverer");
			DisplayOutput("3. Client (Restaraunt)");
			DisplayOutput("4. Return to the previous menu");
			DisplayOutput("Please Enter a choice between 1 and 4:");

			string Input = GetInput();

			if (string.IsNullOrEmpty(Input))
			{
				DisplayError(HandleEmptyInput(Input));
				WaitForKeyPress();
				return;
			}
			if (Input == "1")
			{
				SelectionMenu(Input, EUserType.Customer);
				DisplayOutput("Customer");
			}
			else if (Input == "2")
			{
				SelectionMenu(Input, EUserType.DeliveryPerson);
				DisplayOutput("Deliverer");
			}
			else if (Input == "3")
			{
				SelectionMenu(Input, EUserType.RestaurantOwner);
				DisplayOutput("Client");
			}
			else if (Input == "4")
			{
				DisplayOutput("Returning to the previous menu...");
				return;
			}
			else
			{
				DisplayError("Invalid input. Please try again.");
				WaitForKeyPress();
				return;
			}
		}
		public override bool SelectionMenu(string Input, EUserType userType)
		{
			bool isValid = false;

			//return true;
			/*
				rewrite this to use the new user registration method
				switch on enum - if 1 = customer, 2 = deliverer, 3 = client
			    remove the switch statement and use the enum to call the correct method
			    this will be to assign additional field variables 
				- if customer - address, delivery location

			 */

			if (string.IsNullOrEmpty(Input))
			{
				DisplayError(HandleEmptyInput(Input));
				WaitForKeyPress();
				return false;
			}


			// Assuming userType is an enum that indicates the type of user
			switch (userType)
			{
				case EUserType.Customer:
					userService.RegisterUser(GetRegisterDetails(Input, userType)); // register customer
					break;
				case EUserType.DeliveryPerson:
					userService.RegisterDeliveryPerson(GetDeliveryPerson(Input, userType)); // register delivery person
					break;
				case EUserType.RestaurantOwner:
					GetRegisterDetails(Input, EUserType.RestaurantOwner);
					DisplayOutput("additional Restaurant Owner questions...");
					break;
				default:
					DisplayError("Invalid user type.");
					break;
			}
			return false;
		}

		/// <summary>
		/// GetRegisterDetails method is responsible for collecting user details for registration.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="userType"></param>
		/// <returns></returns>
		private User GetRegisterDetails(string input, EUserType userType)

		{
			User user = new User()
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

			user.UserType = userType; // Set the user type

			DisplayOutput($"Registering {userType} - Please enter your details:");

			DisplayOutput("Enter your username:"); // Added to set the username
			user.Username = GetInput();

			DisplayOutput("Enter your name:");
			user.Name = GetInput();

			DisplayOutput("Enter your age (18-100):");
			user.Age = GetInput();

			DisplayOutput("Enter your Email address:");
			user.Email = GetInput();

			DisplayOutput("Enter your Mobile Number:");
			user.MobileNumber = GetInput();

			DisplayOutput("Enter your Address:");
			user.Address = GetInput();

			// Password validation
			DisplayOutput(
			"Your password must: \n"
			+ "- be at least 8 characters long \n"
			+ "- contain a number \n"
			+ "- contain a lowercase letter \n"
			+ "- contain an uppercase letter \n"
			+ "Please enter a Password:");

			while (true)
			{
				user.Password = GetInput();

				if (string.IsNullOrEmpty(user.Password))
				{
					DisplayError("Password cannot be empty.");
					continue;
				}

				if (user.Password.Length < 8)
				{
					DisplayError("Password must be at least 8 characters long.");
					continue;
				}

				if (!user.Password.Any(char.IsDigit))
				{
					DisplayError("Password must contain at least one number.");
					continue;
				}

				if (!user.Password.Any(char.IsLower))
				{
					DisplayError("Password must contain at least one lowercase letter.");
					continue;
				}

				if (!user.Password.Any(char.IsUpper))
				{
					DisplayError("Password must contain at least one uppercase letter.");
					continue;
				}

				break;
			}

			// Fix password confirmation loop
			DisplayOutput("Confirm your password:");
			string confirmPassword;
			while (true)
			{
				confirmPassword = GetInput();

				if (string.IsNullOrEmpty(confirmPassword))
				{
					DisplayError("Password confirmation cannot be empty.");
					continue;
				}

				if (user.Password != confirmPassword)
				{
					DisplayError("Passwords do not match. Please try again.");
					continue;
				}

				break; // Only break when passwords match
			}

			// Set delivery location
			DisplayOutput("Please enter your delivery location (in the form X,Y)");
			DisplayOutput("X: ");
			string X = GetInput();
			DisplayOutput("Y: ");
			string Y = GetInput();

			// Initialize DeliveryLocation properly
			user.DeliveryLocation = new Location(double.Parse(X), double.Parse(Y));

			// Display confirmation
			DisplayOutput("\nConfirm User Details:");
			DisplayOutput($"Username: {user.Username}");
			DisplayOutput($"Name: {user.Name}");
			DisplayOutput($"Age: {user.Age}");
			DisplayOutput($"Email: {user.Email}");
			DisplayOutput($"Mobile Number: {user.MobileNumber}");
			DisplayOutput($"Address: {user.Address}");
			DisplayOutput($"Password: {user.Password}");
			DisplayOutput($"Location: {X}, {Y}");
			DisplayOutput($"User Type: {userType}");
			DisplayOutput("Press any key to confirm or 'R' to re-enter details.");

			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			if (keyInfo.Key == ConsoleKey.R)
			{
				DisplayOutput("Re-entering details...");
				return GetRegisterDetails(input, userType);
			}

			DisplayOutput($"You have successfully registered as a delivery person, {user.Name}!");

			WaitForKeyPress();
			return user;
		}

		/// <summary>
		/// DeliveryPerson method is responsible for collecting delivery person details for registration.
		/// </summary>
		/// <param name="input"></param>
		/// <param name="userType"></param>
		/// <returns></returns>
		private DeliveryPerson GetDeliveryPerson (string input, EUserType userType)
		{
			DeliveryPerson deliveryPerson = new DeliveryPerson()
			{
				Name = User.Name,
				Age = User.Age,
				Address = User.Address,
				Email = User.Email,
				MobileNumber = User.MobileNumber,
				Username = User.Username,
				Password = User.Password,
				VehicleType = string.Empty,
				LicensePlate = string.Empty,
				VehicleColor = string.Empty,
				CurrentLocation = new Location(0,0),
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				UserType = EUserType.None,
				Id = Guid.NewGuid(),
				CurrentOrder = new List<Order>(),
				CompletedDeliveries = new List<Order>(),
			};

			deliveryPerson.UserType = userType; // Set the user type
			DisplayOutput($"Registering {userType} - Please enter your details:");

			DisplayOutput("Enter your username:"); // Added to set the username
			deliveryPerson.Username = GetInput();

			DisplayOutput("Enter your name:");
			deliveryPerson.Name = GetInput();

			DisplayOutput("Enter your age (18-100):");
			deliveryPerson.Age = GetInput();

			DisplayOutput("Enter your Email address:");
			deliveryPerson.Email = GetInput();

			DisplayOutput("Enter your Mobile Number:");
			deliveryPerson.MobileNumber = GetInput();

			DisplayOutput("Enter your Address:");
			deliveryPerson.Address = GetInput();


			// Password validation
			DisplayOutput(
			"Your password must: \n"
			+ "- be at least 8 characters long \n"
			+ "- contain a number \n"
			+ "- contain a lowercase letter \n"
			+ "- contain an uppercase letter \n"
			+ "Please enter a Password:");

			while (true)
			{
				deliveryPerson.Password = GetInput();

				if (string.IsNullOrEmpty(deliveryPerson.Password))
				{
					DisplayError("Password cannot be empty.");
					continue;
				}

				if (deliveryPerson.Password.Length < 8)
				{
					DisplayError("Password must be at least 8 characters long.");
					continue;
				}

				if (!deliveryPerson.Password.Any(char.IsDigit))
				{
					DisplayError("Password must contain at least one number.");
					continue;
				}

				if (!deliveryPerson.Password.Any(char.IsLower))
				{
					DisplayError("Password must contain at least one lowercase letter.");
					continue;
				}

				if (!deliveryPerson.Password.Any(char.IsUpper))
				{
					DisplayError("Password must contain at least one uppercase letter.");
					continue;
				}

				break;
			}

			// Fix password confirmation loop
			DisplayOutput("Confirm your password:");
			string confirmPassword;
			while (true)
			{
				confirmPassword = GetInput();

				if (string.IsNullOrEmpty(confirmPassword))
				{
					DisplayError("Password confirmation cannot be empty.");
					continue;
				}

				if (deliveryPerson.Password != confirmPassword)
				{
					DisplayError("Passwords do not match. Please try again.");
					continue;
				}

				break; // Only break when passwords match
			}

			// Set delivery location
			DisplayOutput("Please enter your delivery location (in the form X,Y)");
			DisplayOutput("X: ");
			string X = GetInput();
			DisplayOutput("Y: ");
			string Y = GetInput();

			// Initialize DeliveryLocation properly
			deliveryPerson.CurrentLocation = new Location(double.Parse(X), double.Parse(Y));

			// Vehicle details
			DisplayOutput("Enter your vehicle type:");
			deliveryPerson.VehicleType = GetInput();

			DisplayOutput("Enter your vehicle license plate:");
			deliveryPerson.LicensePlate = GetInput();

			DisplayOutput("Enter your vehicle color:");
			deliveryPerson.VehicleColor = GetInput();

			// Display confirmation
			DisplayOutput("\nConfirm User Details:");
			DisplayOutput($"Username: {deliveryPerson.Username}");
			DisplayOutput($"Name: {deliveryPerson.Name}");
			DisplayOutput($"Age: {deliveryPerson.Age}");
			DisplayOutput($"Email: {deliveryPerson.Email}");
			DisplayOutput($"Mobile Number: {deliveryPerson.MobileNumber}");
			DisplayOutput($"Address: {deliveryPerson.Address}");
			DisplayOutput($"Password: {deliveryPerson.Password}");
			DisplayOutput($"Location: {X}, {Y}");
			DisplayOutput($"User Type: {userType}");
			DisplayOutput($"Vehicle Type: {deliveryPerson.VehicleType}");
			DisplayOutput($"License Plate: {deliveryPerson.LicensePlate}");
			DisplayOutput($"Vehicle Color: {deliveryPerson.VehicleColor}");
			DisplayOutput("Press any key to confirm or 'R' to re-enter details.");

			ConsoleKeyInfo keyInfo = Console.ReadKey(true);
			if (keyInfo.Key == ConsoleKey.R)
			{
				DisplayOutput("Re-entering details...");
				return GetDeliveryPerson(input, userType);
			}

			DisplayOutput($"You have successfully registered as a delivery person, {deliveryPerson.Name}!");

			WaitForKeyPress();
			return deliveryPerson;
		}
		private Restaurant RestaurantOwner(string input, EUserType userType)
		{
			return RestaurantOwner(string.Empty, userType);
		}
	}

}