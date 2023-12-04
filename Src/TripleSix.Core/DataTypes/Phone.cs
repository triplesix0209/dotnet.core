using PhoneNumbers;

namespace TripleSix.Core.DataTypes
{
    public class Phone
    {
        private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();
        private readonly PhoneNumber _phoneNumber;

        public Phone(string number)
            : this(number, "VN")
        {
        }

        public Phone(string number, string defaultRegion)
        {
            _phoneNumber = PhoneUtil.Parse(number, defaultRegion);
        }

        public string E164Number => PhoneUtil.Format(_phoneNumber, PhoneNumberFormat.E164);

        public string NationalNumber => PhoneUtil.Format(_phoneNumber, PhoneNumberFormat.NATIONAL)
            .Replace(" ", string.Empty);

        public int CountryCode => _phoneNumber.CountryCode;

        public static Phone Parse(string number, string defaultRegion = "VN")
        {
            return new Phone(number, defaultRegion);
        }

        public static bool TryParse(out Phone result, string number, string defaultRegion = "VN")
        {
            try
            {
                result = new Phone(number, defaultRegion);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public override string ToString()
        {
            return E164Number;
        }
    }
}