using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a review of a restaurant
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Gets the customer who wrote the review
        /// </summary>
        public Customer Customer { get; }

        /// <summary>
        /// Gets the restaurant being reviewed
        /// </summary>
        public Restaurant Restaurant { get; }

        /// <summary>
        /// Gets the rating (1-5 stars)
        /// </summary>
        public int Rating { get; }

        /// <summary>
        /// Gets the comment for the review
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Creates a new review with the specified details
        /// </summary>
        /// <param name="customer">The customer who wrote the review</param>
        /// <param name="restaurant">The restaurant being reviewed</param>
        /// <param name="rating">The rating (1-5 stars)</param>
        /// <param name="comment">The comment for the review</param>
        public Review(Customer customer, Restaurant restaurant, int rating, string comment)
        {
            Customer = customer;
            Restaurant = restaurant;
            Rating = rating;
            Comment = comment;

            // Add this review to the restaurant's reviews
            restaurant.AddReview(this);
        }

        /// <summary>
        /// Returns a string representation of this review
        /// </summary>
        /// <returns>A string representation of this review</returns>
        public override string ToString()
        {
            return $"{Rating} stars - {Comment} (by {Customer.Name})";
        }
    }
}