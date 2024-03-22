using ASP_.NET.Models;
using ASP_.NET.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITasksPay _tasksPay;

        public UserController(ITasksPay tasksPay)
        {
            _tasksPay = tasksPay;
        }

        // GET api/user/id
        [HttpGet("{id}")]
        public ActionResult<UserFeatures> GetUser(int id)
        {
            var user =  _tasksPay.GetUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // POST api/user
        [HttpPost("register")]
        public async Task<ActionResult<UserFeatures>> RegisterUser([FromBody] UserFeatures newUser)
        {

            var registerResult = _tasksPay.Register(newUser);
            if (!registerResult)
            {
                return BadRequest("User registration failed");
            }

            return Ok("Successful Register");
        }

        // POST api/user/lendCredit
        [HttpPost("lendCredit")]
        public async Task<ActionResult> LendCredit( decimal amount, int userId, string method)
        {
            var success = _tasksPay.lendCredit(amount, userId, method);
            if (success)
            {
                return Ok("Credit lend successfully");
            }
            return BadRequest("Failed to lend credit, verify your credit cash or your avalaible accounts.");
        }

        // POST api/user/pay
        [HttpPost("pay")]
        public async Task<ActionResult> Pay(int userId, decimal amount, string method)
        {
            var success = _tasksPay.Pay(userId, amount, method);
            if (success!="Null")
            {
                return Ok("Payment successful\n" + success);
            }
            return BadRequest("Payment failed");
        }

        // POST api/user/recharge
        [HttpPost("recharge")]
        public async Task<ActionResult> Recharge(int userId, decimal amount, string method, string methodToRecharge)
        {
            var success =  _tasksPay.Recharge(userId, amount, method, methodToRecharge);
            if (success != null)
            {
                return Ok("Recharge successful\n "+ success);
            }
            return BadRequest("Recharge failed");
        }
    }
}
