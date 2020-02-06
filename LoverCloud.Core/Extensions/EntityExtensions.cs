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
            return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode([DisallowNull] IEntity obj)
        {
            return obj.Id.GetHashCode();
        }

        public static EntityComparer Default => new EntityComparer();
    }
}
