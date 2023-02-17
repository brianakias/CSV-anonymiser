namespace CSV_anonymiser.Classes
{
    public static class CommunicateWithUser
    {
        /// <summary>
        /// Prompts the user to provide the path to an input or output file.
        /// </summary>
        /// <param name="inputOrOutput">The type of file to prompt for ("input" or "output").</param>
        /// <returns>The file path provided by the user.</returns>
        public static string RequestFilePath(string inputOrOutput)
        {
            string filePath = "";

            if (inputOrOutput == "input")
            {
                do
                {
                    Console.WriteLine("Provide the path of the file to read from: ");
                    filePath = Console.ReadLine();

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"Could not locate file with path `{filePath}`. Please retry.");
                    }

                } while (!File.Exists(filePath));

                Console.WriteLine("Thanks, file exists.");
            }

            else if (inputOrOutput == "output")
            {
                do
                {
                    Console.WriteLine("Provide the path of the file to write to: ");
                    filePath = Console.ReadLine();

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"Could not locate file with path `{filePath}`. Please retry.");
                    }
                } while (!File.Exists(filePath));

                Console.WriteLine("Thanks, file exists.");
            }

            return filePath;
        }
    }
}
