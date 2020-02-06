namespace LoverCloud.Api.Extensions
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Security.Claims;

    public static class ControllerExtensions
    {
        public static string GetUserId(this ControllerBase controllerBase) =>
            controllerBase.User.Claims
                .FirstOrDefault(
                    x => x.Type.Equals(
                        ClaimTypes.NameIdentifier, StringComparison.OrdinalIgnoreCase))
                ?.Value;
    }
}
