namespace ZakaraiMe.Data.Helpers
{
    /// <summary>
    /// Helper class for pictures
    /// </summary>
    public static class PictureDataHelpers
    {
        private const string ImagesFolder = @"\images\database\";
        private const string FormatConstant = ".jpg";

        /// <summary>
        /// Generates a relative path of a picture.
        /// </summary>
        /// <param name="wwwRootPath">The wwwRoot folder path of the application.</param>
        /// <param name="imageName">The name of the image.</param>       
        /// <returns>relative path of the picture</returns>
        public static string GeneratePictureFilePath(string wwwRootPath, string imageName)
        {
            return wwwRootPath + ImagesFolder + imageName + FormatConstant;
        }
    }
}
