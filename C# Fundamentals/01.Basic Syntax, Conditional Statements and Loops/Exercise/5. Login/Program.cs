using System;


namespace _5.Login
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = Console.ReadLine();
            string pasword = "";

            for (int i = username.Length - 1; i >= 0; i--)
            {
                pasword += username[i];
            }

            int inputCount = 0;
            bool isLogIn = false;
            bool isBlocked = false;

            while (!isLogIn && !isBlocked)
            {
                string input = Console.ReadLine();

                if (input == pasword)
                {
                    Console.WriteLine($"User {username} logged in.");
                    isLogIn = true;
                }
                else
                {
                    inputCount += 1;
                    if (inputCount == 4)
                    {
                        Console.WriteLine("User {0} blocked!",username);
                        isBlocked = true;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password. Try again.");
                    }
                }
            }
        }
    }
}
