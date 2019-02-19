namespace ZakaraiMe.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class AuthenticationHelpers
    {
        public static string[] GetNamesFromExternalLogin(string username) // Extract the person's names from his username
        {
            string[] names = new string[2];
            names = username.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            if(names[1] == string.Empty)
            {
                names[1] = names[0];
            }

            return names;
        }

        public static string GenerateUniqueUsername(string firstName, string lastName)
        {
            DateTime centuryBegin = new DateTime(2001, 1, 1);
            DateTime currentDate = DateTime.Now;

            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            string timeSpanNumbersOnly = string.Join("", elapsedSpan.ToString().Split(new char[] {'.', ':'}));

            string extractedUniqueString = timeSpanNumbersOnly.Substring(0, 10);

            return firstName + lastName + extractedUniqueString;
        }
    }
}
