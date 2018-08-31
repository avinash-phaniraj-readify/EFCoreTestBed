using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq2SqlEFCoreBehaviorsTest.Linq2Sql
{
    public partial class Employee
    {
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public partial class EmployeeDevice
    {
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
