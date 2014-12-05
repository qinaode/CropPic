using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;
namespace CropPic
{
    /// <summary>
    /// 辅助工具类
    /// </summary>
    public static class Utility
    {
        public static string ProcessFolder(string path)
        {
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        public static string StreamToMd5(Stream stream)
        {
            StringBuilder sb = new StringBuilder();
            MD5 md5 = new MD5CryptoServiceProvider();
            var bs = md5.ComputeHash(stream);
            foreach (var b in bs)
            {
                sb.Append(b.ToString("x0"));
            }
            return sb.ToString();
        }
        #region 缩放和剪裁图片方法
        public static Image Resize(Image image, int scaledWidth, int scaledHeight)
        {
            return new Bitmap(image, scaledWidth, scaledHeight);
        }
        public static Image Crop(Image image, int x, int y, int width, int height)
        {
            var croppedBitmap = new Bitmap(width, height);

            using (var g = Graphics.FromImage(croppedBitmap))
            {
                g.DrawImage(image,
                    new Rectangle(0, 0, width, height),
                    new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }
            return croppedBitmap;
        }
        #endregion
    }
}