using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp_all.Fraction
{
    internal struct Fraction
    {
        public double Numerator { get; set; }
        public double Denominator { get; set; }

        public Fraction(double numerator, double denominator)
        {
            Numerator = numerator;
            if (denominator == 0)
            {
                throw new DivideByZeroException("Denominator can not be 0");
            }
            Denominator = denominator;
        }

        public override string? ToString()
        {
            return $"{Numerator}/{Denominator}";
        }

        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new((a.Numerator * b.Denominator + b.Numerator * a.Denominator), a.Denominator * b.Denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new((a.Numerator * b.Denominator - a.Denominator * b.Numerator), a.Denominator * b.Denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            return new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }
    }
}
