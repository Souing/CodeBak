using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;


namespace Com.Yst.Framework.Utils
{
    public class ImageUtility
    {
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式:HW-指定高宽缩放（可能变形）；W-指定宽，高按比例；H-指定高，宽按比例；Cut-指定高宽裁减（不变形） </param> 
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;
            switch (mode.ToUpper())
            {
                case "HW"://指定高宽缩放（可能变形） 
                    break;
                case "W"://指定宽，高按比例 
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "CUT"://指定高宽裁减（不变形） 
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                case "AUTO":
                    if (ow < oh)
                    {
                        toheight = originalImage.Height * width / originalImage.Width;                        
                    }
                    else
                    {
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);
            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);
            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            g.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);
            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(thumbnailPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }


        /// <summary>
        /// 上传头像（以Post方式获取数据）
        /// </summary>
        /// <remarks>
        /// 将图片处理为正方型，然后等比缩放
        /// </remarks>
        /// <param name="postedFile">原图HttpPostedFile对象</param>
        /// <param name="fileSaveUrl">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int side, int quality)
        {
            //创建目录
            string dir = Path.GetDirectoryName(thumbnailPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            System.Drawing.Image initImage = System.Drawing.Image.FromFile(originalImagePath, true);

            //原始图片的宽、高
            int initWidth = initImage.Width, initHeight = initImage.Height;

            //非正方型先裁剪为正方型
            if (initWidth != initHeight)
            {
                //截图对象
                System.Drawing.Image pickedImage = null;
                System.Drawing.Graphics pickedG = null;

                //宽大于高的横图
                if (initWidth > initHeight)
                {
                    //对象实例化
                    pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                    pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //定位
                    Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                    Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                    //画图
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                    //重置宽
                    initWidth = initHeight;
                }
                //高大于宽的竖图
                else
                {
                    //对象实例化
                    pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                    pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //定位
                    Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                    Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                    //画图
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                    //重置高
                    initHeight = initWidth;
                }

                //将截图对象赋给原图
                initImage = (System.Drawing.Image)pickedImage.Clone();
                //释放截图资源
                pickedG.Dispose();
                pickedImage.Dispose();
            }

            //缩略图对象
            System.Drawing.Image resultImage = new System.Drawing.Bitmap(side, side);
            System.Drawing.Graphics resultG = System.Drawing.Graphics.FromImage(resultImage);
            //设置质量
            resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //用指定背景色清空画布
            resultG.Clear(Color.White);
            //绘制缩略图
            resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);

            //关键质量控制
            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();//获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo i in icis)
            {
                if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                {
                    ici = i;
                }
            }
            EncoderParameters ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

            //保存缩略图
            resultImage.Save(thumbnailPath, ici, ep);

            //释放关键质量控制所用资源
            ep.Dispose();

            //释放缩略图资源
            resultG.Dispose();
            resultImage.Dispose();

            //释放原始图片资源
            initImage.Dispose();
        }

        /// <summary>
        /// 给图片打分
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="fileSize">图片大小</param>
        /// <param name="alt">图片说明</param>
        /// <returns></returns>
        public static decimal GetImageScore(int width, int height, long fileSize, string alt)
        {
            if (width * height == 0)
            {
                return 0;
            }

            double score = 1 / (Math.Abs(width - 600) / 600 + 0.01) + 1 / (Math.Abs(height - 600) / 600 + 0.01) + (double)fileSize / (width * height) +(string.IsNullOrEmpty(alt) ? 0 : alt.Length);
            return (decimal)score;                        
        }
    }
}
