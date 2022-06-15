using ItPos.Domain.DTO.V1.Offer;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models;
using Mapster;

namespace ItPos.Domain.Mappings;

public class OfferMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<OfferRequest, Offer>()
            .GenerateMapper(MapType.Projection)
            .Ignore(x => x.Id!)
            .Compile();
    }
}