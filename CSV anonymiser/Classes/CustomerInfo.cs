
using Faker;

namespace CSV_anonymiser.Classes
{
    public class CustomerInfo
    {
        public string entity_id { get; set; }
        public string entity_type_id { get; set; }
        public string attribute_set_id { get; set; }
        public string website_id { get; set; }
        public string email { get; set; }
        public string group_id { get; set; }
        public string increment_id { get; set; }
        public string store_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string is_active { get; set; }
        public string disable_auto_group_change { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string confirmation { get; set; }
        public string created_in { get; set; }
        public string default_billing { get; set; }
        public string default_shipping { get; set; }
        public string dob { get; set; }
        public string ekashu_no_frames { get; set; }
        public string ekashu_skip_3ds { get; set; }
        public string email_prefs { get; set; }
        public string gender { get; set; }
        public string intershop_sync_date { get; set; }
        public string is_disabled { get; set; }
        public string middlename { get; set; }
        public string notes { get; set; }
        public string prefix { get; set; }
        public string rewardsref_notify_on_referral { get; set; }
        public string rewards_points_notification { get; set; }
        public string suffix { get; set; }
        public string taxvat { get; set; }
        public string password_hash { get; set; }

        /// <summary>
        /// Replaces values from gdpr related properties. Replaces with values relevant to what the properties represent.
        /// </summary>
        public void Anonymise()
        {
            firstname = Name.First();
            middlename = Name.Middle();
            lastname = Name.Last();
            email = $"{firstname.ToLower()}.{lastname.ToLower()}@escentual.com";
            dob = Identification.DateOfBirth().ToShortDateString();
            gender = MyUtilities.GenerateRandomGender();
        }
    }
}
