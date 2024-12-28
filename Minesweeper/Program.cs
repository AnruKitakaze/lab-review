using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace saper
{
    internal class Program

    {

        //Вывод открытого поля
        static void game_open(int[,] pole)
        {
            Console.Write("   ");
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2} ");
            }
            Console.WriteLine();

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2}|");
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole[i, j] == 0)
                    {
                        Console.Write($"{"( )",2}");
                    }
                    if (pole[i, j] > 0)
                    {
                        Console.Write($"{pole[i, j],2} ");
                    }
                    if (pole[i, j] < 0)
                    {
                        Console.Write($"{" M ",2}");
                    }
                }
                Console.WriteLine();
            }
        }


        //Считаем сколько клеток открыто
        static int cnt_move(int[,] pole, int[,] pole_move)
        {
            int cnt_move = 0;
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                for (int j = 0; j < pole.GetLength(1); j++)
                {
                    if (pole_move[i, j] == 1) cnt_move++;
                }
            }
            return cnt_move;
        }



        //Запрашиваем координаты
        static string move()
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
        static void check_pole_and_print(int[,] pole_flag, int[,] pole, int[,] pole_move, int razmer, int cnt_min)
        {

            //Узнаём координаты и тип хода
            String s = move();
            var ar = s.Split(' ');
            int x = int.Parse(ar[0]);
            int y = int.Parse(ar[1]);
            string type_of_move = ar[2];

            int life = 1; //Заранее жизнь есть

            Console.Clear(); //Очищаем консоль

            //Напоминаем предыдущий ход, по которому идёт действие
            if (type_of_move == "к")
            {
                Console.WriteLine($"Твой ход был: копать {x}, {y} ");
                check_pustoi_move(pole, pole_move, x - 1, y - 1); //Добавляем инфу о ходе и пустых клетках(если они есть)
                life = check_life(pole, x, y); //Узнаём реальное значение жизни
            }
            if (type_of_move == "ф")
            {
                Console.WriteLine($"Твой ход был: флаг {x}, {y} ");
                nominate_flag(pole_flag, x - 1, y - 1); //Ставим флаг на нужное место
            }



            Console.WriteLine();

            //Делаем из координат индексы
            x--;
            y--;


            //Нумеруем клетку сверху
            Console.Write("   ");
            for (int k = 0; k < pole.GetLength(0); k++)
            {
                Console.Write($"{k + 1,2} ");
            }
            Console.WriteLine();

            for (int i = 0; i < pole.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2}|"); //Нумеруем клетку слева
                for (int j = 0; j < pole.GetLength(1); j++)
                {

                    if (pole_flag[i, j] == 1) Console.Write($"{" Ф ",2}"); //Ставим флаги где нужно

                    //Печатаем поле
                    if (pole_flag[i, j] != 1)
                    {
                        if (pole_move[i, j] == 0) //Если нет инфы о клетке
                        {
                            Console.Write("   ");
                        }
                        else
                        if (pole[i, j] == 0 && pole_move[i, j] == 1) //Если известна пустая клетка
                        {
                            Console.Write($"{"( )",2}");
                        }
                        else
                        if (pole[i, j] > 0 && pole_move[i, j] == 1) //Если известна цифра
                        {
                            Console.Write($"{pole[i, j],2} ");
                        }
                        else
                        if (pole[i, j] < 0 && pole_move[i, j] == 1) //Если известна бомба
                        {
                            Console.Write($"{" M ",2}");
                        }
                    }

                }
                Console.WriteLine();
            }

            //Если попали на бомбу
            if (life == 0)
            {
                Console.Clear();
                Console.WriteLine($"Твой ход был: копать {x + 1}, {y + 1}");
                Console.WriteLine("Проиграл!");
                game_open(pole);
                Console.Write("Начать заново? : ");
                string yas_no = Console.ReadLine();
                if (yas_no == "да")
                {
                    Console.Clear();
                    game_again();
                }
            }

            //Если выиграли
            if (cnt_move(pole, pole_move) == (razmer * razmer) - cnt_min)
            {
                Console.Clear();
                Console.WriteLine($"Твой ход был: копать {x}, {y}");
                Console.WriteLine("Ты выиграл!!!");
                game_open(pole);
                Console.Write("Начать заново? : ");
                string yas_no = Console.ReadLine();
                if (yas_no == "да")
                {
                    Console.Clear();
                    game_again();
                }
            }

            //Если не попали на бомбу, то продолжаем игру
            else if (life == 1)
            {
                check_pole_and_print(pole_flag, pole, pole_move, razmer, cnt_min); //Рекурсия. Вызываем эту же функцию
            }
        }


        //Выводим пустое поле. Нужно для начала
        static void print_pustoe_pole(int[,] pole)
        {
            Console.Write("   ");
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                Console.Write($"{i + 1,2} ");
            }
            Console.WriteLine();


            for (int i = 0; i < pole.GetLength(1); i++)
            {
                Console.Write($"{i + 1,2}|");
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        //Создаём полноценное поле. Раскидываем бомбы и нумеруем округу
        static void rand_pole(int[,] pole, int min_need)
        {
            Random rng = new Random();
            int min_ready = 0;
            do
            {
                int x = rng.Next(pole.GetLength(0));
                int y = rng.Next(pole.GetLength(1));
                if (pole[x, y] >= 0)
                {
                    pole[x, y] = -1000;

                    if (x > 0 && y > 0) pole[x - 1, y - 1]++;
                    if (y > 0) pole[x, y - 1]++;
                    if (x > 0) pole[x - 1, y]++;
                    if (x > 0 && y < pole.GetLength(1) - 1) pole[x - 1, y + 1]++;
                    if (x < pole.GetLength(0) - 1 && y > 0) pole[x + 1, y - 1]++;
                    if (x < pole.GetLength(0) - 1 && y < pole.GetLength(1) - 1)
                    {
                        pole[x + 1, y + 1]++;
                    }
                    if (x < pole.GetLength(0) - 1) pole[x + 1, y]++;
                    if (y < pole.GetLength(1) - 1) pole[x, y + 1]++;

                    min_ready++;
                }
            } while (min_ready < min_need);
        }


        static void game_again()
        {
            const int razmer = 5; //Задаём размер поля
            int[,] pole = new int[razmer, razmer]; //Храним внутреигровую инфу
            int[,] pole_move = new int[razmer, razmer]; //Храним инфу о ходах
            int[,] pole_flag = new int[razmer, razmer]; //Храним инфу о флагах
            int cnt_min = 5; //Сколько нужно мин
            print_pustoe_pole(pole); //Для начала создаём пустое поле(клетку)
            rand_pole(pole, cnt_min); //Создаём полноценное поле
            check_pole_and_print(pole_flag, pole, pole_move, razmer, cnt_min); //Оснавная рекурсивня функция игры
        }


        static void Main(string[] args)
        {
            game_again();
        }
    }
}