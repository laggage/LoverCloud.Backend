namespace LoverCloud.Core.Extensions
{
    using LoverCloud.Core.Interfaces;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class EntityComparer : IEqualityComparer<IEntity>
    {
        public bool Equals([AllowNull] IEntity x, [AllowNull] IEntity y)
        {
            if (x == null || y == null) return false;
            return string.Equals(x.Guid, y.Guid);
        }

        public int GetHashCode([DisallowNull] IEntity obj)
        {
            return obj.Guid.GetHashCode();
        }

        public static EntityComparer Default => new EntityComparer();
    }
}
