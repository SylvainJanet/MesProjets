﻿using MiseEnSituation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MiseEnSituation.Repositories
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(MyDbContext db) : base(db)
        {
        }

        public override IQueryable<Skill> Collection()
        {
            return dbSet.AsNoTracking()
                        .Include(s => s.Courses)
                        .Include(s => s.Employees)
                        .Include(s => s.Posts)
                        .AsQueryable();
        }

        public override IQueryable<Skill> CollectionTracked()
        {
            return dbSet.Include(s => s.Courses)
                        .Include(s => s.Employees)
                        .Include(s => s.Posts)
                        .AsQueryable();
        }

        public override Skill FindById(int id)
        {
            return Collection().Include(s => s.Employees)
                               .Include(s => s.Courses)
                               .Include(s => s.Posts)
                               .SingleOrDefault(s => s.Id == id);
        }

        public override Skill FindByIdTracked(int id)
        {
            return CollectionTracked().Include(s => s.Employees)
                                      .Include(s => s.Courses)
                                      .Include(s => s.Posts)
                                      .SingleOrDefault(s => s.Id == id);
        }

        public override List<Skill> GetAll(int start = 0, int maxByPage = int.MaxValue, Expression<Func<Skill, int?>> keyOrderBy = null, Expression<Func<Skill, bool>> predicateWhere = null)
        {
            IQueryable<Skill> req;
            if (keyOrderBy != null)
                req = Collection().Include(s => s.Employees)
                                  .Include(s => s.Courses)
                                  .Include(s => s.Posts)
                                  .OrderBy(keyOrderBy);
            else
                req = Collection().Include(s => s.Employees)
                                  .Include(s => s.Courses)
                                  .Include(s => s.Posts)
                                  .OrderBy(t => t.Id);

            req = WhereSkipTake(req, start, maxByPage, predicateWhere);

            return req.ToList();
        }

        public override List<Skill> GetAllTracked(int start = 0, int maxByPage = int.MaxValue, Expression<Func<Skill, int?>> keyOrderBy = null, Expression<Func<Skill, bool>> predicateWhere = null)
        {
            IQueryable<Skill> req;
            if (keyOrderBy != null)
                req = CollectionTracked().Include(s => s.Employees)
                                         .Include(s => s.Courses)
                                         .Include(s => s.Posts)
                                         .OrderBy(keyOrderBy);
            else
                req = CollectionTracked().Include(s => s.Employees)
                                         .Include(s => s.Courses)
                                         .Include(s => s.Posts)
                                         .OrderBy(t => t.Id);

            req = WhereSkipTake(req, start, maxByPage, predicateWhere);

            return req.ToList();
        }

        public List<Skill> FindMany(int[] ids)
        {
            List<Skill> lst = new List<Skill>();
            foreach (var skillId in ids)
            {
                lst.Add(FindById(skillId));
            }
            return lst;
        }

        public List<Skill> FindManyTracked(int[] ids)
        {
            List<Skill> lst = new List<Skill>();
            foreach (var skillId in ids)
            {
                lst.Add(FindByIdTracked(skillId));
            }
            return lst;
        }

        public List<Skill> SearchByDescription(string searchField)
        {
            return (from s in Collection().Include(s => s.Courses)
                                          .Include(s => s.Employees)
                                          .Include(s => s.Posts)
                    where s.Description.ToLower().Contains(searchField)
                    select s
                    ).ToList();

        }

        public List<Skill> SearchByDescriptionTracked(string searchField)
        {
            return (from s in CollectionTracked().Include(s => s.Courses)
                                                 .Include(s => s.Employees)
                                                 .Include(s => s.Posts)
                    where s.Description.ToLower().Contains(searchField)
                    select s
                    ).ToList();
        }

        protected override void DetachProperties(Skill t)
        {
            foreach (TrainingCourse tc in t.Courses)
            {
                DataContext.Entry(tc).State = EntityState.Unchanged;
            }
            foreach (Post post in t.Posts)
            {
                DataContext.Entry(post).State = EntityState.Unchanged;
            }
            foreach (Employee employee in t.Employees)
            {
                DataContext.Entry(employee).State = EntityState.Unchanged;
            }
        }

        public override void Add(Skill t)
        {
            base.Add(t);
        }

        public void Save(Skill skill, List<TrainingCourse> courses = null, List<Post> posts = null, List<Employee> employees = null)
        {
            courses = courses ?? new List<TrainingCourse>();
            posts = posts ?? new List<Post>();
            employees = employees ?? new List<Employee>();

            using (MyDbContext newContext = new MyDbContext())
            {
                List<TrainingCourse> newcourses = GetFromNewContext(courses, newContext);
                List<Post> newposts = GetFromNewContext(posts, newContext);
                List<Employee> newemployees = GetFromNewContext(employees, newContext);

                skill.Courses = newcourses;
                skill.Posts = newposts;
                skill.Employees = newemployees;

                newContext.Skills.Add(skill);
                DetachProperties(skill);
                newContext.SaveChanges();
            }
        }

        public override void Modify(Skill t)
        {
            base.Modify(t);
        }

        public void Update(Skill skill, List<TrainingCourse> courses = null, List<Post> posts = null, List<Employee> employees = null)
        {
            courses = courses ?? new List<TrainingCourse>();
            posts = posts ?? new List<Post>();
            employees = employees ?? new List<Employee>();

            using (MyDbContext newContext = new MyDbContext())
            {
                Skill sToChange = newContext.Skills.Include(s => s.Courses)
                                                            .Include(s => s.Employees)
                                                            .Include(s => s.Posts)
                                                            .SingleOrDefault(tc => tc.Id == skill.Id);

                sToChange.Description = skill.Description;

                List<TrainingCourse> newcourses = GetFromNewContext(courses, newContext);
                List<Post> newposts = GetFromNewContext(posts, newContext);
                List<Employee> newemployees = GetFromNewContext(employees, newContext);

                skill.Courses = newcourses;
                skill.Posts = newposts;
                skill.Employees = newemployees;

                newContext.Entry(sToChange).State = EntityState.Modified;

                newContext.SaveChanges();
            }
        }
    }
}