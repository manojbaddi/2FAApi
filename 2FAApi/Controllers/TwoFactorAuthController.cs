using _2FAApi.Configuration;
using _2FAApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace _2FAApi.Controllers
{
    [Route("api/2fa")]
    [ApiController]
    public class TwoFactorAuthController : Controller
    {
        private readonly TwoFactorAuthConfig _config;
        private readonly ConcurrentDictionary<string, List<string>> _activeCodes;

        public TwoFactorAuthController(IOptions<TwoFactorAuthConfig> config)
        {
            _config = config.Value;
            _activeCodes = new ConcurrentDictionary<string, List<string>>();
        }

        //[Authorize] -> Uncommented to test. Comment if needed
        [HttpPost("send-code")]
        public IActionResult SendConfirmationCode([FromBody] SendCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Phone))
            {
                return BadRequest("Phone number is required.");
            }

            // Check if the number of active codes for this phone is greater than the allowed limit
            if (_activeCodes.TryGetValue(request.Phone, out var activeCodes)
                && activeCodes.Count >= _config.MaxConcurrentCodesPerPhone)
            {
                return BadRequest("Too many active codes for this phone number.");
            }

            
            var confirmationCode = GenerateRandomCode();
            Console.WriteLine($"Confirmation code for {request.Phone}: {confirmationCode}");

            // Store the code with the phone number
            _activeCodes.AddOrUpdate(request.Phone, new List<string> { confirmationCode }, (_, codes) =>
            {
                codes.Add(confirmationCode);
                return codes;
            });

            return Ok(new { CodeSent = true });
        }

        [Authorize]
        [HttpPost("check-code")]
        public IActionResult CheckConfirmationCode([FromBody] CheckCodeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Phone) || string.IsNullOrWhiteSpace(request.Code))
            {
                return BadRequest("Phone number and code are required.");
            }

            if (_activeCodes.TryGetValue(request.Phone, out var activeCodes) && activeCodes.Contains(request.Code))
            {
                // if Code is valid, remove it from the active codes
                activeCodes.Remove(request.Code);
                return Ok(new { CodeValid = true });
            }

            return BadRequest("Invalid code or expired.");
        }

        private string GenerateRandomCode()
        {
            // Generate a random 6-digit code
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }


    }
}
