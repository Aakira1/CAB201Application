using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.UI.MenuUI;

namespace Arriba_Eats_App.App.Services
{
	class DelivererService : MenuUI
	{
		public override void ShowMenu(bool isActive, EUserType userType)
		{
			while (isActive)
			{
				ClearScreen();
				DisplayOutput("Please make a choice from the menu below:" + "\n");
				DisplayOutput("1. Display your user information");
				DisplayOutput("2. List orders available to deliver");
				DisplayOutput("3. Arrived at restaurant to pick up order");
				DisplayOutput("4. Mark this delivery as complete");
				DisplayOutput("5. Logout");
				DisplayOutput("Please Enter a choice between 1 and 5:");
				string input = GetInput();
				if (string.IsNullOrEmpty(input))
				{
					DisplayError(HandleEmptyInput(input));
					WaitForKeyPress();
					continue;
				}
				isActive = SelectionMenu(input, userType);
			}
			DisplayOutput("Thank you for using Arriba Eats!");
		}

		public override bool SelectionMenu(string input, EUserType userType)
		{
			return true;
		}
	}
}
