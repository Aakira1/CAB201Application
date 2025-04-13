using Arriba_Eats_App.Arriba_Eats_App.UI;
using System;
using System.ComponentModel.DataAnnotations;

namespace Main
{
	class InitiateProgram
	{
		static void Main(string[] args)
		{
			var Menu = new MainMenuUI();
			Menu.ShowMenu();
		}
	}
}