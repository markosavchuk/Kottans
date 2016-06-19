using System;

namespace Citizens.Tests.Helpers
{
    internal sealed class CitizenBuilder
    {
        private Gender _gender;
        private string _firstName;
        private string _lastName;
        private string _vatId;
        private DateTime _dateOfBirth;


        private CitizenBuilder(string firstName, string lastName, Gender gender)
        {
            this._firstName = firstName;
            this._lastName = lastName;
            this._gender = gender;
        }

        public static CitizenBuilder NewMan()
        {
            var builder = new CitizenBuilder("Roger", "Pierce", Gender.Male);
            return builder;
        }

        public static CitizenBuilder NewWoman()
        {
            var builder = new CitizenBuilder("Anne", "Jensen", Gender.Female);
            return builder;
        }

        public CitizenBuilder WithDate(DateTime birthDate)
        {
            _dateOfBirth = birthDate;
            return this;
        }

        public CitizenBuilder WithVatId(string vatId)
        {
            this._vatId = vatId;
            return this;
        }

        public ICitizen Build()
        {
            var citizen = new Citizen(_firstName, _lastName, _dateOfBirth, _gender);
            citizen.VatId = _vatId;
            return citizen;
        }
    }
}
