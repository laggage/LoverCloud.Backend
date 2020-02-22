namespace LoverCloud.Api.Controllers
{
    using AutoMapper;
    using LoverCloud.Core.Interfaces;
    using LoverCloud.Core.Models;
    using LoverCloud.Infrastructure.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/lovers")]
    [Authorize]
    public class LoverController : ControllerBase
    {
        private readonly UserManager<LoverCloudUser> _userManager;
        private readonly ILoverRepository _loverRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
          
        public LoverController(UserManager<LoverCloudUser> userManager,
            ILoverRepository loverRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userManager = userManager;
            _loverRepository = loverRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
