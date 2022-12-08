using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Logic.Models
{
    public class Course : ModelObject, ICloneable
    {
        public string Designation { get; set; } = string.Empty;

        public int TeacherId { get; set; }

        public object Clone()
        {
            return new Course()
            {
                Id = Id,
                Designation = Designation,
                TeacherId = TeacherId
            };
        }

        public override string ToString()
        {
            return $"{Id}; {Designation}; {TeacherId}";
        }
    }
}
