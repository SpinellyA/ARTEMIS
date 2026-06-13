using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;


    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        private User()
        {
             
        }

    public static User Create(string username, string password)
    {
        if (string.IsNullOrEmpty(username)) throw new DomainException("Invalid username!");
        if (string.IsNullOrEmpty(password)) throw new DomainException("Invalid password!");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            PasswordHash = password
        };

        return user;

    }

    
    
    }

