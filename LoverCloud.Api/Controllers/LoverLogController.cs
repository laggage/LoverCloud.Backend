namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Api.Extensions;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Resources;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Authorize]
    [Route("api/lovers/log")]
    public class LoverLogController:ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoverLogRepository _repository;
        private readonly ILoverRepository _loverRepository;

        public LoverLogController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoverLogRepository repository,
            ILoverRepository loverRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = repository;
            this._loverRepository = loverRepository;
        }

        /// <summary>
        /// 情侣日志Api接口
        /// </summary>
        /// <returns>情侣日志</returns>
        [HttpGet("logs")]
        public async Task<IActionResult> GetLoverLogs()
        {
            var loverCloudUserId = this.GetUserId();
            var result = await _repository.GetByUserIdAsync(loverCloudUserId);
            return Ok(result);
        }

        [HttpPost("logs", Name = "CreateLoverLog")]
        public async Task<IActionResult> CreateLoverLog([FromBody]LoverLogAddResource addResource)
        {
            var lover = await _loverRepository.FindByUserIdAsync(this.GetUserId());
            if (lover == null) return NoContent();

            var loverLog = _mapper.Map<LoverLog>(addResource);
            loverLog.Lover = lover;
            loverLog.CreateDateTime = DateTime.Now;
            loverLog.LastUpdateTime = DateTime.Now;
            if (loverLog == null)
                return NoContent();
            _repository.Add(loverLog);
            var res = await _unitOfWork.SaveChangesAsync();
            if (!res) return NoContent();
            var loverLogResource = _mapper.Map<LoverLogResource>(loverLog);
            return CreatedAtRoute("CreateLoverLog", new { loverLog.Id }, loverLogResource);
        }
    }
}
