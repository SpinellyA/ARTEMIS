
using Microsoft.EntityFrameworkCore;

public class WhitelistRequestService
{
    private readonly IUnitOfWork _uow;

    public WhitelistRequestService(IUnitOfWork uow) => _uow = uow;

    public async Task RequestWhitelistAsync(RequestWhitelistDto req)
    {
        var whitelistRequest = WhitelistRequest.Create(req.UserId, req.SchoolId, req.RealName,
                req.FacebookUrl, req.Message);
        _uow.WhitelistRequests.Add(whitelistRequest);
        await _uow.SaveChangesAsync();
    }

    public async Task AcceptWhitelistRequestAsync(AcceptWhitelistRequestDto req)
    {
        var request = await _uow.WhitelistRequests.GetById(req.WhitelistRequestId);
        if (request == null) throw new ApplicationException("Request doens't exist!");
        request.Accept();
        _uow.WhitelistRequests.Update(request);
        await _uow.SaveChangesAsync();
    }
    public async Task RejectWhitelistRequestAsync(AcceptWhitelistRequestDto req)
    {
        var request = await _uow.WhitelistRequests.GetById(req.WhitelistRequestId);
        if (request == null) throw new ApplicationException("Request doens't exist!");
        request.Reject();
        _uow.WhitelistRequests.Update(request);
        await _uow.SaveChangesAsync();
    }
    public async Task<List<WhitelistApplicationDto>> GetAllWhitelistRequestsAsync()
    {
        var requests = _uow.WhitelistRequests.GetAll();

        return await (from r in requests
                join u in _uow.Users.GetAll() on r.Id equals u.Id
                join s in _uow.Schools.GetAll() on r.SchoolId equals s.Id
                select new WhitelistApplicationDto
                {
                    Id = r.Id,
                    FacebookUrl = r.FacebookUrl,
                    Message = r.Message,
                    RealName = r.RealName,
                    School = s.Name,
                    Username = u.Username,
                    Status = r.CurrentStatus.Status.ToString()
                }).ToListAsync();
    }
    public async Task<WhitelistApplicationDto> GetByIdAsync(Guid id)
    {
        var query = from r in _uow.WhitelistRequests.GetAll()
                    join u in _uow.Users.GetAll() on r.UserId equals u.Id // Assuming r.UserId connects them
                    join s in _uow.Schools.GetAll() on r.SchoolId equals s.Id
                    where r.Id == id
                    select new WhitelistApplicationDto
                    {
                        Id = r.Id,
                        FacebookUrl = r.FacebookUrl,
                        Message = r.Message,
                        RealName = r.RealName,
                        School = s.Name,
                        Username = u.Username,
                    };

        return await query.FirstOrDefaultAsync();
    }
}

