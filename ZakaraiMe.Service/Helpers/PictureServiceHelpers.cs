namespace ZakaraiMe.Service.Helpers
{
    using Extensions;
    using Microsoft.AspNetCore.Http;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class PictureServiceHelpers
    {
        public static Bitmap ResizePicture(Image image, int width, int height)
        {
            if (image.Width <= width)
                width = image.Width;
            if (image.Height <= height)
                height = image.Height;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image ConvertIFormFileToImage(IFormFile uploadedImage)
        {
            MemoryStream ms = new MemoryStream();
            uploadedImage.OpenReadStream().CopyTo(ms); // TODO: Check if the file is an image: https://stackoverflow.com/questions/670546/determine-if-file-is-an-image/31229958

            if (!ms.IsImage())
                return null;

            return Image.FromStream(ms);
        }        
    }
}
