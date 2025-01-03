namespace MatrixDeterminant
{
    internal class Program
    {

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


        static double CalculateDeterminant(double[,] matrix, int n)
        {
            double det = 0;
            for (int i = 0; i < n; i++)
            {
                if (n != 2)
                {
                    double[,] mat_1 = matrix_gen(matrix, n, i);
                    int n_1 = mat_1.GetLength(0);
                    det += Math.Pow(-1, i) * matrix[0, i] * CalculateDeterminant(mat_1, n_1);

                }

                else if (n == 2)
                {
                    det = (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]);
                    break;
                }
            }

            return det;
        }


        static string GetEmptyString(int length)
        {
            return new string(' ', length);
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
                        string space_need_left = GetEmptyString((space - mat_el + 1) / 2);
                        string space_need_right = GetEmptyString((space - mat_el - 1) / 2);
                        Console.Write($"{space_need_left}{matrix[i, j]}{space_need_right}");
                    }
                    //Ставим элемент посередине
                    else if ((mat_el % 2) == (space % 2))
                    {
                        string space_need = GetEmptyString((space - mat_el) / 2);
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



        static protected void FillMatrixFromConsole(double[,] matrix)
        {
            int size = matrix.GetLength(0);

            for (int row = 0; row < size; row++)
            {
                string[] userInput;
                while (true)
                {
                    Console.WriteLine($"Введите строку {row + 1} из {size}");
                    userInput = (Console.ReadLine() ?? "0").Split(' ');

                    if (userInput.Length != size)
                    {
                        Console.WriteLine($"Вы ввели {userInput.Length} значений, требуется {size}.");
                        continue;
                    }

                    for (int column = 0; column < size; column++)
                    {
                        int result;
                        while (!int.TryParse(userInput[column], out result))
                        {
                            Console.WriteLine(
                                $"Значение {userInput[column]} в колонке {column + 1} не является целым числом.\n" +
                                "Повторите ввод этого числа:"
                            );
                            userInput[column] = Console.ReadLine() ?? "";
                        }
                        matrix[row, column] = result;
                    }

                    break;
                }

            }
        }

        static double[,] GetMatrixFromConsole()
        {
            int size = 0;
            while (size < 1)
            {
                Console.WriteLine("Задайте размерность матрицы: ");
                // NOTE: А тут подстелили себе всю солому в сарае
                size = int.TryParse(Console.ReadLine(), out int result) ? result : -1;
                // условие ? если тру : если фолс; 

                if (size < 1)
                {
                    Console.WriteLine("Размер матрицы должен быть целым числом больше 0");
                }
            }

            double[,] matrix = new double[size, size];

            FillMatrixFromConsole(matrix);

            return matrix;
        }


        static void Main()
        {
            double[,] matrix = GetMatrixFromConsole();
            int n = matrix.GetLength(0);

            double det = CalculateDeterminant(matrix, n); //Основная функция которая находит определитель

            Console.WriteLine();

            matrix_print(matrix, n, Space(matrix, n), det);

            Console.ReadKey();
        }
    }
}
