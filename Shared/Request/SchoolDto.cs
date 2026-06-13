public record SchoolDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorCode { get; set; } = string.Empty;
}