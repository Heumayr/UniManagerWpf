using Repository.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Logic.Repos
{
    public sealed class CourseRepository : Repository<Course>
    {
        public CourseRepository()
        {

        }

        public override Course Create()
        {
            return new Course();
        }
    }
}
