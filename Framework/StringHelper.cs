using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpModuleDemo.App_Code
{
    public static class StringHelper
    {
          #region 判断对象是否为空
          
          /// <summary>
          /// 判断对象是否为空，为空返回true
          /// </summary>
          /// <param name="data">要验证的对象</param>
          public static bool IsNullOrEmpty(this object data)
          {
              //如果为null
              if (data == null)
              {
                  return true;
              }
   
              //如果为""
              if (data is string)
              {
                  if (string.IsNullOrEmpty(data.ToString().Trim()))
                  {
                      return true;
                  }
              }
   
              //如果为DBNull
              if (data is DBNull)
              {
                  return true;
              }
   
              //不为空
              return false;
          }

          #endregion
    }
}