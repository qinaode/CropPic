using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
namespace CropPic.ashx
{
    /// <summary>
    /// crop 的摘要说明
    /// </summary>
    public class crop : IHttpHandler
    {
        private static readonly JavaScriptSerializer jss = new JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            //参数接收
            context.Response.ContentType = "text/plain";
            string imgUrl = context.Request["imgUrl"];
            int imgW = (int)float.Parse(context.Request["imgW"]);
            int imgH = (int)float.Parse(context.Request["imgH"]);
            int imgY1 = (int)float.Parse(context.Request["imgY1"]);
            int imgX1 = (int)float.Parse(context.Request["imgX1"]);
            int cropW = (int)float.Parse(context.Request["cropW"]);
            int cropH = (int)float.Parse(context.Request["cropH"]);
            //1.读取原图片
            string path = context.Server.MapPath(imgUrl);
            Image oldimage = Image.FromFile(path);
            //2.缩放图片
           Image resizeimage = Utility.Resize(oldimage, imgW, imgH);
            //3.根据位置在缩放后的图片上裁剪
           var cropImage = Utility.Crop(resizeimage, imgX1, imgY1, cropW, cropH);
            //4.保存裁剪后的图片
           using (MemoryStream stream = new MemoryStream())
           {
               cropImage.Save(stream, ImageFormat.Jpeg);
               string fileName = Utility.StreamToMd5(stream) + Path.GetExtension(path);
               cropImage.Save(context.Server.MapPath("\\upload\\" + fileName));
               context.Response.Write(jss.Serialize(new { status = "success", url = "\\upload\\" + fileName }));
           }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}