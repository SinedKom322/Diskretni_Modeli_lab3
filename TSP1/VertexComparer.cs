using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP1
{
    class VertexComparer : IComparer<Vertex>
    {
        public int Compare(Vertex x, Vertex y)
        {
            var result = x.LowerBound.CompareTo(y.LowerBound);
            if (result == 0)
                return 1;
            else
                return result;
        }
    }
}
