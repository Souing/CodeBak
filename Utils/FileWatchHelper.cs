/****************************************************************************************************
 * *
 * *        File Name		: FileWatchHelper.cs
 * *        Functional Description  :  文件监控帮助类
 * *        Remark          : 
 * *
 * ****************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace Com.Yst.Framework.Utils
{
    public delegate void FileUpdate(object state);

    public class FileWatchHelper
    {

        /// <summary>
        /// The timer used to compress the notification events.
        /// </summary>
        private Timer m_timer;

        /// <summary>
        /// The default amount of time to wait after receiving notification before reloading the config file.
        /// </summary>
        private const int TimeoutMillis = 500;
        private FileSystemWatcher watcher;
        #region ==================== Constructed Function ====================
        private FileWatchHelper(FileUpdate updateProcess, string filePath, string fileName)
        {
            // Create a new FileSystemWatcher and set its properties.
            watcher = new FileSystemWatcher();

            watcher.Path = filePath;
            watcher.Filter = fileName;
            watcher.IncludeSubdirectories = true;

            // Set the notification filters
            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers. OnChanged will do for all event handlers that fire a FileSystemEventArgs
            watcher.Changed += new FileSystemEventHandler(ConfigureAndWatchHandler_OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            // Create the timer that will be used to deliver events. Set as disabled
            m_timer = new Timer(new TimerCallback(updateProcess), filePath, Timeout.Infinite, Timeout.Infinite);

        }

        private FileWatchHelper(FileUpdate updateProcess, string filePath)
            : this(updateProcess, filePath, "")
        {
        }

        #endregion

        #region ==================== Method ====================

        /// <summary>
        /// Event handler used by <see cref="ConfigureAndWatchHandler"/>.
        /// </summary>
        /// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
        /// <param name="e">The argument indicates the file that caused the event to be fired.</param>
        /// <remarks>
        /// <para>
        /// This handler reloads the configuration from the file when the event is fired.
        /// </para>
        /// </remarks>
        private void ConfigureAndWatchHandler_OnChanged(object source, FileSystemEventArgs e)
        {
            m_timer.Change(TimeoutMillis, Timeout.Infinite);
        }

        public void StopWatching()
        {
            watcher.Dispose();
            m_timer.Dispose();
        }
        #endregion

        #region ==================== Static Method ====================

        /// <summary>
        /// Start a watch
        /// </summary>
        /// <param name="updateProcess"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        static public FileWatchHelper StartWatching(FileUpdate updateProcess, string filePath)
        {
            //new FileWatchHelper(updateProcess, filePath, fileName);
            return new FileWatchHelper(updateProcess, filePath);
        }

        #endregion

    }
}
