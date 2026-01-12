using AutoMapper;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWorkRepo _uow;
        protected readonly IMapper _mapper;

        protected BaseService(IUnitOfWorkRepo uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
    }
}
