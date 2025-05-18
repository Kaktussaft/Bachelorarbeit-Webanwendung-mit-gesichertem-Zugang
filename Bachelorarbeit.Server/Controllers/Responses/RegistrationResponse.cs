

namespace Bachelorarbeit.Server.Controllers.Responses
{
    public class RegistrationResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string? Message { get; set; }
    }
}
