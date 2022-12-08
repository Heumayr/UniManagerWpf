using Repository.Logic.Models;
using Repository.Logic.Repos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace UniManager.WpfApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<Teacher> Teachers => GetTeachers();

        private ObservableCollection<Teacher> GetTeachers()
        {
            var repo = new TeacherRepository();
            var teachers = string.IsNullOrWhiteSpace(TeacherSearchText) ?
                repo.GetAllAsync().Result.OrderBy(t => t.LastName) :
                repo.GetAllAsync().Result.Where(t => t.FullName.ToUpper().Contains(TeacherSearchText.ToUpper())).OrderBy(t => t.LastName);
            return new(teachers);
        }

        public ObservableCollection<Course> Courses
        {
            get
            {
                if (SelectedTeacher == null)
                    return new();

                var repo = new CourseRepository();

                //var courses = SelectedTeacher != null ? repo.GetAllAsync().Result.Where(c => c.TeacherId == SelectedTeacher.Id) : new ObservableCollection<Course>();

                var courses = string.IsNullOrWhiteSpace(CourseSearchText) ?
                    repo.GetAllAsync().Result.Where(c => c.TeacherId == SelectedTeacher.Id).OrderBy(c => c.Designation) :
                    repo.GetAllAsync().Result.Where(c => c.TeacherId == SelectedTeacher.Id && c.Designation.ToUpper().Contains(CourseSearchText.ToUpper())).OrderBy(c => c.Designation);

                return new(courses);
            }
        }

        //ausgewählte Felder
        private Teacher? selectedTeacher = null;

        public Teacher? SelectedTeacher
        {
            get { return selectedTeacher; }
            set
            {
                selectedTeacher = value;
                SelectedCourse = null;
                OnPropertyChanged(nameof(Courses));
            }
        }

        //searchFields
        private string? teacherSearchText;
        private string? courseSearchText;

        public string? CourseSearchText
        {
            get { return courseSearchText; }
            set
            {
                courseSearchText = value;
                OnPropertyChanged(nameof(Courses));
            }
        }


        public string? TeacherSearchText
        {
            get { return teacherSearchText; }
            set
            {
                teacherSearchText = value;
                OnPropertyChanged(nameof(Teachers));
            }
        }


        private Course? selectedCourse;

        public Course? SelectedCourse
        {
            get { return selectedCourse; }
            set { selectedCourse = value; }
        }

        //commands
        private ICommand? addTeacherCommand = null;
        private ICommand? editTeacherCommand = null;
        private ICommand? deleteTeacherCommand = null;

        private ICommand? addCourseCommand = null;
        private ICommand? editCourseCommand = null;
        private ICommand? deleteCourseCommand = null;

        public ICommand AddTeacherCommand => RelayCommand.CreateCommand(ref addTeacherCommand, (p) => AddTeacher());
        public ICommand EditTeacherCommand => RelayCommand.CreateCommand(ref editTeacherCommand, (p) => EditTeacher(), (p) => SelectedTeacher != null);
        public ICommand DeleteTeacherDommand => RelayCommand.CreateCommand(ref deleteTeacherCommand, (p) => DeleteTeacher(), (p) => SelectedTeacher != null);

        public ICommand AddCourseCommand => RelayCommand.CreateCommand(ref addCourseCommand, (p) => AddCourse(), (p) => SelectedTeacher != null);
        public ICommand EditCourseCommand => RelayCommand.CreateCommand(ref editCourseCommand, (p) => EditCourse(), (p) => (SelectedTeacher != null && SelectedCourse != null));
        public ICommand DeleteCourseCommand => RelayCommand.CreateCommand(ref deleteCourseCommand, (p) => DeleteCourse(), (p) => (SelectedCourse != null && SelectedCourse != null));


        //methods
        private void AddCourse()
        {
            if (SelectedTeacher == null)
                return;

            var courseWindow = new CourseWindow();

            if (courseWindow.ViewModel is CourseViewModel cvm)
            {
                cvm.Course = new Course() { TeacherId = SelectedTeacher.Id };
                courseWindow.ShowDialog();
                OnPropertyChanged(nameof(Courses));
            }
        }

        private void EditCourse()
        {
            if (SelectedTeacher == null || SelectedCourse == null)
                return;

            var courseWindow = new CourseWindow();

            if (courseWindow.ViewModel is CourseViewModel cvm)
            {
                cvm.Course = SelectedCourse;
                courseWindow.ShowDialog();
                OnPropertyChanged(nameof(Courses));
            }
        }

        private void DeleteCourse()
        {
            if (SelectedTeacher == null || SelectedCourse == null)
                return;

            var response = MessageBox.Show($"Do you want to delete {SelectedCourse.Designation}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Stop);

            if (response == MessageBoxResult.Yes)
            {
                var courseRep = new CourseRepository();
                courseRep.DeleteAsync(SelectedCourse.Id).Wait();
                courseRep.SaveChangesAsync().Wait();

                SelectedCourse = null;
                OnPropertyChanged(nameof(Courses));
            }
        }

        private void DeleteTeacher()
        {
            if (SelectedTeacher == null)
                return;

            var courseRepo = new CourseRepository();
            var hasCourses = courseRepo.GetAllAsync().Result.Any(c => c.TeacherId == SelectedTeacher.Id);
            if (hasCourses)
            {
                MessageBox.Show($"Teacher {SelectedTeacher.FullName} still has courses assigned and can't be deleted", "Warning!!!!!!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var response = MessageBox.Show($"Do you want to delete {SelectedTeacher!.FullName}", "Warning!!!", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (response == MessageBoxResult.Yes)
            {
                var repo = new TeacherRepository();

                repo.DeleteAsync(SelectedTeacher.Id).Wait();
                repo.SaveChangesAsync().Wait();
                SelectedTeacher = null;
                OnPropertyChanged(nameof(Teachers));
            }

        }

        private void EditTeacher()
        {
            if (SelectedTeacher != null)
            {
                var teacherWindow = new TeacherWindow();
                if (teacherWindow.ViewModel is TeacherViewModel tvm)
                {
                    tvm.Teacher = SelectedTeacher;
                    teacherWindow.ShowDialog();
                    OnPropertyChanged(nameof(Teachers));
                }
            }
        }

        private void AddTeacher()
        {
            var teacherWindow = new TeacherWindow();
            teacherWindow.ShowDialog();
            OnPropertyChanged(nameof(Teachers));
        }

    }
}
