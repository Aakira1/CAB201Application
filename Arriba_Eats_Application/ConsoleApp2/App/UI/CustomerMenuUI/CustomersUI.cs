using Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.UI.MenuUI.CustomerMenuUI
{
	class CustomersUI : MenuUI
	{
		void CustomerUI()
		{

		}

		public override bool SelectionMenu(string input, EUserType userType)
		{
			return base.SelectionMenu(input, userType);
		}

		public override void ShowMenu(bool isActive, EUserType userType)
		{
			base.ShowMenu(isActive, userType);
		}
	}
}
