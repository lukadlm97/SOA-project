﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Master.SOA.BusinessLogic.Contracts
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll(int? count);
        Task<T> GetById(int id);
        Task<bool> Create(T obj);
        Task<bool> Update(int id, T obj);
        Task<bool> Delete(int id);
    }
}
