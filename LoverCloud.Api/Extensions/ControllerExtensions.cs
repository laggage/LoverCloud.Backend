namespace LoverCloud.Api.Extensions
{
    using LoverCloud.Core.Extensions;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Extensions;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Security.Claims;

    internal static class ControllerExtensions
    {
        public static string GetUserId(this ControllerBase controllerBase) =>
            controllerBase.User.Claims
                .FirstOrDefault(
                    x => x.Type.Equals(
                        ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))
                ?.Value;

        public static IActionResult UserNoLoverResult(
            this ControllerBase contorllerBase, LoverCloudUser user) =>
            contorllerBase.Forbid($"用户 {user?.ToString()??string.Empty} 还没有情侣, 无法操作资源");

        public static void AddPaginationHeaderToResponse<T>(this ControllerBase controller, PaginatedList<T> list) =>
            controller.Response.Headers.Add(
                LoverCloudApiConstraint.PaginationHeaderKey,
                new StringValues(
                    JsonConvert.SerializeObject(new
                    {
                        list.PageIndex,
                        list.PageSize,
                        list.PageCount
                    }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })));

        public static IEnumerable<LinkResource> CreateLinksForResource(
            this ControllerBase controller, string getRouteName, string resourceName, string id, 
            string fields = null, string deleteRouteName = null,
            string updateRouteName = null)
        {
            IUrlHelper url = controller.Url;
            return new LinkResource[]
            {
                string.IsNullOrEmpty(getRouteName) ? new LinkResource("self", "get", url.Link(getRouteName, new {id, fields})) : null,
                string.IsNullOrEmpty(deleteRouteName) ? new LinkResource($"delete_{resourceName}", "delete", url.Link(deleteRouteName, new {id})): null,
                string.IsNullOrEmpty(updateRouteName) ? new LinkResource($"update_{resourceName}", "patch", url.Link(updateRouteName, new{ id })) : null,
            };
        }

        public static IEnumerable<LinkResource> CreatePaginationLinks(
            this ControllerBase controller, string routeName, QueryParameters parameters, 
            bool hasPrevious, bool hasNext)
        {
            var links = new List<LinkResource>
            {
                new LinkResource("current_page", "get", controller.Url.Link(routeName, parameters))
            };

            if(hasPrevious)
            {
                parameters.PageIndex--;
                links.Add(
                    new LinkResource("previous_page", "get", controller.Url.Link(routeName, parameters)));
                parameters.PageIndex++;
            }
            if (hasNext)
            {
                parameters.PageIndex--;
                links.Add(
                    new LinkResource("next_page", "get", controller.Url.Link(routeName, parameters)));
                parameters.PageIndex++;
            }
            return links;
        }

        public static IEnumerable<ExpandoObject> AddLinks(
            this IEnumerable<ExpandoObject> obj, ControllerBase controller, string fields, string resourceName, 
            string getRouteName, string deleteRouteName = null, string updateRouteName = null)
        {
            return obj.Select(x =>
                x.AddLinks(
                    controller, fields, resourceName,
                    getRouteName, deleteRouteName, updateRouteName));
        }

        public static ExpandoObject AddLinks(this ExpandoObject obj, ControllerBase controller, string fields, string resourceName,
            string getRouteName, string deleteRouteName = null, string updateRouteName = null)
        {
            string id = obj.GetOrDefault("Id");
            if (!string.IsNullOrEmpty(id))
                obj.TryAdd(
                    "links",
                    controller.CreateLinksForResource(
                        getRouteName, resourceName, id,
                        fields, deleteRouteName, updateRouteName));
            return obj;
        }
    }
}
