using System;

namespace ArribaEats.Models
{
    /// <summary>
    /// Base class for all users in the system
    /// </summary>
    public abstract class User
    {
        /// <summary>
        /// Gets or sets the name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the user
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the email address of the user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the mobile number of the user
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the password of the user
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// Creates a new user with the specified details
        /// </summary>
        /// <param name="name">The name of the user</param>
        /// <param name="age">The age of the user</param>
        /// <param name="email">The email address of the user</param>
        /// <param name="mobile">The mobile number of the user</param>
        /// <param name="password">The password of the user</param>
        protected User(string name, int age, string email, string mobile, string password)
        {
            Name = name;
            Age = age;
            Email = email;
            Mobile = mobile;
            Password = password;
        }

        /// <summary>
        /// Validates the password against the stored password
        /// </summary>
        /// <param name="password">The password to validate</param>
        /// <returns>True if the password is valid, false otherwise</returns>
        public bool ValidatePassword(string password)
        {
            return Password == password;
        }

        /// <summary>
        /// Gets the user information as a dictionary of property names and values
        /// </summary>
        /// <returns>A dictionary of user information</returns>
        public virtual Dictionary<string, string> GetUserInfo()
        {
            return new Dictionary<string, string>
            {
                { "Name", $"Name:{Name}" },
                { "Age", $"Age:{Age.ToString()}" },
                { "Email", $"Email:{Email}" },
                { "Mobile", $"Mobile:{Mobile}" }
            };
        }
    }
}