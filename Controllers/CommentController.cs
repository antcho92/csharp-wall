using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using theWall.Models;
using theWall.Factory;
using Microsoft.AspNetCore.Identity;

namespace theWall.Controllers
{
    public class CommentController : Controller
    {
        // property of message controller
        private readonly MessageFactory messageFactory;
        private readonly CommentFactory commentFactory;
        // dependency injections of property created earlier
        public CommentController(MessageFactory message, CommentFactory comment)
        {
            messageFactory = message;
            commentFactory = comment;
        }
        [HttpPost]
        [Route("comment")]
        public IActionResult Comment(Comment newComment)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (ModelState.IsValid)
                {
                    TryValidateModel(newComment);
                    commentFactory.Add(newComment, (int)HttpContext.Session.GetInt32("UserId"));
                    return RedirectToAction("Dashboard", "Message");
                }
            }
            return RedirectToAction("Index", "Login");
        }
    }
}