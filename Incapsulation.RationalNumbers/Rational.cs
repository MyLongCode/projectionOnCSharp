using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator; //up 
        public int Denominator; //down 
        public bool IsNan = false;

        public Rational(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                this.Numerator = numerator;
                this.Denominator = denominator;
                IsNan = true;
            }
            else if (numerator == 0)
            {
                this.Numerator = 0;
                this.Denominator = 1;
            }
            else
            {
                this.Numerator = numerator / Calculate_GCD(numerator, denominator);
                this.Denominator = denominator / Calculate_GCD(numerator, denominator);
                if (this.Denominator < 0)
                {
                    this.Denominator = -this.Denominator;
                    this.Numerator = -this.Numerator;
                }
            }
        }

        public Rational(int dividend)
        {
            this.Numerator = dividend;
            this.Denominator = 1;
        }


        int Calculate_GCD(int a, int b)
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }


        public static Rational operator +(Rational r1, Rational r2)
        {
            return new Rational(r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator,
                r2.Denominator * r1.Denominator);
        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            return new Rational((r1.Numerator * r2.Denominator) - (r2.Numerator * r1.Denominator),
                r2.Denominator * r1.Denominator);
        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            return new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator);
        }
        public static Rational operator /(Rational r1, Rational r2)
        {
            if (r2.IsNan) return new Rational(1, 0);
            return new Rational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator);
        }

        public static implicit operator double(Rational r1)
        {
            if (r1.Denominator == 0) return double.NaN;
            return (double)r1.Numerator / r1.Denominator;
        }

        public static implicit operator int(Rational r1)
        {
            if (r1.Numerator % r1.Denominator != 0 && r1.Numerator != 0) throw new Exception();
            return (int)r1.Numerator / r1.Denominator;
        }

        public static implicit operator Rational(int v)
        {
            return new Rational(v);
        }
    }
}
