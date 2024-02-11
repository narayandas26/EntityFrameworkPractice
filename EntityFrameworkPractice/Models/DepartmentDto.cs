using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkPractice.Models
{
    public class DepartmentDto
    {
        public int DeptId { get; set; }

        public string DeptName { get; set; } = null!;

        public virtual IEnumerable<UnmappedEmplyeeDto> Employees { get; set; } = new List<UnmappedEmplyeeDto>();
    }
}
