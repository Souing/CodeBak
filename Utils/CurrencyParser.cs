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


        private const string ELB = "ELB,ELD,��������,������";
        private const string CNY = "CNY,RMB,�����";
        private const string USD = "USD,��Ԫ";
        private const string HKD = "HKD,�۱�";
        private const string GBP = "GBP,Ӣ��";
        private const string CHF = "CHF,��ʿ����";
        private const string EUR = "EUR,ŷԪ";
        private const string MOP = "MOP,����Ԫ";
        private const string AUD = "AUD,�Ĵ�����Ԫ";
        private const string CAD = "CAD,���ô�Ԫ";
        private const string DKK = "DKK,�������";
        private const string NOK = "NOK,Ų������";
        private const string SGD = "SGD,�¼���Ԫ";
        private const string SEK = "SEK,������";
        private const string JPY = "JPY,��Ԫ";




        /*
        private const double  USD = 0.00;//������Ԫ
        private const double  GBP= 0.00;//Ӣ��Ӣ��
        private const double  AFA = 0.00; //�������� - AFA
        private const double  ALL = 0.00; //�����������п� - ALL
        private const double  DZD = 0.00; //���������ǵ��ɶ� - DZD
        private const double  ARS = 0.00; //����͢���� - ARS
        private const double  AUD = 0.00; //�Ĵ�����Ԫ - AUD
        private const double  ATS = 0.00; //�µ������� - ATS
        private const double  BSD = 0.00; //�͹���Ԫ - BSD
        private const double  BHD = 0.00; //���ֵ��ɶ� - BHD
        private const double  BDT = 0.00; //�ϼ��������� - BDT
        private const double  BBD = 0.00; //�ͰͶ�˹Ԫ - BBD
        private const double  BEF = 0.00; //����ʱ���� - BEF*
        private const double  BMD = 0.00; //��Ľ��Ԫ - BMD
        private const double  BRL = 0.00; //�������Ƕ� - BRL
        private const double  BGN = 0.00; //���������и� - BGN
        private const double  CAD = 0.00; //���ô�Ԫ - CAD
        private const double  CLP = 0.00; //�������� - CLP
        private const double  CNY = 0.00; //�й������ - CNY
        private const double  COP = 0.00; //���ױ��Ǳ��� - COP
        private const double  CRC = 0.00; //��˹����ӿ��� - CRC
        private const double  HRK = 0.00; //���޵��ǿ��� - HRK
        private const double  CYP = 0.00; //����·˹�� - CYP
        private const double  CZK = 0.00; //�ݿ˹��͹����� - CZK
        private const double  DKK = 0.00; //������� - DKK
        private const double  DOP = 0.00; //������ӹ��͹����� - DOP
        private const double  EGP = 0.00; //������ - EGP
        private const double  EEK = 0.00; //��ɳ���ǿ�³�� - EEK
        private const double  EUR = 0.00; //ŷԪ - EUR
        private const double  FJD = 0.00; //쳼�Ԫ - FJD
        private const double  FIM = 0.00; //������� - FIM*
        private const double  FRF = 0.00; //�������� - FRF*
        private const double  DEM = 0.00; //�¹���� - DEM*
        private const double  GRD = 0.00; //ϣ���� - GRD*
        private const double  HKD = 0.00; //��۸�Ԫ - HKD
        private const double  HUF = 0.00; //���������� - HUF
        private const double  ISK = 0.00; //�������� - ISK
        private const double  XDR = 0.00; //���ʻ��һ�����֯�ر����Ȩ - XDR
        private const double  INR = 0.00; //ӡ��¬�� - INR
        private const double  IDR = 0.00; //ӡ�������Ƕ� - IDR
        private const double  IRR = 0.00; //�������Ƕ� - IRR
        private const double  IQD = 0.00; //�����˵��ɶ� - IQD
        private const double  IEP = 0.00; //�������� - IEP*
        private const double  ILS = 0.00; //��ɫ����л�˶� - ILS
        private const double  ITL = 0.00; //��������� - ITL*
        private const double  JMD = 0.00; //�����Ԫ - JMD
        private const double  JPY = 0.00; //�ձ���Ԫ - JPY
        private const double  JOD = 0.00; //Լ�����ɶ� - JOD
        private const double  KES = 0.00; //���������� - KES
        private const double  KRW = 0.00; //�������ϳ��ʣ�Բ - KRW
        private const double  KWD = 0.00; //�����ص��ɶ� - KWD
        private const double  LBP = 0.00; //����۰� - LBP
        private const double  LUF = 0.00; //¬ɭ������ - LUF*
        private const double  MYR = 0.00; //���������ּ��� - MYR
        private const double  MTL = 0.00; //��������� - MTL
        private const double  MUR = 0.00; //ë����˹¬�� - MUR
        private const double  MXN = 0.00; //ī������� - MXN
        private const double  MAD = 0.00; //Ħ������ķ - MAD
        private const double  NLG = 0.00; //������ - NLG*
        private const double  NZD = 0.00; //������Ԫ - NZD
        private const double  NOK = 0.00; //Ų������ - NOK
        private const double  OMR = 0.00; //�������Ƕ� - OMR
        private const double  PKR = 0.00; //�ͻ�˹̹¬�� - PKR
        private const double  XPD = 0.00; //�׽𣨰�˾��- XPD
        private const double  PEN = 0.00; //��³������ - PEN
        private const double  PHP = 0.00; //���ɱ����� - PHP
        private const double  PLN = 0.00; //���������� - PLN
        private const double  PTE = 0.00; //��������˹��� - PTE*
        private const double  QAR = 0.00; //���������Ƕ� - QAR
        private const double  ROL = 0.00; //������������ - ROL
        private const double  RUB = 0.00; //����˹¬�� - RUB
        private const double  SAR = 0.00; //ɳ�����Ƕ� - SAR
        private const double  SGD = 0.00; //�¼���Ԫ - SGD
        private const double  SKK = 0.00; //˹�工�˿��� - SKK
        private const double  SIT = 0.00; //˹�������������� - SIT
        private const double  ZAR = 0.00; //�Ϸ����� - ZAR
        private const double  ESP = 0.00; //������������ - ESP*
        private const double  LKR = 0.00; //˹������¬�� - LKR
        private const double  SDD = 0.00; //�յ����ɶ� - SDD
        private const double  SEK = 0.00; //������ - SEK
        private const double  CHF = 0.00; //��ʿ���� - CHF
        private const double  TWD = 0.00; //̨����̨�� - TWD
        private const double  THB = 0.00; //̩��̩�� - THB
        private const double  TTD = 0.00; //�������Ͷ�͸�Ԫ - TTD
        private const double  TND = 0.00; //ͻ��˹���ɶ� - TND
        private const double  TRY = 0.00; //������������ - TRY
        private const double  TRL = 0.00; //���������� - TRL*
        private const double  AED = 0.00; //��������������������ķ - AED
        private const double  VEB = 0.00; //ί������������ - VEB
        private const double  VND = 0.00; //Խ�϶� - VND
        private const double  ZMK = 0.00; //�ޱ��ǿ��߲� - ZMK
        private const double  XAF = 0.00; //���޽��ڹ�ͬ�巨�� (BEAC) - XAF
        private const double  XOF = 0.00; //���޽��ڹ�ͬ�巨�� (BCEAO) - XOF
        private const double  XPF = 0.00; //̫ƽ��������ͬ�巨�� - XPF
        private const double  XCD = 0.00; //�����ձ�Ԫ - XCD
       */
        private static ArrayList RateList; //���ʱ�

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
        /// ���캯��
        /// </summary>
        public CurrencyParser()
        {


        }

        /// <summary>
        ///�����ת��Ϊ���
        /// </summary>
        /// <param name="sourceSymbol">��ҷ���</param>
        /// <param name="Value">���ֵ</param>
        /// <returns>�����ֵ</returns>

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
        /// ���ֻ���ת��
        /// </summary>
        /// <param name="sourceSymbol">��Ҫת���Ļ��ҷ���</param>
        /// <param name="Value">��Ҫת������ֵ</param>
        /// <param name="targetSymbol">Ŀ����ҷ���</param>
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
            throw new Exception("ת������");
        }

        /// <summary>
        /// ����ָ���Ļ��ҷ���.
        /// ����ֵ�뷵��ֵ:RMB-&yen; HKD-HK$  USD-$ ELB-���� 
        /// zhineng.li 2008.3.14
        /// </summary>
        /// <param name="currency">RMB-&yen; HKD-HK$  USD-$ ELB-���� </param>
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
                return "����";
            }
            else
            {
                return currency;
            }

        }

        /// <summary>
        /// ����ָ���Ļ���ö��ֵ
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

            /*��ʿ���� 15 CHF 100 669.73 668.39 671.07 654.33 20050527 
        ������ 21 SEK 100 112.55 112.32 112.78 109.96 20050527 
        �¼���Ԫ 18 SGD 100 498.47 497.47 499.47 487.01 20050527 
        Ų������ 23 NOK 100 129.55 129.29 129.81 126.57 20050527 
        ��Ԫ 27 JPY 100000 7661.4 7647.37 7678.03 7586.07 20050527 
        ������� 22 DKK 100 139.13 138.85 139.41 135.93 20050527 
        ���ô�Ԫ 28 CAD 100 653.34 652.03 654.65 638.31 20050527 
        ����Ԫ 81 MOP 100 103.38 103.17 103.59 101 20050527 
        �Ĵ�����Ԫ 29 AUD 100 628.19 626.93 629.45 613.74 20050527 
        ŷԪ 38 EUR 100 1042.1 1033.84 1037.78 1025.45 20050527 
        ����� 01 RMB 100 100 100 100 100 20050527 

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

            RateList.Add(new Currency(USD, CNY, 7.5));//��Ԫ�������
            RateList.Add(new Currency(HKD, CNY, 1.0636));//�۱Ҷ������
            RateList.Add(new Currency(GBP, CNY, 15.0823));//Ӣ���������
            RateList.Add(new Currency(CHF, CNY, 6.6973));//��ʿ����
            RateList.Add(new Currency(SGD, CNY, 4.9847));
            RateList.Add(new Currency(NOK, CNY, 1.2955));
            RateList.Add(new Currency(DKK, CNY, 1.3913));
            RateList.Add(new Currency(CAD, CNY, 6.5334));
            RateList.Add(new Currency(MOP, CNY, 1.0338));
            RateList.Add(new Currency(EUR, CNY, 10.3384));
            RateList.Add(new Currency(AUD, CNY, 6.2819));
            RateList.Add(new Currency(SEK, CNY, 1.1255));
            RateList.Add(new Currency(JPY, CNY, 0.076614));//��Ԫ�������
            RateList.Add(new Currency(HKD, USD, 0.12814));//�۱Ҷ���Ԫ
            RateList.Add(new Currency(MOP, USD, 0.12455));//�±Ҷ���Ԫ
            RateList.Add(new Currency(SGD, USD, 0.60057));//��Ԫ����Ԫ


            //������һ��һ�һ�
            RateList.Add(new Currency(CNY, CNY, 1.0));//RMB
            RateList.Add(new Currency(USD, USD, 1.0));//��Ԫ
            RateList.Add(new Currency(HKD, HKD, 1.0));//�۱�
            RateList.Add(new Currency(GBP, GBP, 1.0));//Ӣ��
            RateList.Add(new Currency(CHF, CHF, 1.0));//��ʿ����
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
