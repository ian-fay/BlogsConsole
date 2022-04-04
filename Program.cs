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
            string stringChoice = "";
            int intChoice = 0;

                do
            {
                // ask user a question
                Console.WriteLine("Enter your selection:");
                Console.WriteLine("1) Display all blogs");
                Console.WriteLine("2) Add Blog");
                Console.WriteLine("3) Create Post");
                Console.WriteLine("4) Display Posts");
                Console.WriteLine("Enter 'q' to quit");
                stringChoice = Console.ReadLine();
                
                if (stringChoice == "1")
                {
                    logger.Info("User choice: {Choice}", stringChoice);
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

                else if (stringChoice == "2")
                {
                    logger.Info("User choice: {Choice}", stringChoice);
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
                else if(stringChoice == "3") 
                {          
                    logger.Info("User choice: {Choice}", stringChoice);
                    try {
                    var db = new BloggingContext();
                    // Display all Blogs from the database, in the context of which one to be posted to. 
                        var query = db.Blogs.OrderBy(b => b.BlogId);

                        Console.WriteLine("Select what blog you would like to post to:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ") " + item.Name);
                        }

                            try {
                            intChoice = Int32.Parse(Console.ReadLine());
                            }

                             catch(Exception) {
                                logger.Error("Invalid blog ID");
                            }
                            Post post = new Post();
                            Console.WriteLine("Enter the Post Title:");
                            string postTitle = Console.ReadLine();
                            Console.WriteLine("Enter the Post Content:");
                            string postContent = Console.ReadLine();

                            post.BlogId = intChoice;
                            post.Title = postTitle;
                            post.Content = postContent;

                            db.AddPost(post);

                            logger.Info("Post added - {title}", postTitle);

                         var doesBlogExist = db.Blogs.Where(b => b.BlogId == intChoice).Count();
                         if(doesBlogExist == 0) {
                             logger.Error("There are no Blogs saved with that ID");
                         } 


                    } catch(Exception ex) {
                        logger.Error(ex.Message);
                        Console.WriteLine("\n");
                    }


                }
                else if(stringChoice == "4") 
                {
                    int postCount = 0;
                    logger.Info("User choice: {Choice}", stringChoice);

                        Console.WriteLine("Enter your selection:");
                        Console.WriteLine("0)Display all posts from all blogs");

                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.BlogId + ")Posts from " + item.Name);
                        }

                            try {
                            intChoice = Int32.Parse(Console.ReadLine());
                            }

                             catch(Exception) {
                                logger.Error("Invalid blog ID");
                            }
                        
                        if(intChoice == 0) {
                            var allPostScan = db.Posts.OrderBy(b => b.BlogId);

                            foreach (var item in allPostScan) 
                            {
                                postCount++;
                            }
                            
                            Console.WriteLine(postCount + " post(s) returned");

                            foreach (var item in allPostScan)
                            {   
                                Console.WriteLine($"Blog:{item.Blog.Name}\nTitle:{item.Title}\nContent:{item.Content}");
                            }
                            Console.WriteLine("\n");

                        } else {
                            var specificPostScan = db.Posts.Where(b => b.BlogId == intChoice).OrderBy(b => b.BlogId);

                            foreach (var item in specificPostScan) 
                            {
                                postCount++;
                            }

                            Console.WriteLine(postCount + " post(s) returned");

                            foreach (var item in specificPostScan)
                            {   
                                Console.WriteLine($"Blog:{item.Blog.Name}\nTitle:{item.Title}\nContent:{item.Content}");
                            }
                            Console.WriteLine("\n");
                        }
    
                }

            } while (stringChoice == "1" || stringChoice == "2" || stringChoice == "3" || stringChoice == "4");

            logger.Info("Program ended");
        }
    }
}