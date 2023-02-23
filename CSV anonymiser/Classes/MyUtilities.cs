
using System.Text;
using System.Security.Cryptography;

namespace CsvAnonymiser.Classes
{
    public static class MyUtilities
    {
        public static string GenerateRandomGender()
        {
            int randomNumber = new Random().Next(2);
            string gender;

            if (randomNumber == 0)
            {
                gender = "male";
            }

            else
            {
                gender = "female";
            }

            return gender;
        }

        public static string GenerateRandomPasswordHash(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=";
            int charsLength = chars.Length;
            Random random = new Random();
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomCharsIndex = random.Next(1, charsLength);
                result.Append(chars[randomCharsIndex]);
            }

            return result.ToString();
        }
    }
}
