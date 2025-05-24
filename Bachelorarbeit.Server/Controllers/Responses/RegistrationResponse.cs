

using Bachelorarbeit.Server.Dtos;

namespace Bachelorarbeit.Server.Controllers.Responses
{
    public class RegistrationResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static RegistrationResponse GetUserNameExistsResponse()
        {
            return new RegistrationResponse()
            {
                Message = "user already exists",
                Success = false
            };
        }
        
        public static RegistrationResponse GetUserMailExistsResponse()
        {
            return new RegistrationResponse()
            {
                Message = "Email already exists",
                Success = false
            };
        }
        public static RegistrationResponse GetSuccessfullRegistrationResponse()
        {
            return new RegistrationResponse()
            {
                Message = "User registered successfully",
                Success = true,
            };
        }
    }
}
