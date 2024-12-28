using System.Linq.Expressions;

namespace Minesweeper
{
    internal class Program

    {

        //Вывод открытого поля
        static void OpenGame(int[,] field)
        {
            Console.Write("   ");
            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2} ");
            }
            Console.WriteLine();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2}|");
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i, j] == 0)
                    {
                        Console.Write($"{"( )",2}");
                    }
                    if (field[i, j] > 0)
                    {
                        Console.Write($"{field[i, j],2} ");
                    }
                    if (field[i, j] < 0)
                    {
                        Console.Write($"{" M ",2}");
                    }
                }
                Console.WriteLine();
            }
        }

        static int CountOpenedCells(int[,] field, int[,] pole_move)
        {
            int cnt_move = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (pole_move[i, j] == 1) cnt_move++;
                }
            }
            return cnt_move;
        }

        //Запрашиваем координаты
        static string GetUserInput()
        {
            Console.Write("Сделайте ход: ");
            String s = Console.ReadLine();
            return s;
        }

        //Даём значение жизни - возвращаем 1 или 0
        static int check_life(int[,] pole, int x, int y)
        {

            int life = 1;

            if (pole[x - 1, y - 1] < 0)
            {
                life = 0;
            }
            return life;
        }

        //Ставим или убираем флаги
        static void nominate_flag(int[,] pole_flag, int x, int y)
        {
            if (pole_flag[x, y] != 1) pole_flag[x, y] = 1;
            else pole_flag[x, y] = 0;
        }

        //Открываем все пустые рядомстоящие точки
        static void check_pustoi_move(int[,] pole, int[,] pole_move, int x, int y)
        {
            pole_move[x, y] = 1; //Делаем текущий ход видимым

            //Если текущий ход пустой
            if (pole[x, y] == 0)
            {

                //Следующий принцип:
                //Если окружность возможна и она ещё не видна, то делаем её видимой
                //   Ещё если в окружности пустой ход, то от него повторяем операцию поиска по окружности

                if (x > 0 && y > 0 && pole_move[x - 1, y - 1] == 0)
                {
                    pole_move[x - 1, y - 1] = 1;
                    if (pole[x - 1, y - 1] == 0) check_pustoi_move(pole, pole_move, x - 1, y - 1);
                }
                if (y > 0 && pole_move[x, y - 1] == 0)
                {
                    pole_move[x, y - 1] = 1;
                    if (pole[x, y - 1] == 0) check_pustoi_move(pole, pole_move, x, y - 1);
                }
                if (x > 0 && pole_move[x - 1, y] == 0)
                {
                    pole_move[x - 1, y] = 1;
                    if (pole[x - 1, y] == 0) check_pustoi_move(pole, pole_move, x - 1, y);
                }
                if (x > 0 && y < pole.GetLength(1) - 1 && pole_move[x - 1, y + 1] == 0)
                {
                    pole_move[x - 1, y + 1] = 1;
                    if (pole[x - 1, y + 1] == 0) check_pustoi_move(pole, pole_move, x - 1, y + 1);
                }
                if (x < pole.GetLength(0) - 1 && y > 0 && pole_move[x + 1, y - 1] == 0)
                {
                    pole_move[x + 1, y - 1] = 1;
                    if (pole[x + 1, y - 1] == 0) check_pustoi_move(pole, pole_move, x + 1, y - 1);
                }
                if (x < pole.GetLength(0) - 1 && y < pole.GetLength(1) - 1
                    && pole_move[x + 1, y + 1] == 0)
                {
                    pole_move[x + 1, y + 1] = 1;
                    if (pole[x + 1, y + 1] == 0) check_pustoi_move(pole, pole_move, x + 1, y + 1);
                }
                if (x < pole.GetLength(0) - 1 && pole_move[x + 1, y] == 0)
                {
                    pole_move[x + 1, y] = 1;
                    if (pole[x + 1, y] == 0) check_pustoi_move(pole, pole_move, x + 1, y);
                }
                if (y < pole.GetLength(1) - 1 && pole_move[x, y + 1] == 0)
                {
                    pole_move[x, y + 1] = 1;
                    if (pole[x, y + 1] == 0) check_pustoi_move(pole, pole_move, x, y + 1);
                }
            }
        }

        //Выводим поле со значением ходов
        static void Play(int[,] flagsSet, int[,] field, int[,] moves, int fieldSize, int minesAmount)
        {
            int x;
            int y;
            string type_of_move;

            // TODO: Вынести в отдельный метод
            // GetUserAction / GetUserInput
            while (true)
            {
                try
                {
                    Console.Write("Сделайте ход (x y action): ");
                    string? s = Console.ReadLine();
                    if (s == null)
                    {
                        // TODO: Обработать q для выхода из программы
                        Console.WriteLine("Вы ничего не ввели. Если хотите выйти, введите 'q'");
                        continue;
                    }
                    var ar = s.Split(' ');
                    x = int.Parse(ar[0]);
                    y = int.Parse(ar[1]);
                    type_of_move = ar[2];

                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Введи ход в формате 'x y action', где action может быть к или ф:");
                    continue;
                }
            }

            int life = 1;
            Console.Clear();

            // TOOD: Вынести в отдельный метод
            if (type_of_move == "к")
            {
                Console.WriteLine($"Твой ход был: копать {x}, {y} ");
                check_pustoi_move(field, moves, x - 1, y - 1); //Добавляем инфу о ходе и пустых клетках(если они есть)
                life = check_life(field, x, y); //Узнаём реальное значение жизни
            }
            if (type_of_move == "ф")
            {
                Console.WriteLine($"Твой ход был: флаг {x}, {y} ");
                nominate_flag(flagsSet, x - 1, y - 1); //Ставим флаг на нужное место
            }

            Console.WriteLine();
            x--;
            y--;

            //Нумеруем клетку сверху
            Console.Write("   ");
            for (int k = 0; k < field.GetLength(0); k++)
            {
                Console.Write($"{k + 1,2} ");
            }
            Console.WriteLine();

            for (int i = 0; i < field.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2}|"); //Нумеруем клетку слева
                for (int j = 0; j < field.GetLength(1); j++)
                {

                    if (flagsSet[i, j] == 1)
                    {
                        Console.Write($"{" Ф ",2}"); //Ставим флаги где нужно
                        continue;
                    }

                    if (moves[i, j] == 0) //Если нет инфы о клетке
                    {
                        Console.Write("   ");
                    }
                    else if (field[i, j] == 0 && moves[i, j] == 1) //Если известна пустая клетка
                    {
                        Console.Write($"{"( )",2}");
                    }
                    else if (field[i, j] > 0 && moves[i, j] == 1) //Если известна цифра
                    {
                        Console.Write($"{field[i, j],2} ");
                    }
                    else if (field[i, j] < 0 && moves[i, j] == 1) //Если известна бомба
                    {
                        Console.Write($"{" M ",2}");
                    }

                }
                Console.WriteLine();
            }

            Console.Clear();

            if (life == 1 && CountOpenedCells(field, moves) != (fieldSize * fieldSize) - minesAmount)
            {
                Play(flagsSet, field, moves, fieldSize, minesAmount); //Рекурсия. Вызываем эту же функцию
            }
            else if (life == 0)
            {
                Console.WriteLine($"Твой ход был: копать {x + 1}, {y + 1}");
                Console.WriteLine("Проиграл!");
            }
            else
            {
                Console.WriteLine($"Твой ход был: копать {x}, {y}");
                Console.WriteLine("Ты выиграл!!!");
            }

            OpenGame(field);
            Console.Write("Начать заново? : ");
            string yas_no = Console.ReadLine();
            if (yas_no == "да")
            {
                Console.Clear();
                Start();
            }
        }

        static void PrintEmptyField(int x_size, int y_size)
        {
            Console.Write("   ");
            for (int i = 0; i < x_size; i++)
            {
                Console.Write($"{i + 1,2} ");
            }
            Console.WriteLine();


            for (int i = 0; i < y_size; i++)
            {
                Console.Write($"{i + 1,2}|");
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void PopulateFieldWithMines(int[,] field, int mines_needed)
        {
            Random random = new();
            int mines_ready = 0;
            const int Mine = -1000;

            do
            {
                int x = random.Next(field.GetLength(0));
                int y = random.Next(field.GetLength(1));

                if (field[x, y] >= 0)
                {
                    field[x, y] = Mine;

                    if (x > 0 && y > 0) field[x - 1, y - 1]++;
                    if (y > 0) field[x, y - 1]++;
                    if (x > 0) field[x - 1, y]++;
                    if (x > 0 && y < field.GetLength(1) - 1) field[x - 1, y + 1]++;
                    if (x < field.GetLength(0) - 1 && y > 0) field[x + 1, y - 1]++;
                    if (x < field.GetLength(0) - 1 && y < field.GetLength(1) - 1)
                    {
                        field[x + 1, y + 1]++;
                    }
                    if (x < field.GetLength(0) - 1) field[x + 1, y]++;
                    if (y < field.GetLength(1) - 1) field[x, y + 1]++;

                    mines_ready++;
                }
            } while (mines_ready < mines_needed);
        }

        static void Start()
        {
            const int fieldSize = 5;
            const int minesAmount = 5;

            int[,] field = new int[fieldSize, fieldSize];
            int[,] moves = new int[fieldSize, fieldSize];
            int[,] flagsSet = new int[fieldSize, fieldSize];

            PrintEmptyField(field.GetLength(0), field.GetLength(1));
            PopulateFieldWithMines(field, minesAmount);

            Play(flagsSet, field, moves, fieldSize, minesAmount);
        }

        static void Main(string[] args)
        {
            Start();
        }
    }
}