using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Arriba_Eats_App.Data.System
{
	public class MenuItemRepo : IRepository<MenuItem>
	{
		private List<MenuItem> _menuItems = new List<MenuItem>();

		public IEnumerable<MenuItem> GetAll()
		{
			if (_menuItems.Count == 0)
			{
				Console.WriteLine("No menu items found"); // debug line
			}
			Console.WriteLine("Menu items found"); // debug line
			return _menuItems;
		}
		public MenuItem GetById(Guid UUID)
		{
			if (_menuItems.Find(i => i.UUID == UUID) == null)
			{
				Console.WriteLine("Menu item not found"); // debug line
			}
			Console.WriteLine("Menu item found"); // debug line
			return _menuItems.Find(i => i.UUID == UUID);
		}


		public void Add(MenuItem item)
		{
			if (item.UUID == Guid.Empty) item.UUID = Guid.NewGuid(); // generate a new UUID if not set
			_menuItems.Add(item);
		}

		public void Update(MenuItem item)
		{
			var index = _menuItems.FindIndex(i => i.UUID == item.UUID);
			if (index < 0)
			{
				Console.WriteLine("Menu item not found"); // debug line
				return;
			}
			if (index >= 0) _menuItems[index] = item; // update the menu item in the collection
		}
		public void Delete(Guid UUID)
		{
			var item = GetById(UUID);
			if (item != null) _menuItems.Remove(item); // remove the menu item from the collection
		}

		public void SaveChanges()
		{
			// In a real application, this would save changes to a database
			//Console.WriteLine("Changes saved to the database"); // debug line
		}


	}
}
