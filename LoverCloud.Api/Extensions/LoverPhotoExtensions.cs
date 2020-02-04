namespace LoverCloud.Api.Extensions
{
    using LoverCloud.Core.Models;
    using System;
    using System.Collections.Generic;

    public static class LoverPhotoExtensions
    {
        public static void DeletePhysicalFiles(this IEnumerable<LoverPhoto> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            foreach (LoverPhoto loverPhoto in source)
            {
                loverPhoto.DeletePhyicalFile();
            }
        }
    }
}
