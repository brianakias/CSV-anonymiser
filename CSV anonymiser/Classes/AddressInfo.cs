
using Faker;

namespace CsvAnonymiser.Classes
{
    public class AddressInfo
    {
        public string email { get; set; }
        public string customerId { get; set; }
        public string addressId { get; set; }
        public string isDefaultBillingAddress { get; set; }
        public string isDefaultShippingAddress { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string stateCode { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string telephone { get; set; }

        public void Anonymise(CustomerInfo optionalCustomerInfo = null)
        {
            bool customerProvided = optionalCustomerInfo != null;

            if (customerProvided)
            {
                string customersFirstName = optionalCustomerInfo.firstname;
                string customersLastName = optionalCustomerInfo.lastname;
                string customersEmail = optionalCustomerInfo.email;

                firstname = customersFirstName;
                lastname = customersLastName;
                email = customersEmail;
            }

            else
            {
                firstname = Name.First();
                lastname = Name.Last();
                email = $"{firstname.ToLower()}.{lastname.ToLower()}@escentual.com";
            }

            street = Address.StreetAddress();
            city = Address.City();
            state = Address.UkCounty();
            postalCode = Address.UkPostCode().ToUpper();
            telephone = Phone.Number();
        }
    }
}
