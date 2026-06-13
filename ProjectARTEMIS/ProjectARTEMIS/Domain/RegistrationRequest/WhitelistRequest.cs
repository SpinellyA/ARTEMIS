using ProjectARTEMIS.Domain.Enums;


    public class WhitelistRequest
    {
        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string RealName { get; private set; }
        public Guid SchoolId { get; private set; }
        public string FacebookUrl { get; private set; } // will be facebook.
        public string Message { get; private set; }

        private WhitelistRequest()
        {

        }

        private List<WhitelistRequestStatus> _statuses = new();
        public IReadOnlyCollection<WhitelistRequestStatus> Statuses => _statuses.AsReadOnly();


        public static WhitelistRequest Create(Guid userId, Guid schoolId, string realName, string socialUrl, string message)
        {
            if (userId == Guid.Empty)
                throw new DomainException("User ID cannot be empty.");
            if (schoolId == Guid.Empty)
                throw new DomainException("School ID cannot be empty.");

            if (socialUrl == null) throw new DomainException("Social URL cannot be null.");
            var request = new WhitelistRequest
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                SchoolId = schoolId,
                FacebookUrl = socialUrl,
                Message = message,
                RealName = realName,
            };
            request.EnsurePending();

            return request;
        }

        private void EnsurePending()
        {
            if(!_statuses.Any())
            {
                var pStatus = WhitelistRequestStatus.Create(this.Id, RegistrationStatusType.Pending, "This request is pending.");
                _statuses.Add(pStatus);
                return;
            }

            var status = GetCurrentStatus();
            if (status == null) throw new DomainException("Status not found!");
            if (status.Status != RegistrationStatusType.Pending) throw new DomainException("Status is not pending!");
        }
        private void EndCurrentStatus()
        {
            var status = GetCurrentStatus();
            if (status == null) throw new DomainException("Status is null! (it shouldnt be...)");
            status.EndStatus();
        }
        private WhitelistRequestStatus? GetCurrentStatus() => _statuses.FirstOrDefault(x => x.EndTime == null);
        public void Accept(string? message = null)
        {
            EnsurePending();
            EndCurrentStatus();

            var status = WhitelistRequestStatus.Create(Id, RegistrationStatusType.Accepted, message);
            _statuses.Add(status);
        }

        public void Reject(string? reason = null)
        {
            EnsurePending();
            EndCurrentStatus();

            var status = WhitelistRequestStatus.Create(Id, RegistrationStatusType.Rejected, reason);
            _statuses.Add(status);
        }


    }

