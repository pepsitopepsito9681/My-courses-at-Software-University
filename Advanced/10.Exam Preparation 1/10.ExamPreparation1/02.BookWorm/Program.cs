using System;
using System.Text;


namespace _02.BookWorm
{
    class Program
    {
        static void Main(string[] args)
        {
            string initialString = Console.ReadLine();
            var sb = new StringBuilder(initialString);
            int size = int.Parse(Console.ReadLine());
            var field = new char[size, size];
            int playerRow = 0;
            int playerCol = 0;
            for (int row = 0; row < size; row++)
            {
                var currentRow = Console.ReadLine();
                for (int col = 0; col < size; col++)
                {
                    field[row, col] = currentRow[col];
                    if (currentRow[col] == 'P')
                    {
                        playerRow = row;
                        playerCol = col;
                    }
                }
            }

            while (true)
            {
                string command = Console.ReadLine();
                if (command == "end")
                {
                    break;
                }
                if (command == "up")
                {
                    if (playerRow == 0)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            continue;
                        }
                    }

                    field[playerRow, playerCol] = '-';
                    playerRow--;
                    if (char.IsLetter(field[playerRow, playerCol]))
                    {
                        sb.Append(field[playerRow, playerCol]);
                        field[playerRow, playerCol] = 'P';
                    }
                }
                else if (command == "down")
                {
                    if (playerRow == size - 1)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            continue;
                        }
                    }

                    field[playerRow, playerCol] = '-';
                    playerRow++;
                    if (char.IsLetter(field[playerRow, playerCol]))
                    {
                        sb.Append(field[playerRow, playerCol]);
                        field[playerRow, playerCol] = 'P';
                    }
                }
                else if (command == "left")
                {
                    if (playerCol == 0)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            continue;
                        }
                    }

                    field[playerRow, playerCol] = '-';
                    playerCol--;
                    if (char.IsLetter(field[playerRow, playerCol]))
                    {
                        sb.Append(field[playerRow, playerCol]);
                        field[playerRow, playerCol] = 'P';
                    }
                }
                else if (command == "right")
                {
                    if (playerCol == size - 1)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            continue;
                        }
                    }

                    field[playerRow, playerCol] = '-';
                    playerCol++;
                    if (char.IsLetter(field[playerRow, playerCol]))
                    {
                        sb.Append(field[playerRow, playerCol]);
                        field[playerRow, playerCol] = 'P';
                    }
                }
            }

            Console.WriteLine(sb.ToString());
            for (int row = 0; row < size; row++)
            {
                for (int col = 0; col < size; col++)
                {
                    Console.Write(field[row, col]);
                }
                Console.WriteLine();
            }
        }
    }
}
