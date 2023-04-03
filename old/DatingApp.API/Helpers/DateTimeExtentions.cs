using System;

namespace DatingApp.API.Helpers
{
    public static class DateTimeExtentions
    {
        public static int CaculateAge(this DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if(dateOfBirth.AddYears(age) > DateTime.Today)
            {
                age--;
            }
            return age;
        }
    }
}