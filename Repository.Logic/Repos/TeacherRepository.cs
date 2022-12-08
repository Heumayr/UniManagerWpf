using Repository.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Logic.Repos
{
    public sealed class TeacherRepository : Repository<Models.Teacher>
    {
        public TeacherRepository()
        {
        }

        public override Teacher Create()
        {
            return new Teacher();
        }
    }
}
