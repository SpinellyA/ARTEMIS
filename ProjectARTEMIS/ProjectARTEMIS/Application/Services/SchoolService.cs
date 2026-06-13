
using Microsoft.EntityFrameworkCore;

public class SchoolService
{
    private readonly IUnitOfWork _uow;

    public SchoolService(IUnitOfWork uow) => _uow = uow;

    public async Task<List<SchoolDto>> GetSchoolsAsync() 
    {
        var schools = _uow.Schools.GetAll();

        return await schools.Select(s => new SchoolDto
        {
            Id = s.Id,
            Name = s.Name,
            ColorCode = s.ColorCode
        }).ToListAsync();
    }
}
