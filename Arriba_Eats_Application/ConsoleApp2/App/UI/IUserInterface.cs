using Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.UI
{
    public interface IUserInterface
    {
		// non overridable methods
		void ClearScreen();
		void WaitForKeyPress();

		// variable methods 
		string WelcomeMessage()
		{
			return "Welcome to Arriba Eats!";
		}
		string HandleEmptyInput(string input)
		{
			return "Input cannot be empty. Please try again.";
		}
		string GetInput();
        string GetSecuredInput(string input, bool isActive);
        void DisplayOutput(string message);
		void DisplayError(string message);
		bool SelectionMenu(string Input, EUserType userType);
		void ShowMenu(bool IsActive, EUserType userType);
	}
}
