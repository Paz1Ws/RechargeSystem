using ASP_.NET.Models; // Assuming User and PaymentMethod models are here
using ASP_.NET.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RechargeService.Context;

namespace Repository
{
    public class TasksPay : ITasksPay
    {
        public readonly PaymentsContext _context;
        public TasksPay(PaymentsContext context)
        {
            _context = context;
        }
        public UserFeatures GetUser(int userId)
        {
            return _context.User
              .Include(u => u.PaymentMethods)
              .FirstOrDefault(u => u.Id == userId);
        }

        public bool lendCredit(decimal amount, int userId, string method)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                if (user.Credit - amount < 50000 && user.Credit - amount >= 0)
                {
                    foreach (var existingMethod in user.PaymentMethods)
                    {
                        if (existingMethod.Method.ToUpper() == method.ToUpper())
                        {
                            existingMethod.Amount += amount;
                            user.Credit -= amount;
                            user.Bullet += amount;
                            _context.SaveChanges();
                            return true;
                        }
                    }
                   
                }
            }
            return false;
        }

        public string Pay(int userId, decimal amount, string method)
        {
            var user = GetUser(userId);
            if (user != null)
            {
                if (user.PaymentMethods.Count != 0)
                {
                    var paymentMethod = user.PaymentMethods.FirstOrDefault(pm => pm.Method.Equals(method, StringComparison.OrdinalIgnoreCase));
                    if (paymentMethod != null && paymentMethod.Amount >= amount)
                    {
                        paymentMethod.Amount -= amount;
                        user.Bullet -= amount;
                        user.Credit += amount;
                        _context.SaveChanges();
                        return payTicket(user.Name, amount, method); 
                    }
                }
            }
            return "Null";
        }


        public string payTicket(string Name, decimal amount, string method)
        {
            var payTicket =
                "User " + Name +
                " have paid " + amount +
                " by " + method +
                " of their credit sucessfuly";
            return payTicket;
        }

        public string Recharge(int userId, decimal amount, string method, string methodToRecharge)
        {
            var user = GetUser(userId);

            if (user != null)
            {
                dynamic paymentMethod = user.PaymentMethods.FirstOrDefault(pm => pm.Method.ToUpper() == method.ToUpper());
                if (paymentMethod.Amount >= amount)
                {
                    if (method.ToUpper() == "BULLET" && user.Bullet >= amount)
                    {
                        user.Bullet -= amount;
                        var rechargeByBullet = user.PaymentMethods.FirstOrDefault(x => x.Method.ToUpper() == methodToRecharge.ToUpper());
                        if (rechargeByBullet == null)
                            user.PaymentMethods.Add(new PaymentMethod { Method = methodToRecharge.ToUpper(), Amount = amount });
                        else
                            rechargeByBullet.Amount += amount;
                    }
                    else
                    {
                        paymentMethod.Amount -= amount;
                        user.PaymentMethods.Add(new PaymentMethod { Method = methodToRecharge.ToUpper(), Amount = amount });
                    }
                    _context.SaveChanges();
                    return rechargeTicket(user.Name, amount, method.ToUpper(), methodToRecharge);
                }
            }
            return null;
        }

        public string rechargeTicket(string Name, decimal amount, string method, string methodToRecharge)
        {
            var user = _context.User.FirstOrDefault(x => x.Name == Name);
            if (user != null)
            {
                var historyEntry =
                    "User " + Name +
                    " recharged " + amount +
                    " by " + method +
                    " to their another account " + methodToRecharge +
                    " RechargeDate: " + DateTime.Now;
                return historyEntry;
            }
            return null;
        }

        public bool Register([FromBody] UserFeatures newUser)
        {
            if (_context.User.FirstOrDefault(x => x.Email == newUser.Email) == null)
            {
                foreach (var paymentMethod in newUser.PaymentMethods)
                {
                    var paymentMethodVerify = paymentMethod;
                    if (paymentMethod != paymentMethodVerify)
                    {
                        newUser.PaymentMethods.Add(paymentMethod);
                        break;
                    }

                }
                _context.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


    }
}
