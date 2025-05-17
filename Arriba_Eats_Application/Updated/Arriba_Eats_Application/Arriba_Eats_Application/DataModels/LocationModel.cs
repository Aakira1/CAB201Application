using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a location in the 2D grid system using X and Y coordinates
    /// </summary>
    public struct Location
    {
        /// <summary>
        /// Gets or sets the X coordinate
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the Y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Creates a new location with the specified X and Y coordinates
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Calculates the Manhattan distance between this location and another location
        /// </summary>
        /// <param name="other">The other location</param>
        /// <returns>The Manhattan distance between the two locations</returns>
        public int DistanceTo(Location other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        /// <summary>
        /// Returns a string representation of this location in the format "X,Y"
        /// </summary>
        /// <returns>A string representation of this location</returns>
        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}