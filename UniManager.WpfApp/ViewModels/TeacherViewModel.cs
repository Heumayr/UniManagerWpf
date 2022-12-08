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
    public class TeacherViewModel : BaseViewModel
    {
        private Teacher teacher = new();

        public Teacher Teacher
        {
            get { return teacher; }
            set
            {
                teacher = value;
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
            var repo = new TeacherRepository();
            
            //wenn Model Id hat --> Edit
            if (Teacher.Id == 0)
            {
                repo.AddAsync(Teacher).Wait();   
            }
            else
            {
                repo.UpdateAsync(Teacher).Wait();
            }
            repo.SaveChangesAsync().Wait();
            Close();
        }
    }
}
