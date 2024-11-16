using AutoMapper;
using CreditCardAPI.DTOs;
using CreditCardAPI.Models;

namespace CreditCardAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapear de entidad CreditCardHolder a DTO CreditCardStatementDTO
            CreateMap<CreditCardHolder, CreditCardStatementDTO>()
                .ForMember(dest => dest.CardHolderName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => src.CardNumber))
                .ForMember(dest => dest.CreditLimit, opt => opt.MapFrom(src => src.CreditLimit))
                .ForMember(dest => dest.CurrentBalance, opt => opt.MapFrom(src => src.CurrentBalance))
                .ForMember(dest => dest.AvailableBalance, opt => opt.MapFrom(src => src.AvailableBalance));

            // Mapear de DTO AddTransactionDTO a entidad Transaction
            CreateMap<AddTransactionDTO, Transaction>()
                //.ForMember(dest => dest.CardHolderID, opt => opt.MapFrom(src=>src.CardHolderID))
                .ForMember(dest => dest.TransactionDate, opt => opt.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
        }
    }
}
