using Arriba_Eats_App.Arriba_Eats_App.Data.Models;
using System;
using System.Collections.Generic;

namespace Arriba_Eats_App.Data.Repositories
{
	public interface IRepository<T> where T : class
	{
		//Basic CRUD structure for future proofing database and development 
		IEnumerable<T> GetAll(); // read all
		T GetById(Guid UUID); // read by id
		void Add(T entity); // create
		void Update(T entity); // update
		void Delete(Guid UUID); // delete

		// save to memory
		void SaveChanges();
	}
}
