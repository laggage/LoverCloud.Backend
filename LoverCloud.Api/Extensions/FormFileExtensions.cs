namespace LoverCloud.Api.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class FormFileExtensions
    {
        public static async Task SaveToFileAsync(this IFormFile formFile, string filePath)
        {
            if (formFile == null) throw new ArgumentNullException(nameof(formFile));
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentOutOfRangeException(nameof(filePath));
            // 防止目录不存在, 创建目录
            string directory = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directory);   
            // 将formfile内容写入文件
            using var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            await formFile.CopyToAsync(fs);
            fs.Close();
        }

        /// <summary>
        /// 获取文件后缀名
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns>文件后缀名</returns>
        public static string GetFileSuffix(this IFormFile formFile)
        {
            if (formFile == null) throw new  ArgumentNullException(nameof(formFile));
            return formFile.FileName.Split('.').LastOrDefault();
        }
    }
}
