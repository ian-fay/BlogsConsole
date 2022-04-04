using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            logger.Info("Program started");

            //Console.WriteLine("Hello World!");
            string choice = "";

                do
            {
                // ask user a question
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Display all blogs");
                Console.WriteLine("2) Add Blog");
                Console.WriteLine("3) Create Post");
                Console.WriteLine("4) Display Posts");
                Console.WriteLine("Enter 'q' to quit");
                choice = Console.ReadLine();
                
                if (choice == "1")
                {
                    logger.Info("User choice: {Choice}", choice);
                    try {
                    var db = new BloggingContext();
                    // Display all Blogs from the database
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        Console.WriteLine("\n");
                    } catch(Exception ex) {
                        logger.Error(ex.Message);
                        Console.WriteLine("\n");
                    }
                    
                }

                else if (choice == "2")
                {
                    logger.Info("User choice: {Choice}", choice);
                    try
                    {

                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();

                        var blog = new Blog {Name = name};

                        var db = new BloggingContext();
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                        Console.WriteLine("\n");

                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex.Message);
                        Console.WriteLine("\n");
                    }

                }
                else if(choice == "3") 
                {          
                    logger.Info("User choice: {Choice}", choice);

                }
                else if(choice == "4") 
                {
                    logger.Info("User choice: {Choice}", choice);
                }

            } while (choice == "1" || choice == "2" || choice == "3" || choice == "4");

            logger.Info("Program ended");
        }
    }
}