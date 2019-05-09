using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrientedColoring.GraphHelper
{
    public struct Edge
    {
        public readonly int From;
        public readonly int To;
        public readonly double Weight;

        public Edge(int from, int to, double weight = 1)
        {
            From = from;
            To = to;
            Weight = weight;
        }

        public bool Equals(Edge other)
        {
            return From == other.From && To == other.To && Weight.Equals(other.Weight);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Edge && Equals((Edge) obj);
        }


        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = From;
                hashCode = (hashCode * 397) ^ To;
                hashCode = (hashCode * 397) ^ Weight.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Edge({Weight}): {From} -> {To} ";
        }

        public static bool operator ==(Edge e1, Edge e2)
        {
            return Equals(e1, e2);
        }

        public static bool operator !=(Edge e1, Edge e2) => !(e1 == e2);
    }
}
