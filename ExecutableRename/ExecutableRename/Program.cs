namespace ExecutableRename
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Xml;

    public class Program
    {
        protected struct MessageAndColor
        {
            public string Message { get; set; }
            public string Color { get; set; }
        }

        public static void Main(string[] args)
        {
            var sourceExe = Directory.GetCurrentDirectory() + @"\..\..\..\..\Client\ExecutionClient.exe";
            var sourceConfig = Directory.GetCurrentDirectory() + @"\..\..\..\..\Client\ExecutionClient.exe.config";

            var newConfigurations = new[]
            {
                new MessageAndColor
                {
                    Message = "Message for process 1",
                    Color = "Green"
                },
                new MessageAndColor
                {
                    Message = "Process 2's message",
                    Color = "Red"
                }
            };

            Console.WriteLine("Initial process starting...\n");

            ExecuteProcess(sourceExe);

            Console.WriteLine("\nInitial process complete.\nBeginning reconfigured processes.\nPress any key to continue...\n");
            Console.ReadKey();

            foreach (var config in newConfigurations)
            {
                var exeName = GenerateExeName();

                CopyAndRenameExe(sourceExe, exeName);
                Console.WriteLine("\nNew executable generated: {0}", exeName);

                WriteNewConfig(sourceConfig, exeName + ".config", config.Message, config.Color);
                Console.WriteLine("New executable confguration generated: {0}", exeName + ".config");
                Console.WriteLine("\tMessage: '{0}'", config.Message);
                Console.WriteLine("\tColor: '{0}'", config.Color);

                Console.WriteLine("\nVerify new executable file, new configuration file, and expected settings.");
                Console.WriteLine("Press any key to continue...\n");
                Console.ReadKey();

                Console.WriteLine("Executing process '{0}'\n", exeName);
                ExecuteProcess(exeName);

                Console.WriteLine("\nProcessing complete. Removing generated files.");
                RemoveExeAndConfig(exeName);
                Console.WriteLine("Generated files removed. Verify that the files have been deleted.");
                Console.WriteLine("Press any key top continue...");
                Console.ReadKey();
            }

            Console.WriteLine("\nAll processes complete. Press any key to quit...");

            Console.ReadKey();
        }

        private static string GenerateExeName()
        {
            Thread.Sleep(new Random().Next(100, 500));
            return DateTime.Now.ToString("mmssffffff") + ".exe";
        }

        private static void CopyAndRenameExe(string sourceFile, string destinationFileName)
        {
            File.Copy(sourceFile, $@"{Directory.GetCurrentDirectory()}\{destinationFileName}");
        }

        private static void WriteNewConfig(string sourceFile, string destinationFileName, string message, string color)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(sourceFile);

            var messageNode = xmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Message']/@value");

            if(messageNode != null)
                messageNode.Value = message;

            var colorNode = xmlDocument.SelectSingleNode("/configuration/appSettings/add[@key='Color']/@value");

            if (colorNode != null)
                colorNode.Value = color;

            xmlDocument.Save(destinationFileName);
        }

        private static void RemoveExeAndConfig(string exeFileName)
        {
            File.Delete(exeFileName);
            File.Delete(exeFileName + ".config");
        }

        private static void ExecuteProcess(string exePath)
        {
            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    FileName = exePath
                }
            };
            process.Start();
            process.WaitForExit();
        }
    }
}
