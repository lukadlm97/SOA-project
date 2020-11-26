using AutoMapper;
using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.DataTransferObjects;
using Master.SOA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Services
{
    public class TickService : IDataService<Tick>
    {
        private readonly IDataRepository<TickDto> _repository;
        private readonly IMapper _mapper;

        public TickService(IDataRepository<TickDto> repository, IMapper mapper)
        => (_repository, _mapper) = (repository, mapper);

        public Task<bool> Create(Tick obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Tick>> GetAll()
        {
            var ticks = await _repository.GetAll();

            var completed = _mapper.Map<IEnumerable<Tick>>(ticks);

            return completed;
        }

        public async Task<Tick> GetById(int id)
        {
            var quotas = await _repository.GetAll();

            var quota = quotas.FirstOrDefault(x => x.Id == id);

            return _mapper.Map<Tick>(quota);
        }

        public Task<bool> Update(int id, Tick obj)
        {
            throw new NotImplementedException();
        }
    }
}