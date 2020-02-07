using LoverCloud.Core.Interfaces;

namespace LoverCloud.Infrastructure.Resources
{
    public abstract class Resource : IEntity
    {
        public string Id { get; set; }
    }
}
