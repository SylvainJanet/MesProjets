﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiseEnSituation.Models
{
    public class TrainingCourse
    {
        public int? Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartingDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndingDate { get; set; }

        [Required]
        public double DurationInHours { get; set; }

        public double? Price { get; set; }

        public List<Employee> EnrolledEmployees { get; set; }

        [Required]
        public List<Skill> TrainedSkills { get; set; }

        public TrainingCourse(string name, DateTime startingDate, DateTime endingDate, double durationInHours, List<Skill> trainedSkills)
        {
            Name = name;
            StartingDate = startingDate;
            EndingDate = endingDate;
            DurationInHours = durationInHours;
            TrainedSkills = trainedSkills;
            EnrolledEmployees = new List<Employee>();
        }

        public TrainingCourse()
        {
            EnrolledEmployees = new List<Employee>();
            TrainedSkills = new List<Skill>();
        }
    }
}