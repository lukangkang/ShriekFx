﻿using System;
using System.Threading.Tasks;

namespace Shriek.WebApi.Proxy
{
    /// <summary>
    /// 表示回复处理抽象类
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class ApiReturnAttribute : Attribute
    {
        /// <summary>
        /// 获取异步结果
        /// </summary>
        /// <param name="context">上下文</param>
        /// <returns></returns>
        public abstract Task<object> GetTaskResult(ApiActionContext context);
    }
}