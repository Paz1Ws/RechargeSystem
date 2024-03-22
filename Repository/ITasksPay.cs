using ASP_.NET.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET.Repository
{
    public interface ITasksPay
    {
        public bool Register([FromBody] UserFeatures newUser);
        public UserFeatures GetUser(int id);
        public string Recharge(int userId, decimal amount, string method, string methodToRecharge);
        public string Pay(int userId, decimal amount, string method);
        public bool lendCredit(decimal amount, int userId, string method);
        public string rechargeTicket(string Name, decimal amount, string method, string methodToRecharge);
        public string payTicket(string Name, decimal amount, string method);
    }
}
