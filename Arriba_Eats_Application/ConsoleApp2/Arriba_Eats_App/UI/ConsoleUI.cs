using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Arriba_Eats_App.UI
{
	class ConsoleUI : IUserInterface
	{
		public string GetInput()
		{
			Console.Write("");
			return Console.ReadLine() ?? string.Empty;
		}

		public string GetSecuredInput(string input)
		{
			Console.Write(input);
			string password = string.Empty;
			ConsoleKey key;
			do
			{
				var KeyInfo = Console.ReadKey(true);
				key = KeyInfo.Key;

				if (key == ConsoleKey.Backspace && password.Length > 0)
				{
					Console.Write("\b \b");
					password = password[0..^1];
				} else if (!char.IsControl(KeyInfo.KeyChar))
				{
					Console.Write("*");
					password += KeyInfo.KeyChar;
				}
			} 
			while (key != ConsoleKey.Enter);
			
			Console.WriteLine();
			return password;
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
	}
}
