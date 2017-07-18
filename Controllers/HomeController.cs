using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using M101N.Models.Home;
using M101N.Models;
using MongoDB.Bson;
using System.Linq.Expressions;
using MongoDB.Driver;
using System.Globalization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace M101N.Controllers
{
    public class HomeController : Controller
    {
        private BlogContext _blogContext;
        public HomeController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public async Task<ActionResult> Index()
        {
            var recentPosts = await _blogContext.Posts.Find<Post>(x => true).SortByDescending(x => x.CreatedAtUtc).Limit(10).ToListAsync();

            var model = new IndexModel
            {
                RecentPosts = recentPosts
            };

            return View(model);

        }

        [HttpGet]
        public async Task<ActionResult>  Animals()
        {
            var client =  _blogContext.Client;
            var db = client.GetDatabase("test");
            var animals = db.GetCollection<BsonDocument>("animals");

            try
            {
            var animal = new BsonDocument {
                                {"animal", "monkey"}
                            };

            await animals.InsertOneAsync(animal);
            animal.Remove("animal");
            animal.Add("animal", "cat");
            await animals.InsertOneAsync(animal);
            animal.Remove("animal");
            animal.Add("animal", "lion");
            await animals.InsertOneAsync(animal);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return RedirectToAction("Index");

        }
        
        [HttpGet]
        public ActionResult NewPost()
        {
            return View(new NewPostModel());
        }

        [HttpPost]
        public async Task<ActionResult> NewPost(NewPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            try
            {

                var identity = (ClaimsIdentity)User.Identity;
                var nameValue = identity.Claims.Where(x => x.Type == ClaimTypes.GivenName).Single().Value;
                var post = new Post
                {
                    Author = nameValue,
                    Title = model.Title,
                    Content = model.Content,
                    Tags = model.Tags.Split(' ', ',', ';').ToList(),
                    CreatedAtUtc = DateTime.UtcNow,
                    Comments = new List<Comment>()

                };
                await _blogContext.Posts.InsertOneAsync(post);

                return RedirectToAction("Post", new { id = post.Id });
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return RedirectToAction("Index");
            }

        [HttpGet]
        public async Task<ActionResult> Post(string id)
        {

            var post = await _blogContext.Posts.Find(x => x.Id == id).SingleOrDefaultAsync();

            if (post == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PostModel
            {
                Post = post,
                NewComment = new NewCommentModel
                {
                    PostId = id
                }
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Posts(string tag = null)
        {

            Expression<Func<Post, bool>> filter = x => true;

            if (tag != null)
            {
                filter = x => x.Tags.Contains(tag);
            }
            var posts = await _blogContext.Posts.Find(filter)
                        .SortByDescending(x => x.CreatedAtUtc)
                        .Limit(10)
                        .ToListAsync();

            return View(posts);
        }

        [HttpPost]
        public async Task<ActionResult> NewComment(NewCommentModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }
            var identity = (ClaimsIdentity)User.Identity;
            var nameValue = identity.Claims.Where(x => x.Type == ClaimTypes.GivenName).Single().Value;

            var comment = new Comment
            {
                Author = nameValue,
                Content = model.Content,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _blogContext.Posts.UpdateOneAsync(
                 x => x.Id == model.PostId,
                Builders<Post>.Update.Push(x => x.Comments, comment));
            // XXX WORK HERE
            // add a comment to the post identified by model.PostId.
            // you can get the author from "this.User.Identity.Name"

            return RedirectToAction("Post", new { id = model.PostId });
        }

        [HttpPost]
        public async Task<ActionResult> CommentLike(CommentLikeModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = model.PostId });
            }
            var builder = Builders<Post>.Filter;
            var filter = builder.Eq("Id", model.PostId);
            var update = Builders<Post>.Update.Inc(string.Format(CultureInfo.InvariantCulture, "Comments.{0}.Likes", model.Index), 1);
            var result = await _blogContext.Posts.UpdateOneAsync(filter, update);

            return RedirectToAction("Post", new { id = model.PostId });
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        //Making POST request to http://localhost:57912/Home/NewPost with parameters:
        //  'Title'='EJDxPRTryz'
        //  'Content'='WMVCPpifjVFVskIGlBvDc'
        //  'Tags'='LXQ,rCz,wcc'
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
