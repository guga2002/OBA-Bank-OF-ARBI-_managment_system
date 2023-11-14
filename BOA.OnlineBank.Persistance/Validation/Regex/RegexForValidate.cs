using System.Text.RegularExpressions;


namespace Bank_Managment_System.Validation.Regexi
{
    public class RegexForValidate
    {
        #region Regexs


        public static bool NameIsMatch(string Name)
        {
            Regex rgx = new Regex(@"^[A-Za-z]{2,50}$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(Name);
        }
        public static bool SurnameIsMatch(string Surname)
        {
            Regex rgx = new Regex(@"^[A-Za-z]{2,50}$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(Surname);
        }
        public static bool CardNumberISmatch(string card)
        {
            Regex rgx = new Regex(@"^[0-9]{10,16}$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(card);
        }
        public static bool IbanIsMatch(string iban)
        {
            Regex rgx = new Regex(@"^[A-Z]{2}[0-9]{2}[A-Z0-9]+$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(iban);
        }
        public static bool PhoneIsMatch(string phone)
        {
            Regex rgx = new Regex(@"^\+995\s?(\(?\d{2}\)?\s?)?\d{7}$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(phone);
        }

        public static bool AddressIsMatch(string Address)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9\s.,-]*$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(Address);
        }
        public static bool EmailIsMatch(string Email)
        {
            Regex rgx = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.IgnoreCase);
            return rgx.IsMatch(Email);
        }
        public static bool IsStrongPassword(string password)
        {
            string pattern = "^.{7,}$";
            return Regex.IsMatch(password, pattern);
        }

        #endregion
    }
}
