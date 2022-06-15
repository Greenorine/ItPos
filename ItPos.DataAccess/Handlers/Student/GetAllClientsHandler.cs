﻿using ItPos.DataAccess.User;
using ItPos.Domain.DTO.V1.StudentInfo;
using ItPos.Domain.Models;
using ItPos.Domain.Models.User;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ItPos.DataAccess.Handlers.Student;

public record GetAllStudents : IRequest<List<StudentInfoShortResponse>>;

public class GetAllClientsHandler : IRequestHandler<GetAllStudents, List<StudentInfoShortResponse>>
{
    private readonly ItPosDbContext context;

    public GetAllClientsHandler(ItPosDbContext context)
    {
        this.context = context;
    }

    public async Task<List<StudentInfoShortResponse>> Handle(GetAllStudents request,
        CancellationToken cancellationToken)
    {
        return await context.Students
            .AsNoTracking()
            .Include(x => x.User)
            .Where(c => !c.IsDeleted)
            .ProjectToType<StudentInfoShortResponse>()
            .ToListAsync(cancellationToken);
    }
}