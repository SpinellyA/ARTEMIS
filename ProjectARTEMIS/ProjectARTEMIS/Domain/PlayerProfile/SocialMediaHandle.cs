
    public class SocialMediaHandle
    {
        public Guid Id { get; private set; }
        public Guid PlayerProfileId { get; private set; }
        public Guid SocialMediaId { get; private set; }
        public string Link { get; private set; } = string.Empty;

        public bool IsMain { get; private set; }

        private SocialMediaHandle()
        {
             
        }

        public static SocialMediaHandle Create(Guid playerProfileId, Guid socialMediaId, string link, bool main = false)
        {
            if (playerProfileId == Guid.Empty) throw new DomainException("User ID cannot be empty!");
            if (socialMediaId == Guid.Empty) throw new DomainException("Social media ID cannot be empty!");
            if (string.IsNullOrEmpty(link)) throw new DomainException("Link cannot be empty!");

            return new SocialMediaHandle
            {
                Id = Guid.NewGuid(),
                Link = link,
                PlayerProfileId = playerProfileId,
                SocialMediaId = socialMediaId,
                IsMain = main,
            };
        }

        public void UpdateLink(string link)
        {
            if (string.IsNullOrEmpty(link)) throw new DomainException("Link cannot be empty!");
            Link = link;
        }
    }

