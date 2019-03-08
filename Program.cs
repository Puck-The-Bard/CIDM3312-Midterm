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
               //db.Database.EnsureDeleted(); // for testing

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

                //db.AddRange(products);

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


        //-----------------------------------------------checking to see if user is in the database
        static bool IsUserHere(string UsrName) 
        {

            using (var db = new ShopDbContext())
            {
                try //finding out if the user is in there or not
                {
                    
                    Customer FindUser = db.Customers.Include(c => c.Cart).Where(p => p.Email == UsrName).First();

                    //Console.WriteLine("=>>" + FindUser.Cart); error testing
                    
                    Globals.CxLogedIn = FindUser;

                    //Console.WriteLine("=>>>" + Globals.CxLogedIn.Cart); error testing

                    Console.WriteLine($"\n\nWelcome Back " + Globals.CxLogedIn.FirstName);

                    return true;
                }
                catch //if an error is thrown then the return is false
                {
                    return false;
                }
            }
        }

        //---------------------------------------------------creates a new user
        static void CreateUsr(string UsrEmail) 
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

        //-----------------------------------------------takes user menu selection and executes the proper meethods
        static void MenuSelection() 
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
                        RemoveFromCart();
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

        //-----------------------------------------------lists all products available
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


        //-----------------------------------------------Lists all items in users cart
        static void ListCart()
        {
            using(var db = new ShopDbContext())
            {
                Console.WriteLine("Items Currently in Cart \n");
                //Console.WriteLine("==========" + Globals.CxLogedIn + "=============="); for error testing
                //Console.WriteLine("=>>>" + Globals.CxLogedIn.Cart); for error testing

                decimal RunTotal = 0m;
                foreach(var d in db.CxCart.Where(c => c.Cart.CartID == Globals.CxLogedIn.Cart.CartID)) //list all items in cart
                {
                    //Console.WriteLine("=>>>>" + d.CartID); for error testing
                    Console.Write("\tQuantity: " + d + "\t - ");
                    foreach(var p in db.Products.Where(x => x.ProductID == d.ProductID))
                    {
                        Console.WriteLine("$" + p.Price * d.Quantity + " - ID: " + p.ProductID + " - " + p.Description);
                        RunTotal += p.Price*d.Quantity; //keeping a running for items in cart
                    }
                    
                }

                if(RunTotal > 50) //checking to see if customer gets free shipping
                {
                    Console.WriteLine("\nCart Total   : " + "$" + RunTotal);
                    Console.WriteLine("Shipping Cost: " + "Free on orders over $50");
                    Console.WriteLine("Grand Total  : " + "$" + RunTotal);

                }
                else if(RunTotal == 0)
                {
                    Console.WriteLine("\nCart Total   : " + "$" + RunTotal);
                    Console.WriteLine("Shipping Cost: " + "Nothing in Cart yet");
                    Console.WriteLine("Grand Total  : " + "$" + RunTotal);
                }
                else{
                    Console.WriteLine("\nCart Total   : " + "$" + RunTotal);
                    Console.WriteLine("Shipping Cost: " + "$4.99");
                    decimal Grandtotal = RunTotal + 4.99m;
                    Console.WriteLine("Grand Total  : " + "$" + Grandtotal);
                }

                MenuSelection();
            }    
        }

        //-----------------------------------------------checks to see if a product is in a users cart
        static bool IsProductInCart(Product TestProduct) 
        {
            using (var db = new ShopDbContext())
            {
                
                try{ //checks to see if product is in cart, if it isn't it'll throw an exception
                    var d = db.CxCart.Where(c => c.Cart.CartID == Globals.CxLogedIn.Cart.CartID);
                    var e = d.Where(t => t.Product == TestProduct).First();
                    //Console.WriteLine("=>> " + TestProduct.ProductID + "  vs  " + e.ProductID); //for testing.  Sees if it is checking like data and giving proper return
                    return true;
                }   
                catch{
                    return false;
                }

            }

        }

        //-----------------------------------------------adds product to users cart
        static void AddToCart()
        {
            int ProductToAdd = 0;
                try{
                    Console.WriteLine("Which product would you like to add? (please specify product ID)");
                    ProductToAdd = Convert.ToInt32(Console.ReadLine()); //has user select a product

                    using (var db = new ShopDbContext()) //inputs data into database
                        {
                            var StuffToAdd = db.Products.Where(i => i.ProductID == ProductToAdd).First();

                            if (IsProductInCart(StuffToAdd)) //if the product is already in the users cart this updates the quantity
                            {
                                var QuantUpdate = db.CxCart.Where(x => x.ProductID == StuffToAdd.ProductID && x.CartID == Globals.CxLogedIn.Cart.CartID).First(); //selects entry to be updated
                                
                                QuantUpdate.Quantity++;// add one to quantity if user ads item twice
                                db.SaveChanges();
                            }
                            else //if it is a new product this adds it
                            {
                                //Console.WriteLine(StuffToAdd); //for error testing
                                //Console.WriteLine("==========" + Globals.CxLogedIn + "=============="); //for error testing
                                CxCart NewProduct = new CxCart{CartID = Globals.CxLogedIn.Cart.CartID, ProductID = StuffToAdd.ProductID, Quantity = 1};

                                db.Add(NewProduct);
                                db.SaveChanges();
                            }

                            MenuSelection();
                        }
                    
                }

                  catch{
                    Console.WriteLine("sorry that is not a valid product, please try again");
                    AddToCart();
                }            
        }

        //-----------------------------------------------Remove items from cart
        static void RemoveFromCart()
        {
            int ProdIDForRemoval = 0;
            Console.WriteLine("Which product would you like to remove? (specify product ID in cart)");
            ProdIDForRemoval = Convert.ToInt32(Console.ReadLine()); //user selects item ID to remove from their cart
           
            using( var db = new ShopDbContext())
            {
                try //validates user input
                {
                    var ItemForRemoval = db.CxCart.Where(i => i.ProductID == ProdIDForRemoval && Globals.CxLogedIn.Cart.CartID == i.CartID).First();
                    
                    if(ItemForRemoval.Quantity > 1)
                    {
                        ItemForRemoval.Quantity--;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Remove(ItemForRemoval);
                        db.SaveChanges();
                    }
                    MenuSelection();
                }
                catch
                {
                    Console.WriteLine("Item not found in cart");
                    MenuSelection();
                }
            }


        }
    }
}
