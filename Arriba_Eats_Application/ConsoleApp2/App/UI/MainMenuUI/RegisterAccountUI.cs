using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Services.UserServices;
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
			//bool isValid = false;

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
					userService.RegisterDeliveryPerson(GetDeliveryDriverDetails(Input, userType)); // register delivery person
					break;
				case EUserType.RestaurantOwner:
					userService.RegisterClient(GetRestaurantOwnerDetails(Input, userType));
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
			Console.Clear();
			
			User user = new User()
			{
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

			DisplayOutput("Enter your name:");
			user.Name = userService.ValidName(GetInput());

			// Make sure age is a number between 18 and 100
			DisplayOutput("Enter your age (18-100):");
			user.Age = userService.ValidAge(GetInput());

			DisplayOutput("Enter your Mobile Number:");
			user.MobileNumber = userService.ValidMobileNo(GetInput());

			DisplayOutput("Enter your Email address:");
			user.Email = userService.ValidEmail(GetInput());

			// Password validation
			DisplayOutput(
			"Your password must: \n"
			+ "- be at least 8 characters long \n"
			+ "- contain a number \n"
			+ "- contain a lowercase letter \n"
			+ "- contain an uppercase letter \n"
			+ "Please enter a Password:");

			user.Password = userService.ValidPassword(GetInput(), false, user);

			DisplayOutput("Confirm your password:");
			string confirmPassword = userService.ValidPassword(GetInput(), true, user);

			// Set delivery location
			DisplayOutput("Please enter your delivery location (in the form X,Y)");
			DisplayOutput("X: ");
			int X = userService.ValidLocation(int.Parse(GetInput()));
			DisplayOutput("Y: ");
			int Y = userService.ValidLocation(int.Parse(GetInput()));

			// Initialize DeliveryLocation properly
			user.DeliveryLocation = new Location(X,Y);

			// Display confirmation
			DisplayOutput("\nConfirm User Details:");
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
		private DeliveryPerson GetDeliveryDriverDetails (string input, EUserType userType)
		{
			Console.Clear();

			DeliveryPerson deliveryPerson = new DeliveryPerson()
			{
				Name = User.Name,
				Age = User.Age,
				Address = User.Address,
				Email = User.Email,
				MobileNumber = User.MobileNumber,
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

			DisplayOutput("Enter your name:");
			deliveryPerson.Name = GetInput();

			

			DisplayOutput("Enter your age (18-100):");
			deliveryPerson.Age = GetInput();

			DisplayOutput("Enter your Email address:");
			deliveryPerson.Email = GetInput();

			DisplayOutput("Enter your Mobile Number:");
			deliveryPerson.MobileNumber = GetInput();

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
				return GetDeliveryDriverDetails(input, userType);
			}

			DisplayOutput($"You have successfully registered as a delivery person, {deliveryPerson.Name}!");

			WaitForKeyPress();
			return deliveryPerson;
		}
		private Restaurant GetRestaurantOwnerDetails(string input, EUserType userType)
		{
			Console.Clear();
			Restaurant restaurantOwner = new Restaurant()
			{
				Name = User.Name,
				Age = User.Age,
				Address = User.Address,
				Email = User.Email,
				MobileNumber = User.MobileNumber,
				Password = User.Password,
				RestaurantName = string.Empty,
				CuisineType = string.Empty,
				Location = new Location(0, 0),
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				UserType = EUserType.None,
				Id = Guid.NewGuid(),
				PhoneNumber = string.Empty,
			};


			restaurantOwner.UserType = userType; // Set the user type
			DisplayOutput($"Registering {userType} - Please enter your details:");
			DisplayOutput("Enter your name:");
			restaurantOwner.Name = GetInput();
			DisplayOutput("Enter your age (18-100):");
			restaurantOwner.Age = GetInput();
			DisplayOutput("Enter your Mobile Number:");
			restaurantOwner.MobileNumber = GetInput();
			DisplayOutput("Enter your Address:");
			restaurantOwner.Address = GetInput();

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
				restaurantOwner.Password = GetInput();

				if (string.IsNullOrEmpty(restaurantOwner.Password))
				{
					DisplayError("Password cannot be empty.");
					continue;
				}

				if (restaurantOwner.Password.Length < 8)
				{
					DisplayError("Password must be at least 8 characters long.");
					continue;
				}

				if (!restaurantOwner.Password.Any(char.IsDigit))
				{
					DisplayError("Password must contain at least one number.");
					continue;
				}

				if (!restaurantOwner.Password.Any(char.IsLower))
				{
					DisplayError("Password must contain at least one lowercase letter.");
					continue;
				}

				if (!restaurantOwner.Password.Any(char.IsUpper))
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

				if (restaurantOwner.Password != confirmPassword)
				{
					DisplayError("Passwords do not match. Please try again.");
					continue;
				}

				break; // Only break when passwords match
			}

			// Set restaurant name
			DisplayOutput("Please enter your restaurant's name:");
			restaurantOwner.RestaurantName = GetInput();

			// Set restaurant cuisine type
			DisplayOutput("Please select you restaurant's style:");
			DisplayOutput("1. Italian");
			DisplayOutput("2. French");
			DisplayOutput("3. Chinese");
			DisplayOutput("4. Japanese");
			DisplayOutput("5. American");
			DisplayOutput("6. Australian");
			DisplayOutput("Please enter a choice between 1 and 6:");

			int cuisineChoiceInt = int.Parse(Console.ReadLine()!);
			restaurantOwner.CuisineType = userService.ValidCuisineType(cuisineChoiceInt, GetInput()); 

			// Set restaurant location
			DisplayOutput("Please enter your restaurant location (in the form X,Y)");
			DisplayOutput("X: ");
			string restaurantX = GetInput();
			DisplayOutput("Y: ");
			string restaurantY = GetInput();

			// Initialize Restaurant Location properly
			restaurantOwner.Location = new Location(double.Parse(restaurantX), double.Parse(restaurantY));


			//// Initialize DeliveryLocation properly
			//restaurantOwner.Restaurant.Location = new Location(double.Parse(X), double.Parse(Y));
			// Display confirmation
			DisplayOutput("\nConfirm User Details:");
			DisplayOutput($"Name: {restaurantOwner.Name}");
			DisplayOutput($"Age: {restaurantOwner.Age}");
			DisplayOutput($"Email: {restaurantOwner.Email}");
			DisplayOutput($"Mobile Number: {restaurantOwner.MobileNumber}");
			DisplayOutput($"Address: {restaurantOwner.Address}");
			DisplayOutput($"Password: {restaurantOwner.Password}");
			DisplayOutput($"Restaurant Name: {restaurantOwner.RestaurantName}");
			DisplayOutput($"Cuisine Type: {restaurantOwner.CuisineType}");
			DisplayOutput($"Location: {restaurantX}, {restaurantY}");
			//DisplayOutput($"Location: {X}, {Y}");
			DisplayOutput($"User Type: {userType}");
			DisplayOutput("Press any key to confirm or 'R' to re-enter details.");
			ConsoleKeyInfo keyInfo = Console.ReadKey(true);

			if (keyInfo.Key == ConsoleKey.R)
			{
				DisplayOutput("Re-entering details...");
				return GetRestaurantOwnerDetails(input, userType);
			}

			DisplayOutput($"You have successfully registered as a client, {restaurantOwner.Name}!");
			WaitForKeyPress();
			return restaurantOwner;
		}
	}
}