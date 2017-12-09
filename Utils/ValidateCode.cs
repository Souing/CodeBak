/****************************************************************************************************
 * *
 * *        �ļ���    : ValidateCode.cs
 * *        ����˵��  : ��֤������֤��������.
 * * 					��֤��ValidateCode������������֤��.						
 * * 					������ValidateConfig����ʹ�������ļ�ʱʹ��,����������֤��Ĳ��� * 
 * *					ע:ValidateConfig���ڴ��ļ���.���ļ�����������. *						
 * * 
 * *        ��ע      :
 * *        ʹ�þ���  :		ValidateCodeImg.aspx.cs(ͼ��ҳ):(��Load�����м������´��뼴��)
 * 							ValidateCode.Instance.Init();//����ʹ��ָ���������ļ� ValidateCode.Instance.Init("http://resource.elong.com/cn/config/validate.config")
 * *        				string strKey = string.Empty;
 * * 						byte[] data = vc.GenerateVerifyImage(ref strKey);
 * * 						Session["validateCode"] = strKey.ToLower().Trim();//ע:�˴�validateCode�ļ�ֵӦ��������CheckValidateCode�е�string sessionKeyͳһ.
 * * 						HttpContext.Current.Response.ClearContent();
 * * 						HttpContext.Current.Response.ContentType = "image/Jpeg";
 * * 						HttpContext.Current.Response.End();
 *							
 *							ҳ����֤:(����Client��,�������aspxҳ����)
 *								ValidateCode.Instance.CheckValidateCode(this.Page, "validateCode", txtValidateCodeInput.Text.Trim())
 *							����˵��:CheckValidateCode(Page e, string sessionKey, string inputCode)
 *								��һ����ʹ����֤���ҳ��Page����.ʹ��this.Page����.
 *								�ڶ�������֤���Session��ֵ.��Ҫ��ValidateCodeImg.aspx(ͼ��ҳ)��д���Session��ֵ���Ӧ.�Ա������֤.
 *								���������û��������֤��ֵ.
 *							ע:����ʹ��Ĭ��sessionKeyΪ"validateCode"�����غ���.
 * 
 *							��ʾ��֤��:(����Client��,�������aspxҳ����)
 *							<img id="validateImg" style="CURSOR: pointer" src="UserControls/ValidateToken.aspx"  onclick="this.setAttribute('src','UserControls/ValidateToken.aspx?timeStamp=' + new Date().getTime())" alt="����ˢ��ͼƬ" align="absBottom">
 * 
 *			ҳ����֤�ű�����:�°���֤�������ĸ������.�������Ҫ���������ʹ��javascript����û�����,��ʹ������javascript�ű�:
 *								//token_valueΪ�û�������֤��ؼ���valueֵ 
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
    /// ��֤��������.
    /// </summary>
    [System.Serializable]
    public class ValidateCode
    {

        #region ==================== ���Ժ�˽���ֶ�====================
        //Singletonģʽʵ��
        private static ValidateCode instance = new ValidateCode();
        public static ValidateCode Instance
        {
            get { return instance; }
        }

        //�������
        #region isBackWaveRandom �����Ĳ����Ƿ�ʹ�����ɫ

        /// <summary>
        /// �����Ĳ����Ƿ�ʹ�����ɫ
        /// </summary>
        private bool isBackWaveRandom;
        /// <summary>
        /// �����Ĳ����Ƿ�ʹ�����ɫ
        /// </summary>
        public bool IsBackWaveRandom
        {
            get { return isBackWaveRandom; }
            set { isBackWaveRandom = value; }
        }

        #endregion

        //�ַ����
        #region nLen ����ַ�����
        /// <summary>
        /// ����ַ�����
        /// </summary>
        private int nLen;
        /// <summary>
        /// ����ַ�����
        /// </summary>
        public int NLen
        {
            get { return nLen; }
            set { nLen = value; }
        }
        #endregion

        #region isCharRandom �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// <summary>
        /// �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// </summary>
        private bool isCharRandom;
        /// <summary>
        /// �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// </summary>
        public bool IsCharRandom
        {
            get { return isCharRandom; }
            set { isCharRandom = value; }
        }
        #endregion

        #region isUseEnglishChar �Ƿ�ʹ��Ӣ���ַ�
        /// <summary>
        /// �Ƿ�ʹ��Ӣ���ַ�
        /// </summary>
        private bool isUseEnglishChar;
        /// <summary>
        /// �Ƿ�ʹ��Ӣ���ַ�
        /// </summary>
        public bool IsUseEnglishChar
        {
            get { return isUseEnglishChar; }
            set { isUseEnglishChar = value; }
        }
        #endregion

        #region isUseRandomFont �Ƿ�ʹ���������
        /// <summary>
        /// �Ƿ�ʹ���������
        /// </summary>
        private bool isUseRandomFont;
        /// <summary>
        /// �Ƿ�ʹ���������
        /// </summary>
        public bool IsUseRandomFont
        {
            get { return isUseRandomFont; }
            set { isUseRandomFont = value; }
        }
        #endregion

        //���������
        #region isUseRandomLine �Ƿ�ʹ�ø�����
        /// <summary>
        /// �Ƿ�ʹ�ø�����
        /// </summary>
        private bool isUseRandomLine;
        /// <summary>
        /// �Ƿ�ʹ�ø�����
        /// </summary>
        public bool IsUseRandomLine
        {
            get { return isUseRandomLine; }
            set { isUseRandomLine = value; }
        }
        #endregion

        #region isUseRandomLineColor ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// <summary>
        /// ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// </summary>
        private bool isUseRandomLineColor;
        /// <summary>
        /// ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// </summary>
        public bool IsUseRandomLineColor
        {
            get { return isUseRandomLineColor; }
            set { isUseRandomLineColor = value; }
        }
        #endregion

        #region nLines ���������ĸ���
        /// <summary>
        /// ���������ĸ���
        /// </summary>
        private int nLines;
        /// <summary>
        /// ���������ĸ���
        /// </summary>
        public int NLines
        {
            get { return nLines; }
            set { nLines = value; }
        }
        #endregion

        #region nLinesWidth �����������
        /// <summary>
        /// �����������
        /// </summary>
        private int nLinesWidth;
        /// <summary>
        /// �����߿��
        /// </summary>
        public int NLinesWidth
        {
            get { return nLinesWidth; }
            set { nLinesWidth = value; }
        }
        #endregion

        //���ŵ����
        #region isUseRandomPoint �Ƿ�ʹ�ø��ŵ�
        /// <summary>
        /// �Ƿ�ʹ�ø��ŵ�
        /// </summary>
        private bool isUseRandomPoint;
        /// <summary>
        /// �Ƿ�ʹ�ø��ŵ�
        /// </summary>
        public bool IsUseRandomPoint
        {
            get { return isUseRandomPoint; }
            set { isUseRandomPoint = value; }
        }
        #endregion

        #region isUseRandomPointColor ���ŵ���ɫ�Ƿ����
        /// <summary>
        /// ���ŵ���ɫ�Ƿ����
        /// </summary>
        private bool isUseRandomPointColor;
        /// <summary>
        /// ���ŵ���ɫ�Ƿ����
        /// </summary>
        public bool IsUseRandomPointColor
        {
            get { return isUseRandomPointColor; }
            set { isUseRandomPointColor = value; }
        }
        #endregion

        #region pointNum ���ŵ���Ŀ
        /// <summary>
        /// ���ŵ���Ŀ
        /// </summary>
        private int pointNum;
        /// <summary>
        /// ���ŵ���Ŀ
        /// </summary>
        public int PointNum
        {
            get { return pointNum; }
            set { pointNum = value; }
        }
        #endregion

        //�������
        #region isUseConfigFile �Ƿ�ʹ�������ļ�.Ĭ��Ϊ false ��ʹ��.
        /// <summary>
        /// �Ƿ�ʹ�������ļ�.Ĭ��Ϊ false ��ʹ��.
        /// </summary>
        private bool isUseConfigFile = false;
        /// <summary>
        /// �Ƿ�ʹ�������ļ�.Ĭ��Ϊ false ��ʹ��.
        /// </summary>
        public bool IsUseConfigFile
        {
            get { return isUseConfigFile; }
            set { isUseConfigFile = value; }
        }
        #endregion

        #region ��������˽���ֶ�
        /// <summary>
        /// ���������
        /// </summary>
        private System.Random rd;

        /// <summary>
        /// ��ǰ���������Ϣʵ��
        /// </summary>
        private ValidateConfig vConfig;

        /// <summary>
        /// �����ļ�·��
        /// </summary>
        private string filePath;
        #endregion

        #endregion


        #region ==================== ���캯���ͳ�ʼ������ ====================
        /// <summary>
        ///˽�й��캯��
        /// </summary>
        private ValidateCode()
        {
        }

        /// <summary>
        /// ��ʼ������:ʹ��Ĭ�ϵ�������Ϣ��ʼ����֤��.
        /// </summary>
        public void Init()
        {
            rd = new Random((int)System.DateTime.Now.Ticks);//���������
            vConfig = InitDefaultConfig();
            SetConfigInfo(vConfig);
        }

        /// <summary>
        /// ��ʼ������:ʹ��ָ��·���������ļ�������֤��.��ȡ˳��Ϊ:����->���������ļ�->Ĭ��ֵ
        /// </summary>
        /// <param name="configFilePath">�����ļ�·��</param>
        public void Init(string configFilePath)
        {
            rd = new Random((int)System.DateTime.Now.Ticks);//���������
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


        #region ==================== ��֤�����ɺ���====================
        /// <summary>
        /// ����ͼƬ��֤��
        /// </summary>
        /// <param name="strKey">�����������֤�������</param>
        /// <returns>ͼƬ�ֽ���</returns>
        public byte[] GenerateVerifyImage(ref string strKey)
        {
            //����ͼ���Ⱥ͸߶�
            int nBmpWidth = 14 * nLen + 7;//ÿ���ַ�13����,����Ϊÿ���ַ�Ԥ����1px��λ�ռ�.
            int nBmpHeight = 25;

            //����������Ҫ�� Graphics ��bmp����
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(nBmpWidth, nBmpHeight);
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bmp);


            #region ====================�������������ɫ ����ɫΪͳһɫ����ɫ====================

            int nRed = 0, nGreen = 0, nBlue = 0;  // ��������Ԫɫ
            GetRandomColor(ref nRed, ref nGreen, ref nBlue);

            #endregion


            #region ====================���λͼ����====================

            if (isBackWaveRandom)
            {
                graph.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Weave, GetRandomDeepColor(), GetRandomLightColor()), 0, 0, nBmpWidth, nBmpHeight);
            }
            else
            {
                graph.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(HatchStyle.Weave, Color.FromArgb(124, 160, 150), Color.FromArgb(247, 247, 247)), 0, 0, nBmpWidth, nBmpHeight);
            }


            #endregion


            #region ====================���Ƹ������������ñȱ�������һЩ����ɫ====================

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


            #region ====================���������====================

            if (IsUseRandomPoint)
            {

                for (int i = 0; i < pointNum; i++)
                {
                    int x1 = rd.Next() % nBmpWidth;
                    int y1 = rd.Next() % nBmpHeight;
                    float pointWidth = 3 * (float)rd.NextDouble(); //������ŵ��С
                    System.Drawing.Pen pointPen;
                    if (isUseRandomPointColor)//���ŵ���ɫ�Ƿ����
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


            #region ====================ѭ��ȡ���ַ���������====================

            // ���õ��ַ����������漴��չ�������Կ����ַ����ֵļ���
            string strCode;
            if (isUseEnglishChar)
            {
                strCode = "23456789ABCDEFGHJKMNPQRSTUVWXYZ23456789ELNG";  //��΢������ELONG�⼸���ַ����ֵ�Ƶ�� :)
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

                #region  ȷ������
                System.Drawing.Font font;
                if (isUseRandomFont)//�������
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

                char c = strCode[rd.Next(strCode.Length)];  // �����ȡ�ַ�
                strResult += c.ToString();

                #region  �����ַ�
                if (isCharRandom)//�Ƿ�ÿ���ַ�ʹ���Լ������ɫ
                {
                    //ÿ������ʹ���Լ��������ɫ
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


            #region ====================����ֽ���====================

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


        #region ==================== ���������ɫ��غ���====================
        /// <summary>
        /// �������ǳ��ɫ
        /// </summary>
        /// <returns>randomColor</returns>
        public Color GetRandomLightColor()
        {
            int nRed, nGreen, nBlue;    //Խ����ɫԽǳ
            int low = 180;           //ɫ�ʵ�����
            int high = 255;          //ɫ�ʵ�����      
            nRed = rd.Next(high) % (high - low) + low;
            nGreen = rd.Next(high) % (high - low) + low;
            nBlue = rd.Next(high) % (high - low) + low;
            Color color = Color.FromArgb(nRed, nGreen, nBlue);
            return color;
        }

        /// <summary>
        /// �����������ɫ
        /// </summary>
        /// <returns>Color����</returns>
        public Color GetRandomDeepColor()
        {
            int nRed, nGreen, nBlue;    // nBlue,nRed  nGreen ����һ�� nGreen СһЩ
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
            // nBlue,nRed  nGreen ����һ�� nGreen СһЩ  Խ����ɫԽǳ 
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
        /// �������ɫ.
        /// </summary>
        /// <returns>Color����</returns>
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


        #region ==================== �����ļ�,ҳ����֤,��������ļ����� ��غ���====================

        /// <summary>
        /// �õ�ʹ��Ĭ��ֵ��ʼ����ValidateConfig����.
        /// </summary>
        /// <returns>ValidateConfig����</returns>
        private ValidateConfig InitDefaultConfig()
        {
            ValidateConfig tempVC = new ValidateConfig();
            //�������
            tempVC.IsBackWaveRandom = false;

            //�ַ����
            tempVC.NLen = 4;
            tempVC.IsCharRandom = true;
            tempVC.IsUseEnglishChar = true;
            tempVC.IsUseRandomFont = true;

            //���������
            tempVC.IsUseRandomLine = true;
            tempVC.IsUseRandomLineColor = true;
            tempVC.NLines = 8;
            tempVC.NLinesWidth = 3;

            //���ŵ����
            tempVC.IsUseRandomPoint = true;
            tempVC.IsUseRandomPointColor = true;
            tempVC.PointNum = 40;
            return tempVC;
        }


        /// <summary>
        /// ��̬����,�õ�ʹ��Ĭ��ֵ��ʼ����ValidateConfig����.
        /// </summary>
        /// <returns>ValidateConfig����</returns>
        public static ValidateConfig StaticInitDefaultConfig()
        {
            ValidateConfig tempVC = new ValidateConfig();
            //�������
            tempVC.IsBackWaveRandom = false;

            //�ַ����
            tempVC.NLen = 4;
            tempVC.IsCharRandom = true;
            tempVC.IsUseEnglishChar = true;
            tempVC.IsUseRandomFont = true;

            //���������
            tempVC.IsUseRandomLine = true;
            tempVC.IsUseRandomLineColor = true;
            tempVC.NLines = 5;
            tempVC.NLinesWidth = 2;

            //���ŵ����
            tempVC.IsUseRandomPoint = true;
            tempVC.IsUseRandomPointColor = true;
            tempVC.PointNum = 20;
            return tempVC;
        }



        /// <summary>
        /// �õ�������Ϣʵ��
        /// </summary>
        /// <param name="configFilePath">�����ļ���·��</param>
        /// <returns>ValidateConfig����ʵ��</returns>
        private ValidateConfig GetValidateCodeConfig(string configFilePath)
        {

            ValidateConfig tempVConfig = new ValidateConfig();

            filePath = configFilePath;
            System.Web.Caching.Cache cache = HttpRuntime.Cache;//ϵͳ�������
            tempVConfig = cache.Get("validateCodeConfig") as ValidateConfig;
            if (tempVConfig == null)
            {
                try
                {
                    tempVConfig = (ValidateConfig)SerializationHelper.Load(typeof(ValidateConfig), filePath);
                }
                catch
                {
                    tempVConfig = InitDefaultConfig();//��������ļ������ڲ��޷������л�,��ʹ��Ĭ������.
                }
                cache.Insert("validateCodeConfig", tempVConfig);
            }

            return tempVConfig;
        }

        /// <summary>
        /// ��̬����,�õ�������Ϣʵ��
        /// </summary>
        /// <param name="configFilePath">�����ļ���·��</param>
        /// <returns>ValidateConfig����ʵ��</returns>
        public static ValidateConfig StaticGetValidateCodeConfig(string configFilePath)
        {

            ValidateConfig tempVConfig = new ValidateConfig();
            System.Web.Caching.Cache cache = HttpRuntime.Cache;//ϵͳ�������
            tempVConfig = cache.Get("validateCodeConfig") as ValidateConfig;
            if (tempVConfig == null)
            {
                try
                {
                    tempVConfig = (ValidateConfig)SerializationHelper.Load(typeof(ValidateConfig), configFilePath);
                }
                catch
                {
                    tempVConfig = StaticInitDefaultConfig();//��������ļ������ڻ����޷������л�,��ʹ��Ĭ������.
                }
                cache.Insert("validateCodeConfig", tempVConfig);
            }

            return tempVConfig;
        }


        /// <summary>
        /// ʹ��������Ϣʵ����������
        /// </summary>
        /// <param name="tempConfig">������Ϣ��ʵ��</param>
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
        /// ��֤�û������Ƿ���ȷ
        /// </summary>
        /// <param name="e">ҳ�����</param>
        /// <param name="sessionKey">�����Ƚϵ�Session����ļ�ֵ</param>
        /// <param name="inputCode">�û��������֤���ַ���</param>
        /// <returns>�Ƿ���ȷ</returns>
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
        /// ��֤�û������Ƿ���ȷ
        /// </summary>
        /// <param name="e">ҳ�����.Ĭ��session��ֵΪvalidateCode</param>
        /// <param name="inputCode">�û��������֤���ַ���</param>
        /// <returns>�Ƿ���ȷ</returns>
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
        /// ���ValidateConfig�����ļ��Ļ���
        /// </summary>
        /// <returns>�Ƿ�����ɹ�</returns>
        public bool ClearCache()
        {
            try
            {
                System.Web.Caching.Cache cache = HttpRuntime.Cache;//ϵͳ�������
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
    /// ValidateCode��������.����������֤��ĸ�������.
    /// </summary>

    [System.Serializable]
    public class ValidateConfig
    {
        #region ====================���Ժ�˽���ֶ�====================

        #region isBackWaveRandom �����Ĳ����Ƿ�ʹ�����ɫ

        /// <summary>
        /// �����Ĳ����Ƿ�ʹ�����ɫ
        /// </summary>
        private bool isBackWaveRandom;
        /// <summary>
        /// �����Ĳ����Ƿ�ʹ�����ɫ
        /// </summary>
        public bool IsBackWaveRandom
        {
            get { return isBackWaveRandom; }
            set { isBackWaveRandom = value; }
        }

        #endregion

        //�ַ����
        #region nLen ����ַ�����
        /// <summary>
        /// ����ַ�����
        /// </summary>
        private int nLen;
        /// <summary>
        /// ����ַ�����
        /// </summary>
        public int NLen
        {
            get { return nLen; }
            set { nLen = value; }
        }
        #endregion

        #region isCharRandom �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// <summary>
        /// �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// </summary>
        private bool isCharRandom;
        /// <summary>
        /// �Ƿ�ÿ������ʹ�ò�ͬ�����ɫ
        /// </summary>
        public bool IsCharRandom
        {
            get { return isCharRandom; }
            set { isCharRandom = value; }
        }
        #endregion

        #region isUseEnglishChar �Ƿ�ʹ��Ӣ���ַ�
        /// <summary>
        /// �Ƿ�ʹ��Ӣ���ַ�
        /// </summary>
        private bool isUseEnglishChar;
        /// <summary>
        /// �Ƿ�ʹ��Ӣ���ַ�
        /// </summary>
        public bool IsUseEnglishChar
        {
            get { return isUseEnglishChar; }
            set { isUseEnglishChar = value; }
        }
        #endregion

        #region isUseRandomFont �Ƿ�ʹ���������
        /// <summary>
        /// �Ƿ�ʹ���������
        /// </summary>
        private bool isUseRandomFont;
        /// <summary>
        /// �Ƿ�ʹ���������
        /// </summary>
        public bool IsUseRandomFont
        {
            get { return isUseRandomFont; }
            set { isUseRandomFont = value; }
        }
        #endregion

        //���������
        #region isUseRandomLine �Ƿ�ʹ�ø�����
        /// <summary>
        /// �Ƿ�ʹ�ø�����
        /// </summary>
        private bool isUseRandomLine;
        /// <summary>
        /// �Ƿ�ʹ�ø�����
        /// </summary>
        public bool IsUseRandomLine
        {
            get { return isUseRandomLine; }
            set { isUseRandomLine = value; }
        }
        #endregion

        #region isUseRandomLineColor ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// <summary>
        /// ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// </summary>
        private bool isUseRandomLineColor;
        /// <summary>
        /// ÿһ����������ɫ�Ƿ����,����������ÿ��ʹ��ͳһ���ɫ�ĸ�����.
        /// </summary>
        public bool IsUseRandomLineColor
        {
            get { return isUseRandomLineColor; }
            set { isUseRandomLineColor = value; }
        }
        #endregion

        #region nLines ���������ĸ���
        /// <summary>
        /// ���������ĸ���
        /// </summary>
        private int nLines;
        /// <summary>
        /// ���������ĸ���
        /// </summary>
        public int NLines
        {
            get { return nLines; }
            set { nLines = value; }
        }
        #endregion

        #region nLinesWidth �����������
        /// <summary>
        /// �����������
        /// </summary>
        private int nLinesWidth;
        /// <summary>
        /// �����߿��
        /// </summary>
        public int NLinesWidth
        {
            get { return nLinesWidth; }
            set { nLinesWidth = value; }
        }
        #endregion

        //���ŵ����
        #region isUseRandomPoint �Ƿ�ʹ�ø��ŵ�
        /// <summary>
        /// �Ƿ�ʹ�ø��ŵ�
        /// </summary>
        private bool isUseRandomPoint;
        /// <summary>
        /// �Ƿ�ʹ�ø��ŵ�
        /// </summary>
        public bool IsUseRandomPoint
        {
            get { return isUseRandomPoint; }
            set { isUseRandomPoint = value; }
        }
        #endregion

        #region isUseRandomPointColor ���ŵ���ɫ�Ƿ����
        /// <summary>
        /// ���ŵ���ɫ�Ƿ����
        /// </summary>
        private bool isUseRandomPointColor;
        /// <summary>
        /// ���ŵ���ɫ�Ƿ����
        /// </summary>
        public bool IsUseRandomPointColor
        {
            get { return isUseRandomPointColor; }
            set { isUseRandomPointColor = value; }
        }
        #endregion

        #region pointNum ���ŵ���Ŀ
        /// <summary>
        /// ���ŵ���Ŀ
        /// </summary>
        private int pointNum;
        /// <summary>
        /// ���ŵ���Ŀ
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

            //�ַ����
            nLen = 6;
            isCharRandom = true;
            isUseEnglishChar = true;
            isUseRandomFont = true;

            //���������
            isUseRandomLine = true;
            isUseRandomLineColor = true;
            nLines = 8;
            nLinesWidth = 3;

            //���ŵ����
            isUseRandomPoint = true;
            isUseRandomPointColor = true;
            pointNum = 40;
        }
    }
}
