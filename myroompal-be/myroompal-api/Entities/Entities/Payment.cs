using Microsoft.EntityFrameworkCore;

namespace myroompal_api.Entities.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public required DateTime PaidDate { get; set; }
    [Precision(10, 2)]
    public required decimal PaidAmount { get; set; }

    public Guid? RentId { get; set; }
    public Rent? Rent { get; set; }




}