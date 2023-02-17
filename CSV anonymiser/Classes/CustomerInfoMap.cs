
using CsvHelper.Configuration;
using System.Globalization;

namespace CSV_anonymiser.Classes
{
    public class CustomerInfoMap : ClassMap<CustomerInfo>
    {
        public CustomerInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
