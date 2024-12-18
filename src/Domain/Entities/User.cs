namespace KFS.src.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }
        public string? Avatar { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Role Role { get; set; } = null!;
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Consignment> Consignments { get; set; } = new List<Consignment>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public Wallet Wallet { get; set; } = null!;
        public List<Address> Address { get; set; } = new List<Address>();
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}