// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using M101N.Models.NewPost;
// using M101N.Models;
// using MongoDB.Bson;
// using System.Linq.Expressions;
// using MongoDB.Driver;
// using System.Globalization;

// namespace M101N.Controllers
// {
//     public class NewPostController : Controller
//     {
//         private BlogContext _blogContext;
//         public NewPostController(BlogContext blogContext)
//         {
//             _blogContext = blogContext;
//         }

//         [HttpGet]
//         public ActionResult NewPost()
//         {
//             return View(new NewPostModel());
//         }

//         [HttpPost]
//         public async Task<ActionResult> NewPost(NewPostModel model)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return View(model);
//             }

//             // XXX WORK HERE
//             // Insert the post into the posts collection
//             var post = new Post
//             {
//                 Author = User.Identity.Name,
//                 Title = model.Title,
//                 Content = model.Content,
//                 Tags = model.Tags.Split(' ',',',';').ToList(),
//                 CreatedAtUtc = DateTime.UtcNow,
//                 Comments = new List<Comment>()

//             };
//             await _blogContext.Posts.InsertOneAsync(post);
//             return RedirectToAction("Post", new { id = post.Id });
//         }

//         [HttpGet]
//         public async Task<ActionResult> Post(string id)
//         {
            
//             var post = await _blogContext.Posts.Find(x=> x.Id == id).SingleOrDefaultAsync();

//             // XXX WORK HERE
//             // Find the post with the given identifier

//             if (post == null)
//             {
//                 return RedirectToAction("Index");
//             }

//             var model = new PostModel
//             {
//                 Post = post,
//                 NewComment = new NewCommentModel
//                 {
//                     PostId = id
//                 }
//             };

//             return View(model);
//         }

//         [HttpGet]
//         public async Task<ActionResult> Posts(string tag = null)
//         {
            
//             Expression<Func<Post, bool>> filter = x=> true;

//             if (tag != null)
//             {
//                 filter = x=> x.Tags.Contains(tag);
//             }
//             var posts = await _blogContext.Posts.Find(filter)
//                         .SortByDescending(x=> x.CreatedAtUtc)
//                         .Limit(10)
//                         .ToListAsync();

//             return View(posts);
//         }

//         [HttpPost]
//         public async Task<ActionResult> NewComment(NewCommentModel model)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return RedirectToAction("Post", new { id = model.PostId });
//             }
//             var comment = new Comment
//             {
//                 Author = User.Identity.Name,
//                 Content = model.Content,
//                 CreatedAtUtc = DateTime.UtcNow
//             };

//             await _blogContext.Posts.UpdateOneAsync(
//                  x => x.Id == model.PostId,
//                 Builders<Post>.Update.Push(x=> x.Comments, comment));
//             // XXX WORK HERE
//             // add a comment to the post identified by model.PostId.
//             // you can get the author from "this.User.Identity.Name"

//             return RedirectToAction("Post", new { id = model.PostId });
//         }

//  [HttpPost]
//         public async Task<ActionResult> CommentLike(CommentLikeModel model)
//         {
//             if (!ModelState.IsValid)
//             {
//                 return RedirectToAction("Post", new { id = model.PostId });
//             }
//             var builder = Builders<Post>.Filter;
//             var filter = builder.Eq("Id", model.PostId);
//             var update = Builders<Post>.Update.Inc(string.Format(CultureInfo.InvariantCulture, "Comments.{0}.Likes", model.Index), 1);
//     å˚        var result = await _blogContext.Posts.UpdateOneAsync (filter, update);
            
//             return RedirectToAction("Post", new { id = model.PostId });
//         }

//     }
// }
