using AutoMapper;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Enums;

namespace Pymex.Payroll.Mappers
{
    public class MappersProfile : Profile
    {
        public MappersProfile()
        {
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<Position, PositionDto>().ReverseMap();

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract.Name))
                .ReverseMap()
                .ForMember(dest => dest.Department, opt => opt.Ignore())
                .ForMember(dest => dest.Position, opt => opt.Ignore())
                .ForMember(dest => dest.Contract, opt => opt.Ignore());

            CreateMap<Contract, ContractDto>().ReverseMap();

            CreateMap<PayrollVariable, PayrollVariableDto>()
                .ForMember(dest => dest.DataTypeName, opt => opt.MapFrom(src => src.DataType.ToString()))
                .ForMember(dest => dest.BehaviorName, opt => opt.MapFrom(src => src.Behavior.ToString()))
                .ForMember(dest => dest.CoinName, opt => opt.MapFrom(src => src.Coin != null ? src.Coin.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.Coin, opt => opt.Ignore());

            CreateMap<PayrollConcept, PayrollConceptDto>()
                .ForMember(dest => dest.ConceptTypeName, opt => opt.MapFrom(src => src.ConceptType.ToString()))
                .ReverseMap();

            CreateMap<ContractConcept, ContractConceptDto>()
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract.Name))
                .ForMember(dest => dest.PayrollConceptName, opt => opt.MapFrom(src => src.PayrollConcept.Name));

            CreateMap<ContractConceptDto, ContractConcept>()
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.PayrollConcept, opt => opt.Ignore());

            CreateMap<ContractVariable, ContractVariableDto>()
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract!.Name))
                .ForMember(dest => dest.VariableCode, opt => opt.MapFrom(src => src.PayrollVariable!.Code))
                .ForMember(dest => dest.VariableName, opt => opt.MapFrom(src => src.PayrollVariable!.Name))
                .ForMember(dest => dest.DataType, opt => opt.MapFrom(src => (int)src.PayrollVariable!.DataType))
                .ForMember(dest => dest.Behavior, opt => opt.MapFrom(src => (int)src.PayrollVariable!.Behavior))
                .ForMember(dest => dest.CoinId, opt => opt.MapFrom(src => src.PayrollVariable!.CoinId))
                .ForMember(dest => dest.CoinName, opt => opt.MapFrom(src => src.PayrollVariable!.Coin!.Name))
                .ReverseMap()
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.PayrollVariable, opt => opt.Ignore());

            CreateMap<PayrollNovelty, PayrollNoveltyDto>()
                .ForMember(dest => dest.ContractName, opt => opt.MapFrom(src => src.Contract!.Name))
                .ForMember(dest => dest.VariableCode, opt => opt.MapFrom(src => src.PayrollVariable!.Code))
                .ForMember(dest => dest.VariableName, opt => opt.MapFrom(src => src.PayrollVariable!.Name))
                .ReverseMap()
                .ForMember(dest => dest.Contract, opt => opt.Ignore())
                .ForMember(dest => dest.PayrollVariable, opt => opt.Ignore());

            CreateMap<Coins, CoinsDto>().ReverseMap();
            CreateMap<CoinQuotations, CoinQuotationDto>()
                .ForMember(dest => dest.CoinName, opt => opt.MapFrom(src => src.Coin!.Name))
                .ForMember(dest => dest.CoinSymbol, opt => opt.MapFrom(src => src.Coin!.Symbol))
                .ReverseMap()
                .ForMember(dest => dest.Coin, opt => opt.Ignore());
        }
    }
}
