
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
    }
}
