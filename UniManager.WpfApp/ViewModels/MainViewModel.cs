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
            var teachers = repo.GetAllAsync().Result.OrderBy(t => t.LastName);
            return new(teachers);
        }

        public ObservableCollection<Course> Courses
        {
            get
            {
                var repo = new CourseRepository();
                var courses = repo.GetAllAsync().Result;
                return new(courses);
            }
        }

        //ausgewählte Felder
        private Teacher? selectedTeacher = null;

        public Teacher? SelectedTeacher
        {
            get { return selectedTeacher; }
            set { selectedTeacher = value; }
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
        public ICommand DeleteTeacherDommand => RelayCommand.CreateCommand(ref deleteTeacherCommand, DeleteTeacher, (p) => SelectedTeacher != null);


        //methods
        private void DeleteTeacher(object? obj)
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
