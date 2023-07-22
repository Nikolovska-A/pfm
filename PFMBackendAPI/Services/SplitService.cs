using System;
using AutoMapper;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Database.Repositories;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Services
{
	public class SplitService : ISplitService
	{

        ISplitRepository _splitRepository;
        IMapper _mapper;

        public SplitService(ISplitRepository splitRepository, IMapper mapper)
		{
            _splitRepository = splitRepository;
            _mapper = mapper;
		}

        public bool SplitExistByTransactionId(int transactionId)
        {
            return _splitRepository.SplitExistByTransactionId(transactionId);
        }

        public async Task<List<Split>> CreateSplits(List<Split> splits)
        {
            var entities = _mapper.Map<List<SplitEntity>>(splits);
            var result = await _splitRepository.CreateSplits(entities);
            return _mapper.Map<List<Split>>(result);
        }

        public async Task<bool> DeleteSplits(int transactionId)
        {
            return await _splitRepository.DeleteSplits(transactionId);
        }



    }
}

