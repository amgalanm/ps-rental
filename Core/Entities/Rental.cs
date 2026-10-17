namespace Core.Entities;

public class Rental : BaseEntity
{
    public int PsnAccountId { get; set; }
    public  PsnAccount PsnAccount { get; set; } = null!;
    public RentalType Type { get; set; }
    public RentalStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
}
