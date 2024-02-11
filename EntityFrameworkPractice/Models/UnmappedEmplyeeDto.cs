using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkPractice.Models
{
    public class UnmappedEmplyeeDto
    {
        public int EmployeeId { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }
    }
}
