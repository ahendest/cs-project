namespace cs_project.Core.DTOs
{
    public class StationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}
