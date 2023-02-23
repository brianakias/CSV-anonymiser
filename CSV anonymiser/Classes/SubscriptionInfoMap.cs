using CsvHelper.Configuration;
using System.Globalization;

namespace CSV_anonymiser.Classes
{
    public class SubscriptionInfoMap : ClassMap<SubscriptionInfo>
    {
        public SubscriptionInfoMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
        }
    }
}
