using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.UI.MenuUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Services
{
	class RestarauntService : MenuUI
	{
		public override void ShowMenu(bool isActive, EUserType userType)
		{
			while (isActive)
			{
				ClearScreen();
				DisplayOutput("Please make a choice from the menu below:" + "\n");
				DisplayOutput("1. Display your user information");
				DisplayOutput("2. Add item to restaurant menu");
				DisplayOutput("3. See current orders");
				DisplayOutput("4. Start cooking order");
				DisplayOutput("5. Finish Cooking order");
				DisplayOutput("6. Handle deliverers who have arrived");
				DisplayOutput("7. Logout");
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
			return base.SelectionMenu(input, userType);
		}
	}
}

