using System;
using System.Text;
using System.Collections;
using System.Threading;
using System.Globalization;

namespace Com.Yst.Framework.Utils
{

    /// <summary>
    /// 中文字体转换类。用于简体-繁体转换, 汉字-拼音转换等。
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// 代码举例：
    /// <code>
    /// string simplifiedText = ChineseConverter.ToSimplified("漢");       //转换为简体的"汉"
    /// string traditionalText = ChineseConverter.ToTraditionalTest("汉"); //转换为繁体的"漢"
    /// string pinyinText = ChineseConverter.ToPinYin("汉字", ",", false); //转化为“han,zi”(使用“，”分割，首字母不大写)
    /// string sbcText = ChineseConverter.ToSbc("."); //转化为全角的“。”
    /// </code>
    /// </example>
    public class ChineseConverter
    {

        #region ==================== Private Filed ====================
        /// <summary>
        /// 包含字符 ASC 码的整形数组。
        /// </summary>
        private static int[] pv = new int[] { -20319, -20317, -20304, -20295, -20292, -20283, -20265, -20257, -20242, -20230, -20051, -20036, -20032,
                                                -20026, -20002, -19990, -19986, -19982, -19976, -19805, -19784, -19775, -19774, -19763, -19756, -19751, -19746, -19741,
                                                -19739, -19728, -19725, -19715, -19540, -19531, -19525, -19515, -19500, -19484, -19479, -19467, -19289, -19288, -19281,
                                                -19275, -19270, -19263, -19261, -19249, -19243, -19242, -19238, -19235, -19227, -19224, -19218, -19212, -19038, -19023,
                                                -19018, -19006, -19003, -18996, -18977, -18961, -18952, -18783, -18774, -18773, -18763, -18756, -18741, -18735, -18731,
                                                -18722, -18710, -18697, -18696, -18526, -18518, -18501, -18490, -18478, -18463, -18448, -18447, -18446, -18239, -18237,
                                                -18231, -18220, -18211, -18201, -18184, -18183, -18181, -18012, -17997, -17988, -17970, -17964, -17961, -17950, -17947,
                                                -17931, -17928, -17922, -17759, -17752, -17733, -17730, -17721, -17703, -17701, -17697, -17692, -17683, -17676, -17496,
                                                -17487, -17482, -17468, -17454, -17433, -17427, -17417, -17202, -17185, -16983, -16970, -16942, -16915, -16733, -16708,
                                                -16706, -16689, -16664, -16657, -16647, -16474, -16470, -16465, -16459, -16452, -16448, -16433, -16429, -16427, -16423,
                                                -16419, -16412, -16407, -16403, -16401, -16393, -16220, -16216, -16212, -16205, -16202, -16187, -16180, -16171, -16169,
                                                -16158, -16155, -15959, -15958, -15944, -15933, -15920, -15915, -15903, -15889, -15878, -15707, -15701, -15681, -15667,
                                                -15661, -15659, -15652, -15640, -15631, -15625, -15454, -15448, -15436, -15435, -15419, -15416, -15408, -15394, -15385,
                                                -15377, -15375, -15369, -15363, -15362, -15183, -15180, -15165, -15158, -15153, -15150, -15149, -15144, -15143, -15141,
                                                -15140, -15139, -15128, -15121, -15119, -15117, -15110, -15109, -14941, -14937, -14933, -14930, -14929, -14928, -14926,
                                                -14922, -14921, -14914, -14908, -14902, -14894, -14889, -14882, -14873, -14871, -14857, -14678, -14674, -14670, -14668,
                                                -14663, -14654, -14645, -14630, -14594, -14429, -14407, -14399, -14384, -14379, -14368, -14355, -14353, -14345, -14170,
                                                -14159, -14151, -14149, -14145, -14140, -14137, -14135, -14125, -14123, -14122, -14112, -14109, -14099, -14097, -14094,
                                                -14092, -14090, -14087, -14083, -13917, -13914, -13910, -13907, -13906, -13905, -13896, -13894, -13878, -13870, -13859,
                                                -13847, -13831, -13658, -13611, -13601, -13406, -13404, -13400, -13398, -13395, -13391, -13387, -13383, -13367, -13359,
                                                -13356, -13343, -13340, -13329, -13326, -13318, -13147, -13138, -13120, -13107, -13096, -13095, -13091, -13076, -13068,
                                                -13063, -13060, -12888, -12875, -12871, -12860, -12858, -12852, -12849, -12838, -12831, -12829, -12812, -12802, -12607,
                                                -12597, -12594, -12585, -12556, -12359, -12346, -12320, -12300, -12120, -12099, -12089, -12074, -12067, -12058, -12039,
                                                -11867, -11861, -11847, -11831, -11798, -11781, -11604, -11589, -11536, -11358, -11340, -11339, -11324, -11303, -11097,
                                                -11077, -11067, -11055, -11052, -11045, -11041, -11038, -11024, -11020, -11019, -11018, -11014, -10838, -10832, -10815,
                                                -10800, -10790, -10780, -10764, -10587, -10544, -10533, -10519, -10331, -10329, -10328, -10322, -10315, -10309, -10307,
                                                -10296, -10281, -10274, -10270, -10262, -10260, -10256, -10254 };

