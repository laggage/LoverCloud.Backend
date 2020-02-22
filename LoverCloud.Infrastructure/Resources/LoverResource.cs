namespace LoverCloud.Infrastructure.Resources
{
    public class LoverResource
    {
        /// <summary>
        /// 相恋日
        /// </summary>
        public LoverAnniversaryResource LoveDay { get; set; }
        /// <summary>
        /// 结婚纪念日
        /// </summary>
        public LoverAnniversaryResource WeddingDay { get; set; }
        /// <summary>
        /// 男方是否为初恋
        /// </summary>
        public bool IsBoyFirstLove { get; set; }
        /// <summary>
        /// 女方是否为初恋
        /// </summary>
        public bool IsGirlFirstLove { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public LoverPhotoResource CoverImage { get; set; }
    }

    public class LoverUpdateResource
    {
        /// <summary>
        /// 相恋日
        /// </summary>
        public LoverAnniversaryUpdateResource LoveDay { get; set; }
        /// <summary>
        /// 结婚纪念日
        /// </summary>
        public LoverAnniversaryUpdateResource WeddingDay { get; set; }
        /// <summary>
        /// 男方是否为初恋
        /// </summary>
        public bool IsBoyFirstLove { get; set; }
        /// <summary>
        /// 女方是否为初恋
        /// </summary>
        public bool IsGirlFirstLove { get; set; }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string CoverImageId { get; set; }
    }
}
