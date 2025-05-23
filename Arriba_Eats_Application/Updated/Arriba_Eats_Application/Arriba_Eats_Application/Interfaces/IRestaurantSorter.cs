using System.Collections.Generic;
using ArribaEats.Models;

namespace ArribaEats.Interfaces
{
    /// <summary>
    /// Interface for sorting restaurants
    /// </summary>
    public interface IRestaurantSorter
    {
        #region Sorting Method
        /// <summary>
        /// Sorts a list of restaurants based on the customer's preferences
        /// </summary>
        /// <param name="restaurants"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        List<Restaurant> SortRestaurants(List<Restaurant> restaurants, Customer customer);

        #endregion
    }
}
