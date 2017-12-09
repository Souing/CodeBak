using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Yst.Framework.Cache.Helper;
using Com.Yst.Framework.Utils;

namespace Com.Yst.Framework.Handle
{
    /// <summary>
    /// 功能描述：ResponseCacheHandle 
    /// 创 建 者：panzy  2015/6/2 10:55:57
    /// 审 查 者：        审查时间:       
    /// </summary>
    public class ResponseCacheHandle : DelegatingHandler
    {
        /// <summary>
        /// 把请求的报文和返回的报文保存到日志中
        /// 用法：
        /// Gloab中 GlobalConfiguration.Configuration.MessageHandlers.Add(new CustomMessageHandler());
        /// </summary>
        /// <param name="request">请求信息</param>
        /// <param name="cancellationToken">取消操作的标记</param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            string md5 = null;
            //请求的内容作为缓存key
            if (request.Content != null)
            {
                string reqStr;
                string method = request.Method.Method;
                if (method.ToLower().Equals("post"))
                {
                    string uri = request.RequestUri.ToString();
                    string body = request.Content.ReadAsStringAsync().Result;
                    reqStr = string.Format("{0}{1}", uri, body);
                }
                else
                {
                    reqStr = request.RequestUri.ToString();
                }
                md5 = Md5Utils.GetMD5(reqStr);
            }

            //是否有缓存
            var result = CacheFactory.GetCacheManage().Get<string>(md5);
            if (!string.IsNullOrEmpty(result))
            {
                // 创建响应。
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(result)
                };

                // Note: TaskCompletionSource creates a task that does not contain a delegate. 
                // 注：TaskCompletionSource创建一个不含委托的任务。
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);   // Also sets the task state to "RanToCompletion" 
                // 也将此任务设置成“RanToCompletion（已完成）”
                return tsc.Task; 
            }
            else
            {   //如果缓存没有命中，则继续接口查询
                return base.SendAsync(request, cancellationToken).ContinueWith(
                    task =>
                    {
                        //把返回的json放到缓存
                        string json = task.Result.Content.ReadAsStringAsync().Result;
                        //缓存5分钟
                        CacheFactory.GetCacheManage().Set(md5, json, new TimeSpan(0, 0, 5));
                        return task.Result;
                    }, cancellationToken);
            }
        }

    }
}
