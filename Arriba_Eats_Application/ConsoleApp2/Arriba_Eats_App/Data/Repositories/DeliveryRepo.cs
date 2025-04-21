using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arriba_Eats_App.Data.Repositories
{
	public class DeliveryRepo : IRepository<DeliveryService>
	{
		private List<DeliveryService> _deliveryServices = new List<DeliveryService>();
		public IEnumerable<DeliveryService> GetAll()
		{
			return _deliveryServices;
		}
		public DeliveryService GetById(Guid UUID)
		{
			if (_deliveryServices.Find(d => d.UUID == UUID) == null)
			{
				Console.WriteLine("Delivery service not found"); // debug line
			}

			return _deliveryServices.FirstOrDefault(d => d.UUID == UUID);
		}
		public void Add(DeliveryService Entity)
		{
			if (Entity == null)
			{
				Console.WriteLine("Delivery service cannot be null"); // debug line
				return;
			}

			if (Entity.UUID == Guid.Empty) Entity.UUID = Guid.NewGuid(); // generate a new UUID if not set
			_deliveryServices.Add(Entity); // add to in memory collection
		}
		public void Update(DeliveryService Entity)
		{
			if (Entity == null)
			{
				Console.WriteLine("Delivery service cannot be null"); // debug line
				return;
			}

			var index = _deliveryServices.FindIndex(d => d.UUID == Entity.UUID);
			if (index >= 0) _deliveryServices[index] = Entity; // update the delivery service in the collection
		}
		public void Delete(Guid UUID)
		{
			// we do not want to delete delivery service data as it is tied with orders.
		}

		public void SaveChanges()
		{
			// In a real application, this would save changes to a database
			//Console.WriteLine("Changes saved to the database.");
		}
	}
}
