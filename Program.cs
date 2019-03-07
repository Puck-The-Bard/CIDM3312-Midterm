using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ShopList
{

    //GLOBAL VARIABLES
    static class Globals
    {
        public static Customer CxLogedIn;
    }
    class Program
    {
        static void Main(string[] args)
        {
            //============================Ensure Database is created==============================
            using (var db = new ShopDbContext())
            {
               db.Database.EnsureDeleted(); // for testing

                db.Database.EnsureCreated();
         
                 
                List<Product> products = new List<Product>() {
                new Product { Description = "Cards Against Humanity", Price = 25.00M },
                new Product { Description = "WHAT DO YOU MEME? Party Game", Price = 29.97M },
                new Product { Description = "Manhattan Toy Winkel Rattle and Sensory Teether Toy", Price = 14.00M },
                new Product { Description = "Spider-Man: Into the Spider-Verse [Blu-ray]", Price = 22.95M },
                new Product { Description = "Aquaman [Blu-ray]", Price = 24.96M },
                new Product { Description = "Fantastic Beasts: The Crimes of Grindelwald (Blu-ray + DVD + Digital Combo Pack), (BD)", Price = 24.96M },
                new Product { Description = "Fire TV Stick 4K with all-new Alexa Voice Remote, streaming media player", Price = 49.99M },
                new Product { Description = "Fire TV Cube, hands-free with Alexa and 4K Ultra HD (includes all-new Alexa Voice Remote), streaming media player", Price = 119.99M },
                new Product { Description = "All-new Echo Plus (2nd Gen) - Premium sound with built-in smart home hub - Charcoal", Price = 119.99M },
                new Product { Description = "Echo Dot (3rd Gen) - New and improved smart speaker with Alexa - Charcoal", Price = 49.99M },
                new Product { Description = "All-new Kindle Paperwhite – Now Waterproof with 2x the Storage – Includes Special Offers", Price = 99.99M },
                new Product { Description = "Nintendo Switch – Neon Red and Neon Blue Joy-Con", Price = 299.99M },
                new Product { Description = "Super Smash Bros. Ultimate (Platform: Nintendo Switch)", Price = 56.99M},
                new Product { Description = "Kingdom Hearts III (Platform: PlayStation 4)", Price = 59.99M }
                };

                db.AddRange(products);

                db.SaveChanges();
            }


            string UsrEmail = "null";
            Console.WriteLine("Please Login with your eamil:"); //prompt user for email
            UsrEmail = Console.ReadLine(); //save email

            //checking if they are there and prompting them accordingly
            if(IsUserHere(UsrEmail)) //If the user is in the system prompt them with a menu and execute proper functions
            {

                Console.WriteLine("\n\nWelcome to the Super Mega Ultra Store 9000" + "\n"); 
                
                MenuSelection();
            }
            else //if the user doesn't exist ask them if they want to make a new account and if they don't exit them from the app
            {
                String MakeAccount = "n"; //user input value checking if they really want to make a new account
                
                for(;;) //making sure the user doesn't input garbage, and making sure they want a new account
                {
                    Console.WriteLine("Would you like to create a new account? (y/n)");
                    MakeAccount = Console.ReadLine();
                    if(MakeAccount == "y" || MakeAccount == "Y")
                    {
                        CreateUsr(UsrEmail);
                        MenuSelection();
                        break;
                    }
                    else if(MakeAccount == "n" || MakeAccount == "N")
                    {
                        Console.WriteLine("Alright, later skater");
                        System.Environment.Exit(1);
                    }


                    }
                }
            

        }

        //==========================================METHODS========================================


        static bool IsUserHere(string UsrName) //checking to see if user is in the database
        {

            using (var db = new ShopDbContext())
            {
                try //finding out if the user is in there or not
                {
                    Customer FindUser = db.Customers.Where(p => p.Email == UsrName).First();
                    
                    Globals.CxLogedIn = FindUser;

                    Console.WriteLine($"\n\nWelcome Back " + Globals.CxLogedIn.FirstName);

                    return true;
                }
                catch //if an error is thrown then the return is false
                {
                    return false;
                }
            }
        }

        static void CreateUsr(string UsrEmail) //creates a new user
        {
            String FName = "First";
            String LName = "Second";

            Console.WriteLine("We'll go ahead and add you to the system, welcome to the site.  Also expect lots of spam emails from here on out btw");
                
                Console.WriteLine("Please Enter Your First Name");
                FName = Console.ReadLine();

                Console.WriteLine("Please Enter Your Last Name");
                LName = Console.ReadLine();

                using(var db = new ShopDbContext())  //adding customer to database
                    {
                        Customer NewCustomer = new Customer{Email = UsrEmail, FirstName = FName, LastName = LName, Cart = new Cart()};
                        Globals.CxLogedIn = NewCustomer;
                        db.Add(NewCustomer);
                        db.SaveChanges();
                    }
        }

        static void MenuSelection() //takes user menu selection and executes the proper meethods
        {
            int UsrIn = 0;

            Console.WriteLine($"\nWhat would you like to do?\n" + 
            "\n 1 - List all products" +
            "\n 2 - List all products in cart" +
            "\n 3 - Add product to Cart" +
            "\n 4 - Remove product from Cart" +
            "\n 5 - exit" +
            "\n");

            try
            {
            UsrIn = Convert.ToInt32(Console.ReadLine());
            
            switch(UsrIn)
                {
                    case 1:
                            Console.Clear();
                            Console.WriteLine("\n\n\n==================================\n\n");
                            //list all products
                            ListProducts();
                            break;
                        case 2:
                            //list all products in cart
                            ListCart();
                            break;
                        case 3:
                        //Add product to cart
                            AddToCart();
                         
                            break;
                        case 4:
                            //remove a product from cart
                            break;
                        case 5:
                            Console.WriteLine("See you next time");
                            System.Environment.Exit(1);
                            break;
                        default:
                            System.Environment.Exit(1);
                            break;
                }
            }
            catch
            {
                Console.WriteLine("\n----------------PLEASE PICK A NUMBER----------------");
                MenuSelection();
            }
        }

        //lists all products available
        static void ListProducts() 
        {
          using (var db = new ShopDbContext())
            {
                
                foreach(var Q in db.Products)
                {
                    Console.WriteLine(Q + "\n");
                }

                MenuSelection();
            }   
        }


        //Lists all items in users cart
        static void ListCart()
        {
            using(var db = new ShopDbContext())
            {
                foreach(var d in db.CxCart.Where(c => c.CartID == Globals.CxLogedIn.Cart.CartID)) //list all items in cart
                {
                    Console.Write("\tQuantity: " + d + "\t - ");
                    foreach(var p in db.Products.Where(x => x.ProductID == d.ProductID))
                    {
                        Console.WriteLine(p.Description + "  " + p.Price);
                    }
                    
                }
            }    
        }

        //adds product to users cart
        static void AddToCart()
        {
            int ProductToAdd = 0;
                try{
                    Console.WriteLine("Which product would you like to add? (please specify product ID)");
                    ProductToAdd = Convert.ToInt32(Console.ReadLine()); //has user select a product
                    
                    Console.WriteLine("How many would you like?");
                    int QuantityToAdd = Convert.ToInt32(Console.ReadLine()); //asks user how many they would like to add

                    using (var db = new ShopDbContext()) //inputs data into database
                        {
                            var StuffToAdd = db.Products.Where(i => i.ProductID == ProductToAdd).First();
                            Console.WriteLine(StuffToAdd);
                            Console.WriteLine(Globals.CxLogedIn);
                            CxCart NewProduct = new CxCart{CartID = Globals.CxLogedIn.Cart.CartID, ProductID = StuffToAdd.ProductID, Quantity = QuantityToAdd};

                            db.Add(NewProduct);
                            db.SaveChanges();

                            MenuSelection();
                        }
                    
                }

                 catch{
                    Console.WriteLine("sorry that is not a valid product, please try again");
                    AddToCart();
                }
            
        }
    }
}
