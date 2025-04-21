using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using Arriba_Eats_App.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Arriba_Eats_App.Data.System
{
	public class OrderRepo : IRepository<Order>
	{
		private List<Order> _orders = new List<Order>();
		public IEnumerable<Order> GetAll()
		{
			return _orders;
		}
		public Order GetById(Guid UUID)
		{
			if (_orders.Count == 0)
			{
				Console.WriteLine("No orders found"); // debug line
			}

			return _orders.FirstOrDefault(o => o.UUID == UUID);
		}
		public void Add(Order Entity)
		{
			if (Entity == null)
			{
				Console.WriteLine("Order is null"); // debug line
				return;
			}

			if (Entity.UUID == Guid.Empty) Entity.UUID = Guid.NewGuid(); // generate a new UUID if not set
			_orders.Add(Entity); // add to in memory collection
		}
		public void Update(Order Entity)
		{
			if (Entity == null)
			{
				Console.WriteLine("Order is null"); // debug line
				return;
			}

			var index = _orders.FindIndex(o => o.UUID == Entity.UUID);
			if (index >= 0) _orders[index] = Entity; // update the order in the collection
		}
		public void Delete(Guid UUID) // optional - add Order Entity for deleting order data
		{
			var order = GetById(UUID);

			if (order != null) _orders.Remove(order); // remove the order from the collection
			
		}
		public void SaveChanges()
		{
			// In a real application, this would save changes to a database
			//Console.WriteLine("Changes saved to database");
		}
	}
}
