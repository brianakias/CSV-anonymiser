using CSV_anonymiser.Classes;

namespace CSV_anonymiser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("-- Customers file --");
            //string customersFilePath_Input = CommunicateWithUser.RequestFilePath("input");
            //string customersFilePath_Output = CommunicateWithUser.RequestFilePath("output");
            //Console.WriteLine("-- Addresses file --");
            //string addressesFilePath_Input = CommunicateWithUser.RequestFilePath("input");
            //string addressesFilePath_Output = CommunicateWithUser.RequestFilePath("output");

            //ProcessCustomerInfoCSVFile customerInfoAnonymiser = new ProcessCustomerInfoCSVFile();
            //List<CustomerInfo> customerInfoRecords = customerInfoAnonymiser.GetCSVRecords(customersFilePath_Input);
            //foreach (CustomerInfo customerInfo in customerInfoRecords)
            //{
            //    customerInfo.ReplaceSensitiveInfoWithFakeData();
            //}5

            //Console.WriteLine(Environment.CurrentDirectory);
            ProcessCustomerInfoCSVFile processor = new ProcessCustomerInfoCSVFile();


            // https://youtu.be/fRaSeLYYrcQ?t=145 
            // var csvPath = Path.Combine(Environment.CurrentDirectory, $"rockets-{DateTime.Now.ToFi1eTime()}.csv");
            // using the above you can create a file
            // test using this instead of asking for the user to pass the csv file.
        }

    }


}