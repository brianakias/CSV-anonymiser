namespace CsvAnonymiser.Classes
{
    public static class CommunicateWithUser
    {
        public static string RequestFilePath()
        {
            string filePath;

            do
            {
                Console.WriteLine("Provide the path of the file to read from: ");
                filePath = Console.ReadLine();

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Could not locate file with path `{filePath}`. Please retry.");
                }

            } while (!File.Exists(filePath));

            Console.WriteLine("\nThanks, file exists.");
            Console.WriteLine(new string('*', Console.WindowWidth - 2));

            return filePath;
        }

        public static void ProvideOutputFileName(string fileName)
        {
            Console.WriteLine(fileName);
            Console.WriteLine(new string('*', Console.WindowWidth - 2));
        }
    }
}
