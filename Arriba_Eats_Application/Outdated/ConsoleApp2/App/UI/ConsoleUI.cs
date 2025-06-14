﻿using Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.UI
{
	class ConsoleUI : IUserInterface
	{
		public string GetInput()
		{
			Console.Write("");
			return Console.ReadLine() ?? string.Empty;
		}

		public string GetSecuredInput(string input, bool isActive)
		{
			return string.Empty;
		}

		public void DisplayOutput(string message)
		{
			Console.WriteLine(message);
		}

		public void DisplayError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.White;
		}

		public void ClearScreen()
		{
			Console.Clear();
		}

		public void WaitForKeyPress()
		{
			Console.WriteLine("Press any key to continue...");
			Console.ReadKey(true);
		}

		public bool SelectionMenu(string Input, EUserType userType)
		{
			Console.WriteLine("Default Selection Menu");
			return true;
		}

		public void ShowMenu(bool IsActive, EUserType userType)
		{
			Console.WriteLine(IsActive ? "Menu is active" : "Menu is inactive");
		}
	}
}
