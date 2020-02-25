namespace LoverCloud.Api.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    static class UrlHelperExtensions
    {
        /// <summary>
        /// 根据指定的 routeName 生成相对url
        /// </summary>
        /// <param name="url"> <see cref="IUrlHelper"/>  </param>
        /// <param name="routeName"> 路由名称 </param>
        /// <param name="value"> 路由参数 </param>
        /// <returns> 相对url </returns> 
        public static string LinkRelative(this IUrlHelper url, string routeName, object value)
        {
            var uri = new Uri(url.Link(routeName, value), UriKind.Absolute);
            return uri.PathAndQuery;
        }
    }
}
