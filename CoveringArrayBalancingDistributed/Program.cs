using System;
using System.Collections.Generic;
using System.Linq;

namespace CoveringArrayBalancingDistributed
{
    class Program
    {
        internal static bool possible = false;
        internal static Dictionary<int, string> rowName = new Dictionary<int, string>()
        {
            { 0, " " },
            { 1, "A" },
            { 2, "B" },
            { 3, "C" },
            { 4, "D" },
            { 5, "E" },
            { 6, "F" },
            { 7, "G" },
            { 8, "H" },
            { 9, "I" },
            { 10, "J" },
            { 11, "K" },
            { 12, "L" },
            { 13, "M" },
            { 14, "N" },
            { 15, "O" },
        };

        static void Main(string[] args)
        {
            int[,] array = new int[9, 11]
            {
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10 },
                { 1, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
                { 2, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0 },
                { 3, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
                { 4, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
                { 5, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0 },
                { 6, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0 },
                { 7, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0 },
                { 8, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 }
            };

            List<int> fullCoveringRows = new List<int>();
            do
            {
                possible = false;
                List<int> listCoveringRows = new List<int>();
                List<int> listCoveringColumns = new List<int>();
                List<int> listCoreRows = new List<int>();
                List<int> listCoreColumns = new List<int>();
                List<int> listAntiCoreRows = new List<int>();
                                
                Research.CoreLine(array, GetRowsCount(array), GetColumnsCount(array), listCoreRows, fullCoveringRows, listCoreColumns);                
                foreach (int i in listCoreRows.Distinct().ToList())
                    array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);
                foreach (int i in listCoreColumns.Distinct().ToList())
                    array = DeleteColumn(array, GetRowsCount(array), GetColumnsCount(array), i);

                Print(array, GetRowsCount(array), GetColumnsCount(array));
                
                Research.AntiCoreLine(array, GetRowsCount(array), GetColumnsCount(array), listAntiCoreRows);                
                foreach (int i in listAntiCoreRows.Distinct().ToList())
                    array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);

                Print(array, GetRowsCount(array), GetColumnsCount(array));
                
                Research.RowsCovering(array, GetRowsCount(array), GetColumnsCount(array), listCoveringRows);                

                Print(array, GetRowsCount(array), GetColumnsCount(array));
                
                Research.ColumnsCovering(array, GetRowsCount(array), GetColumnsCount(array), listCoveringColumns);               
                foreach (int i in listCoveringRows.Distinct().ToList())
                    array = DeleteRow(array, GetRowsCount(array), GetColumnsCount(array), i);
                foreach (int i in listCoveringColumns.Distinct().ToList())
                    array = DeleteColumn(array, GetRowsCount(array), GetColumnsCount(array), i);

                Console.WriteLine("\n\t\tТекущий результат");
                Print(array, GetRowsCount(array), GetColumnsCount(array));

            } while (possible);

            Console.WriteLine("\n\tФинальный ЦО:");
            Print(array, GetRowsCount(array), GetColumnsCount(array));

            Console.Write("\n\tПокрытие включает такие строки:\t");
            foreach (int i in fullCoveringRows.Distinct().ToList()) 
                Console.Write(rowName[i] + "  ");
            Console.ReadKey();
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
        public static void Print(int[,] array, int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine();
                Console.Write("\t");
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
            Console.WriteLine("\n");
        }

        public static int GetRowsCount(int[,] array) => array.GetUpperBound(0) + 1;
        public static int GetColumnsCount(int[,] array) => array.Length / (array.GetUpperBound(0) + 1);
        
    }    
}
