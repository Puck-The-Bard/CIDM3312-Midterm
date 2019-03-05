using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShopList
{
    class Program
    {
        static void Main(string[] args)
        {
            //============================Ensure Database is created==============================
            using (var db = new ShopDbContext())
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            }

            

        }
    }
}
