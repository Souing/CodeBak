//===================================================================
// 文件名:		Singleton.cs
// 版权:		Copyright (C) 
// 描述:		通用的单一实例类基类
// 备注:		
//===================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Com.Yst.Framework.Utils
{
    public class Singleton<T> where T : class, new()
    {
        private static T _Instance;

        public static T Instance
        {
            get
            {
                Interlocked.CompareExchange<T>(ref _Instance, new T(), null);
                return _Instance;
            }
        }
    }
}
