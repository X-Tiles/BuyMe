using System.IO;
using System.Windows.Media.Imaging;

namespace eBuyListApplication.Model
{
    public class Utilities
    {
        public static BitmapImage ByteArraytoBitmapImage(byte[] byteArray)
        {
            var stream = new MemoryStream(byteArray);
            var bitmapImage = new BitmapImage();
            bitmapImage.SetSource(stream);
            return bitmapImage;
        }
    }
}
