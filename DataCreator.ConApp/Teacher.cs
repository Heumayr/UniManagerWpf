using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCreator.ConApp
{
    public class Teacher : Identity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string? Title { get; set; }

        public override string ToString()
        {
            return $"{Id}; {FirstName}; {LastName}; {Title}";
        }

    }
}