        /// <summary>
        /// 包含汉字拼音的字符串数组。
        /// </summary>
        private static string[] ps = new string[] { "a", "ai", "an", "ang", "ao", "ba", "bai", "ban", "bang", "bao", "bei",
                                                      "ben", "beng", "bi", "bian", "biao", "bie", "bin", "bing", "bo", "bu", "ca", "cai", "can", "cang", "cao", "ce", "ceng",
                                                      "cha", "chai", "chan", "chang", "chao", "che", "chen", "cheng", "chi", "chong", "chou", "chu", "chuai", "chuan", "chuang",
                                                      "chui", "chun", "chuo", "ci", "cong", "cou", "cu", "cuan", "cui", "cun", "cuo", "da", "dai", "dan", "dang", "dao", "de",
                                                      "deng", "di", "dian", "diao", "die", "ding", "diu", "dong", "dou", "du", "duan", "dui", "dun", "duo", "e", "en", "er",
                                                      "fa", "fan", "fang", "fei", "fen", "feng", "fo", "fou", "fu", "ga", "gai", "gan", "gang", "gao", "ge", "gei", "gen", "geng",
                                                      "gong", "gou", "gu", "gua", "guai", "guan", "guang", "gui", "gun", "guo", "ha", "hai", "han", "hang", "hao", "he", "hei",
                                                      "hen", "heng", "hong", "hou", "hu", "hua", "huai", "huan", "huang", "hui", "hun", "huo", "ji", "jia", "jian", "jiang", "jiao",
                                                      "jie", "jin", "jing", "jiong", "jiu", "ju", "juan", "jue", "jun", "ka", "kai", "kan", "kang", "kao",
                                                      "ke", "ken", "keng", "kong", "kou", "ku", "kua", "kuai", "kuan", "kuang", "kui", "kun", "kuo", "la", "lai", "lan", "lang", "lao", "le", "lei", "leng", "li", "lia", "lian", "liang", "liao", "lie", "lin", "ling", "liu", "long", "lou", "lu", "lv", "luan", "lue", "lun", "luo", "ma", "mai", "man", "mang", "mao", "me", "mei", "men", "meng", "mi", "mian", "miao", "mie", "min", "ming", "miu", "mo", "mou", "mu", "na", "nai", "nan", "nang", "nao", "ne", "nei", "nen", "neng", "ni", "nian", "niang", "niao", "nie", "nin", "ning", "niu", "nong", "nu", "nv", "nuan", "nue", "nuo", "o", "ou", "pa", "pai", "pan", "pang", "pao", "pei", "pen", "peng", "pi", "pian", "piao", "pie", "pin", "ping", "po", "pu", "qi", "qia", "qian", "qiang", "qiao", "qie", "qin", "qing", "qiong", "qiu", "qu", "quan", "que", "qun", "ran", "rang", "rao", "re", "ren", "reng", "ri", "rong", "rou", "ru", "ruan", "rui", "run", "ruo", "sa", "sai", "san", "sang", "sao", "se", "sen", "seng", "sha", "shai", "shan", "shang", "shao", "she", "shen", "sheng", "shi", "shou", "shu", "shua", "shuai", "shuan", "shuang", "shui", "shun", "shuo", "si", "song", "sou", "su", "suan", "sui", "sun", "suo", "ta", "tai", "tan", "tang", "tao", "te", "teng", "ti", "tian", "tiao", "tie", "ting", "tong", "tou", "tu", "tuan", "tui", "tun", "tuo", "wa", "wai", "wan", "wang", "wei", "wen", "weng", "wo", "wu", "xi", "xia", "xian", "xiang", "xiao", "xie", "xin", "xing", "xiong", "xiu", "xu", "xuan", "xue", "xun", "ya", "yan", "yang", "yao", "ye", "yi", "yin", "ying", "yo", "yong", "you", "yu", "yuan", "yue", "yun", "za", "zai", "zan", "zang", "zao", "ze", "zei", "zen", "zeng", "zha", "zhai", "zhan", "zhang", "zhao", "zhe", "zhen", "zheng", "zhi", "zhong", "zhou", "zhu", "zhua", "zhuai", "zhuan", "zhuang", "zhui", "zhun", "zhuo", "zi", "zong", "zou", "zu", "zuan", "zui", "zun", "zuo" };

