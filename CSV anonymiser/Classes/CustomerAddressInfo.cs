
using Faker;

namespace CSV_anonymiser.Classes
{
    public class CustomerAddressInfo
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

    }
}
