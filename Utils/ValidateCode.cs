/****************************************************************************************************
 * *
 * *        文件名    : ValidateCode.cs
 * *        功能说明  : 验证码与验证码配置类.
 * * 					验证码ValidateCode类用于生成验证码.						
 * * 					配置类ValidateConfig类在使用配置文件时使用,用于设置验证码的参数 * 
 * *					注:ValidateConfig类在此文件中.此文件包含两个类. *						
 * * 
 * *        备注      :
 * *        使用举例  :		ValidateCodeImg.aspx.cs(图像页):(在Load方法中加入以下代码即可)
 * 							ValidateCode.Instance.Init();//或者使用指定的配置文件 ValidateCode.Instance.Init("http://resource.elong.com/cn/config/validate.config")
 * *        				string strKey = string.Empty;
 * * 						byte[] data = vc.GenerateVerifyImage(ref strKey);
 * * 						Session["validateCode"] = strKey.ToLower().Trim();//注:此处validateCode的键值应该与下面CheckValidateCode中的string sessionKey统一.
 * * 						HttpContext.Current.Response.ClearContent();
 * * 						HttpContext.Current.Response.ContentType = "image/Jpeg";
 * * 						HttpContext.Current.Response.End();
 *							
 *							页面验证:(用于Client端,即具体的aspx页面中)
 *								ValidateCode.Instance.CheckValidateCode(this.Page, "validateCode", txtValidateCodeInput.Text.Trim())
 *							参数说明:CheckValidateCode(Page e, string sessionKey, string inputCode)
 *								第一个是使用验证码的页的Page对象.使用this.Page即可.
 *								第二个是验证码的Session键值.需要与ValidateCodeImg.aspx(图像页)中写入的Session键值相对应.以便完成验证.
 *								第三个是用户输入的验证码值.
 *							注:可以使用默认sessionKey为"validateCode"的重载函数.
 * 
 *							显示验证码:(用于Client端,即具体的aspx页面中)
 *							<img id="validateImg" style="CURSOR: pointer" src="UserControls/ValidateToken.aspx"  onclick="this.setAttribute('src','UserControls/ValidateToken.aspx?timeStamp=' + new Date().getTime())" alt="单击刷新图片" align="absBottom">
 * 
 *			页面验证脚本更新:新版验证码包含字母和数字.如果有需要在浏览器端使用javascript检测用户输入,请使用以下javascript脚本:
 *								//token_value为用户输入验证码控件的value值 
 *								var validateCodeRegex = new RegExp("[a-zA-Z0-9]{4}","i");
 *								if ( token_value == null || token_value == "" || !validateCodeRegex.test(token_value))
 *								{		
 *									document.getElementById("errToken").style.display = "";	
 *									document.getElementById("LoginTravel_tbSinaToken").value = "";
 *									document.getElementById("LoginTravel_tbSinaToken").focus();
 *									return false;
 *								}		
 *							
 * *
 * *  
 * ****************************************************************************************************/


