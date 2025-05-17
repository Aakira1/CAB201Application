using System;
using System.ComponentModel.DataAnnotations;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.UI;
using Arriba_Eats_App.UI.MenuUI;
using Arriba_Eats_App.Services;
using Arriba_Eats_App.UI.MenuUI.MainMenuUI;
using System.Globalization;
using Arriba_Eats_App.Data;

namespace Main 
{
	class InitiateProgram
	{
		static void Main(string[] args)
		{
			EnhancedMenuSystem enhancedMenuSystem = new EnhancedMenuSystem(new ConsoleUI());
			enhancedMenuSystem.SetUI(new ConsoleUI());
        }
	}


}