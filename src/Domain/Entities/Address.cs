namespace KFS.src.Domain.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = null!;
        public string WardCode { get; set; } = null!;
        public int DistrictId { get; set; }
        public int ProvinceId { get; set; }
        public Guid UserId { get; set; }
        public List<User> User { get; set; } = new List<User>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}