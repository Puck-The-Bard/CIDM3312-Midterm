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
            optionsBuilder.UseSqlite("Data source = MovieDb.db");
        }
        public DbSet<Customer> Customers {get; set;}

        public DbSet<Cart> Carts{get; set;}

        public DbSet<Product> Product {get; set;}

        public DbSet<CxCart> CxCart {get; set;}
    }

    public class Customer
    {
        public int CustoemrID {get; set;}

        public string CxFirstName {get; set;}

        public string CxLastName {get; set;}

        public string CxEmail {get; set;}
    }

    public class Cart
    {
        public int CartID {get; set;}

        public CxCart CxCart {get; set;}
    }

    public class Product
    {
        public int ProductID {get; set;}

        public CxCart CxCart {get; set;}
    }

    public class CxCart
    {

        
    }
}