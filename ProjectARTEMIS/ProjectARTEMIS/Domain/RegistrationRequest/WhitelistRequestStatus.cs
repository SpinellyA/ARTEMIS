using ProjectARTEMIS.Domain.Enums;

    public class WhitelistRequestStatus
    {
        public Guid Id { get; private set; }
        public Guid RegistrationRequestId { get; private set; }

        public RegistrationStatusType Status { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public DateTime StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        private WhitelistRequestStatus()
        {
             
        }

        public static WhitelistRequestStatus Create(Guid regRequestId, RegistrationStatusType status, string? message = null)
        {
            return new WhitelistRequestStatus
            {
                Id = Guid.NewGuid(),
                RegistrationRequestId = regRequestId,
                Status = status,
                Message = message == null ? string.Empty : message,
                StartTime = DateTime.UtcNow
            };
        }
        
        public void EndStatus()
        {
            if (EndTime != null) throw new DomainException("This status is already ended!");
            EndTime = DateTime.UtcNow;
        }
    }

