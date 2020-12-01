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

        public async Task<bool> Create(Tick obj)
        {
            return await _repository.Create(_mapper.Map<TickDto>(obj));
        }

        public async Task<bool> Delete(int id)
        {
            return await _repository.Delete(id);
        }

        public async Task<IEnumerable<Tick>> GetAll(int? count=null)
        {
            var ticks = await _repository.GetAll();

            if (count == null)
            {
                return _mapper.Map<IEnumerable<Tick>>(ticks);
            }

            var selected = ticks.Take((int)count);

            return _mapper.Map<IEnumerable<Tick>>(selected);
        }

        public async Task<Tick> GetById(int id)
        {
            var quotas = await _repository.GetAll();

            var quota = quotas.FirstOrDefault(x => x.Id == id);

            if (quota == null)
                return null;

            return _mapper.Map<Tick>(quota);
        }

        public async Task<bool> Update(int id, Tick obj)
        {
            return await _repository.Update(id, _mapper.Map<TickDto>(obj));
        }
    }
}