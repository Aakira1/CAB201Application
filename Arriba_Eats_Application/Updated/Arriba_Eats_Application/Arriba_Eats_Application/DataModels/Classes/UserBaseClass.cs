using System;
using System.Collections.Generic;

namespace ArribaEats.Models
{
    /// <summary>
    /// Base class for all users in the system
    /// </summary>
    public abstract class User
    {
        #region Properties
        /// <summary>
        /// Gets or sets the user's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the user's age
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// Gets or sets the user's email address
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user's mobile number
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Gets or sets the user's password
        /// </summary>
        protected string Password { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new user with the specified details
        /// </summary>
        protected User(string name, int age, string email, string mobile, string password)
        {
            Name = name;
            Age = age;
            Email = email;
            Mobile = mobile;
            Password = password;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the password against the stored password
        /// </summary>
        public bool ValidatePassword(string password)
        {
            return Password == password;
        }

        /// <summary>
        /// Gets the user information as a dictionary of property names and values
        /// </summary>
        public virtual Dictionary<string, string> GetUserInfo()
        {
            return new Dictionary<string, string>
            {
                { "Name", Name },
                { "Age", Age.ToString() },
                { "Email", Email },
                { "Mobile", Mobile }
            };
        }

        #endregion
    }
}
