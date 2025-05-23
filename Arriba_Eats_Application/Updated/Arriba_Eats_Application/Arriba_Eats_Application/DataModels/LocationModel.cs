using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Represents a location in the 2D grid system using X and Y coordinates
    /// </summary>
    public struct Location
    {
        #region Properties

        /// <summary>Gets or sets the X coordinate</summary>
        public int X { get; set; }

        /// <summary>Gets or sets the Y coordinate</summary>
        public int Y { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new location with the specified X and Y coordinates
        /// </summary>
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Calculates the Manhattan distance between this location and another location
        /// </summary>
        public int DistanceTo(Location other)
        {
            // Manhattan distance is the sum of the absolute differences of their coordinates
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
        }

        /// <summary>
        /// Returns a string representation of this location in the format "X,Y"
        /// </summary>
        public override string ToString()
        {
            // Format the location as "X,Y"
            return $"{X},{Y}";
        }

        #endregion
    }
}
