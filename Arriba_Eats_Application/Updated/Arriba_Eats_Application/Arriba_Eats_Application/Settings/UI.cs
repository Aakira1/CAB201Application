using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Services;
using ArribaEats.Utils;
using ArribaEats.UI.Settings;
using ArribaEats.Interfaces;

namespace ArribaEats.UI
{
    /// <summary>
    /// User interface for the ArribaEats application
    /// </summary>
    public class ArribaEatsUI
    {
        #region Fields and Constructor

        private readonly IArribaEatsService _service;
        private readonly InputChecks checks = new();
        private readonly Restaurant restaurant1;

        public ArribaEatsUI(IArribaEatsService service)
        {
            _service = service;
        }

        #endregion

        #region Application Entry

        /// <summary>
        /// Main entry point for the application
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
                        case Customer:
                            ShowCustomerMenu();
                            break;
                        case Deliverer:
                            ShowDelivererMenu();
                            break;
                        case Client:
                            ShowClientMenu();
                            break;
                    }
                }
            }

            Console.WriteLine("Thank you for using Arriba Eats!");
        }

        #endregion

        #region Startup/Login/Registration

        private bool ShowStartupMenu()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Login as a registered user");
            Console.WriteLine("2: Register as a new user");
            Console.WriteLine("3: Exit");

            int choice = GetMenuChoice(1, 3);

            return choice switch
            {
                1 => TryLogin(),
                2 => TryRegister(),
                3 => true,
                _ => false,
            };
        }

        private bool TryLogin()
        {
            LoginUser();
            return false;
        }

        private bool TryRegister()
        {
            RegisterUserMenu();
            return false;
        }

        private void LoginUser()
        {
            string email = checks.GetValidInput("Email:", InputValidator.ValidateEmail, "Invalid email address.");
            string password = checks.GetValidInput("Password:", _ => true, "");

            if (!_service.Login(email, password))
            {
                Console.WriteLine("Invalid email or password.");
            }
            else
            {
                Console.WriteLine($"Welcome back, {_service.CurrentUser.Name}!");
            }
        }

        private void RegisterUserMenu()
        {
            Console.WriteLine("Which type of user would you like to register as?");
            Console.WriteLine("1: Customer");
            Console.WriteLine("2: Deliverer");
            Console.WriteLine("3: Client");
            Console.WriteLine("4: Return to the previous menu");

            int choice = GetMenuChoice(1, 4);
            if (choice == 4) return;

            var userType = choice switch
            {
                1 => UserType.Customer,
                2 => UserType.Deliverer,
                3 => UserType.Client,
                _ => throw new ArgumentOutOfRangeException()
            };

            RegisterUser(userType);
        }

        private void RegisterUser(UserType userType)
        {
            string name = checks.GetValidInput("Please enter your name: ", InputValidator.ValidateName, "Invalid name.");
            int age = checks.GetValidIntInput("Please enter your age (18-100): ", InputValidator.ValidateAge, "Invalid age.");
            string email = GetUniqueEmail();
            string mobile = checks.GetValidInput("Please enter your mobile phone number:", InputValidator.ValidateMobile, "Invalid phone number.");
            string password = checks.GetValidPassword();

            bool success = userType switch
            {
                UserType.Customer => _service.RegisterCustomer(name, age, email, mobile, password, checks.GetValidLocation()),
                UserType.Deliverer => _service.RegisterDeliverer(name, age, email, mobile, password,
                    checks.GetValidInput("Please enter your licence plate: ", InputValidator.ValidateLicencePlate, "Invalid licence plate.")),
                UserType.Client => _service.RegisterClient(
                    name,
                    age,
                    email,
                    mobile,
                    password,
                    checks.GetValidInput("Please enter your restaurant's name: ", InputValidator.ValidateRestaurantName, "Invalid restaurant name."),
                    InputValidator.SelectFoodStyle(),
                    checks.GetValidLocation()
                ),
                _ => false
            };

            Console.WriteLine(success
                ? $"You have been successfully registered as a {userType.ToString().ToLower()}, {name}!"
                : "Registration failed. Please try again.");
        }

        private string GetUniqueEmail()
        {
            while (true)
            {
                string email = checks.GetValidInput("Please enter your email address:", InputValidator.ValidateEmail, "Invalid email address.");
                if (_service.IsEmailInUse(email))
                    Console.WriteLine("This email address is already in use.");
                else
                    return email;
            }
        }

        #endregion

        #region Menu Dispatchers

        private void ShowCustomerMenu()
        {
            Customer customer = (Customer)_service.CurrentUser;
            bool exit = false;

            while (!exit)
            {
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
                        exit = true;
                        break;
                }
            }
        }
        private void ShowDelivererMenu()
        {
            Deliverer deliverer = (Deliverer)_service.CurrentUser;

            bool exit = false;
            while (!exit)
            {
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
                        DisplayDelivererInfo(deliverer);
                        break;
                    case 2:
                        ViewAvailableOrders(_service.Restaurants);
                        break;
                    case 3:
                        UpdateDeliveryStatus(deliverer);
                        break;
                    case 4:
                        UpdateDeliveryStatus(deliverer);
                        break;
                    case 5:
                        Logout();
                        exit = true;
                        break;
                }
            }
        }
        private void ShowClientMenu()
        {
            Client client = (Client)_service.CurrentUser;
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Please make a choice from the menu below:");
                Console.WriteLine("1: Display your user information");
                Console.WriteLine("2: Add item to restaurant menu");
                Console.WriteLine("3: See current orders");
                Console.WriteLine("4: Start cooking order");
                Console.WriteLine("5: Finish cooking order");
                Console.WriteLine("6: Handle deliverers who have arrived");
                Console.WriteLine("7: Logout");

                int choice = GetMenuChoice(1, 7);

                switch (choice)
                {
                    case 1:
                        DisplayClientInfo(client);
                        break;
                    case 2:
                        AddMenuItem(client);
                        break;
                    case 3:
                        ViewRestaurantOrders(client.Restaurant);
                        break;
                    case 4:
                        CookOrders(client);
                        break;
                    case 5:
                        FinishCookingOrders(client);
                        break;
                    case 6:
                        HandleDeliveryDispatch(client);
                        break;
                    case 7:
                        Logout();
                        exit = true;
                        break;
                }
            }
        }
        private void Logout()
        {
            _service.Logout();
            Console.WriteLine("You are now logged out.");
        }
        #endregion

        #region Helpers

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
            }
        }

        #endregion

        #region Display Methods

        private void DisplayUserInfo(Customer user)
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Age: {user.Age}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Mobile: {user.Mobile}");
            Console.WriteLine($"Location: {user.Location}");
            Console.WriteLine($"You've made {user.Orders.Count} order(s) and spent a total of ${user.Orders.Sum(order => order.TotalPrice):F2} here.");
        }

        /// <summary>
        /// Displays user information for a deliverer, including active order and vehicle
        /// </summary>
        private void DisplayDelivererInfo(Deliverer deliverer)
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {deliverer.Name}");
            Console.WriteLine($"Age: {deliverer.Age}");
            Console.WriteLine($"Email: {deliverer.Email}");
            Console.WriteLine($"Mobile: {deliverer.Mobile}");
            Console.WriteLine($"Licence plate: {deliverer.LicencePlate}");

            if (deliverer.CurrentOrder != null)
            {
                Console.WriteLine("You are currently delivering the following order:");
                Console.WriteLine($"Order ID: {deliverer.CurrentOrder.Id}");
                Console.WriteLine($"Restaurant: {deliverer.CurrentOrder.Restaurant.Name} at {deliverer.CurrentOrder.Restaurant.Location}");
                Console.WriteLine($"Customer: {deliverer.CurrentOrder.Customer.Name} at {deliverer.CurrentOrder.Customer.Location}");
                Console.WriteLine($"Status: {deliverer.CurrentOrder.Status}");
            }
        }

        private void DisplayClientInfo(Client client)
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {client.Name}");
            Console.WriteLine($"Age: {client.Age}");
            Console.WriteLine($"Email: {client.Email}");
            Console.WriteLine($"Mobile: {client.Mobile}");
            Console.WriteLine($"Restaurant name: {client.Restaurant.Name}");
            Console.WriteLine($"Restaurant style: {client.Restaurant.Style}");
            Console.WriteLine($"Restaurant location: {client.Restaurant.Location}");
        }


        private void DisplayRestaurantInfo(Restaurant restaurant)
        {
            var info = restaurant.GetRestaurantInfo();
            Console.WriteLine("Restaurant Information:");
            foreach (var item in info)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        #endregion

        #region Customer Order & Review
        /// <summary>
        /// Views the restaurants available to a customer
        /// </summary>
        /// <param name="customer">The customer viewing the restaurants</param>
        private void ViewRestaurants(Customer customer)
        {
            Console.WriteLine("How would you like the list of restaurants ordered?");
            Console.WriteLine("1: Sorted alphabetically by name");
            Console.WriteLine("2: Sorted by distance");
            Console.WriteLine("3: Sorted by style");
            Console.WriteLine("4: Sorted by average rating");
            Console.WriteLine("5: Return to the previous menu");

            int choice = GetMenuChoice(1, 5);

            if (choice == 5) return;

            IRestaurantSorter sorter = choice switch
            {
                1 => new NameSorter(),
                2 => new DistanceSorter(),
                3 => new StyleSorter(),
                4 => new RatingSorter(),
                _ => throw new InvalidOperationException("Invalid selection.")
            };

            var restaurants = sorter.SortRestaurants(_service.Restaurants, customer);

            Console.WriteLine("You can order from the following restaurants:");
            Console.WriteLine("Restaurant Name       Loc     Dist  Style       Rating");

            for (int i = 0; i < restaurants.Count; i++)
            {
                var r = restaurants[i];
                var location = $"{r.Location.X},{r.Location.Y}";
                int distance = r.Location.DistanceTo(customer.Location);
                string ratingDisplay = r.AverageRating > 0 ? r.AverageRating.ToString("F1") : "-";

                Console.WriteLine($"{i + 1}: {r.Name.PadRight(20)} {location.PadRight(7)} {distance.ToString().PadRight(6)} {r.Style.ToString().PadRight(10)} {ratingDisplay}");
            }

            Console.WriteLine($"{restaurants.Count + 1}: Return to the previous menu");
            int selection = GetMenuChoice(1, restaurants.Count + 1);

            if (selection <= restaurants.Count)
            {
                var selectedRestaurant = restaurants[selection - 1];
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
            var order = _service.CreateOrder(customer, restaurant);
            bool ordering = true;
            bool orderPlaced = false;

            while (ordering)
            {
                Console.WriteLine($"Current order total: ${order.TotalPrice:F2}");

                // Dynamically display all menu items
                for (int i = 0; i < restaurant.Menu.Count; i++)
                {
                    var item = restaurant.Menu[i];
                    Console.WriteLine($"{i + 1}: ${item.Price:F2} {item.Name}");
                }

                int completeOption = restaurant.Menu.Count + 1;
                int cancelOption = restaurant.Menu.Count + 2;

                Console.WriteLine($"{completeOption}: Complete order");
                Console.WriteLine($"{cancelOption}: Cancel order");

                int choice = GetMenuChoice(1, cancelOption);

                if (choice >= 1 && choice <= restaurant.Menu.Count)
                {
                    var selectedItem = restaurant.Menu[choice - 1];

                    Console.WriteLine($"Adding {selectedItem.Name} to order.");
                    Console.WriteLine("Please enter quantity (0 to cancel): ");

                    if (int.TryParse(Console.ReadLine(), out int qty) && qty > 0)
                    {
                        _service.AddItemToOrder(order, selectedItem, qty);
                        Console.WriteLine($"Added {qty} x {selectedItem.Name} to order.");
                    }
                }
                else if (choice == completeOption)
                {

                    // Now finalize and register the order
                    _service.FinalizeOrder(order);
                    Console.WriteLine($"Your order has been placed. Your order number is #{order.Id}.");
                    orderPlaced = true;
                    ordering = false;

                }
                else if (choice == cancelOption)
                {
                    //Console.WriteLine("Order cancelled.");
                    ordering = false;
                }
            }

            // If the order wasn't completed, clean it up from the system
            if (!orderPlaced)
            {
                // Optional: If needed, remove from restaurant.Orders or perform cleanup.
                order = null;
            }
        }



        /// <summary>
        /// Views the current order status for a customer
        /// </summary>
        /// <param name="customer">The customer viewing their order status</param>
        private void ViewCurrentOrderStatus(Customer customer)
        {
            var activeOrders = customer.Orders.Where(o => o.Status != OrderStatus.Delivered).ToList();

            foreach (var order in activeOrders)
            {
                Console.WriteLine($"Order #{order.Id} from {order.Restaurant.Name}: {order.Status}");

                // Remove any potential duplication logic above or below this
                foreach (var item in order.Items)
                {
                    Console.WriteLine($"{item.Value} x {item.Key.Name}");
                }
            }
            if (activeOrders.Count == 0)
            {
                Console.WriteLine("You have not placed any orders.");
            }
            //else
            //{
            //    Console.WriteLine("1: Return to previous menu");
            //    int choice = GetMenuChoice(1, 1);
            //    if (choice == 1) return;
            //}
        }

        /// <summary>
        /// Allows a customer to write a review for a delivered order
        /// </summary>
        private void WriteReview(Customer customer)
        {
            var deliveredOrders = customer.Orders
                .Where(o => o.Status == OrderStatus.Delivered)
                .Select(o => o.Restaurant)
                .Distinct()
                .ToList();

            Console.WriteLine("Select a previous order to rate the restaurant it came from:");
            for (int i = 0; i < deliveredOrders.Count; i++)
            {
                Console.WriteLine($"{i + 1}: {deliveredOrders[i].Name}");
            }
            Console.WriteLine($"{deliveredOrders.Count + 1}: Return to the previous menu");

            int choice = GetMenuChoice(1, deliveredOrders.Count + 1);
            if (choice == deliveredOrders.Count + 1) return;

            var restaurant = deliveredOrders[choice - 1];

            int rating = checks.GetValidIntInput(
                "Please enter your rating (1-5 stars):",
                r => r >= 1 && r <= 5,
                "Invalid rating. Please enter a number between 1 and 5.");

            string comment = checks.GetValidInput(
                "Please enter your comment:",
                c => !string.IsNullOrWhiteSpace(c),
                "Invalid comment. Please enter a non-empty comment.");

            var review = _service.CreateReview(customer, restaurant, rating, comment);
            Console.WriteLine("Thank you for your review!");
        }

        /// <summary>
        /// Views the available orders for a deliverer
        /// </summary>
        /// <param name="deliverer">The deliverer viewing the orders</param>
        private void ViewAvailableOrders(List<Restaurant> restaurant)
        {
            InputChecks inputChecks = new InputChecks();
            Location loc = inputChecks.GetValidLocation();
            var r = restaurant[0]; // Assuming you want to check the first restaurant in the list

            //int distance = (int)Math.Sqrt(Math.Pow(r.Location.X - _service.CurrentCustomer.Location.X, 2) + Math.Pow(r.Location.Y - _service.CurrentCustomer.Location.Y, 2));
            //int total = (int)Math.Sqrt(Math.Pow(loc.X + _service.CurrentCustomer.Location.X, r.Location.X) + Math.Pow(loc.Y - _service.CurrentCustomer.Location.Y, r.Location.Y));

            //Console.WriteLine($"Distance from restaurant to customer: {total} units");

            Console.WriteLine("The following orders are available for delivery. Select an order to accept it:");
            Console.WriteLine("Order  Restaurant Name       Loc    Customer Name    Loc    Dist");

            for (int i = 0; i < _service.GetAvailableOrders(r).Count; i++)
            {
                // make it match Console.WriteLine("Order  Restaurant Name       Loc    Customer Name    Loc    Dist");
                var order = _service.GetAvailableOrders(r)[i];

                var parts = loc;
                int restaurantX = r.Location.X;
                int restaurantY = r.Location.Y;
                int customerX = order.Customer.Location.X;
                int customerY = order.Customer.Location.Y;
                int distDelivererToRestauraunt = (int)Math.Sqrt(Math.Pow(restaurantX - loc.X, 2) + Math.Pow(restaurantY - loc.Y, 2));
                int distRestaurantToCustomer = (int)Math.Sqrt(Math.Pow(restaurantX - customerX, 2) + Math.Pow(restaurantY - customerY, 2));
                int total = distDelivererToRestauraunt + distRestaurantToCustomer;


                Console.WriteLine($"{i + 1}      {order.Restaurant.Name}       {order.Restaurant.Location.X},{order.Restaurant.Location.Y}    {order.Customer.Name}       {order.Customer.Location.X},{order.Customer.Location.Y}    {total}");

            }
        }

        /// <summary>
        /// Updates the delivery status for a deliverer
        /// </summary>
        /// <param name="deliverer">The deliverer updating the status</param>
        private void UpdateDeliveryStatus(Deliverer deliverer)
        {
            var order = deliverer.CurrentOrder; 
             
            if (order == null)
            {
                Console.WriteLine("You have not accepted any orders yet.");
                return;
            }

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
                Console.WriteLine("You have not placed any orders.");
                Console.WriteLine("1: Return to the previous menu");

                int choice = GetMenuChoice(1, 1);
                switch (choice)
                {
                    case 1:
                        return;
                }
            }

            for (int i = 0; i < restaurant.Orders.Count; i++)
            {
                var order = restaurant.Orders[i];
                Console.WriteLine($"Order #{order.Id} - Status: {order.Status}");
                //Console.WriteLine($"   Items: {order.Items.Count}");
                //Console.WriteLine($"   Total: ${order.TotalPrice:F2}");

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
        /// <summary>
        /// Shows all orders for the client's restaurant and allows updating status to "Cooking"
        /// </summary>
        private void CookOrders(Client client)
        {
            var restaurant = client.Restaurant;
            var ordersToCook = restaurant.Orders
                .Where(o => o.Status == OrderStatus.Ordered)
                .ToList();

            Console.WriteLine("Select an order to start cooking:");
            for (int i = 0; i < ordersToCook.Count; i++)
            {
                var order = ordersToCook[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} - {order.Customer.Name} - Status: {order.Status}");
            }
            Console.WriteLine($"{ordersToCook.Count + 1}: Return to previous menu");

            int choice = GetMenuChoice(1, ordersToCook.Count + 1);
            if (choice == ordersToCook.Count + 1) return;

            var selectedOrder = ordersToCook[choice - 1];
            selectedOrder.SetStatus(OrderStatus.Cooking);
            Console.WriteLine($"Order #{selectedOrder.Id} is now marked as Cooking.");
        }
        /// <summary>
        /// Handles cooking orders after a driver has already been assigned
        /// </summary>
        private void FinishCookingOrders(Client client)
        {
            var restaurant = client.Restaurant;
            var ordersToFinish = restaurant.Orders
                .Where(o => o.Status == OrderStatus.Cooking)
                .ToList();

            Console.WriteLine("Select an order to mark as cooked:");
            for (int i = 0; i < ordersToFinish.Count; i++)
            {
                var order = ordersToFinish[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} - {order.Customer.Name} - Status: {order.Status}");
            }
            Console.WriteLine($"{ordersToFinish.Count + 1}: Return to previous menu");

            int choice = GetMenuChoice(1, ordersToFinish.Count + 1);
            if (choice == ordersToFinish.Count + 1) return;

            var selectedOrder = ordersToFinish[choice - 1];
            bool success = _service.UpdateOrderStatus(selectedOrder, OrderStatus.Cooked);

            if (success)
            {
                Console.WriteLine($"Order #{selectedOrder.Id} has been marked as Cooked.");
            }
            else
            {
                Console.WriteLine("Failed to update the order status.");
            }
        }
        /// <summary>
        /// Marks a delivery as completed once the deliverer reaches the customer's location
        /// </summary>
        /// <summary>
        /// Marks an order as "Being Delivered" if it has been cooked and assigned a deliverer,
        /// and then completes the delivery for demonstration/testing flow.
        /// </summary>
        private void HandleDeliveryDispatch(Client client)
        {
            var restaurant = client.Restaurant;
            var readyOrders = restaurant.Orders
                .Where(o => o.Status == OrderStatus.Cooked && o.Deliverer != null)
                .ToList();


            Console.WriteLine("Select an order to mark as 'Being Delivered':");
            for (int i = 0; i < readyOrders.Count; i++)
            {
                var order = readyOrders[i];
                Console.WriteLine($"{i + 1}: Order #{order.Id} - Deliverer: {order.Deliverer.Name} - Status: {order.Status}");
            }
            Console.WriteLine($"{readyOrders.Count + 1}: Return to previous menu");

            int choice = GetMenuChoice(1, readyOrders.Count + 1);
            if (choice == readyOrders.Count + 1) return;

            var selectedOrder = readyOrders[choice - 1];
            bool dispatched = _service.UpdateOrderStatus(selectedOrder, OrderStatus.BeingDelivered);

            if (dispatched)
            {
                Console.WriteLine($"Order #{selectedOrder.Id} has been marked as 'Being Delivered'.");

                // Auto-complete for flow testing
                selectedOrder.Deliverer?.CompleteDelivery();
                Console.WriteLine($"Order #{selectedOrder.Id} has been marked as Delivered by {selectedOrder.Deliverer?.Name}.");
            }
            else
            {
                Console.WriteLine("Failed to update order status.");
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

#endregion

    }
}
