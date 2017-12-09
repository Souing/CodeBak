using System;
using System.Collections;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// Summary description for CurrencyParser.
    /// </summary>
    [System.Serializable]
    public class CurrencyParser
    {


        private const string ELB = "ELB,ELD,艺龙积分,艺龙币";
        private const string CNY = "CNY,RMB,人民币";
        private const string USD = "USD,美元";
        private const string HKD = "HKD,港币";
        private const string GBP = "GBP,英镑";
        private const string CHF = "CHF,瑞士法郎";
        private const string EUR = "EUR,欧元";
        private const string MOP = "MOP,澳门元";
        private const string AUD = "AUD,澳大利亚元";
        private const string CAD = "CAD,加拿大元";
        private const string DKK = "DKK,丹麦克朗";
        private const string NOK = "NOK,挪威克朗";
        private const string SGD = "SGD,新加坡元";
        private const string SEK = "SEK,瑞典克朗";
        private const string JPY = "JPY,日元";




        /*
        private const double  USD = 0.00;//美国美元
        private const double  GBP= 0.00;//英国英镑
        private const double  AFA = 0.00; //阿富汗尼 - AFA
        private const double  ALL = 0.00; //阿尔巴尼亚列克 - ALL
        private const double  DZD = 0.00; //阿尔及利亚第纳尔 - DZD
        private const double  ARS = 0.00; //阿根廷比索 - ARS
        private const double  AUD = 0.00; //澳大利亚元 - AUD
        private const double  ATS = 0.00; //奥地利先令 - ATS
        private const double  BSD = 0.00; //巴哈马元 - BSD
        private const double  BHD = 0.00; //巴林第纳尔 - BHD
        private const double  BDT = 0.00; //孟加拉国塔卡 - BDT
        private const double  BBD = 0.00; //巴巴多斯元 - BBD
        private const double  BEF = 0.00; //比利时法郎 - BEF*
        private const double  BMD = 0.00; //百慕大元 - BMD
        private const double  BRL = 0.00; //巴西雷亚尔 - BRL
        private const double  BGN = 0.00; //保加利亚列弗 - BGN
        private const double  CAD = 0.00; //加拿大元 - CAD
        private const double  CLP = 0.00; //智利比索 - CLP
        private const double  CNY = 0.00; //中国人民币 - CNY
        private const double  COP = 0.00; //哥伦比亚比索 - COP
        private const double  CRC = 0.00; //哥斯达黎加科朗 - CRC
        private const double  HRK = 0.00; //克罗地亚库纳 - HRK
        private const double  CYP = 0.00; //塞浦路斯镑 - CYP
        private const double  CZK = 0.00; //捷克共和国克朗 - CZK
        private const double  DKK = 0.00; //丹麦克朗 - DKK
        private const double  DOP = 0.00; //多米尼加共和国比索 - DOP
        private const double  EGP = 0.00; //埃及镑 - EGP
        private const double  EEK = 0.00; //爱沙尼亚克鲁恩 - EEK
        private const double  EUR = 0.00; //欧元 - EUR
        private const double  FJD = 0.00; //斐济元 - FJD
        private const double  FIM = 0.00; //芬兰马克 - FIM*
        private const double  FRF = 0.00; //法国法郎 - FRF*
        private const double  DEM = 0.00; //德国马克 - DEM*
        private const double  GRD = 0.00; //希腊币 - GRD*
        private const double  HKD = 0.00; //香港港元 - HKD
        private const double  HUF = 0.00; //匈牙利福林 - HUF
        private const double  ISK = 0.00; //冰岛克朗 - ISK
        private const double  XDR = 0.00; //国际货币基金组织特别提款权 - XDR
        private const double  INR = 0.00; //印度卢比 - INR
        private const double  IDR = 0.00; //印度尼西亚盾 - IDR
        private const double  IRR = 0.00; //伊朗里亚尔 - IRR
        private const double  IQD = 0.00; //伊拉克第纳尔 - IQD
        private const double  IEP = 0.00; //爱尔兰镑 - IEP*
        private const double  ILS = 0.00; //以色列新谢克尔 - ILS
        private const double  ITL = 0.00; //意大利里拉 - ITL*
        private const double  JMD = 0.00; //牙买加元 - JMD
        private const double  JPY = 0.00; //日本日元 - JPY
        private const double  JOD = 0.00; //约旦第纳尔 - JOD
        private const double  KES = 0.00; //肯尼亚先令 - KES
        private const double  KRW = 0.00; //韩国（南朝鲜）圆 - KRW
        private const double  KWD = 0.00; //科威特第纳尔 - KWD
        private const double  LBP = 0.00; //黎巴嫩镑 - LBP
        private const double  LUF = 0.00; //卢森堡法郎 - LUF*
        private const double  MYR = 0.00; //马来西亚林吉特 - MYR
        private const double  MTL = 0.00; //马耳他里拉 - MTL
        private const double  MUR = 0.00; //毛里求斯卢比 - MUR
        private const double  MXN = 0.00; //墨西哥比索 - MXN
        private const double  MAD = 0.00; //摩洛哥迪拉姆 - MAD
        private const double  NLG = 0.00; //荷兰盾 - NLG*
        private const double  NZD = 0.00; //新西兰元 - NZD
        private const double  NOK = 0.00; //挪威克朗 - NOK
        private const double  OMR = 0.00; //阿曼里亚尔 - OMR
        private const double  PKR = 0.00; //巴基斯坦卢比 - PKR
        private const double  XPD = 0.00; //白金（盎司）- XPD
        private const double  PEN = 0.00; //秘鲁新索尔 - PEN
        private const double  PHP = 0.00; //菲律宾比索 - PHP
        private const double  PLN = 0.00; //波兰兹罗提 - PLN
        private const double  PTE = 0.00; //葡萄牙埃斯库多 - PTE*
        private const double  QAR = 0.00; //卡塔尔里亚尔 - QAR
        private const double  ROL = 0.00; //罗马尼亚列伊 - ROL
        private const double  RUB = 0.00; //俄罗斯卢布 - RUB
        private const double  SAR = 0.00; //沙特里亚尔 - SAR
        private const double  SGD = 0.00; //新加坡元 - SGD
        private const double  SKK = 0.00; //斯洛伐克克朗 - SKK
        private const double  SIT = 0.00; //斯洛文尼亚托拉尔 - SIT
        private const double  ZAR = 0.00; //南非兰特 - ZAR
        private const double  ESP = 0.00; //西班牙比塞塔 - ESP*
        private const double  LKR = 0.00; //斯里兰卡卢比 - LKR
        private const double  SDD = 0.00; //苏丹第纳尔 - SDD
        private const double  SEK = 0.00; //瑞典克朗 - SEK
        private const double  CHF = 0.00; //瑞士法郎 - CHF
        private const double  TWD = 0.00; //台湾新台币 - TWD
        private const double  THB = 0.00; //泰国泰铢 - THB
        private const double  TTD = 0.00; //特立尼达和多巴哥元 - TTD
        private const double  TND = 0.00; //突尼斯第纳尔 - TND
        private const double  TRY = 0.00; //土耳其新里拉 - TRY
        private const double  TRL = 0.00; //土耳其里拉 - TRL*
        private const double  AED = 0.00; //阿拉伯联合酋长国迪拉姆 - AED
        private const double  VEB = 0.00; //委内瑞拉博利瓦 - VEB
        private const double  VND = 0.00; //越南盾 - VND
        private const double  ZMK = 0.00; //赞比亚克瓦查 - ZMK
        private const double  XAF = 0.00; //非洲金融共同体法郎 (BEAC) - XAF
        private const double  XOF = 0.00; //非洲金融共同体法郎 (BCEAO) - XOF
        private const double  XPF = 0.00; //太平洋法兰西共同体法郎 - XPF
        private const double  XCD = 0.00; //东加勒比元 - XCD
       */
        private static ArrayList RateList; //汇率表

        private class Currency
        {
            public string CurrencyA;
            public string CurrencyB;
            public double Rate;
            public Currency(string cA, string cB, double A2B)
            {
                CurrencyA = cA;
                CurrencyB = cB;
                Rate = A2B;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CurrencyParser()
        {


        }

        /// <summary>
        ///人民币转换为外币
        /// </summary>
        /// <param name="sourceSymbol">外币符号</param>
        /// <param name="Value">外币值</param>
        /// <returns>人民币值</returns>

        public static double Parse(string sourceSymbol, double Value)
        {
            try
            {
                return Parse(sourceSymbol, Value, "CNY");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        /// <summary>
        /// 两种货币转换
        /// </summary>
        /// <param name="sourceSymbol">需要转换的货币符号</param>
        /// <param name="Value">需要转换的数值</param>
        /// <param name="targetSymbol">目标货币符号</param>
        /// <returns></returns>
        public static double Parse(string sourceSymbol, double Value, string targetSymbol)
        {
            if (sourceSymbol == null || Value == 0 || targetSymbol == null) return 0;

            ArrayList rateList = buildList();
            foreach (Currency c in rateList)
            {
                if (c.CurrencyA.IndexOf(sourceSymbol.Trim().ToUpper()) > -1 && c.CurrencyB.IndexOf(targetSymbol.Trim().ToUpper()) > -1)
                {
                    if (targetSymbol.ToUpper() == "ELB")
                    {
                        return Math.Round((Value * c.Rate) / 10, 0) * 10;
                    }
                    else
                    {
                        return Value * c.Rate;
                    }
                }
                if (c.CurrencyA.IndexOf(targetSymbol.Trim().ToUpper()) > -1 && c.CurrencyB.IndexOf(sourceSymbol.Trim().ToUpper()) > -1)
                {
                    if (targetSymbol.ToUpper() == "ELB")
                    {
                        return Math.Round((Value / c.Rate) / 10, 0) * 10;
                    }
                    else
                    {
                        return Value / c.Rate;
                    }
                }

            }
            throw new Exception("转换错误");
        }

        /// <summary>
        /// 返回指定的货币符号.
        /// 传入值与返回值:RMB-&yen; HKD-HK$  USD-$ ELB-积分 
        /// zhineng.li 2008.3.14
        /// </summary>
        /// <param name="currency">RMB-&yen; HKD-HK$  USD-$ ELB-积分 </param>
        /// <returns></returns>
        public static string ParseCurrencyToSymbol(string currency)
        {

            if (currency.ToUpper() == "RMB")
            {
                return "&yen;";

            }
            else if (currency.ToUpper() == "HKD")
            {
                return "HK$";
            }
            else if ((currency.ToUpper() == "USD"))
            {
                return "$";
            }
            else if ((currency.ToUpper() == "ELB"))
            {
                return "积分";
            }
            else
            {
                return currency;
            }

        }

        /// <summary>
        /// 返回指定的货币枚举值
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static int ParseCurrencyToInt(string currency)
        {
            switch (currency.ToUpper())
            {
                case "RMB":
                    return 0;
                case "USD":
                    return 1;
                case "HKD":
                    return 2;
                case "MOP":
                    return 3;
                case "SGD":
                    return 4;
                default:
                    return 0;
            }
        }

        private static ArrayList buildList()
        {

            /*瑞士法郎 15 CHF 100 669.73 668.39 671.07 654.33 20050527 
        瑞典克朗 21 SEK 100 112.55 112.32 112.78 109.96 20050527 
        新加坡元 18 SGD 100 498.47 497.47 499.47 487.01 20050527 
        挪威克朗 23 NOK 100 129.55 129.29 129.81 126.57 20050527 
        日元 27 JPY 100000 7661.4 7647.37 7678.03 7586.07 20050527 
        丹麦克朗 22 DKK 100 139.13 138.85 139.41 135.93 20050527 
        加拿大元 28 CAD 100 653.34 652.03 654.65 638.31 20050527 
        澳门元 81 MOP 100 103.38 103.17 103.59 101 20050527 
        澳大利亚元 29 AUD 100 628.19 626.93 629.45 613.74 20050527 
        欧元 38 EUR 100 1042.1 1033.84 1037.78 1025.45 20050527 
        人民币 01 RMB 100 100 100 100 100 20050527 

        */
            RateList = new ArrayList();

            RateList.Add(new Currency(CNY, ELB, 100.00));
            RateList.Add(new Currency(HKD, ELB, 70.9067));
            RateList.Add(new Currency(USD, ELB, 553.3333));
            RateList.Add(new Currency(MOP, ELB, 68.92));

            RateList.Add(new Currency(ELB, CNY, 0.015));
            RateList.Add(new Currency(ELB, HKD, 0.0141));
            RateList.Add(new Currency(ELB, USD, 0.00181));
            RateList.Add(new Currency(ELB, MOP, 0.01451));

            RateList.Add(new Currency(USD, CNY, 7.5));//美元兑人民币
            RateList.Add(new Currency(HKD, CNY, 1.0636));//港币对人民币
            RateList.Add(new Currency(GBP, CNY, 15.0823));//英镑对人民币
            RateList.Add(new Currency(CHF, CNY, 6.6973));//瑞士法郎
            RateList.Add(new Currency(SGD, CNY, 4.9847));
            RateList.Add(new Currency(NOK, CNY, 1.2955));
            RateList.Add(new Currency(DKK, CNY, 1.3913));
            RateList.Add(new Currency(CAD, CNY, 6.5334));
            RateList.Add(new Currency(MOP, CNY, 1.0338));
            RateList.Add(new Currency(EUR, CNY, 10.3384));
            RateList.Add(new Currency(AUD, CNY, 6.2819));
            RateList.Add(new Currency(SEK, CNY, 1.1255));
            RateList.Add(new Currency(JPY, CNY, 0.076614));//日元兑人民币
            RateList.Add(new Currency(HKD, USD, 0.12814));//港币对美元
            RateList.Add(new Currency(MOP, USD, 0.12455));//奥币对美元
            RateList.Add(new Currency(SGD, USD, 0.60057));//新元对美元


            //以下是一比一兑换
            RateList.Add(new Currency(CNY, CNY, 1.0));//RMB
            RateList.Add(new Currency(USD, USD, 1.0));//美元
            RateList.Add(new Currency(HKD, HKD, 1.0));//港币
            RateList.Add(new Currency(GBP, GBP, 1.0));//英镑
            RateList.Add(new Currency(CHF, CHF, 1.0));//瑞士法郎
            RateList.Add(new Currency(SGD, SGD, 1.0));
            RateList.Add(new Currency(NOK, NOK, 1.0));
            RateList.Add(new Currency(DKK, DKK, 1.0));
            RateList.Add(new Currency(CAD, CAD, 1.0));
            RateList.Add(new Currency(MOP, MOP, 1.0));
            RateList.Add(new Currency(EUR, EUR, 1.0));
            RateList.Add(new Currency(AUD, AUD, 1.0));
            RateList.Add(new Currency(SEK, SEK, 1.0));
            RateList.Add(new Currency(JPY, JPY, 1.0));
            RateList.Add(new Currency(ELB, ELB, 1.0));
            return RateList;
        }


    }
}
