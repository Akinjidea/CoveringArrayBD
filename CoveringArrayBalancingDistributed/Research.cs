using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoveringArrayBalancingDistributed
{
    class Research
    {
        public static void CoreLine(int[,] array, int rows, int columns, List<int> listCoreRows, List<int> fullCoveringRows, List<int> listCoreColumns)
        {
            Console.WriteLine("\tПроверка строк на ядерность...");
            Console.ForegroundColor = ConsoleColor.Red;
            for (int j = 1; j < columns; j++)
            {
                int coreLine = 0;
                int index = 0;
                for (int i = 1; i < rows; i++)
                {
                    if (array[i, j] == 1 && coreLine == 1)
                        break;
                    if (array[i, j] == 1)
                    {
                        coreLine = 1;
                        index = i;
                    }
                    if (i == rows - 1)
                    {
                        for (int k = 1; k < columns; k++)
                        {
                            if (array[index, k] == 1)
                                listCoreColumns.Add(array[0, k]);
                        }
                        Console.WriteLine($"\tВ столбце {array[0, j]} ядерная строка - {Program.rowName[array[index, 0]]}!");
                        listCoreRows.Add(array[index, 0]);
                        fullCoveringRows.Add(array[index, 0]);
                        Program.possible = true;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tПроверка строк на ядерность завершена!");
        }
        public static void AntiCoreLine(int[,] array, int rows, int columns, List<int> listAntiCoreRows)
        {
            Console.WriteLine("\tПроверка строк на антиядерность...");
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 1; i < rows; i++)
            {
                for (int j = 1; j < columns; j++)
                {
                    if (array[i, j] == 1)
                        break;
                    if (j == columns - 1)
                    {
                        Console.WriteLine($"\tСтрока {Program.rowName[i]} является антиядерной!");
                        listAntiCoreRows.Add(array[i, 0]);
                        Program.possible = true;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tПроверка строк на антиядерность завершена!");
        }
        public static void RowsCovering(int[,] array, int rows, int columns, List<int> listRows)
        {
            Console.WriteLine("\tПроверка строк на покрытие...");
            bool evenCount = (rows - 1) % 2 == 0;
            int newRows = (rows - 1) / 2;
            int totalCount = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 1; i < newRows + 1; i++)
            {
                for (int inc = i; inc < rows - 1; inc++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalRows = true;

                    for (int k = 1; k < columns; k++)
                    {
                        if (array[i, k] != array[inc + 1, k] && equalRows)
                            equalRows = false;
                        if (k == columns - 1 && equalRows)
                        {
                            Console.WriteLine($"\tСтроки {Program.rowName[i]} и {Program.rowName[inc + 1]} является одинаковыми!");
                            listRows.Add(array[i, 0]);
                            Program.possible = true;
                        }

                        if (array[i, k] > array[inc + 1, k] || !couldCover)
                            firstCovering = true;
                        if (array[i, k] < array[inc + 1, k] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;
                        if (k == columns - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[array[i, 0]]} поглощает строку {Program.rowName[array[inc + 1, 0]]}!");
                                listRows.Add(array[inc + 1, 0]);
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[inc + 1]} поглощает строку {Program.rowName[i]}!");
                                listRows.Add(array[i, 0]);
                                Program.possible = true;
                            }
                        }
                    }
                }
                // Для балансированого покрытия с четным количество строк! Количество строк измененно!
                for (int a = rows - i, b = rows - i; b < rows - 1; b++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalRows = true;

                    int inc = b;

                    for (int k = 1; k < columns; k++)
                    {
                        if (array[a, k] != array[inc + 1, k] && equalRows)
                            equalRows = false;
                        if (k == columns - 1 && equalRows)
                        {
                            Console.WriteLine("\tСтроки {0} и {1} является одинаковыми!", Program.rowName[a], Program.rowName[inc + 1]);
                            listRows.Add(array[a, 0]);
                            Program.possible = true;
                        }

                        if (array[a, k] > array[inc + 1, k] || !couldCover)
                            firstCovering = true;
                        if (array[a, k] < array[inc + 1, k] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;
                        if (k == columns - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[a]} поглощает строку {Program.rowName[inc + 1]}!");
                                listRows.Add(array[inc + 1, 0]);
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[inc + 1]} поглощает строку {Program.rowName[a]}!");
                                listRows.Add(array[a, 0]);
                                Program.possible = true;
                            }
                        }
                    }
                    inc++;
                }
                // Для балансированого покрытия с нечетным количество строк! Количество строк изменено!
                if (!evenCount)
                {
                    for (int b = rows - newRows - 1, a = rows - newRows - 1; a < b + 1; a++)
                    {
                        bool firstCovering = false;
                        bool lastCovering = false;
                        bool couldCover = true;
                        bool equalRows = true;

                        int inc = b + i - 1;

                        for (int k = 1; k < columns; k++)
                        {
                            if (array[b, k] != array[inc + 1, k] && equalRows)
                                equalRows = false;
                            if (k == columns - 1 && equalRows)
                            {
                                Console.WriteLine($"\tСтроки {Program.rowName[b]} и {Program.rowName[inc + 1]} является одинаковыми!");
                                listRows.Add(array[b, 0]);
                                Program.possible = true;
                            }

                            if (array[b, k] > array[inc + 1, k] || !couldCover)
                                firstCovering = true;
                            if (array[b, k] < array[inc + 1, k] || !couldCover)
                                lastCovering = true;

                            if (firstCovering && lastCovering && couldCover)
                                couldCover = false;
                            if (k == columns - 1 && couldCover)
                            {
                                if (firstCovering)
                                {
                                    Console.WriteLine($"\tСтрока {Program.rowName[b]} поглощает строку {Program.rowName[inc + 1]}!");
                                    listRows.Add(array[inc + 1, 0]);
                                }
                                else if (lastCovering)
                                {
                                    Console.WriteLine($"\tСтрока {Program.rowName[inc + 1]} поглощает строку {Program.rowName[b]}!");
                                    listRows.Add(array[b, 0]);
                                    Program.possible = true;
                                }
                            }
                        }
                    }
                }

                totalCount = i;
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"\tОбщее количество итераций при проверке строк - {totalCount}");
            Console.WriteLine("\tПроверка строк на покрытие завешенна!");
        }
        public static void ColumnsCovering(int[,] array, int rows, int columns, List<int> listColumns)
        {
            Console.WriteLine("\tПроверка столбцов на покрытие...");
            bool evenCount = (columns - 1) % 2 == 0;
            int newColumns = (columns - 1) / 2;
            int totalCount = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 1; i < newColumns + 1; i++)
            {
                for (int inc = i; inc < columns - 1; inc++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalColumns = true;

                    for (int k = 1; k < rows; k++)
                    {
                        if (array[k, i] != array[k, inc + 1] && equalColumns)
                            equalColumns = false;

                        if (k == rows - 1 && equalColumns)
                        {
                            Console.WriteLine($"\tСтолбцы {array[0, i]} и {array[0, inc + 1]} являются одинаковыми!");
                            listColumns.Add(array[0, i]);
                            Program.possible = true;
                        }

                        if (array[k, i] > array[k, inc + 1] || !couldCover)
                            firstCovering = true;
                        if (array[k, i] < array[k, inc + 1] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;

                        if (k == rows - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, i]} поглощает столбец {array[0, inc + 1]}!");
                                listColumns.Add(array[0, i]);
                                Program.possible = true;
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, inc + 1]} поглощает столбец {array[0, i]}!");
                                listColumns.Add(array[0, inc + 1]);
                                Program.possible = true;
                            }
                        }
                    }
                }
                // Для балансированого покрытия с четным количество столбцов! Количество столбцов измененно!
                for (int a = columns - i, b = columns - i; b < columns - 1; b++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalColumns = true;

                    int inc = b;

                    for (int k = 1; k < rows; k++)
                    {
                        if (array[k, a] != array[k, inc + 1] && equalColumns)
                            equalColumns = false;

                        if (k == rows - 1 && equalColumns)
                        {
                            Console.WriteLine($"\tСтолбцы {array[0, a]} и {array[0, inc + 1]} являются одинаковыми!");
                            listColumns.Add(array[0, a]);
                            Program.possible = true;
                        }

                        if (array[k, a] > array[k, inc + 1] || !couldCover)
                            firstCovering = true;
                        if (array[k, a] < array[k, inc + 1] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;

                        if (k == rows - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, a]} поглощает столбец {array[0, inc + a]}!");
                                listColumns.Add(array[0, a]);
                                Program.possible = true;
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, inc + 1]} поглощает столбец {array[0, a]}!");
                                listColumns.Add(array[0, inc + 1]);
                                Program.possible = true;
                            }
                        }
                    }
                }
                // Для балансированого покрытия с нечетным количество столбцов! Количество столбцов изменено!
                if (!evenCount)
                {
                    for (int b = columns - newColumns - 1, a = columns - newColumns - 1; a < b + 1; a++)
                    {
                        bool firstCovering = false;
                        bool lastCovering = false;
                        bool couldCover = true;
                        bool equalColumns = true;

                        int inc = b + i - 1;

                        for (int k = 1; k < rows; k++)
                        {
                            if (array[k, b] != array[k, inc + 1] && equalColumns)
                                equalColumns = false;

                            if (k == rows - 1 && equalColumns)
                            {
                                Console.WriteLine($"\tСтолбцы {array[0, b]} и {array[0, inc + 1]} являются одинаковыми!");
                                listColumns.Add(array[0, b]);
                                Program.possible = true;
                            }

                            if (array[k, b] > array[k, inc + 1] || !couldCover)
                                firstCovering = true;
                            if (array[k, b] < array[k, inc + 1] || !couldCover)
                                lastCovering = true;

                            if (firstCovering && lastCovering && couldCover)
                                couldCover = false;

                            if (k == rows - 1 && couldCover)
                            {
                                if (firstCovering)
                                {
                                    Console.WriteLine($"\tСтолбец {array[0, b]} поглощает столбец {array[0, inc + b]}!");
                                    listColumns.Add(array[0, b]);
                                    Program.possible = true;
                                }
                                else if (lastCovering)
                                {
                                    Console.WriteLine($"\tСтолбец {array[0, inc + 1]} поглощает столбец {array[0, b]}!");
                                    listColumns.Add(array[0, inc + 1]);
                                    Program.possible = true;
                                }
                            }
                        }
                    }
                }

                totalCount = i;
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"\tОбщее количество итераций при проверке столбцов - {totalCount}");
            Console.WriteLine("\tПроверка столбцов на покрытие завешена!");
        }
    }
}
