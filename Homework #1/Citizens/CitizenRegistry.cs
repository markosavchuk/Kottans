using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Citizens
{
    public class CitizenRegistry : ICitizenRegistry
    {
        private readonly DateTime _startDate = DateTime.Parse("31.12.1899");

        private int _registeredMan = 0;
        private int _registeredWoman = 0;
       
        private ICitizen[] _registartions = new ICitizen[100];
        private DateTime _lastRegistration;

        public void Register(ICitizen citizen)
        {
            if (!String.IsNullOrEmpty(citizen.VatId))
                throw new InvalidOperationException();            

            int days = (int)(citizen.BirthDate - _startDate).TotalDays;

            switch (citizen.Gender)
            {
                case Gender.Male:
                    _registeredMan++;
                    break;
                case Gender.Female:
                    _registeredWoman++;
                    break;
            }

            string numberStr = days.ToString("D5");
            numberStr += citizen.Gender == Gender.Male
                ? (_registeredMan/5).ToString("D3") + (_registeredMan%5).ToString()
                : (_registeredWoman/5).ToString("D3") + (_registeredWoman%5+1).ToString();
            numberStr += Checksum(numberStr).ToString();
            citizen.VatId = numberStr;

            _registartions[_registeredMan + _registeredWoman - 1] = 
                new Citizen(citizen.FirstName, citizen.LastName, citizen.BirthDate, citizen.Gender, citizen.VatId);

            _lastRegistration = SystemDateTime.Now.Invoke();
        }

        public ICitizen this[string id]
        {
            get
            {
                if (String.IsNullOrEmpty(id))
                    throw new ArgumentNullException();

                for (int i = 0; i < _registeredMan + _registeredWoman; i++)
                {
                    if (_registartions[i].VatId == id) return _registartions[i];
                }
                return null;
            }
        }

        public string Stats()
        {
            string stats = String.Format("{0} men and {1} women", _registeredMan, _registeredWoman);
            if (_registeredWoman+_registeredMan>0)
                stats += String.Format(". Last registration was {0}", _lastRegistration.ToFriendlyDateString());
            return stats;
        }

        private int Checksum(string numberStr)
        {
            int number = Int32.Parse(numberStr);

            int[] args = { -1, 5, 7, 9, 4, 6, 10, 5, 7};
            int length = args.Length;

            int x = 0;

            for (int i = 0; number > 0; number /= 10, i++)
            {
                x += number%10 * args[length-i-1];
            }

            return x%11%10;
        }
    }

    public static class DateTimeExtensions
    {
        public static string ToFriendlyDateString(this DateTime Date)
        {
            string FormattedDate = "";
            if (Date.Date == SystemDateTime.Now.Invoke().Date)
            {
                FormattedDate = "today";
            }
            else if (Date.Date == SystemDateTime.Now.Invoke().AddDays(-1))
            {
                FormattedDate = "yesterday";
            }
            else if (Date.Date > SystemDateTime.Now.Invoke().AddDays(-6))
            {
                // *** Show the Day of the week
                FormattedDate = Date.ToString("dddd").ToString();
            }
            else
            {
                FormattedDate = Date.ToString("MMMM dd, yyyy");
            }

            return FormattedDate;
        }
    }
}
