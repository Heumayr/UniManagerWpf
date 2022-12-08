using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCreator.ConApp
{
    public class Course : Identity
    {
        public string Designation { get; set; } = string.Empty;

        public int TeacherId { get; set; }

        public override string ToString()
        {
            return $"{Id}; {Designation}; {TeacherId}";
        }
    }
}
