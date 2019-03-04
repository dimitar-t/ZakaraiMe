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
        /// <summary>
        /// Resizes the picture to the given 
        /// </summary>
        /// <param name="image">The picture converted in System.Drawing.Image</param>
        /// <param name="width">The new width of the picture</param>
        /// <param name="height">The new height of the picture</param>
        /// <returns></returns>
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

        /// <summary>
        /// Converts the uploaded image to System.Drawing.Image
        /// </summary>
        /// <param name="uploadedImage">The image uploaded from the front end.</param>
        /// <returns></returns>
        public static Image ConvertIFormFileToImage(IFormFile uploadedImage)
        {
            MemoryStream ms = new MemoryStream();
            uploadedImage.OpenReadStream().CopyTo(ms);

            if (!ms.IsImage())
                return null;

            return Image.FromStream(ms);
        }        
    }
}
