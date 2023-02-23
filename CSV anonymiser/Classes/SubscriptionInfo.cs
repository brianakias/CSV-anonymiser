using CsvAnonymiser.Classes;
using Faker;

namespace CSV_anonymiser.Classes
{
    public class SubscriptionInfo
    {
        public string customer_id { get; set; }
        public string subscriber_email { get; set; }

        public void Anonymise(CustomerInfo optionalCustomerInfo = null)
        {
            bool customerProvided = optionalCustomerInfo != null;

            if (customerProvided)
            {
                string customersEmail = optionalCustomerInfo.email;

                subscriber_email = customersEmail;
            }

            else
            {
                subscriber_email = Internet.Email();
            }
        }
    }
}
