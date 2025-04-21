using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;


namespace Arriba_Eats_App.Data.Repositories
{
	public class CustomerRepo : IRepository<Customer>
	{
		// in memory collection
		private List<Customer> _customers = new List<Customer>();

		public IEnumerable<Customer> GetAll()
		{
			if (_customers.Count == 0)
			{
				Console.WriteLine("No customers found"); // debug line
			}
			Console.WriteLine("Customers found"); // debug line
			return _customers;
		}

		public Customer GetById(Guid UUID)
		{
			if (_customers.Find(c => c.UUID == UUID) == null)
			{
				Console.WriteLine("Customer not found"); // debug line
			}
			Console.WriteLine("Customer found"); // debug line
			return _customers.Find(c => c.UUID == UUID);
		}

		public void Add(Customer Entity)
		{
			if (Entity.Address == null || Entity.MobileNumber == null)
			{
				Console.WriteLine("Address not found"); // debug line
			}

			if (Entity.UUID == Guid.Empty) Entity.UUID = Guid.NewGuid(); // generate a new UUID if not set
			_customers.Add(Entity);
		}

		public void Update(Customer Entity)
		{
			var index = _customers.FindIndex(c => c.UUID == Entity.UUID);
			if(index < 0)
			{
				Console.WriteLine("Customer not found"); // debug line
				return;
			}

			if (index >= 0) _customers[index] = Entity; // update the customer in the collection
		}

		public void Delete(Guid UUID)
		{
			var customer = GetById(UUID);
			if (customer != null) _customers.Remove(customer);// remove the customer from the collection
		}

		public void SaveChanges()
		{
			// In a real application, this would save changes to a database
			// as a result of the assessment I have made the structure memory based only as requirements.
		}

		/// Get a customer by email
		public Customer GetByDetails(string UserDetail)
		{
			var CustomerByEmail = _customers.Find(c => c.Email == UserDetail);
			var CustomerByMobile = _customers.Find(c => c.MobileNumber == UserDetail);
			var CustomerByUUID = _customers.Find(c => c.UUID.ToString() == UserDetail);

			if (CustomerByEmail == null && CustomerByMobile == null && CustomerByUUID == null)
			{
				Console.WriteLine("Customer not found"); // debug line
				return null;
			}

			Console.WriteLine("Customer found"); // debug line
			return CustomerByEmail ?? CustomerByMobile ?? CustomerByUUID;
		}
	}
}
