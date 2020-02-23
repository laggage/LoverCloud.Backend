namespace LoverCloud.Api.Authorizations
{
    using Microsoft.AspNetCore.Authorization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 对于用户信息的查询, 非本人只允许获得指定的用户信息字段
    /// </summary>
    public class LoverCloudUserFieldsRequirement : IAuthorizationRequirement
    {
        public readonly List<string> AllowedFields;

        public LoverCloudUserFieldsRequirement()
        {
            AllowedFields = new List<string>();
        }

        public LoverCloudUserFieldsRequirement(string allowedFields) : this()
        {
            string[] splitedFields = allowedFields.Split(',');
            foreach (string field in splitedFields)
            {
                string trimmedField = field.Trim();
                AllowedFields.Add(trimmedField);
            }
        }

        public LoverCloudUserFieldsRequirement(params string[] allowedFields) : this()
        {
            AllowedFields.AddRange(allowedFields);
        }
    }

    internal class LoverCloudUserFieldsHandler : AuthorizationHandler<LoverCloudUserFieldsRequirement, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoverCloudUserFieldsRequirement requirement, string resource)
        {
            string[] fields = resource.Split(',');
            bool result = true;

            ParallelLoopResult loopResult = Parallel.ForEach(fields, (f, s) =>
            {
                string splitedField = f.Trim();
                if (!(requirement.AllowedFields?.Any(
                    x => x.Equals(splitedField,
                    StringComparison.OrdinalIgnoreCase))
                ?? false))
                {
                    result = false;
                    s.Break();
                }
            });
            //while (!loopResult.IsCompleted) ;
            if (result)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
