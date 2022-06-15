using ItPos.Domain.DTO.V1.Form;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models;
using Mapster;

namespace ItPos.Domain.Mappings;

public class FormMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FormRequest, Form>()
            .GenerateMapper(MapType.Projection)
            .Ignore(x => x.Id!)
            .Compile();
    }
}