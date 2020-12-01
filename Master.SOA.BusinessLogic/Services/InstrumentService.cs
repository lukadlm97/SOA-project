using AutoMapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.DataTransferObjects;
using Master.SOA.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Services
{
    public class InstrumentService : IInstrumentService
    {
        private readonly IDataRepository<TickDto> _repository;
        private readonly IMapper _mapper;

        public InstrumentService(IDataRepository<TickDto> repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

        public async Task<IEnumerable<Tick>> GetTicks(int symbolId)
        {
            var ticks = await GetAllTicks();

            return ticks.Where(x => x.Instrument.Id == symbolId);
        }

        private async Task<IEnumerable<Tick>> GetAllTicks()
        {
            var ticks = await _repository.GetAll();

            return _mapper.Map<IEnumerable<Tick>>(ticks);
        }
    }
}