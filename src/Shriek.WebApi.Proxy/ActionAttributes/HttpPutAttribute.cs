﻿using System;
using System.Net.Http;

namespace Shriek.WebApi.Proxy
{
    /// <summary>
    /// 表示Put请求
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HttpPutAttribute : HttpMethodAttribute
    {
        /// <summary>
        /// Put请求
        /// </summary>
        /// <param name="path">相对路径</param>
        /// <exception cref="ArgumentNullException"></exception>
        public HttpPutAttribute(string path)
            : base(HttpMethod.Put, path)
        {
        }
    }
}