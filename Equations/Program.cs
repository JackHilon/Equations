using System;
using System.Collections.Generic;

namespace Equations
{
    class Program
    {
        static void Main(string[] args)
        {
            // Equations
            // https://open.kattis.com/problems/equations 
            // common solution of two linear equation of two variables
            // -- I think my solution is right but Kattis gives me (ERROR: Wrong Answer)
            // -------------------------------------------------------------------------

            //var myStr = "2x + 3y - -x = 4";
            //var myEq = myStr.Split(' ');
            //var nums = ToRightForm(myEq);
            //PrintArray(nums);

            var answers = new List<string>();
            try
            {
                var counter = EnterYourCounter();
                answers = ProgramCore(counter);
            }
            finally
            {
                PrintList(answers);
            }
        }
        private static List<string> ProgramCore(int counter)
        {
            var answers = new List<string>();

            string[] myEq1, myEq2;
            int[] eq1, eq2;
            string[] mySol;
            for (int i = 0; i < counter; i++)
            {
                myEq1 = EnterEquation();
                myEq2 = EnterEquation();

                eq1 = ToRightForm(myEq1);
                eq2 = ToRightForm(myEq2);

                mySol = Solution(eq1, eq2);
                answers.Add(mySol[0]);
                answers.Add(mySol[1]);
                answers.Add(" ");
                //PrintArray(mySol);
            } // -- end for --

            return answers;
        }


        // ==========================================================\/===========================
        private static string[] Solution(int[] a, int[] b)
        {
            int det = 0;
            double inv = 0;
            double first = 0, second = 0;
            string[] result = new string[2] {"","" };

            try
            {
                //                 a     b     c     d
                det = Determinant2(a[0], a[1], b[0], b[1]);
                
                if (SolutionConditions(det, a[2], b[2]) == 2)
                    throw new ArgumentException();

                else if (SolutionConditions(det, a[2], b[2]) == 3)
                    throw new DivideByZeroException();

                else
                    throw new FormatException();
            }
            catch(ArgumentException ex1)
            {
                result[0] = "don't know";
                result[1] = "don't know";
                return result;
            }
            catch (DivideByZeroException ex2)
            {
                //result[0] = "don't know";
                //result[1] = "0";
                result[0] = "don't know";
                result[1] = "don't know";
                return result;
            }
            catch(FormatException ex3)
            {
            }
            //inv = (double)1 / det;

            //first = (double)inv * (b[1] * a[2] - a[1] * b[2]);
            //second = (double)inv * (-b[0] * a[2] + a[0] * b[2]);
            int fis = b[1] * a[2] - a[1] * b[2];
            int sec = -b[0] * a[2] + a[0] * b[2];
            //result[0] = first.ToString();
            //result[1] = second.ToString();
            result[0] = Fraction(fis, det);
            result[1] = Fraction(sec, det);
            return result;
        }
        // ==========================================================/\===========================
        private static int Determinant2(int a, int b, int c, int d)
        {
            int res = a * d - b * c;
            return res;
        }
        private static int[] ToRightForm(string[] arr)
        {
            var xxx = new List<int>();
            var yyy = new List<int>();
            var ccc = new List<int>();

            var opIndex = GetOperandIndex(arr);

            string item;
            for (int i = 0; i < arr.Length; i++)
            {
                item = arr[i];
                if (TypeOfForm(item) == "x" && i < opIndex)
                    xxx.Add(int.Parse(WithoutLastItem(item)));
                else if (TypeOfForm(item) == "x" && i > opIndex)
                    xxx.Add(-1 * int.Parse(WithoutLastItem(item)));
                // -- Y --
                else if (TypeOfForm(item) == "y" && i < opIndex)
                    yyy.Add(int.Parse(WithoutLastItem(item)));
                else if (TypeOfForm(item) == "y" && i > opIndex)
                    yyy.Add(-1 * int.Parse(WithoutLastItem(item)));
                // -- const --
                else if (TypeOfForm(item) == "Const" && i > opIndex)
                    ccc.Add(int.Parse(item));
                else if (TypeOfForm(item) == "Const" && i < opIndex)
                    ccc.Add(-1 * int.Parse(item));
                // -- operand --
                else if (TypeOfForm(item) == "Plus")
                    continue;
                else if (TypeOfForm(item) == "Minus")
                    arr[i + 1] = MultByMinusOne(arr[i + 1]);
                // -- else not identified item --
                else
                    continue;
            }
            int[] resultEquation = new int[3];
            resultEquation[0] = SumList(xxx);
            resultEquation[1] = SumList(yyy);
            resultEquation[2] = SumList(ccc);
            return resultEquation;
        }
        
