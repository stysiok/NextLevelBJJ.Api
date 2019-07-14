using NextLevelBJJ.DataService.Models;
using NextLevelBJJ.DataServices.Abstraction;
using NextLevelBJJ.WebContentServices.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextLevelBJJ.DataServices
{
    public class AttendancesService : IAttendancesService
    {
        private NextLevelContext _db { get; set; }
        private IClassesService _classesService;

        public AttendancesService(NextLevelContext db, IClassesService classesService)
        {
            try
            {
                if (db.ChangeTracker != null)
                    db.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            }
            catch (Exception)
            {
            }

            _db = db;
            _classesService = classesService;
        }

        public Task<List<Attendance>> GetStudentAttendences(int studentId, int skip, int take)
        {
            try
            {
                return Task.FromResult(_db.Attendances
                .Where(a => a.StudentId == studentId && a.IsEntityAccesible)
                .OrderByDescending(a => a.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania uczestnictwa klubowicza w zajęciach. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<int> GetAttendancesAmountTrackedOnPass(int passId)
        {
            try
            {
                return Task.FromResult(_db.Attendances.Count(a => a.PassId == passId && a.IsEntityAccesible));
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania ilości odbytych treningów na danym karnecie. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<Attendance> GetRecentAttendance(int studentId)
        {
            try
            {
                return Task.FromResult(_db.Attendances
                 .Where(a => a.StudentId == studentId && a.IsEntityAccesible)
                 .OrderByDescending(a => a.CreatedDate)
                 .FirstOrDefault());
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania ostaniego treningu klubowicza. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<int> AddAttendance(int studentId, int passId)
        {
            try
            {
                return Task.Factory.StartNew(() =>
                {
                    var date = DateTime.UtcNow;
                    var attendance = new Attendance
                    {
                        StudentId = studentId,
                        PassId = passId,
                        CreatedDate = date,
                        ModifiedDate = date,
                        CreatedBy = 0,
                        ModifiedBy = 0,
                        IsDeleted = false,
                        IsEnabled = true,
                        IsFree = false,                        
                    };
                    _db.Attendances.Add(attendance);
                    return _db.SaveChanges();
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas dodawania obecności na treningu. Dodatkowa informacja: " + ex.Message);
            }
        }

        public Task<bool> CurrentClassAlreadyAttended(int studentId, bool isKidsPass)
        {
            try
            {
                return Task.Factory.StartNew(() =>
                {
                    var lastAttendance = GetRecentAttendance(studentId).Result;

                    if (lastAttendance == null)
                    {
                        return true;
                    }

                    var now = DateTime.Now;
                    var lastAttendedClass = _classesService.GetClass(lastAttendance.CreatedDate, isKidsPass);
                    var currentClass = _classesService.GetClass(now, isKidsPass);

                    return now.Date == lastAttendance.CreatedDate.Date && currentClass.Name == lastAttendedClass.Name;
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas sprawdzania, czy student odbył aktualne zajęcia. Dodatkowa informacja: " + ex.Message);
            }
        }
    }
}
