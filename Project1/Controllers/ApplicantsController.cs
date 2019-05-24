using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Project1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Project1.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly ApplicationDbContext context_;
        public ApplicantsController(ApplicationDbContext context)
        {
            context_ = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Roles("Admin", "User")]
        //Get Project Details
        public IActionResult Project(int ?id)
        {
            var ptypes = from ProjectDisplay p in Enum.GetValues(typeof(ProjectDisplay))
                         select new { type = p.ToString() };
            ViewBag.ProjectDisplayDDL = new SelectList(ptypes, "type", "type");
            List<Project> projList = context_.Projects.ToList<Project>();
            List<Project> data;
            if (id == 1)
            {
                var projEntry = from proj in projList
                                where proj.ptype == ProjectType.Professional
                               select proj;
                ViewBag.ProjectDisplayDDL = new SelectList(ptypes, "type", "type",ProjectDisplay.Professional);
                data = projEntry.ToList();
            }
            else if (id == 2)
            {
                var projEntry = from proj in projList
                                where proj.ptype == ProjectType.Academic
                                select proj;
                ViewBag.ProjectDisplayDDL = new SelectList(ptypes, "type", "type", ProjectDisplay.Academic);
                data = projEntry.ToList();
            }
            else
            {
                data = projList;
            }
            return View(data);
        }
        [Roles("Admin", "User")]
        [HttpPost]
        public IActionResult Project(IFormCollection form)
        {
            var ptypes = from ProjectDisplay p in Enum.GetValues(typeof(ProjectDisplay))
                         select new { type = p.ToString() };
            string strDDLValue = form["ProjectDisplayDDL"].ToString();
            List<Project> projList = context_.Projects.ToList<Project>();
            List<Project> data;
            if(strDDLValue.Equals(ProjectDisplay.All.ToString()))
            {
                data = projList;
            }
            else
            {
                var projEntry = from proj in projList
                                where (proj.ptype.ToString()).Equals(strDDLValue)
                                select proj;
                data = projEntry.ToList();
            }
            ViewBag.ProjectDisplayDDL = new SelectList(ptypes, "type", "type", form["ProjectDisplayDDL"]);
            return View(data);
        }


        [Authorize(Roles ="Admin")]
        //Create A Project
        [HttpGet]
        public IActionResult CreateProject(int? id)
        {
            var model = new Project();
            var ptypes = from ProjectType p in Enum.GetValues(typeof(ProjectType))
                         select new { type = p.ToString() };
            //ViewBag.ProjectTypeDDL = new SelectList(ptypes, "typeId", "type");
            //model.ptype = new SelectList(ptypes, "OrderTemplateId", "OrderTemplateName", 1);
            if (id == 1)
            {
                model.ptype = ProjectType.Professional;
                //ViewBag.ProjectTypeDDL = new SelectList(ptypes, "typeId", "type", model.ptype);
            }
            else if (id == 2)
            {
                model.ptype = ProjectType.Academic;
                //ViewBag.ProjectTypeDDL = new SelectList(ptypes, "typeId", "type", model.ptype);
            }
            return View(model);
        }

        //----< posts back new proj details >---------------------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProject(int id, Project proj)
        {
            var ptypes = from ProjectType p in Enum.GetValues(typeof(ProjectType))
                         select new { type = p.ToString() };
            if (ModelState.IsValid)
            {
                context_.Projects.Add(proj);
                context_.SaveChanges();
                return RedirectToAction("Project");
            }
            ViewBag.ProjectTypeDDL = new SelectList(ptypes, "type", "type", proj.ptype);
            return View(proj);
        }
        [Authorize(Roles = "Admin")]
        //Edit Project
        [HttpGet]
        public IActionResult EditProject(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Project proj = context_.Projects.Find(id);

            if (proj == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(proj);
        }

        //----< posts back edited results for specific proj >------
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditProject(int? id, Project proj)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var project = context_.Projects.Find(id);

            if (project != null)
            {
                project.Title = proj.Title;
                project.Technology = proj.Technology;
                project.Description = proj.Description;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("Project");
        }

        [Authorize(Roles = "Admin")]
        //Delete Project
        public IActionResult DeleteProject(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var proj = context_.Projects.Find(id);
                if (proj != null)
                {
                    context_.Projects.Remove(proj);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Project");
        }
        [Roles("Admin", "User")]
        //Get Academic Details
        public IActionResult Academic()
        {
            return View(context_.Academics.ToList<Academic>());
        }

        [Authorize(Roles = "Admin")]
        //Create An Academic record
        [HttpGet]
        public IActionResult CreateAcademic(int id)
        {
            var model = new Academic();
            return View(model);
        }

        //----< posts back new academic details >---------------------

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateAcademic(int id, Academic acad)
        {
            if (ModelState.IsValid)
            {
                context_.Academics.Add(acad);
                context_.SaveChanges();
                return RedirectToAction("Academic");
            }
            return View(acad);
        }

        [Authorize(Roles = "Admin")]
        //Edit Academic
        [HttpGet]
        public IActionResult EditAcademic(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Academic acad = context_.Academics.Find(id);

            if (acad == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(acad);
        }

        //----< posts back edited results for specific academic entry >------

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditAcademic(int? id, Academic acad)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var academics = context_.Academics.Find(id);

            if (academics != null)
            {
                academics.Degree = acad.Degree;
                academics.GPA = acad.GPA;
                academics.GraduationYear = acad.GraduationYear;
                academics.SchoolName = acad.SchoolName;

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("Academic");
        }

        [Authorize(Roles = "Admin")]
        //Delete Academic record
        public IActionResult DeleteAcademic(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var acad = context_.Academics.Find(id);
                if (acad != null)
                {
                    context_.Remove(acad);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Academic");
        }

        [Roles("Admin", "User")]
        //Get Professional Details
        public IActionResult Professional()
        {
            return View(context_.Professionals.ToList<Professional>());
        }

        [Authorize(Roles = "Admin")]
        //Create A Professional record
        [HttpGet]
        public IActionResult CreateProfession(int id)
        {
            var model = new Professional();
            return View(model);
        }

        //----< posts back new professional details >---------------------

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProfession(int id, Professional prof)
        {
            if (ModelState.IsValid)
            {
                context_.Professionals.Add(prof);
                context_.SaveChanges();
                return RedirectToAction("Professional");
            }
            return View(prof);
        }

        [Authorize(Roles = "Admin")]
        //Edit Professional
        [HttpGet]
        public IActionResult EditProfession(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Professional prof = context_.Professionals.Find(id);

            if (prof == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(prof);
        }

        //----< posts back edited results for specific profession entry >------

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult EditProfession(int? id, Professional prof)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var profession = context_.Professionals.Find(id);

            if (profession != null)
            {
                profession.CompanyName = prof.CompanyName;
                profession.DepartmentName = prof.DepartmentName;
                profession.RoleDescription = prof.RoleDescription;
                profession.TechnologiesUsed = prof.TechnologiesUsed;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // do nothing for now
                }
            }
            return RedirectToAction("Professional");
        }


        [Authorize(Roles = "Admin")]
        //Delete Professional record
        public IActionResult DeleteProfession(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var prof = context_.Professionals.Find(id);
                if (prof != null)
                {
                    context_.Remove(prof);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Professional");
        }

        [Roles("Admin", "User")]
        //Get File Details
        public IActionResult File()
        {
            return View(context_.FilesData.ToList<FileData>());
        }

        [Authorize(Roles = "Admin")]
        //Create file
        [HttpGet]
        public IActionResult CreateFile()
        {
            return View();
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