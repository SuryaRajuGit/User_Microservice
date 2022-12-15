using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace User_Microservice.Entity.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<Address> Address { get; set; }

        public DbSet<Card> Card { get; set; }

        public DbSet<Phone> Phone { get; set; }

        public DbSet<UserSecret> UserSecret { get; set; }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string path = @"C:\Users\Hp\source\repos\User_Microservice\User_Microservice\Entity\Files\Admin.csv";
            string ReadCSV = File.ReadAllText(path);
            var data = ReadCSV.Split('\r');
            // var list = new List<RefTerm>();
            foreach (string item in data)
            {
                string[] row = item.Split(",");
                Guid Id = Guid.NewGuid();
                UserSecret userSecret = new UserSecret {Id = Guid.NewGuid(), UserId = Id, Password = row[0]};
                User user = new User { Role = row[1], FirstName = row[2], LastName = row[3], EmailAddress = row[4], Id = Id };
                Phone phone = new Phone { Id = Guid.NewGuid(),PhoneNumber =row[5],Type=row[6],UserId=Id };
                Address address = new Address {Id=Guid.NewGuid(),Line1 = row[7], Line2 = row[8], City = row[9], Zipcode = row[10], StateName = row[11], Country = row[12], Type = row[13],UserId=Id };

                modelBuilder.Entity<User>().HasData(user);
                modelBuilder.Entity<Phone>().HasData(phone);
                modelBuilder.Entity<Address>().HasData(address);
                modelBuilder.Entity<UserSecret>().HasData(userSecret);
            }
        }
    }
}
