using Repository.Logic.Models;
using Repository.Logic.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UniManager.WpfApp.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        private Course course = new();


        public Teacher Teacher //for label binding
        { 
            get
            {
                var repo = new TeacherRepository();
                var teacher = repo.GetByIdAsync(Course.TeacherId).Result;
                return teacher ?? new();
            }
        }

        public Course Course
        {
            get { return course; }
            set
            {
                course = value;
                OnPropertyChanged(nameof(Course));
                OnPropertyChanged(nameof(Teacher));
            }
        }

        private ICommand? saveCommand = null;
        private ICommand? closeCommand = null;
        public Action? CloseAction { get; set; } = null;

        public ICommand SaveCommand => RelayCommand.CreateCommand(ref saveCommand, (p) => Save());
        public ICommand CloseCommand => RelayCommand.CreateCommand(ref closeCommand, (p) => Close());

        private void Close()
        {
            CloseAction?.Invoke();
        }

        private void Save()
        {
            var repo = new CourseRepository();

            //wenn Model Id hat --> Edit
            if (Course.Id == 0)
            {
                repo.AddAsync(Course).Wait();
            }
            else
            {
                repo.UpdateAsync(Course).Wait();
            }
            repo.SaveChangesAsync().Wait();
            Close();
        }
    }
}
