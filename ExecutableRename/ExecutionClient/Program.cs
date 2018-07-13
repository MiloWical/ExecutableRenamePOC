namespace ExecutionClient
{
    using System;
    using System.Configuration;

    public class Program
    {
        public static void Main(string[] args)
        {
            var message = ConfigurationManager.AppSettings["Message"];

            var color = Console.ForegroundColor;
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), ConfigurationManager.AppSettings["Color"]);
            Console.WriteLine(message);
            Console.ForegroundColor = color;
        }
    }
}
