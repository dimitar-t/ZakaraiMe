namespace ZakaraiMe.Service.Helpers
{
    using Microsoft.AspNetCore.Http;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    public static class PictureHelpers
    {
        public static Bitmap ResizePicture(IFormFile uploadedImage, int width, int height)
        {
            Image image = ConvertIFormFileToImage(uploadedImage);

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

        private static Image ConvertIFormFileToImage(IFormFile uploadedImage)
        {
            MemoryStream ms = new MemoryStream();
            uploadedImage.OpenReadStream().CopyTo(ms);

            return Image.FromStream(ms);
        }
    }
}
