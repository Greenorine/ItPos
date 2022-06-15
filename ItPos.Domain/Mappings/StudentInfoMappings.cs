using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models;
using Mapster;

namespace ItPos.Domain.Mappings;

public class StudentInfoMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<StudentInfo, StudentInfoShortResponse>()
            .Map(dest => dest.Institute, src => src.User.Group)
            .CompileProjection();
        
        config.NewConfig<StudentInfo, StudentInfoResponse>()
            .Map(dest => dest.Institute, src => src.User.Group)
            .Map(dest => dest.Login, src => src.User.Login)
            .Map(dest => dest.Password, src => src.User.Password)
            .Compile();
        
        config.NewConfig<StudentInfoRequest, StudentInfo>()
            .GenerateMapper(MapType.Projection)
            .Ignore(x => x.Id!)
            .Compile();
    }
}