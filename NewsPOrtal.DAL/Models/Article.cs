﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace NewsPOrtal.DAL.Models
{
    [Table("Articles")]
    public class Article
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Tittle")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter short description")]
        [Display(Name = "Short Description")]
        public string Description { get; set; }
        [AllowHtml]
        [Required(ErrorMessage = "Please enter body")]
        public string Body { get; set; }

        [Display(Name = "Created On")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd,yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Please enter article category")]
        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int? Category_Id { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Is Published")]
        public bool IsPublished { get; set; }
        [Required(ErrorMessage = "Please upload image")]
        public string ImageTitle { get; set; }
        [Display(Name = "Upload File")]
        public string ImagePath { get; set; }

    }
}
