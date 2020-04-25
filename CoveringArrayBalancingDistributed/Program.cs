using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoveringArrayBalancingDistributed
{
    class Program
    {
        internal static bool possible = false;
        internal static Dictionary<int, string> rowName = new Dictionary<int, string>
        {
            {0, " "},
            {1, "A"},
            {2, "B"},
            {3, "C"},
            {4, "D"},
            {5, "E"},
            {6, "F"},
            {7, "G"},
            {8, "H"},
            {9, "I"},
            {10, "J"},
            {11, "K"},
            {12, "L"},
            {13, "M"},
            {14, "N"},
            {15, "O"}
        };

        static void Main(string[] args)
        {
            int[,] orgArray = new int[9, 11]
            {
              // 1 2 3 4 5 6 7 8 9 10
                {0,1,2,3,4,5,6,7,8,9,10 },
                {1,0,1,0,0,0,0,1,1,0,0 }, //1
                {2,1,0,0,1,0,0,0,0,0,0 }, //2
                {3,0,0,0,0,0,1,0,0,1,0 }, //3
                {4,0,1,1,0,0,0,0,0,1,1 }, //4
                {5,0,0,0,1,0,0,0,1,0,0 }, //5
                {6,1,0,0,0,0,1,0,0,0,1 }, //6
                {7,0,0,1,0,1,0,0,0,1,0 }, //7
                {8,1,0,0,0,1,0,1,0,0,0 },  //8
            };

            int[,] array = orgArray;
            List<int> fullCoveringRows = new List<int>();

            do
            {
                possible = false;
                List<int> listCoveringRows = new List<int>();
                List<int> listCoveringColumns = new List<int>();
                List<int> listCoreRows = new List<int>();
                List<int> listCoreColumns = new List<int>();
                List<int> listAntiCoreRows = new List<int>();

                {
                    CoreLine(array, GetRowsCount(array), GetColumnsCount(array), listCoreRows, fullCoveringRows, listCoreColumns);
                    foreach (int i in listCoreRows.Distinct().ToList()) array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);
                    foreach (int i in listCoreColumns.Distinct().ToList()) array = DeleteColumn(array, GetRowsCount(array), GetColumnsCount(array), i);
                }
                {
                    AntiCoreLine(array, GetRowsCount(array), GetColumnsCount(array), listAntiCoreRows);
                    foreach (int i in listAntiCoreRows.Distinct().ToList()) array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);
                }
                {
                    RowsCovering(array, GetRowsCount(array), GetColumnsCount(array), listCoveringRows);
                    ColumnsCovering(array, GetRowsCount(array), GetColumnsCount(array), listCoveringColumns);
                    foreach (int i in listCoveringRows.Distinct().ToList()) array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);
                    foreach (int i in listCoveringColumns.Distinct().ToList()) array = DeleteColumn(array, GetRowsCount(array), GetColumnsCount(array), i);
                }
                Print(array, GetRowsCount(array), GetColumnsCount(array));
                {
                    foreach (int i in listCoreRows.Distinct().ToList()) orgArray = DeleteRow(orgArray, GetRowsCount(orgArray), GetColumnsCount(orgArray), i);
                    foreach (int i in listCoreColumns.Distinct().ToList()) orgArray = DeleteColumn(orgArray, GetRowsCount(orgArray), GetColumnsCount(orgArray), i);
                    foreach (int i in listCoveringColumns.Distinct().ToList()) orgArray = DeleteColumn(orgArray, GetRowsCount(orgArray), GetColumnsCount(orgArray), i);
                }
            } while (possible);

            Console.WriteLine("\tФинальный ЦО:");
            Print(orgArray, GetRowsCount(orgArray), GetColumnsCount(orgArray));

            Console.WriteLine("\n\tПокрытие включает такие строки:\n");
            foreach (int i in fullCoveringRows) Console.Write(rowName[i] + "  ");
            Console.ReadKey();
        }

        internal static void CoreLine(int[,] array, int rows, int columns, List<int> listCoreRows, List<int> fullCoveringRows, List<int> listCoreColumns)
        {
            int coreLine = 0;
            int index = 0;
            for (int j = 1; j < columns; j++)
            {
                coreLine = 0;
                index = 0;
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
                        Console.WriteLine($"\tВ столбце {array[0, j]} ядерная строка - {rowName[index]}!");
                        listCoreRows.Add(array[index, 0]);
                        fullCoveringRows.Add(array[index, 0]);
                        Program.possible = true;
                    }
                }
            }
        } //Не нужно балансировать
        internal static void AntiCoreLine(int[,] array, int rows, int columns, List<int> listAntiCoreRows)
        {
            for (int i = 1; i < rows; i++)
            {
                for (int j = 1; j < columns; j++)
                {
                    if (array[i, j] == 1)
                        break;
                    if (j == columns - 1)
                    {
                        Console.WriteLine($"\tСтрока {rowName[i]} является антиядерной!");
                        listAntiCoreRows.Add(array[i, 0]);
                        Program.possible = true;
                    }
                }
            }
        } // Не нужно балансировать
        internal static void RowsCovering(int[,] array, int rows, int columns, List<int> listRows)
        {
            bool evenCount = (rows - 1) % 2 == 0;
            int newRows = (rows - 1) / 2;

            for (int i = 1; i < newRows + 1; i++)
            {
                Console.WriteLine();
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
                            Console.WriteLine($"\tСтроки {rowName[i]} и {rowName[inc + 1]} является одинаковыми!");
                            listRows.Add(array[i, 0]);
                            possible = true;
                        }

                        if (array[i, k] > array[inc + 1, k] || !couldCover)
                            firstCovering = true;
                        if (array[i, k] < array[inc + 1, k] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                        {
                            Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {i}-{inc + 1}");
                            couldCover = false;
                        }
                        if (k == columns - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтрока {rowName[i]} поглощает строку {rowName[inc + 1]}!");
                                listRows.Add(array[inc + 1, 0]);
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтрока {rowName[inc + 1]} (которая выше) поглощает строку {rowName[i]}!");
                                listRows.Add(array[i, 0]);
                                possible = true;
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
                            Console.WriteLine($"\tСтроки {rowName[a]} и {rowName[inc + 1]} является одинаковыми!");
                            listRows.Add(array[a, 0]);
                            possible = true;
                        }

                        if (array[a, k] > array[inc + 1, k] || !couldCover)
                            firstCovering = true;
                        if (array[a, k] < array[inc + 1, k] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                        {
                            Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {a}-{inc + 1}");
                            couldCover = false;
                        }
                        if (k == columns - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтрока {rowName[a]} поглощает строку {rowName[inc + 1]}!");
                                listRows.Add(array[inc + 1, 0]);
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтрока {rowName[inc + 1]} (которая выше) поглощает строку {rowName[a]}!");
                                listRows.Add(array[a, 0]);
                                possible = true;
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
                                Console.WriteLine($"\tСтроки {rowName[b]} и {rowName[inc + 1]} является одинаковыми!");
                                listRows.Add(array[b, 0]);
                                possible = true;
                            }

                            if (array[b, k] > array[inc + 1, k] || !couldCover)
                                firstCovering = true;
                            if (array[b, k] < array[inc + 1, k] || !couldCover)
                                lastCovering = true;

                            if (firstCovering && lastCovering && couldCover)
                            {
                                Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {b}-{inc + 1}");
                                couldCover = false;
                            }
                            if (k == columns - 1 && couldCover)
                            {
                                if (firstCovering)
                                {
                                    Console.WriteLine($"\tСтрока {rowName[b]} поглощает строку {rowName[inc + 1]}!");
                                    listRows.Add(array[inc + 1, 0]);
                                }
                                else if (lastCovering)
                                {
                                    Console.WriteLine($"\tСтрока {rowName[inc + 1]} (которая выше) поглощает строку {rowName[b]}!");
                                    listRows.Add(array[b, 0]);
                                    possible = true;
                                }
                            }
                        }
                    }
                } //Не провеверено, должно работать
            }
        }
        internal static void ColumnsCovering(int[,] array, int rows, int columns, List<int> listColumns)
        {
            bool evenCount = (columns - 1) % 2 == 0;
            int newColumns = (columns - 1) / 2;

            for (int i = 1; i < newColumns + 1; i++)
            {
                Console.WriteLine();
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
                        {
                            Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {i}-{inc + 1}");
                            couldCover = false;
                        }

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
                                Console.WriteLine($"\tСтолбец {array[0, inc + 1]} (который левее) поглощает столбец {array[0, i]}!");
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
                        {
                            Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {a}-{inc + 1}");
                            couldCover = false;
                        }

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
                                Console.WriteLine($"\tСтолбец {array[0, inc + 1]} (который левее) поглощает столбец {array[0, a]}!");
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
                            {
                                Console.WriteLine($"НЕЛЬЗЯ ПОГЛОТИТЬ - {b}-{inc + 1}");
                                couldCover = false;
                            }

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
                                    Console.WriteLine($"\tСтолбец {array[0, inc + 1]} (который левее) поглощает столбец {array[0, b]}!");
                                    listColumns.Add(array[0, inc + 1]);
                                    Program.possible = true;
                                }
                            }
                        }
                    }
                } // Не проверено, должно работать
            }
        }

        public static int[,] DeleteRow(int[,] array, int rows, int columns, int num)
        {
            int[,] temp = new int[rows - 1, columns];
            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                if (array[i, 0] == num)
                    continue;
                else
                {
                    for (int j = 0; j < columns; j++)
                        temp[index, j] = array[i, j];
                    index++;
                }
            }
            return temp;
        }
        public static int[,] DeleteColumn(int[,] array, int rows, int columns, int num)
        {
            int[,] temp = new int[rows, columns - 1];
            int index = 0;
            for (int i = 0; i < columns; i++)
            {
                if (array[0, i] == num)
                    continue;
                else
                {
                    for (int j = 0; j < rows; j++)
                        temp[j, index] = array[j, i];
                    index++;
                }
            }
            return temp;
        }


        public static int GetRowsCount(int[,] array) => array.GetUpperBound(0) + 1;
        public static int GetColumnsCount(int[,] array) => array.Length / (array.GetUpperBound(0) + 1);

        public static void Print(int[,] array, int rows, int columns)
        {
            Console.WriteLine();
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine();
                for (int j = 0; j < columns; j++)
                {
                    if (j == 0)
                    {
                        Console.Write(rowName[array[i, 0]] + "  ");
                        continue;
                    }
                    Console.Write(array[i, j] + "  ");
                }
            }
            Console.WriteLine();
        }
    }
}
