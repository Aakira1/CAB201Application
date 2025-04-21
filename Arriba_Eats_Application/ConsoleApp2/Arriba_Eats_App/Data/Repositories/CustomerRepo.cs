using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;


namespace Arriba_Eats_App.Data.Repositories
{
	public class CustomerRepo : IRepository<User>
	{
		// in memory collection
		private List<User> _users = new List<User>();

		public IEnumerable<User> GetAll()
		{
			if (_users.Count == 0)
			{
				Console.WriteLine("No customers found"); // debug line
			}
			Console.WriteLine("Customers found"); // debug line
			return _users;
		}

		public User GetById(Guid UUID)
		{
			if (_users.Find(c => c.UUID == UUID) == null)
			{
				Console.WriteLine("User not found"); // debug line
			}
			Console.WriteLine("User found"); // debug line
			return _users.Find(c => c.UUID == UUID);
		}

		public void Add(User Entity)
		{
			if (Entity.Address == null || Entity.MobileNumber == null)
			{
				Console.WriteLine("Address not found"); // debug line
			}

			if (Entity.UUID == Guid.Empty) Entity.UUID = Guid.NewGuid(); // generate a new UUID if not set
			_users.Add(Entity);
		}

		public void Update(User Entity)
		{
			var index = _users.FindIndex(c => c.UUID == Entity.UUID);
			if(index < 0)
			{
				Console.WriteLine("User not found"); // debug line
				return;
			}

			if (index >= 0) _users[index] = Entity; // update the customer in the collection
		}

		public void Delete(Guid UUID)
		{
			var customer = GetById(UUID);
			if (customer != null) _users.Remove(customer);// remove the customer from the collection
		}

		public void SaveChanges()
		{
			// In a real application, this would save changes to a database
			// as a result of the assessment I have made the structure memory based only as requirements.
		}
	}
}
