using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Project1.Models;

namespace Project1.Controllers
{
    public class RecruitersController : Controller
    {
        private readonly ApplicationDbContext context_;
        public RecruitersController(ApplicationDbContext context)
        {
            context_ = context;
        }

        [Roles("Admin", "User")]
        [HttpGet]
        public IActionResult Comment(string name)
        {
            name = User.Identity.Name;
            //name is present in Recruiter.email-> return list of comments
            List<Recruiter> recList = context_.Recruiters.ToList<Recruiter>();
            var recEntry = from rec in recList
                           where rec.RecruiterEmail == name
                           select rec;
            List<Recruiter> data;
            //select rec.RecruiterId;
            
            //else admin user, so return entire list
            if (recEntry != null && User.IsInRole("User"))
            {
               data=recEntry.ToList();
            }
            else
            {
               data = recList;
            }
            return View(data);

        }

        [Authorize(Roles = "User")]
        //Edit Comment
        [HttpGet]
        public IActionResult EditComment(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Recruiter rec = context_.Recruiters.Find(id);

            if (rec == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(rec);
        }

        //----< posts back edited results for specific profession entry >------
        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult EditComment(int? id, Recruiter rec)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var recruiter = context_.Recruiters.Find(id);

            if (recruiter != null)
            {
                recruiter.Comments = rec.Comments;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("Comment");
        }


        [Authorize(Roles = "User")]
        //Create A Comment
        [HttpGet]
        public IActionResult CreateComment(int id,string email)
        {
            
            Recruiter rec = context_.Recruiters.Find(id);
            return View(rec);
        }

        //----< posts back new Comment details >---------------------

        [Authorize(Roles = "User")]
        [HttpPost]
        public IActionResult CreateComment(int id, Recruiter rec)
        {
            if (ModelState.IsValid)
            {
                Recruiter recruiter = context_.Recruiters.Find(id);
                if (recruiter != null)
                {
                    recruiter.Comments = rec.Comments;
                    context_.SaveChanges();
                }
                return RedirectToAction("Comment");
            }
            return View(rec);
        }

        [Roles("Admin", "User")]
        //Delete Comments
        public IActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var comment = context_.Recruiters.Find(id);
                if (comment != null)
                {
                    comment.Comments = null;
                    //context_.Remove(proj);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Comment");
        }

        public class RolesAttribute : AuthorizeAttribute
        {
            public RolesAttribute(params string[] roles)
            {
                Roles = String.Join(",", roles);
            }
        }
    }
}