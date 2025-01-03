using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MatrixDeterminant
{
    internal class Program
    {

        //Считаем определитель матрицы 2х2
        static double matrix_det_2x2(double[,] matrix)
        {
            double det = ((matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]));
            return det;
        }



        //Генерируем разложенную матрицу
        static double[,] matrix_gen(double[,] mat, int n, int k)
        {
            double[,] mat_1 = new double[n - 1, n - 1];

            for (int i = 1; i < n; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    if (j < k)
                    {
                        mat_1[i - 1, j] = mat[i, j];
                    }
                    else if (j >= k)
                    {
                        mat_1[i - 1, j] = mat[i, j + 1];
                    }
                }
            }
            return mat_1;
        }


        //Раскладывает и считает определитель
        static double det_gen(double[,] matrix, int n)
        {
            double det = 0;
            for (int i = 0; i < n; i++)
            {
                //Повторно раскладываем матрицу
                if (n != 2)
                {
                    double[,] mat_1 = matrix_gen(matrix, n, i);
                    int n_1 = mat_1.GetLength(0);
                    det += Math.Pow(-1, i) * matrix[0, i] * det_gen(mat_1, n_1);

                }

                //Когда дошли до размера 2х2, то считаем мини определитель
                else if (n == 2)
                {
                    det = matrix_det_2x2(matrix);
                    break;
                }
            }
            //matrix_print(matrix, n, Space(matrix, n), det);

            return det; //Выводим итоговый определитель
        }



        //Делает строку пробелов
        static string space_gen(int n)
        {
            string s = "";
            for (int i = 0; i < n; i++)
            {
                s += " ";
            }
            return s;
        }



        //Сколько нужно места для самого большого элемента
        static int Space(double[,] mat, int n)
        {
            int max_space = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    //Узнаём длину элемента и сравниваем
                    double mat_element_0 = mat[i, j];
                    string mat_element_1 = mat_element_0.ToString();
                    int spase = mat_element_1.Length;
                    if (spase > max_space)
                    {
                        max_space = spase;
                    }
                }
            }
            return max_space + 2;
        }



        //выводим матрицу с АФИГЕННЫМ форматированием
        static void matrix_print(double[,] matrix, int n, int space, double det)
        {
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                Console.Write("|");
                for (int j = 0; j < n; j++)
                {
                    //Длина текущего элемента
                    int mat_el = matrix[i, j].ToString().Length;

                    //Если посередине поставить не получится
                    if ((mat_el % 2) != (space % 2))
                    {
                        // то сдвигаемся чуть вправо
                        string space_need_left = space_gen((space - mat_el + 1) / 2);
                        string space_need_right = space_gen((space - mat_el - 1) / 2);
                        Console.Write($"{space_need_left}{matrix[i, j]}{space_need_right}");
                    }
                    //Ставим элемент посередине
                    else if ((mat_el % 2) == (space % 2))
                    {
                        string space_need = space_gen((space - mat_el) / 2);
                        Console.Write($"{space_need}{matrix[i, j]}{space_need}");
                    }
                }
                Console.Write("|");

                //Красиво ставим "= det" 
                //Тут посередине 
                if ((n % 2) != 0 && i == ((n - 1) / 2))
                {
                    Console.Write($" = {det,2}");
                }
                //Тут чуть ниже
                else if (i == (n / 2))
                {
                    Console.Write($" = {det,2}");
                }

                Console.WriteLine();
            }
        }



        //Запрашивем и дозаполняем начальную матрицу
        static void matrix_finish(double[,] matrix, int n)
        {
            for (int i = 1; i < n; i++)
            {
                string a = Console.ReadLine();
                var b = a.Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(b[j]);
                }
            }
        }



        static void Main(string[] args)
        {
            string str_0 = Console.ReadLine(); //Берём первую строку
            var str_1 = str_0.Split(' ');
            int n = str_1.Length;
            double[,] matrix = new double[n, n]; //Создаём матрицу размерности первой строки

            //Заносим в матрицу первую строку
            for (int i = 0; i < n; i++)
            {
                matrix[0, i] = int.Parse(str_1[i]);
            }
            matrix_finish(matrix, n); //Запрашиваем и заносим всё оставшиеся



            double det = det_gen(matrix, n); //Основная функция которая находит определитель


            //Делаем отступ
            Console.WriteLine();


            //выводим матрицу с форматированием
            matrix_print(matrix, n, Space(matrix, n), det);



            Console.ReadKey();
        }
    }
}
