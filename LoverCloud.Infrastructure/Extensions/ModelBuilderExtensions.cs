namespace LoverCloud.Infrastructure.Extensions
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class ModelBuilderExtensions
    {
        public static void SetTableName(this ModelBuilder modelBuilder, string tableName)
        {
            foreach (IMutableEntityType mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                mutableEntityType.SetTableName(tableName);
            }
        }

        public static void SetTableName<TEntity>(this EntityTypeBuilder<TEntity> builder, string tableName) where TEntity: class
        {
            //foreach (IMutableEntityType mutableEntityType in builder.Metadata.Model.GetEntityTypes())
            //{
            //    if (mutableEntityType.BaseType == null)
            //        mutableEntityType.SetTableName(tableName);
            //}
            //if(typeof(TEntity).BaseType == null)
            builder.Metadata.SetTableName(tableName);
        }
    }
}
