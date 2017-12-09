using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace Com.Yst.Framework.Utils
{
    /// <summary>
    /// 计时类。提供了在方法内添加测试和在方法外测试两种方式。
    /// 同一个实例的多次Start和Stop会累加，使用Reset()方法可重置。
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <example>
    /// 代码举例：
    /// <code>
    /// <![CDATA[
    /// //在方法内添加测试
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
    /// //在方法外测试
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
        /// 计时使用的StopWatch对象。
        /// </summary>
        public Stopwatch StopWatch
        {
            get{ return  m_StopWatch; }
            set{ m_StopWatch = value; }
        }
        private Stopwatch m_StopWatch;

        /// <summary>
        /// 用时（ms）
        /// </summary>
        public long Elapsed
        {
            get;
            set;
        }

        /// <summary>
        /// 消耗CPU时间
        /// </summary>
        public long CpuTime
        {
            get;
            set;
        }

        /// <summary>
        /// 各代GC的回收次数
        /// </summary>
        public int[]  GCTimes
        {
            get;
            set;
        }

        /// <summary>
        /// 将所有信息汇总的结果字符串
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
        /// 是否回收GC
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
        /// 构造函数
        /// </summary>
        /// <param name="collectGc">是否会受GC</param>
        public CodeTimer(bool collectGc)
        {
            m_StopWatch = new Stopwatch();
            this.CollectGc = collectGc;
        }

        /// <summary>
        /// 构造函数。默认不回收GC
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
        /// 得到CPU执行时间
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
        /// 测试action方法
        /// </summary>
        /// <param name="name">测试名称</param>
        /// <param name="iteration">测试次数</param>
        /// <param name="action">测试方法</param>
        /// <returns>包含测试结果的CodeTimer实例</returns>
        public static CodeTimer TimeObj(int iteration, Action action)
        {
            string result = String.Empty;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            if (action == null)
            {
                throw new ArgumentException("action参数为空");
            }

            CodeTimer codeTimer = new CodeTimer(true);
            codeTimer.Start();
            for (int i = 0; i < iteration; i++) action();
            codeTimer.Stop();
            return codeTimer;
        }

        /// <summary>
        /// 测试action方法
        /// </summary>
        /// <param name="name">测试名称</param>
        /// <param name="iteration">测试次数</param>
        /// <param name="action">测试方法</param>
        /// <returns>结果字符串</returns>
        public static string Time(int iteration, Action action)
        {
            string result = String.Empty;
            CodeTimer codeTimer = CodeTimer.TimeObj(iteration, action);
            result = codeTimer.Result;
            return result;
        }

        /// <summary>
        /// 开始计时
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
        /// 结束计时
        /// </summary>
        public void Stop()
        {
            m_StopWatch.Stop();
            stopTime = GetCurrentThreadTimes();

            //记录方法耗时
            this.Elapsed = m_StopWatch.ElapsedMilliseconds;

            //记录消耗的CPU时间片
            this.CpuTime += (stopTime - startTime)/10;

            //记录垃圾回收次数
            for (int i = 0; i <= GC.MaxGeneration; i++)
            {
                stopGC[i] = GC.CollectionCount(i) - startGC[i];
                GCTimes[i] += stopGC[i];
            }
        }

        /// <summary>
        /// 重置计数器
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

