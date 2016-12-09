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
    public class MessageController : Controller
    {
        // property of message controller
        private readonly MessageFactory messageFactory;
        private readonly CommentFactory commentFactory;
        // dependency injections of property created earlier
        public MessageController(MessageFactory message, CommentFactory comment)
        {
            messageFactory = message;
            commentFactory = comment;
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.Errors = "";
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
                ViewBag.Messages = messageFactory.GetMessages();
                ViewBag.Comments = commentFactory.GetComments();
                return View("Dashboard");
            }

            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        [Route("message")]
        public IActionResult Add(Message newMessage)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (ModelState.IsValid)
                {
                    TryValidateModel(newMessage);
                    int user_id = (int)HttpContext.Session.GetInt32("UserId");
                    messageFactory.AddMessage(newMessage, user_id);
                    return RedirectToAction("Dashboard");
                }
                else
                {
                    // Errors list creation for storage in TempData to be passed to index method
                    List<string> Errors = new List<string>();
                    System.Console.WriteLine(ModelState.Values);
                    foreach (var error in ModelState.Values)
                    {
                        // Checks if there is Errors in error?
                        if (error.Errors.Count > 0)
                        {
                            Errors.Add(error.Errors[0].ErrorMessage);
                        }
                    }
                    TempData["Errors"] = Errors;
                }
            }
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        [Route("message/delete/{MessageId}")]
        public IActionResult Delete(int MessageId)
        {
            System.Console.WriteLine(MessageId);
            if (HttpContext.Session.GetString("UserId") != null)
            {
                messageFactory.DeleteMessage(MessageId, (int)HttpContext.Session.GetInt32("UserId"));
                return RedirectToAction("Dashboard");
            }
            else{
                System.Console.WriteLine("User not logged in.");
                return RedirectToAction("Index", "Login");
            }
        }
    }
}