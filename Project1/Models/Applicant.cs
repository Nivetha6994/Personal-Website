using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage ="Project Title cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$",ErrorMessage="Please enter a valid alphanumeric entry")]
        [StringLength(50,ErrorMessage ="The entry is too long")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Technology Used cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [StringLength(50, ErrorMessage = "The entry is too long. Please enter only the major technologies used")]
        public string Technology { get; set; }

        [Required(ErrorMessage ="Project Description cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [StringLength(150, ErrorMessage = "The entry is too long. Please keep the description brief")]
        public string Description { get; set; }        
        public ProjectType ptype { get; set; }
    }

    public enum ProjectType
    {
        Academic, 
        Professional,
        Personal
    }
    public enum ProjectDisplay
    {
        All,
        Academic,
        Professional,
        Personal
        
    }
    public class Academic
    {
        public int AcademicId { get; set; }

        [Required(ErrorMessage ="University Name cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please enter a valid entry(comprising of only alphabets)")]
        [Display(Name ="University Name")]
        public string SchoolName { get; set; }

        [Required(ErrorMessage ="Degree Name cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please enter a valid entry(comprising of only alphabets)")]
        [Display(Name = "Degree Name")]
        public string Degree { get; set; }

        [Required(ErrorMessage ="Entry cannot be blank")]
        [RegularExpression(@"^([1-9]{1}[0-9]{3})$", ErrorMessage = "Please enter a valid year")]
        [Display(Name = "Graduation Year")]
        public string GraduationYear { get; set; }

        [Required(ErrorMessage = "Entry cannot be blank")]
        [RegularExpression(@"^([0-4]{1}\.[0-9]{2})$", ErrorMessage = "Please enter a valid GPA in the range between (0.00 to 4.00)")]
        public string GPA { get; set; }

    }

    public class Professional
    {
        public int ProfessionalId { get; set; }


        [Required(ErrorMessage = "Company Name cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Department Name cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Role Description cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [StringLength(250, ErrorMessage = "The entry is too long. Please keep the description brief")]
        [Display(Name = "Role Description")]
        public string RoleDescription { get; set; }

        [Required(ErrorMessage = "Technology Used cannot be blank")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$", ErrorMessage = "Please enter a valid alphanumeric entry")]
        [StringLength(50, ErrorMessage = "The entry is too long. Please enter only the major technologies used")]
        [Display(Name = "Technologies Used")]
        public string TechnologiesUsed { get; set; }
    }

    public class FileData
    {

        public int FileDataId { get; set; }
        public string FileName { get; set; }

        public string FilePath { get; set; }
        public string FileDescription { get; set; }
    }

}
