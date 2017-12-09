using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// ��ʱ�ࡣ�ṩ���ڷ�������Ӳ��Ժ��ڷ�����������ַ�ʽ��
    /// ͬһ��ʵ���Ķ��Start��Stop���ۼӣ�ʹ��Reset()���������á�
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// ���������
    /// <code>
    /// <![CDATA[
    /// //�ڷ�������Ӳ���
    /// CodeTimer target = new CodeTimer();
    /// target.Start();  
    /// string temp = String.Empty;
    /// for (int i = 0; i < 10000; i++)
    /// {
    ///     temp += "a";
    /// }	
    /// target.Stop();
    /// Console.Write(target.Result); 
    /// 
    /// //�ڷ��������
    /// string temp = String.Empty;
    /// string result = CodeTimer.Time(20000, () => { temp += "a"; });
    /// CodeTimer resultObj = CodeTimer.TimeObj(20000, () => { temp += "a"; });
    /// Console.Write(result); 
    /// Console.Write(resultObj.Result); 
    /// </code>
    /// ]]>
    /// </example>
    [System.Serializable]
    public class CodeTimer
    {

        #region ==================== Private Filed ====================
        private long startTime, stopTime;
        private int[] startGC, stopGC;
        #endregion


        #region ==================== Property ====================

        public delegate void ActionDelegate();

        /// <summary>
        /// ��ʱʹ�õ�StopWatch����
        /// </summary>
        public Stopwatch StopWatch
        {
            get{ return  m_StopWatch; }
            set{ m_StopWatch = value; }
        }
        private Stopwatch m_StopWatch;

        /// <summary>
        /// ��ʱ��ms��
        /// </summary>
        public long Elapsed
        {
            get;
            set;
        }

        /// <summary>
        /// ����CPUʱ��
        /// </summary>
        public long CpuTime
        {
            get;
            set;
        }

        /// <summary>
        /// ����GC�Ļ��մ���
        /// </summary>
        public int[]  GCTimes
        {
            get;
            set;
        }

        /// <summary>
        /// ��������Ϣ���ܵĽ���ַ���
        /// </summary>
        public string Result
        {
            get 
            {
                string tempGCTimes = String.Empty;
                for (int i = 0; i < GCTimes.Length; i++)
                {
                    tempGCTimes += String.Format(@",Gen{0}:{1}times", i.ToString(), GCTimes[i].ToString());
                }
                return String.Format("Elapsed:{0} ms, CpuTime:{1} ms, {2}", Elapsed, CpuTime.ToString("N0"), tempGCTimes);
            }
        }

        /// <summary>
        /// �Ƿ����GC
        /// </summary>
        public bool CollectGc
        {
            get { return m_CollectGc; }
            set { m_CollectGc = value;}
        }
        private bool m_CollectGc = false;

        #endregion


        #region ==================== Constructed Method ====================
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="collectGc">�Ƿ����GC</param>
        public CodeTimer(bool collectGc)
        {
            m_StopWatch = new Stopwatch();
            this.CollectGc = collectGc;
        }

        /// <summary>
        /// ���캯����Ĭ�ϲ�����GC
        /// </summary>
        public CodeTimer()
            : this(false)
        {
        }
        #endregion


        #region ==================== Private Method ====================
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool QueryThreadCycleTime(IntPtr threadHandle, ref ulong cycleTime);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetThreadTimes(IntPtr hThread, out long lpCreationTime,
           out long lpExitTime, out long lpKernelTime, out long lpUserTime);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();

        /// <summary>
        /// �õ�CPUִ��ʱ��
        /// </summary>
        /// <returns></returns>
        private static long GetCurrentThreadTimes()
        {
            long l;
            long kernelTime, userTimer;
            GetThreadTimes(GetCurrentThread(), out l, out l, out kernelTime,
               out userTimer);
            return kernelTime + userTimer;
        }
        #endregion


        #region ==================== Public Method ====================

        /// <summary>
        /// ����action����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="iteration">���Դ���</param>
        /// <param name="action">���Է���</param>
        /// <returns>�������Խ����CodeTimerʵ��</returns>
        public static CodeTimer TimeObj(int iteration, Action action)
        {
            string result = String.Empty;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            if (action == null)
            {
                throw new ArgumentException("action����Ϊ��");
            }

            CodeTimer codeTimer = new CodeTimer(true);
            codeTimer.Start();
            for (int i = 0; i < iteration; i++) action();
            codeTimer.Stop();
            return codeTimer;
        }

        /// <summary>
        /// ����action����
        /// </summary>
        /// <param name="name">��������</param>
        /// <param name="iteration">���Դ���</param>
        /// <param name="action">���Է���</param>
        /// <returns>����ַ���</returns>
        public static string Time(int iteration, Action action)
        {
            string result = String.Empty;
            CodeTimer codeTimer = CodeTimer.TimeObj(iteration, action);
            result = codeTimer.Result;
            return result;
        }

        /// <summary>
        /// ��ʼ��ʱ
        /// </summary>
        public void Start()
        {
            m_StopWatch.Start();
            startTime = GetCurrentThreadTimes(); //100 nanosecond one tick

            //Record the latest GC counts
            if(CollectGc)
            {
                GC.Collect(GC.MaxGeneration);
            }
            startGC = new int[GC.MaxGeneration + 1];
            stopGC = new int[GC.MaxGeneration + 1];
            GCTimes = new int[GC.MaxGeneration + 1];
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                startGC[i] = GC.CollectionCount(i);
                stopGC[i] = 0; 
            }
        }

        /// <summary>
        /// ������ʱ
        /// </summary>
        public void Stop()
        {
            m_StopWatch.Stop();
            stopTime = GetCurrentThreadTimes();

            //��¼������ʱ
            this.Elapsed = m_StopWatch.ElapsedMilliseconds;

            //��¼���ĵ�CPUʱ��Ƭ
            this.CpuTime += (stopTime - startTime)/10;

            //��¼�������մ���
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                stopGC[i] = GC.CollectionCount(i) - startGC[i];
                GCTimes[i] += stopGC[i];
            }
        }

        /// <summary>
        /// ���ü�����
        /// </summary>
        public void Reset()
        {
            m_StopWatch.Reset();
            this.Elapsed = 0;
            this.CpuTime = 0;
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                GCTimes[i]=0;
            }
        }

        #endregion

    }
}

