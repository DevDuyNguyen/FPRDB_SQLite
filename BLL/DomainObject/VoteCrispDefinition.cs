using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class VoteCrispDefinition<T>
    {
        public List<T> subSet;
        public float mass;

        public VoteCrispDefinition(List<T> subSet, float mass)
        {
            this.subSet = subSet;
            this.mass = mass;
        }
    }
}
