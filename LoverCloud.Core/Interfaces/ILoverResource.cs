namespace LoverCloud.Core.Interfaces
{
    using LoverCloud.Core.Models;

    public interface ILoverResource
    {
        string LoverId { get; set; }
        Lover Lover { get; set; }
    }
}
