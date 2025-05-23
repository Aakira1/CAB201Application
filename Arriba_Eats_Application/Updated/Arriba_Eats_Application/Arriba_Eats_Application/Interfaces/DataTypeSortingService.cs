using System;
using System.Collections.Generic;
using System.Linq;
using ArribaEats.Models;
using ArribaEats.Interfaces;

namespace ArribaEats.Interfaces
{
    #region Name Sorter
    // Sort by Name
    public class NameSorter : IRestaurantSorter
    {
        public List<Restaurant> SortRestaurants(List<Restaurant> restaurants, Customer customer)
        {
            return restaurants.OrderBy(r => r.Name).ToList();
        }
    }

    #endregion

    #region Rating Sorter
    // Sort by Average Rating
    public class RatingSorter : IRestaurantSorter
    {
        /// <summary>
        /// Sorts restaurants by their average rating
        /// </summary>
        /// <param name="restaurants"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public List<Restaurant> SortRestaurants(List<Restaurant> restaurants, Customer customer)
        {
            return restaurants.OrderByDescending(r => r.AverageRating).ToList();
        }
    }

    #endregion

    #region Distance Sorter
    // Sort by Distance
    /// <summary>
    /// Sorts restaurants by their distance from the customer
    /// </summary>
    public class DistanceSorter : IRestaurantSorter
    {
        public List<Restaurant> SortRestaurants(List<Restaurant> restaurants, Customer customer)
        {
            return restaurants.OrderBy(r => r.Location.DistanceTo(customer.Location)).ToList();
        }
    }
    #endregion

    #region Style Sorter
    // Sort by Style
    /// <summary>
    /// Sorts restaurants by their food style
    /// </summary>
    public class StyleSorter : IRestaurantSorter
    {
        public List<Restaurant> SortRestaurants(List<Restaurant> restaurants, Customer customer)
        {
            return restaurants.OrderBy(r => r.Style.ToString()).ToList();
        }
    }
    #endregion
}
