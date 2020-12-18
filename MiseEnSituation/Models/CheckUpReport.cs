﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MiseEnSituation.Models
{
    public class CheckUpReport : BaseEntity
    {
        [Required]
        public string Content { get; set; }

        [InverseProperty("ReportsFinished")]
        public IList<TrainingCourse> FinishedCourses { get; set; }

        [InverseProperty("ReportsWished")]
        public IList<TrainingCourse> WishedCourses { get; set; }

        public CheckUpReport(string content) : this()
        {
            Content = content;
        }

        public CheckUpReport()
        {
            FinishedCourses = new List<TrainingCourse>();
            WishedCourses = new List<TrainingCourse>();
        }
    }
}