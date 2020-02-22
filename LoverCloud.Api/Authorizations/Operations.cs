namespace LoverCloud.Api.Authorizations
{
    using Microsoft.AspNetCore.Authorization.Infrastructure;

    public static class Operations
    {
        public static OperationAuthorizationRequirement Create => new OperationAuthorizationRequirement
        {
            Name = "LoverResource.Create"
        };
        public static OperationAuthorizationRequirement Delete => new OperationAuthorizationRequirement
        {
            Name = "LoverResource.Delete"
        };
        public static OperationAuthorizationRequirement Update => new OperationAuthorizationRequirement
        {
            Name = "LoverResource.Update"

        };
        public static OperationAuthorizationRequirement Read => new OperationAuthorizationRequirement
        {
            Name = "LoverResource.Read"
        };
    }
}
