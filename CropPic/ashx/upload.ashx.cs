using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Drawing;
using System.IO;
namespace CropPic.ashx
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {
        public static readonly JavaScriptSerializer jss = new JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plan";//必须使用纯文本输出格式，不然前台转换会失败。(前端控件的bug没有判断返回数据类型，直接转换导致)
            var file = context.Request.Files["img"];
            var ext = Path.GetExtension(file.FileName);
            //根据文件流计算文件MD5值，以此为图片名
            string fileName = Utility.StreamToMd5(file.InputStream) + ext;
            file.SaveAs(Utility.ProcessFolder(context.Server.MapPath("\\upload")) + "\\" +fileName);
            //如果图片本身宽度和高度没有剪裁区域大，前台剪裁的时候会有问题
            Image image = System.Drawing.Image.FromStream(file.InputStream);
            context.Response.Write(jss.Serialize(new { status = "success", url = "\\upload\\" + fileName, width = image.Width, height = image.Height }));
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