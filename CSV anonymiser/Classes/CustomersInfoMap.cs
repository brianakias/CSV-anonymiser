
using CsvHelper.Configuration;
using System.Globalization;

namespace CSV_anonymiser.Classes
{
    public class CustomersInfoMap : ClassMap<CustomerInfo>
    {
        public CustomersInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
