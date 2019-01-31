using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SFLOnline.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace SFLOnline.DAL
{
    public class SchoolContext : DbContext
    {

        public SchoolContext() : base("SchoolContext")
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<EnrollmentStudent> EnrollmentsStudent { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<TrackCourses> trackCourses { get; set; }
        public DbSet<Grade> grades { get; set; }
        public DbSet<GradePercentage> gradePercentages { get; set; }
        public DbSet<Instructor> ınstructors { get; set; }
        public DbSet<EnrollmentInstructor> enrollmentInstructors { get; set; }
        public DbSet<Module> modules { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<StudentAttendance> studentAttendances { get; set; }
        public DbSet<Slot> slots { get; set; }
        public DbSet<Day> days { get; set; }
        public DbSet<ClassSchedule> classSchedules { get; set; }
        public DbSet<InformationPassed> ınformationPasseds { get; set; }
        public DbSet<Announcement> announcements { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);

        }

        public System.Data.Entity.DbSet<SFLOnline.Models.Exit> Exits { get; set; }

        public System.Data.Entity.DbSet<SFLOnline.Models.ExitTrack> ExitTracks { get; set; }

        public System.Data.Entity.DbSet<SFLOnline.Models.StudentExitGrade> StudentExitGrades { get; set; }
    }
}