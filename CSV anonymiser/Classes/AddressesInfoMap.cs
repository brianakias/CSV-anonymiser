using CsvHelper.Configuration;
using System.Globalization;

namespace CsvAnonymiser.Classes
{
    public class AddressesInfoMap : ClassMap<AddressInfo>
    {
        public AddressesInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
