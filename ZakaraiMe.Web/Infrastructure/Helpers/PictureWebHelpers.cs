namespace ZakaraiMe.Web.Infrastructure.Helpers
{
    using System;
    using System.Net;

    public static class PictureWebHelpers
    {
        /// <summary>
        /// Converts picture from its URI to byte array
        /// </summary>
        /// <param name="requestUri">URI of the picture</param>
        /// <returns></returns>
        public static byte[] DownloadPicture(string requestUri)
        {
            WebClient client = new WebClient();
            byte[] imageInBytes = client.DownloadData(new Uri(requestUri));

            return imageInBytes;
        }
    }
}
