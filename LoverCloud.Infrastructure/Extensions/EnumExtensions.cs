namespace LoverCloud.Infrastructure.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum source)
        {
            Type type = source.GetType();
            var memberInfos = type.GetMember(source.ToString());
            if (memberInfos != null && memberInfos.Length > 0)
            {
                return (memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[])?.FirstOrDefault()?.Description;
            }

            return source.ToString();
        }
    }
}
