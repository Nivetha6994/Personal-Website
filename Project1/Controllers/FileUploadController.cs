using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project1.Data;
using Project1.Models;

namespace Project1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment_;
        private string webRootPath = null;
        private string filePath = null;
        private readonly ApplicationDbContext context_;
        
        public FileUploadController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
        {
            hostingEnvironment_ = hostingEnvironment;
            webRootPath = hostingEnvironment_.WebRootPath;
            filePath = Path.Combine(webRootPath, "FileStorage");
            context_ = context;
        }


        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
           var request = HttpContext.Request;

            string desc = request.Form["DescriptionF"];

            foreach (var file in request.Form.Files)
            {

                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    FileData fd = new FileData
                    {
                        FileDescription = desc,
                        FileName = fileName
                    };
                    var path = Path.Combine(filePath, file.FileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    fd.FilePath = path;
                    context_.FilesData.Add(fd);
                    context_.SaveChanges();

                }
                else
                {
                    return BadRequest();
                }
            }
            return RedirectToAction("File","Applicants");
            //return Ok();
        }

        //[Roles("Admin", "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadFile(int id,int? flag)
        {
            if (flag == null)
            {
                List<string> files = null;
                string filename = "";
                try
                {
                    files = Directory.GetFiles(filePath).ToList<string>();
                    if (0 <= id && id < files.Count)
                        filename = Path.GetFileName(files[id]);
                    else
                        return NotFound();
                }
                catch
                {
                    return NotFound();
                }
                var mem = new MemoryStream();
                filename = files[id];
                using (var stream = new FileStream(filename, FileMode.Open))
                {
                    await stream.CopyToAsync(mem);
                }
                mem.Position = 0;
                return File(mem, GetContentType(filename), Path.GetFileName(filename));
            }
            FileData fileList = context_.FilesData.Find(id);
            var memory = new MemoryStream();
            var file = fileList.FilePath;
            using (var stream = new FileStream(file, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(file), Path.GetFileName(file));
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            FileData fileList = context_.FilesData.Find(id);
            var file = fileList.FilePath;
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                context_.FilesData.Remove(fileList);
                context_.SaveChanges();
            }
            return RedirectToAction("File", "Applicants");
            //return Ok();
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