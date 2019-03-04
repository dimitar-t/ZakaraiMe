namespace ZakaraiMe.Web.Infrastructure.Helpers
{
    using System;
    using System.Linq;

    public static class AuthenticationHelpers
    {
        /// <summary>
        /// Extracts facebook user's first and last names
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static string[] GetNamesFromExternalLogin(string username)
        {
            string[] names = new string[2];
            names = username.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            if (names[1] == string.Empty)
            {
                names[1] = names[0];
            }

            return names;
        }

        /// <summary>
        /// Generates unique username for each user based on his first and last names
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static string GenerateUniqueUsername(string firstName, string lastName)
        {
            DateTime centuryBegin = new DateTime(2001, 1, 1);
            DateTime currentDate = DateTime.Now;

            long elapsedTicks = currentDate.Ticks - centuryBegin.Ticks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            string timeSpanNumbersOnly = string.Join("", elapsedSpan.ToString().Split(new char[] { '.', ':' }));

            string extractedUniqueString = timeSpanNumbersOnly.Substring(0, 10);

            return firstName + lastName + extractedUniqueString;
        }
    }
}
