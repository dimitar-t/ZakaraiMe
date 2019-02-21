namespace ZakaraiMe.Web.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message, params string[] args)
        {
            tempData[WebConstants.TempDataSuccessMessageKey] = string.Format(message, args);
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message, params string[] args)
        {
            tempData[WebConstants.TempDataErrorMessageKey] = string.Format(message, args);
        }

        public static void AddWarningMessage(this ITempDataDictionary tempData, string message, params string [] args)
        {
            tempData[WebConstants.TempDataWarningMessageKey] = string.Format(message, args);
        }
    }
}
