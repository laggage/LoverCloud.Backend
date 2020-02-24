namespace LoverCloud.Core.Models
{
    using LoverCloud.Core.Interfaces;
    using System;

    /// <summary>
    /// 情侣请求, 请求和他/她成为情侣
    /// </summary>
    public class LoverRequest : IEntity
    {
        public LoverRequest()
        {
            Id = Guid.NewGuid().ToString();
            Succeed = null;
        }

        public string Id { get; set; }
        /// <summary>
        /// 情侣请求发起方
        /// </summary>
        public virtual LoverCloudUser Requester { get; set; }
        public string RequesterId { get; set; }
        /// <summary>
        /// 情侣请求接收方/被请求方
        /// </summary>
        public virtual LoverCloudUser Receiver { get; set; }
        public string ReceiverId { get; set; }

        public DateTime RequestDate { get; set; }
        /// <summary>
        /// 情侣请求是否成功
        /// </summary>
        public bool? Succeed { get; set; }
        /// <summary>
        /// 情侣请求成功后该请求属于的情侣
        /// </summary>
        public virtual Lover Lover { get; set; }
        public string LoverId { get; set; }
    }
}
