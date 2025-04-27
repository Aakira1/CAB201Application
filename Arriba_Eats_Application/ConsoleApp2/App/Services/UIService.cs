using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arriba_Eats_App.UI;

namespace Arriba_Eats_App.Services
{
    public static class UIService
    {
        private static IUserInterface _userinterface = new ConsoleUI(); // default implementation

        public static IUserInterface Current => _userinterface;

		public static void SetUserInterface(IUserInterface userinterface)
		{
			_userinterface = userinterface ?? throw new ArgumentNullException(nameof(userinterface), "User cannot be null.");
		}
	}
}
