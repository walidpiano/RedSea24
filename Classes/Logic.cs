using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Drawing;
using System.IO;

namespace RedSea24.Classes
{
    public static class Logic
    {
        public static string CleanString(string inputString, Int16 type)
        {
            string result = "";
            inputString = inputString.Trim();

            if (type == 0)
            {
                result = inputString.ToUpper();
            }
            else if (type == 1)
            {
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                result = textInfo.ToTitleCase(inputString);
            }
            else if (type == 2)
            {
                result = inputString.ToLower();
            } 
            else
            {
                result = inputString;
            }
            return result;
        }

        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null)
                return null;

            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            if (imageIn == null)
                return null;

            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
