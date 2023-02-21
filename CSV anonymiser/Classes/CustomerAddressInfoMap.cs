
using CsvHelper.Configuration;
using System.Globalization;

namespace CsvAnonymiser.Classes
{
    public class CustomerAddressInfoMap : ClassMap<CustomerAddressInfo>
    {
        public CustomerAddressInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
