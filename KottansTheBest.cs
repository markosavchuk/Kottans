using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    class KottansTheBest
    {
        public enum CreditCardVendor
        {
            AmericanExpress,
            Maestro,
            MasterCard,
            VISA,
            JCB,
            Unknown
        }

        class Vendor
        {
            public CreditCardVendor NameVendor;
            public List<Tuple<string, string>> IinRanges = new List<Tuple<string, string>>();
            public List<int> AvailableLength = new List<int>(); 
        }

        public CreditCardVendor GetCreditCardVendor(string creditCard)
        {
            var vendors = new List<Vendor>()
            {
                new Vendor()
                {
                    NameVendor = CreditCardVendor.AmericanExpress,
                    AvailableLength = new List<int>() {15},
                    IinRanges = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("34", "34"),
                        new Tuple<string, string>("37", "37")
                    }
                },
                new Vendor()
                {
                    NameVendor = CreditCardVendor.Maestro,
                    AvailableLength = new List<int>() {12,13,14,15,16,17,18,19},
                    IinRanges = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("50", "50"),
                        new Tuple<string, string>("56", "69")
                    }
                },
                new Vendor()
                {
                    NameVendor = CreditCardVendor.MasterCard,
                    AvailableLength = new List<int>() {16},
                    IinRanges = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("51", "55")
                    }
                },
                new Vendor()
                {
                    NameVendor = CreditCardVendor.VISA,
                    AvailableLength = new List<int>() {13,16,19},
                    IinRanges = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("4", "4")
                    }
                },
                new Vendor()
                {
                    NameVendor = CreditCardVendor.JCB,
                    AvailableLength = new List<int>() {16},
                    IinRanges = new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>("3528", "3589")
                    }
                },
            };

            var normilizeCreditCard = creditCard.Replace(" ", "");

            if (!IsCreditCardNumberValid(normilizeCreditCard))
                return CreditCardVendor.Unknown;

            foreach (var vendor in vendors)
            {
                foreach (var number in vendor.IinRanges)
                {
                    var iinCard = normilizeCreditCard.Substring(0, number.Item2.Length);
                    if (int.Parse(iinCard)>=int.Parse(number.Item1) 
                        && int.Parse(iinCard)<=int.Parse(number.Item2))
                        return 
                            vendor.AvailableLength.Contains(normilizeCreditCard.Length)
                            ? vendor.NameVendor
                            : CreditCardVendor.Unknown;
                }
            }

            return CreditCardVendor.Unknown;
        }

        public bool IsCreditCardNumberValid(string creditCard)
        {
            var normilizeCreditCard = creditCard.Replace(" ", "");
            int sum = 0;
            for (int i = 0; i < normilizeCreditCard.Length; i++)
            {
                var digit = normilizeCreditCard[i];
                int k = int.Parse(digit.ToString());

                if ((i%2 != 0 && normilizeCreditCard.Length%2==0) 
                    || (i%2 == 0 && normilizeCreditCard.Length%2 != 0))
                {
                    sum += k;
                    continue;
                }

                if (k*2 > 9) sum += k*2 - 9;
                    else sum += k*2;
            }
            if (sum%10 == 0) return true;
                else return false;
        }

        /// <summary>
        /// return next valid credit card according to Luhn algorithm or invalid credit card number
        /// </summary>
        public string GenerateNextCreditCardNumber(string creditCard)
        {
            var normilizeCreditCard = creditCard.Replace(" ", "");
            do
            {
                UInt64 number;
                var succeed = UInt64.TryParse(normilizeCreditCard, out number);
                if (succeed)
                    normilizeCreditCard = (++number).ToString();
                else
                    return "invalid credit card number";
            } while (!IsCreditCardNumberValid(normilizeCreditCard));
            if (GetCreditCardVendor(creditCard) == GetCreditCardVendor(normilizeCreditCard))
                return normilizeCreditCard;
            else
                return
                    "No more card numbers available for this vendor in this range or invalid credit card number.";
        }
    }
}
