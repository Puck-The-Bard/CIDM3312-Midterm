using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShopList
{
    public class ShopDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data source = ShopDb.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //creates composit key
        {
            modelBuilder.Entity<CxCart>()
                .HasKey(e => new {e.CartID, e.ProductID});
        }
        public DbSet<Customer> Customers {get; set;}

        public DbSet<Cart> Carts{get; set;}

        public DbSet<Product> Products {get; set;}

        public DbSet<CxCart> CxCart {get; set;}
    }

    public class Customer
    {
        public int CustomerID {get; set;} //pk

        public string FirstName {get; set;}

        public string LastName {get; set;}

        public string Email {get; set;}

        public Cart Cart {get; set;} // nav property

        public override string ToString()
        {
            return$"{CustomerID} {FirstName} {LastName} {Email} {Cart}";
        }

    }

    public class Cart
    {
        public int CartID {get; set;} //pk

        public List<CxCart> productLink{get; set;}

        public override string ToString()
        {
            return$"Cart ID: {CartID}";
        }

    }

    public class Product
    {
        public int ProductID {get; set;} //pk

        public string Description {get; set;}

        public decimal Price {get; set;}

        public List<CxCart> productLink{get; set;}

         public override string ToString()
        {
            return$"{ProductID}    -   {Description}   -   ${Price}";
        }
        

    }

    public class CxCart
    {
        public int CartID {get; set;} // composit key

        public int ProductID {get; set;} // composit key

        public Cart Cart {get; set;} //nav property

        public Product Product {get; set;} //nav property
        
        public int Quantity {get; set;}

        public override string ToString()
        {
            return$" {Quantity}";
        }
    }
}