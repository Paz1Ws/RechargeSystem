using RechargeService.Context;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASP_.NET.Models
{
    public class UserFeatures
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Phone { get; set; }
        public decimal Bullet { get; set; }
        public decimal Credit { get; set; }
        public string rechargeHistory {get; set;}
        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public decimal Amount { get; set; }
    }
}
