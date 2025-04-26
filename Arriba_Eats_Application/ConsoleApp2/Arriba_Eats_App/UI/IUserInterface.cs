using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Arriba_Eats_App.UI
{
    public interface IUserInterface
    {
        public virtual void ShowMenu(bool isActive)
		{
			Console.WriteLine("Default Menu");
		}
		string WelcomeMessage()
		{
			return "Welcome to Arriba Eats!";
		}
		string HandleEmptyInput(string input)
		{
			return "Input cannot be empty. Please try again.";
		}
		string GetInput();
        string GetSecuredInput(string input);
        void DisplayOutput(string message);
		void DisplayError(string message);
        void ClearScreen();
		void WaitForKeyPress();
	}
}