using System;
using System.Collections;
using System.Drawing.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 验证码生成类.
    /// </summary>
    [System.Serializable]
    public class ValidateCode
    {

        #region ==================== 属性和私有字段====================
        //Singleton模式实例
        private static ValidateCode instance = new ValidateCode();
        public static ValidateCode Instance
        {
            get { return instance; }
        }

        //背景相关
        #region isBackWaveRandom 背景的波纹是否使用随机色

        /// <summary>
        /// 背景的波纹是否使用随机色
        /// </summary>
        private bool isBackWaveRandom;
        /// <summary>
        /// 背景的波纹是否使用随机色
        /// </summary>
        public bool IsBackWaveRandom
        {
            get { return isBackWaveRandom; }
            set { isBackWaveRandom = value; }
        }

        #endregion

        //字符相关
        #region nLen 随机字符长度
        /// <summary>
        /// 随机字符长度
        /// </summary>
        private int nLen;
        /// <summary>
        /// 随机字符长度
        /// </summary>
        public int NLen
        {
            get { return nLen; }
            set { nLen = value; }
        }
        #endregion

        #region isCharRandom 是否每个文字使用不同的随机色
        /// <summary>
        /// 是否每个文字使用不同的随机色
        /// </summary>
        private bool isCharRandom;
        /// <summary>
        /// 是否每个文字使用不同的随机色
        /// </summary>
        public bool IsCharRandom
        {
            get { return isCharRandom; }
            set { isCharRandom = value; }
        }
        #endregion

        #region isUseEnglishChar 是否使用英文字符
        /// <summary>
        /// 是否使用英文字符
        /// </summary>
        private bool isUseEnglishChar;
        /// <summary>
        /// 是否使用英文字符
        /// </summary>
        public bool IsUseEnglishChar
        {
            get { return isUseEnglishChar; }
            set { isUseEnglishChar = value; }
        }
        #endregion

        #region isUseRandomFont 是否使用随机字体
        /// <summary>
        /// 是否使用随机字体
        /// </summary>
        private bool isUseRandomFont;
        /// <summary>
        /// 是否使用随机字体
        /// </summary>
        public bool IsUseRandomFont
        {
            get { return isUseRandomFont; }
            set { isUseRandomFont = value; }
        }
        #endregion

        //干扰线相关
        #region isUseRandomLine 是否使用干扰线
        /// <summary>
        /// 是否使用干扰线
        /// </summary>
        private bool isUseRandomLine;
        /// <summary>
        /// 是否使用干扰线
        /// </summary>
        public bool IsUseRandomLine
        {
            get { return isUseRandomLine; }
            set { isUseRandomLine = value; }
        }
        #endregion

        #region isUseRandomLineColor 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// <summary>
        /// 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// </summary>
        private bool isUseRandomLineColor;
        /// <summary>
        /// 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// </summary>
        public bool IsUseRandomLineColor
        {
            get { return isUseRandomLineColor; }
            set { isUseRandomLineColor = value; }
        }
        #endregion

        #region nLines 干扰线条的个数
        /// <summary>
        /// 干扰线条的个数
        /// </summary>
        private int nLines;
        /// <summary>
        /// 干扰线条的个数
        /// </summary>
        public int NLines
        {
            get { return nLines; }
            set { nLines = value; }
        }
        #endregion

        #region nLinesWidth 干扰线条宽度
        /// <summary>
        /// 干扰线条宽度
        /// </summary>
        private int nLinesWidth;
        /// <summary>
        /// 干扰线宽度
        /// </summary>
        public int NLinesWidth
        {
            get { return nLinesWidth; }
            set { nLinesWidth = value; }
        }
        #endregion

        //干扰点相关
        #region isUseRandomPoint 是否使用干扰点
        /// <summary>
        /// 是否使用干扰点
        /// </summary>
        private bool isUseRandomPoint;
        /// <summary>
        /// 是否使用干扰点
        /// </summary>
        public bool IsUseRandomPoint
        {
            get { return isUseRandomPoint; }
            set { isUseRandomPoint = value; }
        }
        #endregion

        #region isUseRandomPointColor 干扰点颜色是否随机
        /// <summary>
        /// 干扰点颜色是否随机
        /// </summary>
        private bool isUseRandomPointColor;
        /// <summary>
        /// 干扰点颜色是否随机
        /// </summary>
        public bool IsUseRandomPointColor
        {
            get { return isUseRandomPointColor; }
            set { isUseRandomPointColor = value; }
        }
        #endregion

        #region pointNum 干扰点数目
        /// <summary>
        /// 干扰点数目
        /// </summary>
        private int pointNum;
        /// <summary>
        /// 干扰点数目
        /// </summary>
        public int PointNum
        {
            get { return pointNum; }
            set { pointNum = value; }
        }
        #endregion

        //配置相关
        #region isUseConfigFile 是否使用配置文件.默认为 false 不使用.
        /// <summary>
        /// 是否使用配置文件.默认为 false 不使用.
        /// </summary>
        private bool isUseConfigFile = false;
        /// <summary>
        /// 是否使用配置文件.默认为 false 不使用.
        /// </summary>
        public bool IsUseConfigFile
        {
            get { return isUseConfigFile; }
            set { isUseConfigFile = value; }
        }
        #endregion

        #region 非属性用私有字段
        /// <summary>
        /// 随机数对象
        /// </summary>
        private System.Random rd;

        /// <summary>
        /// 当前类的配置信息实例
        /// </summary>
        private ValidateConfig vConfig;

        /// <summary>
        /// 配置文件路径
        /// </summary>
        private string filePath;
        #endregion

        #endregion


        #region ==================== 构造函数和初始化函数 ====================
        /// <summary>
        ///私有构造函数
        /// </summary>
        private ValidateCode()
        {
        }

        /// <summary>
        /// 初始化函数:使用默认的配置信息初始化验证码.
        /// </summary>
        public void Init()
        {
            rd = new Random((int)System.DateTime.Now.Ticks);//随机数对象
            vConfig = InitDefaultConfig();
            SetConfigInfo(vConfig);
        }

        /// <summary>
        /// 初始化函数:使用指定路径的配置文件配置验证码.读取顺序为:缓存->物理配置文件->默认值
        /// </summary>
        /// <param name="configFilePath">配置文件路径</param>
        public void Init(string configFilePath)
        {
            rd = new Random((int)System.DateTime.Now.Ticks);//随机数对象
            if (configFilePath != null && configFilePath.Trim() != string.Empty)
            {
                vConfig = GetValidateCodeConfig(configFilePath);
            }
            else
            {
                vConfig = InitDefaultConfig();
            }
            SetConfigInfo(vConfig);
        }
        #endregion


        #region ==================== 验证码生成函数====================
        /// <summary>
        /// 生成图片验证码
        /// </summary>
        /// <param name="strKey">输出参数，验证码的内容</param>
        /// <returns>图片字节流</returns>
        public byte[] GenerateVerifyImage(ref string strKey)
        {
            //建立图像宽度和高度
            int nBmpWidth = 14 * nLen + 7;//每个字符13像素,这里为每个字符预留了1px移位空间.
            int nBmpHeight = 25;

            //建立绘制需要的 Graphics 和bmp对象
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(nBmpWidth, nBmpHeight);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp);


            #region ====================生成随机背景颜色 此颜色为统一色调颜色====================

            int nRed = 0, nGreen = 0, nBlue = 0;  // 背景的三元色
            GetRandomColor(ref nRed, ref nGreen, ref nBlue);

            #endregion


            #region ====================填充位图背景====================

            if (isBackWaveRandom)
            {
                graph.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Weave, GetRandomDeepColor(), GetRandomLightColor()), 0, 0, nBmpWidth, nBmpHeight);
            }
            else
            {
                graph.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Weave, Color.FromArgb(124, 160, 150), Color.FromArgb(247, 247, 247)), 0, 0, nBmpWidth, nBmpHeight);
            }


            #endregion


            #region ====================绘制干扰线条，采用比背景略深一些的颜色====================

            if (isUseRandomLine)
            {
                Pen linePen;
                if (!isUseRandomLineColor)
                {
                    linePen = new System.Drawing.Pen(Color.FromArgb(nRed - 17, nGreen - 17, nBlue - 17), nLinesWidth);
                    for (int a = 0; a < nLines; a++)
                    {
                        int x1 = rd.Next() % nBmpWidth;
                        int y1 = rd.Next() % nBmpHeight;
                        int x2 = rd.Next() % nBmpWidth;
                        int y2 = rd.Next() % nBmpHeight;
                        graph.DrawLine(linePen, x1, y1, x2, y2);
                    }
                }
                else
                {
                    int tLineRed = 0, tLineGreen = 0, tLineBlue = 0;
                    for (int a = 0; a < nLines; a++)
                    {
                        int x1 = rd.Next() % nBmpWidth;
                        int y1 = rd.Next() % nBmpHeight;
                        int x2 = rd.Next() % nBmpWidth;
                        int y2 = rd.Next() % nBmpHeight;

                        GetRandomColor(ref tLineRed, ref tLineGreen, ref tLineBlue);
                        linePen = new System.Drawing.Pen(Color.FromArgb(nRed - 17, nGreen - 17, nBlue - 17), nLinesWidth);
                        graph.DrawLine(linePen, x1, y1, x2, y2);
                    }
                }
            }
            #endregion


            #region ====================随机输出躁点====================

            if (IsUseRandomPoint)
            {

                for (int i = 0; i < pointNum; i++)
                {
                    int x1 = rd.Next() % nBmpWidth;
                    int y1 = rd.Next() % nBmpHeight;
                    float pointWidth = 3 * (float)rd.NextDouble(); //随机干扰点大小
                    System.Drawing.Pen pointPen;
                    if (isUseRandomPointColor)//干扰点颜色是否随机
                    {
                        pointPen = new Pen(GetRandomLightColor(), pointWidth);
                    }
                    else
                    {
                        pointPen = new Pen(Color.FromArgb(120, 150, 130), pointWidth);
                    }
                    graph.DrawRectangle(pointPen, x1, y1, 1, 1);
                }
            }


            #endregion


            #region ====================循环取得字符，并绘制====================

            // 采用的字符集，可以随即拓展，并可以控制字符出现的几率
            string strCode;
            if (isUseEnglishChar)
            {
                strCode = "23456789ABCDEFGHJKMNPQRSTUVWXYZ23456789ELNG";  //略微加重了ELONG这几个字符出现的频率 :)
            }
            else
            {
                strCode = "1234567890";
            }

            string strResult = "";
            for (int i = 0; i < nLen; i++)
            {
                int x = (i * 14 + rd.Next(3));
                int y = rd.Next(3) + 1;

                #region  确定字体
                System.Drawing.Font font;
                if (isUseRandomFont)//随机字体
                {
                    ArrayList fontAL = new ArrayList(9);
                    fontAL.Add("Arial");
                    fontAL.Add("Comic Sans MS");
                    fontAL.Add("Courier New");
                    fontAL.Add("Lucida Sans");
                    fontAL.Add("MS Sans Serif");
                    fontAL.Add("Tahoma");
                    fontAL.Add("Times New Roman");
                    fontAL.Add("Verdana");
                    fontAL.Add("Gungsuh");

                    InstalledFontCollection installedFontCollection = new InstalledFontCollection();
                    FontFamily[] fontFamilies = installedFontCollection.Families;
                    int fontCount = fontFamilies.Length;

                    //Font randomFont = new Font(fontFamilies[rd.Next(fontCount)].Name,12 + rd.Next()%4,System.Drawing.FontStyle.Bold);
                    Font randomFont = new Font(fontAL[rd.Next(fontAL.Capacity)].ToString(), 12 + rd.Next() % 4, System.Drawing.FontStyle.Bold);

                    font = randomFont;
                }
                else
                {
                    font = new System.Drawing.Font("Courier New", 12 + rd.Next() % 4, System.Drawing.FontStyle.Bold);
                }
                #endregion

                char c = strCode[rd.Next(strCode.Length)];  // 随机获取字符
                strResult += c.ToString();

                #region  绘制字符
                if (isCharRandom)//是否每个字符使用自己的随机色
                {
                    //每个字体使用自己的随机颜色
                    int tempRed = 0, tempGreen = 0, tempBlue = 0;
                    GetRandomColor(ref tempRed, ref tempGreen, ref tempBlue);

                    graph.DrawString(c.ToString(),
                        font,
                        new SolidBrush(System.Drawing.Color.FromArgb(tempRed - 120, tempGreen - 120, tempBlue - 120)),
                        x,
                        y);
                }
                else
                {
                    graph.DrawString(c.ToString(),
                        font,
                        new SolidBrush(System.Drawing.Color.FromArgb(nRed - 120 + y * 3, nGreen - 120 + y * 3, nBlue - 120 + y * 3)),
                        x,
                        y);
                }
                #endregion

            }
            #endregion


            #region ====================输出字节流====================

            System.IO.MemoryStream bstream = new System.IO.MemoryStream();
            bmp.Save(bstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();
            graph.Dispose();

            strKey = strResult;
            byte[] byteReturn = bstream.ToArray();
            bstream.Close();

            return byteReturn;

            #endregion


        }
        #endregion


        #region ==================== 生成随机颜色相关函数====================
        /// <summary>
        /// 生成随机浅颜色
        /// </summary>
        /// <returns>randomColor</returns>
        public Color GetRandomLightColor()
        {
            int nRed, nGreen, nBlue;    //越大颜色越浅
            int low = 180;           //色彩的下限
            int high = 255;          //色彩的上限      
            nRed = rd.Next(high) % (high - low) + low;
            nGreen = rd.Next(high) % (high - low) + low;
            nBlue = rd.Next(high) % (high - low) + low;
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }

        /// <summary>
        /// 生成随机深颜色
        /// </summary>
        /// <returns>Color对象</returns>
        public Color GetRandomDeepColor()
        {
            int nRed, nGreen, nBlue;    // nBlue,nRed  nGreen 相差大一点 nGreen 小一些
            //int high = 255;       
            int redLow = 120;
            int greenLow = 0;
            int blueLow = 120;
            nRed = rd.Next(redLow);
            nGreen = rd.Next(greenLow);
            nBlue = rd.Next(blueLow);
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }
        public Color GetRandomDeepColor(ref int nRed, ref int nGreen, ref int nBlue)
        {
            // nBlue,nRed  nGreen 相差大一点 nGreen 小一些  越大颜色越浅 
            //int high = 255;       
            int redLow = 100;
            int greenLow = 10;
            int blueLow = 100;
            nRed = rd.Next(redLow);
            nGreen = rd.Next(greenLow);
            nBlue = rd.Next(blueLow);
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }

        /// <summary>
        /// 生成随机色.
        /// </summary>
        /// <returns>Color对象</returns>
        public Color GetRandomColor()
        {
            int nRed, nGreen, nBlue;
            nRed = rd.Next(255) % 128 + 128;
            nGreen = rd.Next(255) % 128 + 128;
            nBlue = rd.Next(255) % 128 + 128;
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }
        public Color GetRandomColor(ref int nRed, ref int nGreen, ref int nBlue)
        {
            nRed = rd.Next(255) % 128 + 128;
            nGreen = rd.Next(255) % 128 + 128;
            nBlue = rd.Next(255) % 128 + 128;
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }
        #endregion


        #region ==================== 配置文件,页面验证,清除配置文件缓存 相关函数====================

        /// <summary>
        /// 得到使用默认值初始化的ValidateConfig对象.
        /// </summary>
        /// <returns>ValidateConfig对象</returns>
        private ValidateConfig InitDefaultConfig()
        {
            ValidateConfig tempVC = new ValidateConfig();
            //背景相关
            tempVC.IsBackWaveRandom = false;

            //字符相关
            tempVC.NLen = 4;
            tempVC.IsCharRandom = true;
            tempVC.IsUseEnglishChar = true;
            tempVC.IsUseRandomFont = true;

            //干扰线相关
            tempVC.IsUseRandomLine = true;
            tempVC.IsUseRandomLineColor = true;
            tempVC.NLines = 8;
            tempVC.NLinesWidth = 3;

            //干扰点相关
            tempVC.IsUseRandomPoint = true;
            tempVC.IsUseRandomPointColor = true;
            tempVC.PointNum = 40;
            return tempVC;
        }


        /// <summary>
        /// 静态方法,得到使用默认值初始化的ValidateConfig对象.
        /// </summary>
        /// <returns>ValidateConfig对象</returns>
        public static ValidateConfig StaticInitDefaultConfig()
        {
            ValidateConfig tempVC = new ValidateConfig();
            //背景相关
            tempVC.IsBackWaveRandom = false;

            //字符相关
            tempVC.NLen = 4;
            tempVC.IsCharRandom = true;
            tempVC.IsUseEnglishChar = true;
            tempVC.IsUseRandomFont = true;

            //干扰线相关
            tempVC.IsUseRandomLine = true;
            tempVC.IsUseRandomLineColor = true;
            tempVC.NLines = 5;
            tempVC.NLinesWidth = 2;

            //干扰点相关
            tempVC.IsUseRandomPoint = true;
            tempVC.IsUseRandomPointColor = true;
            tempVC.PointNum = 20;
            return tempVC;
        }



        /// <summary>
        /// 得到配置信息实例
        /// </summary>
        /// <param name="configFilePath">配置文件的路径</param>
        /// <returns>ValidateConfig对象实例</returns>
        private ValidateConfig GetValidateCodeConfig(string configFilePath)
        {

            ValidateConfig tempVConfig = new ValidateConfig();

            filePath = configFilePath;
            System.Web.Caching.Cache cache = HttpRuntime.Cache;//系统缓存对象
            tempVConfig = cache.Get("validateCodeConfig") as ValidateConfig;
            if (tempVConfig == null)
            {
                try
                {
                    tempVConfig = (ValidateConfig)SerializationHelper.Load(typeof(ValidateConfig), filePath);
                }
                catch
                {
                    tempVConfig = InitDefaultConfig();//如果配置文件不存在并无法反序列化,则使用默认配置.
                }
                cache.Insert("validateCodeConfig", tempVConfig);
            }

            return tempVConfig;
        }

        /// <summary>
        /// 静态方法,得到配置信息实例
        /// </summary>
        /// <param name="configFilePath">配置文件的路径</param>
        /// <returns>ValidateConfig对象实例</returns>
        public static ValidateConfig StaticGetValidateCodeConfig(string configFilePath)
        {

            ValidateConfig tempVConfig = new ValidateConfig();
            System.Web.Caching.Cache cache = HttpRuntime.Cache;//系统缓存对象
            tempVConfig = cache.Get("validateCodeConfig") as ValidateConfig;
            if (tempVConfig == null)
            {
                try
                {
                    tempVConfig = (ValidateConfig)SerializationHelper.Load(typeof(ValidateConfig), configFilePath);
                }
                catch
                {
                    tempVConfig = StaticInitDefaultConfig();//如果配置文件不存在或者无法反序列化,则使用默认配置.
                }
                cache.Insert("validateCodeConfig", tempVConfig);
            }

            return tempVConfig;
        }


        /// <summary>
        /// 使用配置信息实例设置属性
        /// </summary>
        /// <param name="tempConfig">配置信息类实例</param>
        private void SetConfigInfo(ValidateConfig tempConfig)
        {
            this.IsBackWaveRandom = tempConfig.IsBackWaveRandom;
            this.NLen = tempConfig.NLen;
            this.IsCharRandom = tempConfig.IsCharRandom;
            this.IsUseEnglishChar = tempConfig.IsUseEnglishChar;
            this.IsUseRandomFont = tempConfig.IsUseRandomFont;
            this.IsUseRandomLine = tempConfig.IsUseRandomLine;
            this.IsUseRandomLineColor = tempConfig.IsUseRandomLineColor;
            this.NLines = tempConfig.NLines;
            this.NLinesWidth = tempConfig.NLinesWidth;
            this.IsUseRandomPoint = tempConfig.IsUseRandomPoint;
            this.IsUseRandomPointColor = tempConfig.IsUseRandomPointColor;
            this.PointNum = tempConfig.PointNum;
        }


        /// <summary>
        /// 验证用户输入是否正确
        /// </summary>
        /// <param name="e">页面对象</param>
        /// <param name="sessionKey">用来比较的Session对象的键值</param>
        /// <param name="inputCode">用户输入的验证码字符串</param>
        /// <returns>是否正确</returns>
        public bool CheckValidateCode(Page e, string sessionKey, string inputCode)
        {
            if (sessionKey != null && e.Session[sessionKey] != null && inputCode != null)
            {
                if (e.Session[sessionKey].ToString().ToLower() == inputCode.ToString().ToLower())
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 验证用户输入是否正确
        /// </summary>
        /// <param name="e">页面对象.默认session键值为validateCode</param>
        /// <param name="inputCode">用户输入的验证码字符串</param>
        /// <returns>是否正确</returns>
        public bool CheckValidateCode(Page e, string inputCode)
        {
            if (e.Session["validateCode"] != null && inputCode != null)
            {
                if (e.Session["validateCode"].ToString().ToLower() == inputCode.ToString().ToLower())
                {
                    return true;
                }

            }
            return false;
        }

        /// <summary>
        /// 清除ValidateConfig配置文件的缓存
        /// </summary>
        /// <returns>是否清除成功</returns>
        public bool ClearCache()
        {
            try
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;//系统缓存对象
                cache["validateCodeConfig"] = null;
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

    }


    /// <summary>
    /// ValidateCode的配置类.用于设置验证码的各项配置.
    /// </summary>

    [System.Serializable]
    public class ValidateConfig
    {
        #region ====================属性和私有字段====================

        #region isBackWaveRandom 背景的波纹是否使用随机色

        /// <summary>
        /// 背景的波纹是否使用随机色
        /// </summary>
        private bool isBackWaveRandom;
        /// <summary>
        /// 背景的波纹是否使用随机色
        /// </summary>
        public bool IsBackWaveRandom
        {
            get { return isBackWaveRandom; }
            set { isBackWaveRandom = value; }
        }

        #endregion

        //字符相关
        #region nLen 随机字符长度
        /// <summary>
        /// 随机字符长度
        /// </summary>
        private int nLen;
        /// <summary>
        /// 随机字符长度
        /// </summary>
        public int NLen
        {
            get { return nLen; }
            set { nLen = value; }
        }
        #endregion

        #region isCharRandom 是否每个文字使用不同的随机色
        /// <summary>
        /// 是否每个文字使用不同的随机色
        /// </summary>
        private bool isCharRandom;
        /// <summary>
        /// 是否每个文字使用不同的随机色
        /// </summary>
        public bool IsCharRandom
        {
            get { return isCharRandom; }
            set { isCharRandom = value; }
        }
        #endregion

        #region isUseEnglishChar 是否使用英文字符
        /// <summary>
        /// 是否使用英文字符
        /// </summary>
        private bool isUseEnglishChar;
        /// <summary>
        /// 是否使用英文字符
        /// </summary>
        public bool IsUseEnglishChar
        {
            get { return isUseEnglishChar; }
            set { isUseEnglishChar = value; }
        }
        #endregion

        #region isUseRandomFont 是否使用随机字体
        /// <summary>
        /// 是否使用随机字体
        /// </summary>
        private bool isUseRandomFont;
        /// <summary>
        /// 是否使用随机字体
        /// </summary>
        public bool IsUseRandomFont
        {
            get { return isUseRandomFont; }
            set { isUseRandomFont = value; }
        }
        #endregion

        //干扰线相关
        #region isUseRandomLine 是否使用干扰线
        /// <summary>
        /// 是否使用干扰线
        /// </summary>
        private bool isUseRandomLine;
        /// <summary>
        /// 是否使用干扰线
        /// </summary>
        public bool IsUseRandomLine
        {
            get { return isUseRandomLine; }
            set { isUseRandomLine = value; }
        }
        #endregion

        #region isUseRandomLineColor 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// <summary>
        /// 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// </summary>
        private bool isUseRandomLineColor;
        /// <summary>
        /// 每一条干扰线颜色是否随机,如果不随机则每次使用统一随机色的干扰线.
        /// </summary>
        public bool IsUseRandomLineColor
        {
            get { return isUseRandomLineColor; }
            set { isUseRandomLineColor = value; }
        }
        #endregion

        #region nLines 干扰线条的个数
        /// <summary>
        /// 干扰线条的个数
        /// </summary>
        private int nLines;
        /// <summary>
        /// 干扰线条的个数
        /// </summary>
        public int NLines
        {
            get { return nLines; }
            set { nLines = value; }
        }
        #endregion

        #region nLinesWidth 干扰线条宽度
        /// <summary>
        /// 干扰线条宽度
        /// </summary>
        private int nLinesWidth;
        /// <summary>
        /// 干扰线宽度
        /// </summary>
        public int NLinesWidth
        {
            get { return nLinesWidth; }
            set { nLinesWidth = value; }
        }
        #endregion

        //干扰点相关
        #region isUseRandomPoint 是否使用干扰点
        /// <summary>
        /// 是否使用干扰点
        /// </summary>
        private bool isUseRandomPoint;
        /// <summary>
        /// 是否使用干扰点
        /// </summary>
        public bool IsUseRandomPoint
        {
            get { return isUseRandomPoint; }
            set { isUseRandomPoint = value; }
        }
        #endregion

        #region isUseRandomPointColor 干扰点颜色是否随机
        /// <summary>
        /// 干扰点颜色是否随机
        /// </summary>
        private bool isUseRandomPointColor;
        /// <summary>
        /// 干扰点颜色是否随机
        /// </summary>
        public bool IsUseRandomPointColor
        {
            get { return isUseRandomPointColor; }
            set { isUseRandomPointColor = value; }
        }
        #endregion

        #region pointNum 干扰点数目
        /// <summary>
        /// 干扰点数目
        /// </summary>
        private int pointNum;
        /// <summary>
        /// 干扰点数目
        /// </summary>
        public int PointNum
        {
            get { return pointNum; }
            set { pointNum = value; }
        }
        #endregion

        #endregion

        public ValidateConfig()
        {
            isBackWaveRandom = false;

            //字符相关
            nLen = 6;
            isCharRandom = true;
            isUseEnglishChar = true;
            isUseRandomFont = true;

            //干扰线相关
            isUseRandomLine = true;
            isUseRandomLineColor = true;
            nLines = 8;
            nLinesWidth = 3;

            //干扰点相关
            isUseRandomPoint = true;
            isUseRandomPointColor = true;
            pointNum = 40;
        }
    }
}
