using System;

using AutoMapper;
using PFMBackendAPI.Database.Entities;
using PFMBackendAPI.Models;

namespace PFMBackendAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<TransactionEntity, Transaction>()
                .ForMember(t => t.TransactionId, e => e.MapFrom(x => x.TransactionId));

            CreateMap<Transaction, TransactionEntity>()
                .ForMember(e => e.TransactionId, t => t.MapFrom(x => x.TransactionId));

            CreateMap<PagedSortedList<TransactionEntity>, PagedSortedList<Transaction>>();

            CreateMap<CategoryEntity, Category>()
              .ForMember(c => c.CodeId, e => e.MapFrom(x => x.Code));

            CreateMap<Category, CategoryEntity>()
                .ForMember(e => e.Code, c => c.MapFrom(x => x.CodeId));

        }
    }
}