        #endregion


        #region ==================== Property ====================

        private static Hashtable m_PinYinSpecialList;
        /// <summary>
        /// 拼音转换时，包含列外词组读音的键/值对的组合。
        /// 主要用于多音字，比如重字的拼音是“zhong”，但是“重庆”的中读作“chong”.此时需要特殊加入“重庆”一词。
        /// </summary>
        public static Hashtable PinYinSpecialList
        {
            get
            {
                if (m_PinYinSpecialList == null)
                {
                    m_PinYinSpecialList = new Hashtable();

                    m_PinYinSpecialList.Add("重庆", "Chong Qing");
                    m_PinYinSpecialList.Add("深圳", "Shen Zhen");
                    m_PinYinSpecialList.Add("什么", "Shen Me");
                }

                return m_PinYinSpecialList;
            }

            set { m_PinYinSpecialList = value; }
        }

        #endregion


        #region ==================== Private Method ====================

        #endregion


        #region ==================== Public Method ====================

        //#region ========== 简体-繁体 转换 ==========

        ///// <summary>
        ///// 转化为繁体字符串
        ///// </summary>
        ///// <param name="input">待转换的中文字符串</param>
        ///// <returns>繁体字符串</returns>
        //public static string ToTraditional(string text)
        //{
        //    string result = String.Empty;
        //    result = Strings.StrConv(text, VbStrConv.TraditionalChinese, 0);
        //    return result;
        //}

        ///// <summary>
        ///// 转换为简体字符串
        ///// </summary>
        ///// <param name="input">待转换的中文字符串</param>
        ///// <returns>繁体字符串</returns>
        //public static string ToSimplified(string text)
        //{
        //    string result = String.Empty;
        //    result = Strings.StrConv(text, VbStrConv.SimplifiedChinese, 0);
        //    return result;
        //}

        //#endregion


        #region ========== 全角-半角 转换 ==========

        /// <summary>
        /// 将字符串转换成全角(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSbc(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary>
        /// 将字符串转换成半角(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDbc(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }


        /// <summary>
        /// 判断字符是否是半角字符或标点
        /// </summary>
        /// <remarks>
        /// 32    空格
        /// 33-47    标点
        /// 48-57    0~9
        /// 58-64    标点
        /// 65-90    A~Z
        /// 91-96    标点
        /// 97-122    a~z
        /// 123-126  标点
        /// </remarks>
        /// <param name="c">待判断字符</param>
        /// <returns>是否是半角字符</returns>
        public static bool IsDbc(char c)
        {
            int i = (int)c;
            return i >= 32 && i <= 126;
        }


        /// <summary>
        /// 判断字符是否是全角字符或标点
        /// </summary>
        /// <remarks>
        /// 全角字符 - 65248 = 半角字符
        /// 全角空格例外
        /// </remarks>
        /// <param name="c">待判断字符</param>
        /// <returns>是否是全角字符</returns>        
        public static bool IsSbc(char c)
        {
            if (c == '\u3000')
            {
                return true;
            }
            int i = (int)c - 65248;
            if (i < 32)
            {
                return false;
            }
            else
            {
                return IsDbc((char)i);
            }
        }

        #endregion


        #region ========== 汉字-拼音 转换 ==========

