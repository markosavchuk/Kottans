using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citizens
{
    public class Citizen : ICitizen
    {
        public string FirstName { get; }
        public string LastName { get; }
        public Gender Gender { get; }
        public DateTime BirthDate { get; }
        public string VatId { get; set; }

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender)
        {        
            FirstName = NormilizeName(firstName);
            LastName = NormilizeName(lastName);

            CheckDate(dateOfBirth);
            BirthDate = dateOfBirth.Date;
           
            CheckGender(gender);
            Gender = gender;
        }

        public Citizen(string firstName, string lastName, DateTime dateOfBirth, Gender gender, string id)
            :this(firstName, lastName, dateOfBirth, gender)
        {
            VatId = id;
        }

        private void CheckDate(DateTime dt)
        {
            if (dt.CompareTo(SystemDateTime.Now.Invoke()) > 0)
                throw new ArgumentException();
        }

        private void CheckGender(Gender gender)
        {
            if ((int)gender >= Enum.GetNames(typeof(Gender)).Length)
                throw new ArgumentOutOfRangeException();
        }

        private string NormilizeName(string name)
        {
            name = name.ToLower();
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            //according to this sourse http://www.dotnetperls.com/uppercase-first-letter 
            //this approach is most effective
            char[] a = name.ToCharArray();
            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }
    }
}
