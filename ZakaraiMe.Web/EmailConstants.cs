namespace ZakaraiMe.Web
{
    public static class EmailConstants
    {
        public const string JoinSubject = "Нов пътник във Ваше пътуване!";
        public const string JoinBody = "Имате нов пътник във Вашето пътуване на {0} в {1} и остават {2}/{3} свободни места.{4}";

        public const string LeaveSubject = "Отказал се пътник във Ваше пътуване!";
        public const string LeaveBody = "Един от пътниците във Вашето пътуване на {0} в {1} се е отказал от него и остават {2}/{3} свободни места.{4}";

        public const string RemoveSubject = "Отменено пътуване";
        public const string RemoveBody = "Ваше пътуване на {0} в {1} беше отменено от шофьора. Съжаляваме за неудобството. Заповядайте в сайта ни, за да намерите ново, подходящо за Вас, пътуване.";

        public const string Regards = "\n\nИскрено Ваши,\nЕкипът на ZakaraiMe";
    }
}
