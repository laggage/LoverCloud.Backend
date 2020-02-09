namespace LoverCloud.Infrastructure.Repositories
{
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Database;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MenstruationDescriptionRepository: IMenstruationDescriptionRepository
    {
        private readonly LoverCloudDbContext _dbContext;

        public MenstruationDescriptionRepository(LoverCloudDbContext dbContext)
        {
            _dbContext=dbContext;
        }

        public void Add(MenstruationDescription entity)
        {
            _dbContext.MenstruationDescriptions.Add(entity);
        }

        public void Delete(MenstruationDescription entity)
        {
            _dbContext.MenstruationDescriptions.Remove(entity);
        }

        public void Update(MenstruationDescription entity)
        {
            _dbContext.MenstruationDescriptions.Update(entity);
        }

        public Task<MenstruationDescription> FindByIdAsync(string id)
        {
            return _dbContext.MenstruationDescriptions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<MenstruationDescription> FindByIdAsync(
            string id, Func<DbSet<MenstruationDescription>, IQueryable<MenstruationDescription>> configInclude)
        {
            IQueryable<MenstruationDescription> query = configInclude?.Invoke(_dbContext.MenstruationDescriptions) ?? _dbContext.MenstruationDescriptions;
            return query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PaginatedList<MenstruationDescription>> GetAsync(
            string userId, string menstruationLogId,　MenstruationDescriptionParameters parameters)
        {
            IQueryable<MenstruationDescription> descriptions = _dbContext.MenstruationDescriptions.Where(
                x => x.MenstruationLog.Id == menstruationLogId && 
                x.MenstruationLog.LoverCloudUser.Id == userId);

            IQueryable<MenstruationDescription> query = descriptions.Skip(
                parameters.PageSize * (parameters.PageIndex - 1))
                .Take(parameters.PageSize);

            return new PaginatedList<MenstruationDescription>(
                parameters.PageIndex, parameters.PageSize, await descriptions.CountAsync(), await query.ToListAsync());
        }

        /// <summary>
        /// 获取姨妈描述翻页集合
        /// </summary>
        /// <param name="userId">拥有本资源的用户Id</param>
        /// <param name="menstruationLogId">姨妈日志Id</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="configInclude">配置数据库表连接</param>
        /// <returns></returns>
        public async Task<PaginatedList<MenstruationDescription>> GetAsync(
            string userId, string menstruationLogId, MenstruationDescriptionParameters parameters, 
            Func<DbSet<MenstruationDescription>, IQueryable<MenstruationDescription>> configInclude = null)
        {
            IQueryable<MenstruationDescription> descriptions = (configInclude?.Invoke(
                _dbContext.MenstruationDescriptions) ?? _dbContext.MenstruationDescriptions)
                .Where(x => userId == x.MenstruationLog.LoverCloudUser.Id && x.MenstruationLog.Id == menstruationLogId);

            IQueryable<MenstruationDescription> query = descriptions.Where(
                x => parameters.Date.HasValue ? x.Date.Date == parameters.Date.Value.Date : true)
                .Skip(parameters.PageSize*(parameters.PageIndex-1))
                .Take(parameters.PageSize);

            return new PaginatedList<MenstruationDescription>(
                parameters.PageIndex, parameters.PageSize, await descriptions.CountAsync(), await query.ToListAsync());
        }
    }
}
