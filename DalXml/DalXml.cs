using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public sealed class DalXml : IDal
    {
        public ITask Task =>  new TaskImplementation();

        public IDependency Dependency => new DependencyImplementation();

        public IEngineer Engineer => throw new NotImplementedException();
    }
}
