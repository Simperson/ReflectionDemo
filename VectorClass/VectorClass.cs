using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsNewAttributes;

[assembly: SupportWhatsNew]
namespace VectorClass
{
    [LastModified("14 Feb 2010", "IEnumerable interface implemented" +
        " So Vector can now be treated as a collection")]
    [LastModified("10 Feb 2010", "IFormattable interface implement " +
        "So Vector now responds to format specifiers N and VE")]
    public class Vector : IFormattable, IEnumerable
    {
        public double x, y, z;
        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                return ToString();
            }
            string formatUpper = format.ToUpper();
            switch (formatUpper)
            {
                case "N":
                    return "||" + Norm().ToString() + "||";
                case "VE":
                    return string.Format("({0:E},{1:E},{2:E})", x, y, z);
                case "IJK":
                    StringBuilder sb = new StringBuilder(x.ToString(), 30);
                    sb.Append(" i+");
                    sb.Append(y.ToString());
                    sb.Append(" j+");
                    sb.Append(z.ToString());
                    sb.Append(" k");
                    return sb.ToString();
                default:
                    return ToString();
            }
        }

        public Vector(Vector rhs)
        {
            x = rhs.x;
            y = rhs.y;
            z = rhs.z;
        }
        private double Norm()
        {
            return x * y + y * y + z * z;
        }

        [LastModified("14 Feb 2010", "Method added in order to provide collection support")]
        public IEnumerator GetEnumerator()
        {
            return new VectorEnumerator(this);
        }

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }
        public double this[uint i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException(
                            "Attempt to retrieve Vector element" + i);
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException(
                            "Attempt to set Vector element" + i);
                }
            }
        }

        public static bool operator ==(Vector lhs, Vector rhs)
        {
            if (Math.Abs(lhs.x - rhs.x) < double.Epsilon &&
                Math.Abs(lhs.y - rhs.y) < double.Epsilon &&
                Math.Abs(lhs.z - rhs.z) < double.Epsilon)
                return true;
            else
                return false;
        }
        public static Vector operator +(Vector lhs, Vector rhs)
        {
            Vector result = new Vector(lhs);
            result.x += rhs.x;
            result.y += rhs.y;
            result.z += rhs.z;
            return result;
        }
        public static Vector operator *(double lhs, Vector rhs)
        {
            return new Vector(lhs * rhs.x, lhs * rhs.y, lhs * rhs.z);
        }
        public static Vector operator *(Vector lhs,double rhs)
        {
            return rhs * lhs;
        }
        public static double operator*(Vector lhs,Vector rhs)
        {
            return lhs.x * rhs.x + lhs.y * rhs.y + lhs.z + rhs.z;
        }
        #region enumerator class
        [LastModified("14 Feb 2010", "Class created as part of collection support for Vector")]
        private class VectorEnumerator : IEnumerator
        {
            readonly Vector _theVector;
            int _location;
            public VectorEnumerator(Vector theVector)
            {
                _theVector = theVector;
                _location = -1;
            }
            public object Current
            {
                get
                {
                    if (_location < 0 || _location > 2)
                        throw new InvalidOperationException(
                            "The enumerator is either before the first element or" +
                            "after the last element of the Vector");
                    return _theVector[(uint)_location];
                }
            }

            public bool MoveNext()
            {
                ++_location;
                return (_location > 2) ? false : true;
            }

            public void Reset()
            {
                _location = -1;
            }
        }
        #endregion
    }
}
