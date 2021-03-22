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
            for (int column = 1; column < columns; column++)
            {
                bool coreLine = false;
                int coreLineIndex = 0;
                for (int row = 1; row < rows; row++)
                {
                    if (array[row, column] == 1 && coreLine)
                        break;
                    if (array[row, column] == 1)
                    {
                        coreLine = true;
                        coreLineIndex = row;
                    }
                    if (row == rows - 1)
                    {
                        for (int coreColumn = 1; coreColumn < columns; coreColumn++)
                        {
                            if (array[coreLineIndex, coreColumn] == 1)
                                listCoreColumns.Add(array[0, coreColumn]);
                        }
                        Console.WriteLine($"\tВ столбце {array[0, column]} ядерная строка - {Program.rowName[array[coreLineIndex, 0]]}!");
                        listCoreRows.Add(array[coreLineIndex, 0]);
                        fullCoveringRows.Add(array[coreLineIndex, 0]);
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
            for (int row = 1; row < rows; row++)
            {
                for (int column = 1; column < columns; column++)
                {
                    if (array[row, column] == 1)
                        break;
                    if (column == columns - 1)
                    {
                        Console.WriteLine($"\tСтрока {Program.rowName[row]} является антиядерной!");
                        listAntiCoreRows.Add(array[row, 0]);
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
            int totalCount = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            for (int basicRow = 1; basicRow < rows; basicRow++)
            {
                for (int comparedRow = basicRow; comparedRow < rows - 1; comparedRow++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalRows = true;

                    for (int column = 1; column < columns; column++)
                    {
                        if (array[basicRow, column] != array[comparedRow + 1, column] && equalRows)
                            equalRows = false;
                        if (column == columns - 1 && equalRows)
                        {
                            Console.WriteLine($"\tСтроки {Program.rowName[basicRow]} и {Program.rowName[comparedRow + 1]} является одинаковыми!");
                            listRows.Add(array[basicRow, 0]);
                            Program.possible = true;
                        }

                        if (array[basicRow, column] > array[comparedRow + 1, column] || !couldCover)
                            firstCovering = true;
                        if (array[basicRow, column] < array[comparedRow + 1, column] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;
                        if (column == columns - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[array[basicRow, 0]]} поглощает строку {Program.rowName[array[comparedRow + 1, 0]]}!");
                                listRows.Add(array[comparedRow + 1, 0]);
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтрока {Program.rowName[comparedRow + 1]} поглощает строку {Program.rowName[basicRow]}!");
                                listRows.Add(array[basicRow, 0]);
                                Program.possible = true;
                            }
                        }
                    }
                }

                totalCount = basicRow;
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"\tОбщее количество итераций при проверке строк - {totalCount}");
            Console.WriteLine("\tПроверка строк на покрытие завешенна!");
        }
        public static void ColumnsCovering(int[,] array, int rows, int columns, List<int> listColumns)
        {
            Console.WriteLine("\tПроверка столбцов на покрытие...");

            int totalCount = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            for (int basicColumn = 1; basicColumn < columns; basicColumn++)
            {
                for (int comparedColumn = basicColumn; comparedColumn < columns - 1; comparedColumn++)
                {
                    bool firstCovering = false;
                    bool lastCovering = false;
                    bool couldCover = true;
                    bool equalColumns = true;

                    for (int row = 1; row < rows; row++)
                    {
                        if (array[row, basicColumn] != array[row, comparedColumn + 1] && equalColumns)
                            equalColumns = false;

                        if (row == rows - 1 && equalColumns)
                        {
                            Console.WriteLine($"\tСтолбцы {array[0, basicColumn]} и {array[0, comparedColumn + 1]} являются одинаковыми!");
                            listColumns.Add(array[0, basicColumn]);
                            Program.possible = true;
                        }

                        if (array[row, basicColumn] > array[row, comparedColumn + 1] || !couldCover)
                            firstCovering = true;
                        if (array[row, basicColumn] < array[row, comparedColumn + 1] || !couldCover)
                            lastCovering = true;

                        if (firstCovering && lastCovering && couldCover)
                            couldCover = false;

                        if (row == rows - 1 && couldCover)
                        {
                            if (firstCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, basicColumn]} поглощает столбец {array[0, comparedColumn + 1]}!");
                                listColumns.Add(array[0, basicColumn]);
                                Program.possible = true;
                            }
                            else if (lastCovering)
                            {
                                Console.WriteLine($"\tСтолбец {array[0, comparedColumn + 1]} поглощает столбец {array[0, basicColumn]}!");
                                listColumns.Add(array[0, comparedColumn + 1]);
                                Program.possible = true;
                            }
                        }
                    }
                }

                totalCount = basicColumn;
            }
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"\tОбщее количество итераций при проверке столбцов - {totalCount}");
            Console.WriteLine("\tПроверка столбцов на покрытие завешена!");
        }
    }
}
