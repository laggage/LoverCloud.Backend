namespace LoverCloud.Api.Extensions
{
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Routing;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class LoverCloudUserExtensions
    {
        /// <summary>
        /// 获取用户头像链接, 结果赋给 <see cref="LoverCloudUserResource.ProfileImageUrl"/>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="url"></param>
        public static void GetProfileImageUrl(
            this LoverCloudUserResource user, IUrlHelper url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            // 生成相对url路径, 因为生成环境可能使用反向代理
            user.ProfileImageUrl = url.LinkRelative("GetProfileImage", new { userId = user.Id });
        }

        public static void GetProfileImageUrl(
            this IEnumerable<LoverCloudUserResource> users, IUrlHelper url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            foreach (LoverCloudUserResource user in users)
                user.GetProfileImageUrl(url);
        }
    }
}
