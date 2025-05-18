using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.Utils;
using ArribaEats.UI.Settings;

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

        private readonly InputChecks checks = new InputChecks();

        /// <summary>
        /// Runs the application
        /// </summary>
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to Arriba Eats!");
            Console.ResetColor();

            bool exit = false;
            while (!exit)
            {
                if (_service.CurrentUser == null)
                {
                    exit = ShowStartupMenu();
                }
                else
                {
                    switch (_service.CurrentUser)
                    {
                        case Customer customer:
                            ShowCustomerMenu(); // doesn't return until done
                            break;

                        case Deliverer deliverer:
                            ShowDelivererMenu();
                            break;

                        case Client client:
                            ShowClientMenu();
                            break;
                    }
                }
            }

            Console.WriteLine("Thank you for using Arriba Eats!");
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

            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Display your user information");
            Console.WriteLine("2: List orders available to deliver");
            Console.WriteLine("3: Arrived at restaurant to pick up order");
            Console.WriteLine("4: Mark this delivery as complete");
            Console.WriteLine("5: Logout");

            int choice = GetMenuChoice(1, 5);

            switch (choice)
            {
                case 1:
                    DisplayUserInfo(deliverer);
                    break;
                case 2:
                    ViewAvailableOrders(deliverer);
                    break;
                case 3:
                    UpdateDeliveryStatus(deliverer);
                    break;
                case 4:
                    UpdateDeliveryStatus(deliverer);
                    break;
                case 5:
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

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nPlease make a choice from the menu below:");
                Console.WriteLine("1: Display your user information");
                Console.WriteLine("2: Add item to restaurant menu");
                Console.WriteLine("3: See current orders");
                Console.WriteLine("4: Start cooking order");
                Console.WriteLine("5: Finish cooking order");
                Console.WriteLine("6: Handle deliverers who have arrived");
                Console.WriteLine("7: Log out");

                int choice = GetMenuChoice(1, 7);

                switch (choice)
                {
                    case 1:
                        DisplayUserInfo(client);
                        break;

                    case 2:
                        AddMenuItem(client);
                        break;

                    case 3:
                        ViewRestaurantOrders(client.Restaurant);
                        break;

                    case 4:
                        if (client.Restaurant.Orders.Count > 0)
                            client.StartCooking(client.Restaurant.Orders[0]);
                        else
                            Console.WriteLine("No orders available to start cooking.");
                        break;

                    case 5:
                        if (client.Restaurant.Orders.Count > 0)
                            client.FinishCooking(client.Restaurant.Orders[0]);
                        else
                            Console.WriteLine("No orders available to finish.");
                        break;

                    case 6:
                        if (client.Restaurant.Orders.Count > 0)
                            client.SetOrderBeingDelivered(client.Restaurant.Orders[0]);
                        else
                            Console.WriteLine("No orders available to assign to a deliverer.");
                        break;

                    case 7:
                        Logout();
                        exit = true;
                        break;
                }
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
                string input = Console.ReadLine()?.Trim();

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
            string name = checks.GetValidInput("Please enter your name:", InputValidator.ValidateName, "Invalid name.");
            int age = checks.GetValidIntInput("Please enter your age (18-100):", a => InputValidator.ValidateAge(a), "Invalid age.");
            string email = GetUniqueEmail();
            string mobile = checks.GetValidInput("Please enter your mobile phone number:", InputValidator.ValidateMobile, "Invalid phone number.");
            string password = checks.GetValidPassword();

            bool success = false;

            switch (userType)
            {
                case UserType.Customer:
                    Location location = checks.GetValidLocation();
                    success = _service.RegisterCustomer(name, age, email, mobile, password, location);
                    break;
                case UserType.Deliverer:
                    string licencePlate = checks.GetValidInput("Please enter your licence plate:", InputValidator.ValidateLicencePlate, "Invalid licence plate.");
                    success = _service.RegisterDeliverer(name, age, email, mobile, password, licencePlate);
                    break;
                case UserType.Client:
                    string restaurantName = checks.GetValidInput("Please enter your restaurant's name:", InputValidator.ValidateRestaurantName, "Invalid restaurant name.");

                    Console.WriteLine("Please select your restaurant's style:");
                    Console.WriteLine("1: Italian");
                    Console.WriteLine("2: French");
                    Console.WriteLine("3: Chinese");
                    Console.WriteLine("4: Japanese");
                    Console.WriteLine("5: American");
                    Console.WriteLine("6: Australian");

                    int styleChoice = GetMenuChoice(1, 6);
                    FoodStyle style = (FoodStyle)styleChoice;

                    Location restaurantLocation = checks.GetValidLocation();

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
                string email = checks.GetValidInput("Please enter your email address:", InputValidator.ValidateEmail, "Invalid email address.");

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
            string email = checks.GetValidInput("Email:", InputValidator.ValidateEmail, "Invalid email address.");
            string password = checks.GetValidInput("Password:", p => true, null); // Any password format is accepted for login attempt

            bool success = _service.Login(email, password);

            if(_service.CurrentUser == null || !success)
            {
                Console.WriteLine("Invalid email or password.");
                return;
            }
            Console.WriteLine($"Welcome back, {_service.CurrentUser.Name}!");
        }

        /// <summary>
        /// Displays user information
        /// </summary>
        /// <param name="user">The user to display information for</param>
        private void DisplayUserInfo(User user)
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Age: {user.Age}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Mobile: {user.Mobile}");

            if (user is Customer customer)
            {
                Console.WriteLine($"Location: {customer.Location}");
                Console.WriteLine($"You've made {customer.Orders.Count} order(s) and spent a total of ${customer.Orders.Sum(order => order.TotalPrice):F2} here.");
            }
            else if (user is Deliverer deliverer)
            {
                Console.WriteLine($"Licence plate: {deliverer.LicencePlate}");
            }
            else if (user is Client client)
            {
                Console.WriteLine($"Restaurant name: {client.Restaurant.Name}");
                Console.WriteLine($"Restaurant style: {client.Restaurant.Style}");
                Console.WriteLine($"Restaurant location: {client.Restaurant.Location}");
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
            //if (_service.Restaurants.Count == 0)
            //{
            //    Console.WriteLine("There are no restaurants available.");
            //    return;
            //}

            Console.WriteLine("How would you like the list of restaurants ordered?");
            Console.WriteLine("1: Sorted alphabetically by name");
            Console.WriteLine("2: Sorted by distance");
            Console.WriteLine("3: Sorted by style");
            Console.WriteLine("4: Sorted by average rating");
            Console.WriteLine("5: Return to the previous menu");

            int choice = GetMenuChoice(1, 5);

            // Exit early if user selects "Return"
            if (choice == 5)
            {
                return;
            }

            string sortBy = choice switch
            {
                1 => "name",
                2 => "distance",
                3 => "style",
                4 => "rating",
                _ => "name" // default fallback just in case
            };


            var restaurants = _service.GetSortedRestaurants(customer, sortBy);

            Console.WriteLine("You can order from the following restaurants:");
            Console.WriteLine("Restaurant Name       Loc     Dist  Style       Rating");

            for (int i = 0; i < restaurants.Count; i++)
            {
                var restaurant = restaurants[i];
                var location = $"{restaurant.Location.X},{restaurant.Location.Y}";
                int distance = restaurant.Location.DistanceTo(customer.Location);
                string ratingDisplay = restaurant.AverageRating > 0 ? restaurant.AverageRating.ToString("F1") : "-";

                Console.WriteLine($"{i + 1}: {restaurant.Name.PadRight(20)} {location.PadRight(7)} {distance.ToString().PadRight(6)} {restaurant.Style.ToString().PadRight(10)} {ratingDisplay}");
            }

            int returnOption = restaurants.Count + 1;
            Console.WriteLine($"{returnOption}: Return to the previous menu");
            int choice1 = GetMenuChoice(1, returnOption);

            switch (choice1)
            {
                case var n when n >= 1 && n <= restaurants.Count:
                    var selectedRestaurant = restaurants[n - 1];
                    ViewRestaurantDetails(customer, selectedRestaurant);
                    break;

                case var r when r == returnOption:
                    return;
            }
        }

        /// <summary>
        /// Views the details of a restaurant
        /// </summary>
        /// <param name="customer">The customer viewing the restaurant</param>
        /// <param name="restaurant">The restaurant to view</param>
        private void ViewRestaurantDetails(Customer customer, Restaurant restaurant)
        {
            Console.WriteLine($"Placing order from {restaurant.Name}.");

            bool backToMain = false;
            while (!backToMain)
            {
                Console.WriteLine("1: See this restaurant's menu and place an order");
                Console.WriteLine("2: See reviews for this restaurant");
                Console.WriteLine("3: Return to main menu");
                int choice = GetMenuChoice(1, 3);

                switch (choice)
                {
                    case 1:
                        PlaceOrder(customer, restaurant);
                        break;

                    case 2:
                        Console.WriteLine($"Reviews for {restaurant.Name}:");
                        foreach (var review in restaurant.Reviews)
                        {
                            Console.WriteLine($"- {review.Customer.Name}: {review.Rating}/5 - {review.Comment}");
                        }
                        
                        break;

                    case 3:
                        backToMain = true;
                        break;
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
            var menuItem = restaurant.Menu[0]; // only one item, assumed by Gradescope
            bool ordering = true;

            while (ordering)
            {
                Console.WriteLine($"Current order total: ${order.TotalPrice:F2}");
                Console.WriteLine($"1: ${menuItem.Price:F2} {menuItem.Name}");
                Console.WriteLine("2: Complete order");
                Console.WriteLine("3: Cancel order");

                int choice = GetMenuChoice(1, 3);

                switch (choice)
                {
                    case 1:
                        Console.WriteLine($"Adding {menuItem.Name} to order.");
                        int qty = checks.GetValidIntInput(
                            "Please enter quantity (0 to cancel):",
                            q => q >= 0,
                            "Invalid quantity. Please enter 0 or a positive number."
                        );

                        if (qty == 0) break;

                        _service.AddItemToOrder(order, menuItem, qty);
                        Console.WriteLine($"Added {qty} x {menuItem.Name} to order.");
                        break;

                    case 2:
                        Console.WriteLine($"Adding {menuItem.Name} to order.");
                        int finalQty = checks.GetValidIntInput(
                            "Please enter quantity (0 to cancel):",
                            q => q > 0,
                            "Invalid quantity. Must be greater than 0."
                        );

                        _service.AddItemToOrder(order, menuItem, finalQty);
                        Console.WriteLine($"Added {finalQty} x {menuItem.Name} to order.");
                        Console.WriteLine($"Your order has been placed. Your order number is #{order.Id}.");

                        return; // Must return to stop prompting

                    case 3:
                        Console.WriteLine("Order cancelled.");
                        return; // Must return to avoid re-looping
                }
            }
        }

        /// <summary>
        /// Views the current order status for a customer
        /// </summary>
        /// <param name="customer">The customer viewing their order status</param>
        private void ViewCurrentOrderStatus(Customer customer)
        {
            if (customer.Orders.Count == 0)
            {
                Console.WriteLine("You have not placed any orders.");
                return;
            }
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
                Console.WriteLine("Select a previous order to rate the restaurant it came from:");
                Console.WriteLine("1: Return to the previous menu");

                int fallback = GetMenuChoice(1, 1);
                switch (fallback)
                {
                    case 1:
                        return;
                }
            }

            Console.WriteLine("Select a restaurant to review:");
            for (int i = 0; i < deliveredOrders.Count; i++)
            {
                var restaurant = deliveredOrders[i];
                Console.WriteLine($"{i + 1}: {restaurant.Name}");
            }
            Console.WriteLine($"{deliveredOrders.Count + 1}: Return to the previous menu");

            int choice = GetMenuChoice(1, deliveredOrders.Count + 1);
            switch (choice)
            {
                case var c when c == deliveredOrders.Count + 1:
                    return;

                case var c when c >= 1 && c <= deliveredOrders.Count:
                    var selectedRestaurant = deliveredOrders[c - 1];

                    int rating = checks.GetValidIntInput(
                        "Please enter your rating (1-5 stars):",
                        r => r >= 1 && r <= 5,
                        "Invalid rating. Please enter a number between 1 and 5."
                    );

                    string comment = checks.GetValidInput(
                        "Please enter your comment:",
                        c => !string.IsNullOrWhiteSpace(c),
                        "Invalid comment. Please enter a non-empty comment."
                    );

                    var review = _service.CreateReview(customer, selectedRestaurant, rating, comment);
                    Console.WriteLine("Thank you for your review!");
                    return;
            }
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
                Console.WriteLine("1: Return to the previous menu");

                int choice = GetMenuChoice(1, 1);
                switch (choice)
                {
                    case 1:
                        return;
                }
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
        private void UpdateRestaurantOrderStatus(Client client, OrderStatus newStatus)
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
            var restaurant = client.Restaurant;

            Console.WriteLine("This is your restaurant's current menu:");
            for (int i = 0; i < restaurant.Menu.Count; i++)
            {
                var item = restaurant.Menu[i];
                Console.WriteLine($"${item.Price:F2} {item.Name}");
            }

            while (true)
            {
                Console.WriteLine("Please enter the name of the new item (blank to cancel): ");
                string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                    return; // Exit to previous menu

                if (int.TryParse(name, out _))
                    continue; // Ignore numeric names, no message

                decimal price = checks.GetValidPrice(
                    "Please enter the price of the new item (without the $):",
                    "Invalid price."
                );

                bool success = _service.AddMenuItem(name, price);

                if (success)
                {
                    Console.WriteLine($"Successfully added {name} (${price:F2}) to menu.");
                }
                else if (restaurant.Menu.Any(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    Console.WriteLine("This item already exists in the menu.");
                }
                else if (string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Failed to add the item.");
                }

                break;
            }
        }
    }
}