namespace ZakaraiMe.Web.Infrastructure.Helpers
{
    using System;
    using System.Net;

    public static class PictureWebHelpers
    {
        public static byte[] DownloadPicture(string requestUri)
        {
            WebClient client = new WebClient();
            byte[] imageInBytes = client.DownloadData(new Uri(requestUri));

            return imageInBytes;
        }
    }
}
