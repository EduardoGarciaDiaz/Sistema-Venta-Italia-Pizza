using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ItaliaPizza_Cliente.Utilidades
{
    public static class ConvertidorBytes
    {
        public static BitmapImage ConvertirBytesABitmapImage(Byte[] foto)
        {
            BitmapImage bitmapImage = new BitmapImage();

            try
            {
                using (MemoryStream stream = new MemoryStream(foto))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.DecodePixelWidth = 100;
                    bitmapImage.EndInit();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (OutOfMemoryException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return bitmapImage;
        }
    }
}
