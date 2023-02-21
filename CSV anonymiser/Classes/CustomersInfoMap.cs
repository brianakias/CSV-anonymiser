
using CsvHelper.Configuration;
using System.Globalization;

namespace CsvAnonymiser.Classes
{
    public class CustomersInfoMap : ClassMap<CustomerInfo>
    {
        public CustomersInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
