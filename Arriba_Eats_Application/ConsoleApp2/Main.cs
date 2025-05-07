using System;
using System.ComponentModel.DataAnnotations;
using Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.UI;
using Arriba_Eats_App.UI.MenuUI;
using Arriba_Eats_App.Services;
using Arriba_Eats_App.UI.MenuUI.MainMenuUI;

namespace Main 
{
	class InitiateProgram
	{
		static void Main(string[] args)
		{
			EnhancedMenuSystem enhancedMenuSystem = new EnhancedMenuSystem(new ConsoleUI());
			enhancedMenuSystem.RunMenu();
		}
	}

	#region MenuSystem
	/// <summary>
	/// EnhancedMenuSystem class is responsible for managing the menu system of the application.
	/// </summary>
	partial class EnhancedMenuSystem
	{
		//private readonly IUserInterface _UI;
		/// <summary>
		/// Constructor for EnhancedMenuSystem class.
		/// </summary>
		/// <param name="UI"></param>
		public EnhancedMenuSystem(IUserInterface UI = null!)
		{
			UI = UI ?? UIService.Current!; // Use the default UI if none is provided
		}

		/// <summary>
		/// RunMenu method is responsible for displaying the main menu and handling user input.
		/// </summary>
		public void RunMenu()
		{
			//_UI.DisplayOutput("Menu");

			// use the existing menu structure but with improved and enhanced functionality
			var MainMenu = new MainMenuUI();
			MainMenu.ShowMenu(true, EUserType.None);
		}

		/// <summary>
		/// SetTestUI method is used to set a test user interface for testing purposes.
		/// </summary>
		/// <param name="testUI"></param>
		public void SetTestUI(IUserInterface testUI)
		{
			UIService.SetUserInterface(testUI);
		}

	}
	#endregion
}