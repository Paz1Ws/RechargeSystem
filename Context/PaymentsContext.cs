using ASP_.NET.Models;
using Microsoft.EntityFrameworkCore;
namespace RechargeService.Context;
public class PaymentsContext : DbContext
{
    public PaymentsContext(DbContextOptions<PaymentsContext> options) : base(options)
    {
    }

    public DbSet<UserFeatures> User { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; } 

   
}
