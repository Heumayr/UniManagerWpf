using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Logic.Models
{
    public class Teacher : ModelObject, ICloneable
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? Title { get; set; }

        public object Clone()
        {
            return new Teacher
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Title = Title
            };
        }

        public override string ToString()
        {
            return $"{Id}; {FirstName}; {LastName}; {Title}";
        }

    }
}
