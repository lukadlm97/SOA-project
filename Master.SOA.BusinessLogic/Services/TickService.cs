using Master.SOA.BusinessLogic.Contracts;
using Master.SOA.Domain.DataTransferObjects;
using Master.SOA.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Services
{
    public class TickService : IDataService<Tick>
    {
        private readonly IDataRepository<TickDto> _repository;

        public TickService(IDataRepository<TickDto> repository)
        => (_repository) = (repository);

        public Task<bool> Create(Tick obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tick>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Tick> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, Tick obj)
        {
            throw new NotImplementedException();
        }
    }
}