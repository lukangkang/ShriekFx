﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shriek.WebApi.Proxy
{
    /// <summary>
    /// 表示参数为HttpContent或派生类型的特性
    /// 此特性不需要显示声明
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class HttpContentAttribute : ApiParameterAttribute
    {
        /// <summary>
        /// http请求之前
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="parameter">特性关联的参数</param>
        /// <returns></returns>
        public sealed override async Task BeforeRequestAsync(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            if (context.RequestMessage.Method == HttpMethod.Get)
            {
                return;
            }

            var httpContent = this.GetHttpContent(context, parameter);
            context.RequestMessage.Content = httpContent;
            await TaskExtensions.CompletedTask;
        }

        /// <summary>
        /// 获取http请求内容
        /// </summary>
        /// <param name="context">上下文</param>
        /// <param name="parameter">特性关联的属性</param>
        /// <returns></returns>
        protected virtual HttpContent GetHttpContent(ApiActionContext context, ApiParameterDescriptor parameter)
        {
            return parameter.Value as HttpContent;
        }
    }
}