        /// <summary>
        /// 将指定中文字符串转换为拼音形式。
        /// </summary>
        /// <param name="text">要转换的中文字符串。</param>
        /// <param name="separator">连接拼音之间的分隔符。</param>
        /// <param name="initialCap">指定是否将首字母大写。</param>
        /// <returns>包含中文字符串的拼音的字符串。</returns>
        public static string ToPinYin(string text, string separator, bool initialCap)
        {
            string result = String.Empty;

            if (text == null || text.Length == 0) return "";
            if (separator == null || separator.Length == 0) separator = "";

            // 例外词组
            foreach (DictionaryEntry de in ChineseConverter.PinYinSpecialList)
            {
                //text = text.Replace(de.Key.ToString(), String.Format(" {0} ", de.Value.ToString().Replace(" ", separator)));
                text = text.Replace(de.Key.ToString(), de.Value.ToString());
                if (!initialCap)
                {
                    text = text.ToLower();
                }
            }

            //先对字符做全角转半角
            text = ToDbc(text);
            //将繁体字符传换成简体字符
            //text = ToSimplified(text);

            byte[] array = new byte[2];

            int chrasc = 0;
            int i1 = 0;
            int i2 = 0;
            bool b = false;
            char[] nowchar = text.ToCharArray();
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            TextInfo ti = ci.TextInfo;

            for (int j = 0; j < nowchar.Length; j++)
            {
                array = Encoding.Default.GetBytes(nowchar[j].ToString());
                string s = nowchar[j].ToString(); ;

                if (array.Length == 1)
                {
                    b = true;
                    result += s;
                }
                else
                {
                    if (s == "？")
                    {
                        if (result == "" || b == true) result += s;
                        else result += separator + s;

                        continue;
                    }

                    i1 = (short)(array[0]);
                    i2 = (short)(array[1]);

                    chrasc = i1 * 256 + i2 - 65536;

                    for (int i = (pv.Length - 1); i >= 0; i--)
                    {
                        if (pv[i] <= chrasc)
                        {
                            s = ps[i];

                            if (initialCap == true) s = ti.ToTitleCase(s);

                            if (result == "" || b == true) result += s;
                            else result += separator + s;

                            break;
                        }
                    }

                    b = false;
                }
            }

            result = result.Replace(" ", separator);
            return result;
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式。
        /// </summary>
        /// <param name="text">要转换的中文字符串。</param>
        /// <param name="separator">连接拼音之间的分隔符。</param>
        /// <returns>包含中文字符串的拼音的字符串。</returns>
        public static string ToPinYin(string text, string separator)
        {
            return ChineseConverter.ToPinYin(text, separator, false);
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式。
        /// </summary>
        /// <param name="text">要转换的中文字符串。</param>
        /// <param name="initialCap">指定是否将首字母大写。</param>
        /// <returns>包含中文字符串的拼音的字符串。</returns>
        public static string ToPinYin(string text, bool initialCap)
        {
            return ChineseConverter.ToPinYin(text, "", initialCap);
        }

        /// <summary>
        /// 将指定中文字符串转换为拼音形式。
        /// </summary>
        /// <param name="text">要转换的中文字符串。</param>
        /// <returns>包含中文字符串的拼音的字符串。</returns>
        public static string ToPinYin(string text)
        {
            return ChineseConverter.ToPinYin(text, "");
        }

        /// <summary>
        /// 汉字转拼音缩写
        /// </summary>
        /// <param name="str">要转换的汉字字符串</param>
        /// <returns>拼音缩写</returns>
        public static string GetShortPY(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {//字母和符号原样保留
                    tempStr += c.ToString();
                }
                else
                {//累加拼音声母
                    tempStr += GetPYChar(c.ToString());
                }
            }
            return tempStr;
        }

        /// <summary>
        /// 取单个字符的拼音声母
        /// </summary>
        /// <param name="c">要转换的单个汉字</param>
        /// <returns>拼音声母</returns>
        public static string GetPYChar(string c)
        {
            try
            {
                if (c == null || c == " " || c == "\t") return string.Empty;
                byte[] array = new byte[2];
                array = System.Text.Encoding.Default.GetBytes(c);
                int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));

                if (i < 0xB0A1) return string.Empty;
                if (i < 0xB0C5) return "a";
                if (i < 0xB2C1) return "b";
                if (i < 0xB4EE) return "c";
                if (i < 0xB6EA) return "d";
                if (i < 0xB7A2) return "e";
                if (i < 0xB8C1) return "f";
                if (i < 0xB9FE) return "g";
                if (i < 0xBBF7) return "h";
                if (i < 0xBFA6) return "j";
                if (i < 0xC0AC) return "k";
                if (i < 0xC2E8) return "l";
                if (i < 0xC4C3) return "m";
                if (i < 0xC5B6) return "n";
                if (i < 0xC5BE) return "o";
                if (i < 0xC6DA) return "p";
                if (i < 0xC8BB) return "q";
                if (i < 0xC8F6) return "r";
                if (i < 0xCBFA) return "s";
                if (i < 0xCDDA) return "t";
                if (i < 0xCEF4) return "w";
                if (i < 0xD1B9) return "x";
                if (i < 0xD4D1) return "y";
                if (i < 0xD7FA) return "z";

            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }


        #endregion

        #endregion

    }

    /// <summary>
    /// 中文字体枚举值
    /// </summary>
    public enum ChineseStyle
    {
        /// <summary>
        /// don't change the current Chinese style.
        /// </summary>
        Unchanged = 0,
        /// <summary>
        /// Simplified Chinese (简体)
        /// </summary>
        SimplifiedChinese = 1,
        /// <summary>
        /// Traditional Chinese (繁体)
        /// </summary>
        TraditionalChinese = 2
    }
}