        private static string TypeOfForm(string str)
        {
            if (str == "+")
                return "Plus";
            else if (str == "-")
                return "Minus";
            else if (IsNumber(str) == true)
                return "Const";
            else if (ContainsXXX(str) == true)
                return "x";
            else if (ContainsYYY(str) == true)
                return "y";
            else
                return "ERROR";
        }
        private static bool ContainsYYY(string str)
        {
            if (str[str.Length - 1] == 'y')
                return true;
            else
                return false;
        }
        private static bool ContainsXXX(string str)
        {
            if (str[str.Length - 1] == 'x')
                return true;
            else
                return false;
        }
        private static int GetOperandIndex(string[] arr)
        {
            string equalSign = "=";
            
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == equalSign)
                    return i;
            }
            return arr.Length;
        }

        private static int GetIndex(string[] arr, string obj)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] == obj)
                    return i;
            }
            return arr.Length;
        }
        private static string[] EnterEquation()
        {
            //string[] arr = Console.ReadLine().Split(' ');
            //return arr;
            string str = "";
            string[] arr;
            try
            {
                str = Console.ReadLine();
                arr = str.Split(' ');
                if (EqCond(arr) == false)
                    throw new IndexOutOfRangeException();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return EnterEquation();
            }
            return arr;
        }
        private static bool EqCond(string[] arr)
        {
            if (arr.Length < 3)
                return false;
            for (int i = 1; i < arr.Length - 1; i++)
            {
                //if (arr[i] == "=" && i != 0 && i != arr.Length - 1)
                if (arr[i] == "=")
                    return true;
            }
            return false;
        }
        private static bool IsNumber(string str)
        {
            int a = 0;
            try
            {
                a = int.Parse(str);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        private static string WithoutLastItem(string str)
        {
            // 5x   x  -x
            string sum = "";

            if (str.Length > 2)
            {
                for (int i = 0; i < str.Length - 1; i++)
                {
                    sum = sum + char.ToString(str[i]);
                }
                return sum;
            }
            else if (str.Length == 2 && IsNumber(str[0].ToString()) == true)
                return str[0].ToString();
            else if (str.Length == 2 && IsNumber(str[0].ToString()) == false)
                return "-1";
            else if (str.Length == 1)
                return "1";
            else
                return "ERROR";
        }
        private static string MultByMinusOne(string str)
        {
            int res;
            if (TypeOfForm(str) == "Const")
            {
                res = -1 * int.Parse(str);
                return res.ToString();
            }
            else if (TypeOfForm(str) == "x")
            {
                res = -1 * int.Parse(WithoutLastItem(str));
                return res.ToString() + "x";
            }
            else if (TypeOfForm(str) == "y")
            {
                res = -1 * int.Parse(WithoutLastItem(str));
                return res.ToString() + "y";
            }
            else
                return "ERROR";
        }
        private static int SumList(List<int> list)
        {
            int sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sum = sum + list[i];
            }
            return sum;
        }
        private static void PrintArray(string[] a)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Console.WriteLine(a[i]);
            }
        }
        private static int SolutionConditions(int det, int e, int f )
        {
            if (det != 0)
                return 1;
            else if (det == 0 && e == 0 && f == 0)
                return 2;
            else if (det == 0 && e != 0 && f == 0)
                return 3;
            else if (det == 0 && e == 0 && f != 0)
                return 3;
            else if (det == 0 && e != 0 && f != 0)
                return 3;
            else
                return 4;
        }
        private static int EnterYourCounter()
        {
            int a = 0;
            try
            {
                a = int.Parse(Console.ReadLine());
                if (a < 1 || a > 100)
                    throw new ArgumentException();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return EnterYourCounter();
            }
            return a;
        }
        private static void PrintList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
        }
        private static string Fraction(int num, int den)
        {
            if (num * den < 0 && num % den == 0)
                return $"-{Math.Abs(num) / Math.Abs(den)}";
            else if (num * den < 0 && num % den != 0)
                return $"-{Math.Abs(num)}/{Math.Abs(den)}";
            else if (num * den >= 0 && num % den == 0)
                return $"{Math.Abs(num) / Math.Abs(den)}";
            else if (num * den >= 0 && num % den != 0)
                return $"{Math.Abs(num)}/{Math.Abs(den)}";
            else
                return "ERROR";
        }
    }
}
