using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.Utils;

namespace ArribaEats.UI
{
    /// <summary>
    /// User interface for the ArribaEats application
    /// </summary>
    public class ArribaEatsUI
    {
        private readonly IArribaEatsService _service;
        /// <summary>
        /// Creates a new instance of the ArribaEats user interface
        /// </summary>
        /// <param name="service">The service to use</param>
        public ArribaEatsUI(IArribaEatsService service)
        {
            _service = service;
        }

        /// <summary>
        /// Runs the application
        /// </summary>
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Title color
            Console.WriteLine("Welcome to Arriba Eats!");
            Console.ResetColor();

            bool exit = false;
            while (!exit)
            {
                if (_service.CurrentUser == null)
                {
                    exit = ShowStartupMenu();
                }
                else if (_service.CurrentUser is Customer)
                {
                    ShowCustomerMenu();
                }
                else if (_service.CurrentUser is Deliverer)
                {
                    ShowDelivererMenu();
                }
                else if (_service.CurrentUser is Client)
                {
                    ShowClientMenu();
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow; // Title color
            Console.WriteLine("Thank you for using Arriba Eats!");
            Console.ResetColor();
        }

        /// <summary>
        /// Shows the startup menu
        /// </summary>
        /// <returns>True if the user chose to exit, false otherwise</returns>
        private bool ShowStartupMenu()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Login as a registered user");
            Console.WriteLine("2: Register as a new user");
            Console.WriteLine("3: Exit");

            int choice = GetMenuChoice(1, 3);

            switch (choice)
            {
                case 1:
                    LoginUser();
                    return false;
                case 2:
                    RegisterUserMenu();
                    return false;
                case 3:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Shows the user registration menu
        /// </summary>
        private void RegisterUserMenu()
        {
            Console.WriteLine("Which type of user would you like to register as?");
            Console.WriteLine("1: Customer");
            Console.WriteLine("2: Deliverer");
            Console.WriteLine("3: Client");
            Console.WriteLine("4: Return to the previous menu");

            int choice = GetMenuChoice(1, 4);

            if (choice == 4)
            {
                return;
            }

            UserType userType;
            switch (choice)
            {
                case 1:
                    userType = UserType.Customer;
                    break;
                case 2:
                    userType = UserType.Deliverer;
                    break;
                case 3:
                    userType = UserType.Client;
                    break;
                default:
                    return;
            }

            RegisterUser(userType);
        }

        /// <summary>
        /// Shows the customer menu
        /// </summary>
        private void ShowCustomerMenu()
        {
            Customer customer = (Customer)_service.CurrentUser;

            Console.WriteLine($"Welcome back, {customer.Name}!");
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Display your user information");
            Console.WriteLine("2: Select a list of restaurants to order from");
            Console.WriteLine("3: See the status of your orders");
            Console.WriteLine("4: Rate a restaurant you've ordered from");
            Console.WriteLine("5: Logout");

            int choice = GetMenuChoice(1, 5);

            switch (choice)
            {
                case 1:
                    DisplayUserInfo(customer);
                    break;
                case 2:
                    ViewRestaurants(customer);
                    break;
                case 3:
                    ViewCurrentOrderStatus(customer);
                    break;
                case 4:
                    WriteReview(customer);
                    break;
                case 5:
                    Logout();
                    break;
            }
        }

        /// <summary>
        /// Shows the deliverer menu
        /// </summary>
        private void ShowDelivererMenu()
        {
            Deliverer deliverer = (Deliverer)_service.CurrentUser;

            Console.WriteLine($"Welcome, {deliverer.Name}!");
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: View available orders");
            Console.WriteLine("2: View my information");
            Console.WriteLine("3: Update delivery status");
            Console.WriteLine("4: Logout");

            int choice = GetMenuChoice(1, 4);

            switch (choice)
            {
                case 1:
                    ViewAvailableOrders(deliverer);
                    break;
                case 2:
                    DisplayUserInfo(deliverer);
                    break;
                case 3:
                    UpdateDeliveryStatus(deliverer);
                    break;
                case 4:
                    Logout();
                    break;
            }
        }

        /// <summary>
        /// Shows the client menu
        /// </summary>
        private void ShowClientMenu()
        {
            Client client = (Client)_service.CurrentUser;

            Console.WriteLine($"Welcome, {client.Name}!");
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: View my restaurant information");
            Console.WriteLine("2: View my information");
            Console.WriteLine("3: Add item to menu");
            Console.WriteLine("4: View orders");
            Console.WriteLine("5: Update order status");
            Console.WriteLine("6: Logout");

            int choice = GetMenuChoice(1, 6);

            switch (choice)
            {
                case 1:
                    DisplayRestaurantInfo(client.Restaurant);
                    break;
                case 2:
                    DisplayUserInfo(client);
                    break;
                case 3:
                    AddMenuItem(client);
                    break;
                case 4:
                    ViewRestaurantOrders(client.Restaurant);
                    break;
                case 5:
                    UpdateRestaurantOrderStatus(client);
                    break;
                case 6:
                    Logout();
                    break;
            }
        }

        /// <summary>
        /// Logs out the current user
        /// </summary>
        private void Logout()
        {
            _service.Logout();
            Console.WriteLine("You are now logged out.");
        }

        /// <summary>
        /// Gets a menu choice from the user
        /// </summary>
        /// <param name="min">The minimum valid choice</param>
        /// <param name="max">The maximum valid choice</param>
        /// <returns>The user's choice</returns>
        private int GetMenuChoice(int min, int max)
        {
            while (true)
            {
                Console.WriteLine($"Please enter a choice between {min} and {max}: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }

                Console.WriteLine($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="userType">The type of user to register</param>
        private void RegisterUser(UserType userType)
        {
            // Get common user details
            string name = GetValidInput("Please enter your name:", InputValidator.ValidateName, "Invalid name.");
            int age = GetValidIntInput("Please enter your age (18-100):", a => InputValidator.ValidateAge(a), "Invalid age.");
            string email = GetUniqueEmail();
            string mobile = GetValidInput("Please enter your mobile phone number:", InputValidator.ValidateMobile, "Invalid phone number.");

            // Show password requirements
            Console.WriteLine("Your password must:");
            Console.WriteLine("- be at least 8 characters long");
            Console.WriteLine("- contain a number");
            Console.WriteLine("- contain a lowercase letter");
            Console.WriteLine("- contain an uppercase letter");

            string password = GetValidInput("Please enter a password:", InputValidator.ValidatePassword, "Invalid password.");


            // Confirm password
            string confirmPassword = GetValidInput("Please confirm your password:", p => p == password, "Passwords do not match.");

            bool success = false;

            switch (userType)
            {
                case UserType.Customer:
                    Location location = GetValidLocation("Please enter your location (in the form of X,Y):", "Invalid location format. Please use format X,Y where X and Y are integers.");
                    success = _service.RegisterCustomer(name, age, email, mobile, password, location);
                    break;
                case UserType.Deliverer:
                    string licencePlate = GetValidInput("Please enter your licence plate:", InputValidator.ValidateLicencePlate, "Invalid licence plate.");
                    success = _service.RegisterDeliverer(name, age, email, mobile, password, licencePlate);
                    break;
                case UserType.Client:
                    string restaurantName = GetValidInput("Please enter your restaurant name:", InputValidator.ValidateRestaurantName, "Invalid restaurant name.");

                    Console.WriteLine("Please select your restaurant style:");
                    Console.WriteLine("1: Italian");
                    Console.WriteLine("2: French");
                    Console.WriteLine("3: Chinese");
                    Console.WriteLine("4: Japanese");
                    Console.WriteLine("5: American");
                    Console.WriteLine("6: Australian");

                    int styleChoice = GetMenuChoice(1, 6);
                    FoodStyle style = (FoodStyle)styleChoice;

                    Location restaurantLocation = GetValidLocation("Please enter your restaurant location (in the form of X,Y):", "Invalid location format. Please use format X,Y where X and Y are integers.");

                    success = _service.RegisterClient(name, age, email, mobile, password, restaurantName, style, restaurantLocation);
                    break;
            }

            if (success)
            {
                Console.WriteLine($"You have been successfully registered as a {userType.ToString().ToLower()}, {name}!");
            }
            else
            {
                Console.WriteLine("Registration failed. Please try again.");
            }
        }

        /// <summary>
        /// Gets a valid email that is not already in use
        /// </summary>
        /// <returns>A valid email</returns>
        private string GetUniqueEmail()
        {
            while (true)
            {
                string email = GetValidInput("Please enter your email address:", InputValidator.ValidateEmail, "Invalid email address.");

                if (_service.IsEmailInUse(email))
                {
                    Console.WriteLine("This email address is already in use.");
                }
                else
                {
                    return email;
                }
            }
        }

        /// <summary>
        /// Logs in a user
        /// </summary>
        private void LoginUser()
        {
            string email = GetValidInput("Email:", InputValidator.ValidateEmail, "Invalid email address.");
            string password = GetValidInput("Password:", p => true, null); // Any password format is accepted for login attempt

            bool success = _service.Login(email, password);

            if (!success)
            {
                Console.WriteLine("Login failed. Invalid email or password.");
            }
        }

        /// <summary>
        /// Displays user information
        /// </summary>
        /// <param name="user">The user to display information for</param>
        private void DisplayUserInfo(User user)
        {
            var info = user.GetUserInfo();

            Console.WriteLine("User Information:");
            foreach (var item in info)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        /// <summary>
        /// Displays restaurant information
        /// </summary>
        /// <param name="restaurant">The restaurant to display information for</param>
        private void DisplayRestaurantInfo(Restaurant restaurant)
        {
            var info = restaurant.GetRestaurantInfo();

            Console.WriteLine("Restaurant Information:");
            foreach (var item in info)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        /// <summary>
        /// Views the restaurants available to a customer
        /// </summary>
        /// <param name="customer">The customer viewing the restaurants</param>
        private void ViewRestaurants(Customer customer)
        {
            if (_service.Restaurants.Count == 0)
            {
                Console.WriteLine("There are no restaurants available.");
                return;
            }

            Console.WriteLine("How would you like to sort the restaurants?");
            Console.WriteLine("1: By name");
            Console.WriteLine("2: By distance");
            Console.WriteLine("3: By style");
            Console.WriteLine("4: By rating");

            int choice = GetMenuChoice(1, 4);

            string sortBy;
            switch (choice)
            {
                case 1:
                    sortBy = "name";
                    break;
                case 2:
                    sortBy = "distance";
                    break;
                case 3:
                    sortBy = "style";
                    break;
                case 4:
                    sortBy = "rating";
                    break;
                default:
                    sortBy = "name";
                    break;
            }

            var restaurants = _service.GetSortedRestaurants(customer, sortBy);

            Console.WriteLine("Available Restaurants:");
            for (int i = 0; i < restaurants.Count; i++)
            {
                var restaurant = restaurants[i];
                int distance = restaurant.Location.DistanceTo(customer.Location);

                Console.WriteLine($"{i + 1}: {restaurant.Name} ({restaurant.Style}) - {distance} units away - Rating: {restaurant.AverageRating:F1}");
            }

            Console.WriteLine("Enter the number of the restaurant to view, or 0 to go back:");
            int restaurantChoice = GetMenuChoice(0, restaurants.Count);

            if (restaurantChoice > 0)
            {
                var selectedRestaurant = restaurants[restaurantChoice - 1];
                ViewRestaurantDetails(customer, selectedRestaurant);
            }
        }

        /// <summary>
        /// Views the details of a restaurant
        /// </summary>
        /// <param name="customer">The customer viewing the restaurant</param>
        /// <param name="restaurant">The restaurant to view</param>
        private void ViewRestaurantDetails(Customer customer, Restaurant restaurant)
        {
            Console.WriteLine($"Restaurant: {restaurant.Name}");
            Console.WriteLine($"Style: {restaurant.Style}");
            Console.WriteLine($"Location: {restaurant.Location}");
            Console.WriteLine($"Distance: {restaurant.Location.DistanceTo(customer.Location)} units");
            Console.WriteLine($"Rating: {restaurant.AverageRating:F1} stars");

            if (restaurant.Menu.Count == 0)
            {
                Console.WriteLine("This restaurant has no menu items.");
            }
            else
            {
                Console.WriteLine("Menu:");
                for (int i = 0; i < restaurant.Menu.Count; i++)
                {
                    var item = restaurant.Menu[i];
                    Console.WriteLine($"{i + 1}: {item.Name} - ${item.Price:F2}");
                }

                Console.WriteLine("Would you like to place an order? (y/n)");
                string response = Console.ReadLine().ToLower();

                if (response == "y" || response == "yes")
                {
                    PlaceOrder(customer, restaurant);
                }
            }
        }

        /// <summary>
        /// Places an order at a restaurant
        /// </summary>
        /// <param name="customer">The customer placing the order</param>
        /// <param name="restaurant">The restaurant to order from</param>
        private void PlaceOrder(Customer customer, Restaurant restaurant)
        {
            if (restaurant.Menu.Count == 0)
            {
                Console.WriteLine("This restaurant has no menu items to order.");
                return;
            }

            var order = _service.CreateOrder(customer, restaurant);

            bool ordering = true;
            while (ordering)
            {
                Console.WriteLine("Select a menu item to add to your order (0 to finish):");
                for (int i = 0; i < restaurant.Menu.Count; i++)
                {
                    var item = restaurant.Menu[i];
                    Console.WriteLine($"{i + 1}: {item.Name} - ${item.Price:F2}");
                }

                int choice = GetMenuChoice(0, restaurant.Menu.Count);

                if (choice == 0)
                {
                    if (order.Items.Count == 0)
                    {
                        Console.WriteLine("Order cancelled - no items selected.");
                        return;
                    }

                    ordering = false;
                }
                else
                {
                    var menuItem = restaurant.Menu[choice - 1];
                    int quantity = GetValidIntInput("How many would you like?", q => q > 0, "Invalid quantity. Please enter a positive number.");

                    _service.AddItemToOrder(order, menuItem, quantity);
                    Console.WriteLine($"Added {quantity}x {menuItem.Name} to your order.");
                }
            }

            Console.WriteLine("Order Summary:");
            Console.WriteLine(order.GetItemsAsString());
            Console.WriteLine($"Total: ${order.TotalPrice:F2}");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine("Your order has been placed!");
        }

        /// <summary>
        /// Views the current order status for a customer
        /// </summary>
        /// <param name="customer">The customer viewing their order status</param>
        private void ViewCurrentOrderStatus(Customer customer)
        {
            var activeOrders = customer.Orders.Where(o => o.Status != OrderStatus.Delivered).ToList();

            if (activeOrders.Count == 0)
            {
                Console.WriteLine("You have no active orders.");
                return;
            }

            Console.WriteLine("Your active orders:");
            for (int i = 0; i < activeOrders.Count; i++)
            {
                var order = activeOrders[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} from {order.Restaurant.Name} - Status: {order.Status}");

                if (order.Status == OrderStatus.BeingDelivered && order.Deliverer != null)
                {
                    Console.WriteLine($"   Deliverer: {order.Deliverer.Name}");
                    Console.WriteLine($"   Licence Plate: {order.Deliverer.LicencePlate}");
                }
            }
        }

        /// <summary>
        /// Writes a review for a restaurant
        /// </summary>
        /// <param name="customer">The customer writing the review</param>
        private void WriteReview(Customer customer)
        {
            var deliveredOrders = customer.Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Select(o => o.Restaurant)
                .Distinct()
                .ToList();

            if (deliveredOrders.Count == 0)
            {
                Console.WriteLine("You have not completed any orders yet.");
                return;
            }

            Console.WriteLine("Select a restaurant to review:");
            for (int i = 0; i < deliveredOrders.Count; i++)
            {
                var restaurant = deliveredOrders[i];
                Console.WriteLine($"{i + 1}: {restaurant.Name}");
            }

            int choice = GetMenuChoice(1, deliveredOrders.Count);
            var selectedRestaurant = deliveredOrders[choice - 1];

            int rating = GetValidIntInput("Please enter your rating (1-5 stars):", r => r >= 1 && r <= 5, "Invalid rating. Please enter a number between 1 and 5.");
            string comment = GetValidInput("Please enter your comment:", c => !string.IsNullOrWhiteSpace(c), "Invalid comment. Please enter a non-empty comment.");

            var review = _service.CreateReview(customer, selectedRestaurant, rating, comment);

            Console.WriteLine("Thank you for your review!");
        }

        /// <summary>
        /// Views the available orders for a deliverer
        /// </summary>
        /// <param name="deliverer">The deliverer viewing the orders</param>
        private void ViewAvailableOrders(Deliverer deliverer)
        {
            if (deliverer.CurrentOrder != null)
            {
                Console.WriteLine("You already have an active delivery job.");
                return;
            }

            var availableOrders = _service.GetAvailableOrders();

            if (availableOrders.Count == 0)
            {
                Console.WriteLine("There are no orders available for delivery.");
                return;
            }

            Console.WriteLine("Available orders for delivery:");
            for (int i = 0; i < availableOrders.Count; i++)
            {
                var order = availableOrders[i];
                int restaurantDistance = order.Restaurant.Location.DistanceTo(deliverer.CurrentLocation);
                int customerDistance = order.Restaurant.Location.DistanceTo(order.Customer.Location);
                int totalDistance = restaurantDistance + customerDistance;

                Console.WriteLine($"{i + 1}: Order #{order.Id} from {order.Restaurant.Name}");
                Console.WriteLine($"   Restaurant at {order.Restaurant.Location} ({restaurantDistance} units away)");
                Console.WriteLine($"   Customer at {order.Customer.Location} ({customerDistance} units away)");
                Console.WriteLine($"   Total distance: {totalDistance} units");
            }

            Console.WriteLine("Enter the number of the order to accept, or 0 to go back:");
            int orderChoice = GetMenuChoice(0, availableOrders.Count);

            if (orderChoice > 0)
            {
                var selectedOrder = availableOrders[orderChoice - 1];
                bool success = _service.AcceptOrder(deliverer, selectedOrder);

                if (success)
                {
                    Console.WriteLine($"You have accepted order #{selectedOrder.Id}.");
                }
                else
                {
                    Console.WriteLine("Failed to accept the order.");
                }
            }
        }

        /// <summary>
        /// Updates the delivery status for a deliverer
        /// </summary>
        /// <param name="deliverer">The deliverer updating the status</param>
        private void UpdateDeliveryStatus(Deliverer deliverer)
        {
            if (deliverer.CurrentOrder == null)
            {
                Console.WriteLine("You are not currently delivering any orders.");
                return;
            }

            var order = deliverer.CurrentOrder;

            Console.WriteLine($"Current order: #{order.Id} from {order.Restaurant.Name} to {order.Customer.Name}");
            Console.WriteLine($"Current status: {order.Status}");

            if (order.Status == OrderStatus.Cooked)
            {
                Console.WriteLine("Would you like to mark yourself as at the restaurant? (y/n)");
                string response = Console.ReadLine().ToLower();

                if (response == "y" || response == "yes")
                {
                    deliverer.ArriveAtRestaurant();
                    Console.WriteLine("You have arrived at the restaurant.");
                    Console.WriteLine("Please wait for the restaurant to mark the order as being delivered.");
                }
            }
            else if (order.Status == OrderStatus.BeingDelivered)
            {
                Console.WriteLine("Would you like to mark the order as delivered? (y/n)");
                string response = Console.ReadLine().ToLower();

                if (response == "y" || response == "yes")
                {
                    deliverer.CompleteDelivery();
                    Console.WriteLine("Order has been marked as delivered.");
                }
            }
        }

        /// <summary>
        /// Views the orders for a restaurant
        /// </summary>
        /// <param name="restaurant">The restaurant to view orders for</param>
        private void ViewRestaurantOrders(Restaurant restaurant)
        {
            if (restaurant.Orders.Count == 0)
            {
                Console.WriteLine("You have no active orders.");
                return;
            }

            Console.WriteLine("Active orders:");
            for (int i = 0; i < restaurant.Orders.Count; i++)
            {
                var order = restaurant.Orders[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} - Status: {order.Status}");
                Console.WriteLine($"   Items: {order.Items.Count}");
                Console.WriteLine($"   Total: ${order.TotalPrice:F2}");

                if (order.Deliverer != null)
                {
                    Console.WriteLine($"   Deliverer: {order.Deliverer.Name}");
                    Console.WriteLine($"   Licence Plate: {order.Deliverer.LicencePlate}");
                }
            }
        }

        /// <summary>
        /// Updates the status of an order for a restaurant
        /// </summary>
        /// <param name="client">The client updating the order status</param>
        private void UpdateRestaurantOrderStatus(Client client)
        {
            var restaurant = client.Restaurant;

            if (restaurant.Orders.Count == 0)
            {
                Console.WriteLine("You have no active orders.");
                return;
            }

            Console.WriteLine("Select an order to update:");
            for (int i = 0; i < restaurant.Orders.Count; i++)
            {
                var order = restaurant.Orders[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} - Status: {order.Status}");
            }

            int choice = GetMenuChoice(1, restaurant.Orders.Count);
            var selectedOrder = restaurant.Orders[choice - 1];

            OrderStatus newStatus;
            bool validStatus = false;

            switch (selectedOrder.Status)
            {
                case OrderStatus.Ordered:
                    Console.WriteLine("Would you like to start cooking this order? (y/n)");
                    string startCooking = Console.ReadLine().ToLower();

                    if (startCooking == "y" || startCooking == "yes")
                    {
                        newStatus = OrderStatus.Cooking;
                        validStatus = true;
                    }
                    else
                    {
                        return;
                    }
                    break;
                case OrderStatus.Cooking:
                    Console.WriteLine("Would you like to mark this order as cooked? (y/n)");
                    string finishCooking = Console.ReadLine().ToLower();

                    if (finishCooking == "y" || finishCooking == "yes")
                    {
                        newStatus = OrderStatus.Cooked;
                        validStatus = true;
                    }
                    else
                    {
                        return;
                    }
                    break;
                case OrderStatus.Cooked:
                    if (selectedOrder.Deliverer != null)
                    {
                        Console.WriteLine("Would you like to mark this order as being delivered? (y/n)");
                        string startDelivery = Console.ReadLine().ToLower();

                        if (startDelivery == "y" || startDelivery == "yes")
                        {
                            newStatus = OrderStatus.BeingDelivered;
                            validStatus = true;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("This order does not have a deliverer assigned yet.");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("You cannot update the status of this order.");
                    return;
            }

            if (validStatus)
            {
                bool success = _service.UpdateOrderStatus(selectedOrder, newStatus);

                if (success)
                {
                    Console.WriteLine($"Order #{selectedOrder.Id} has been updated to {newStatus}.");
                }
                else
                {
                    Console.WriteLine("Failed to update the order status.");
                }
            }
        }

        /// <summary>
        /// Adds a menu item to a restaurant
        /// </summary>
        /// <param name="client">The client adding the menu item</param>
        private void AddMenuItem(Client client)
        {
            string name = GetValidInput("Please enter the item name:", n => !string.IsNullOrWhiteSpace(n), "Invalid item name. Please enter a non-empty name.");
            decimal price = GetValidPrice("Please enter the item price (format: $X.XX):", "Invalid price format. Please use format $X.XX and ensure the price is between $0.00 and $999.99.");

            bool success = _service.AddMenuItem(name, price);

            if (success)
            {
                Console.WriteLine($"Added {name} to the menu for ${price:F2}.");
            }
            else
            {
                Console.WriteLine("Failed to add the menu item.");
            }
        }

        /// <summary>
        /// Gets valid input from the user using a validation function
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="validator">The validation function</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid input</returns>
        private string GetValidInput(string prompt, Func<string, bool> validator, string errorMessage)
        {
            while (true)
            {
                if (!string.IsNullOrEmpty(prompt))
                {
                    Console.WriteLine(prompt);
                }

                string input = Console.ReadLine();

                if (validator(input))
                {
                    return input;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }

        /// <summary>
        /// Gets a valid integer input from the user using a validation function
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="validator">The validation function</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid integer input</returns>
        private int GetValidIntInput(string prompt, Func<int, bool> validator, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (int.TryParse(input, out int value) && validator(value))
                {
                    return value;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }
        }

        /// <summary>
        /// Gets a valid location from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid location</returns>
        private Location GetValidLocation(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                if (InputValidator.ValidateLocation(input, out Location location))
                {
                    return location;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid location.");
                }
            }
        }

        /// <summary>
        /// Gets a valid price from the user
        /// </summary>
        /// <param name="prompt">The prompt to display</param>
        /// <param name="errorMessage">The error message to display</param>
        /// <returns>The valid price</returns>
        private decimal GetValidPrice(string prompt, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();

                // Add $ prefix if missing
                if (!string.IsNullOrEmpty(input) && !input.StartsWith("$"))
                {
                    input = "$" + input;
                }

                if (InputValidator.ValidateItemPrice(input, out decimal price))
                {
                    return price;
                }

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
                else
                {
                    Console.WriteLine("Invalid price format. Please use format $X.XX and ensure the price is between $0.00 and $999.99.");
                }
            }
        }
    }
}