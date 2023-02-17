
using CsvHelper.Configuration;
using System.Globalization;

namespace CSV_anonymiser.Classes
{
    public class CustomerAddressInfoMap : ClassMap<CustomerAddressInfo>
    {
        public CustomerAddressInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
