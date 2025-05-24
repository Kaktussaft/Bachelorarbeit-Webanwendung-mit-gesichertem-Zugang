namespace Bachelorarbeit.Server.Controllers.Responses
{
 public class LoginResponse
 {
  public bool Success { get; set; }
  public string? Token { get; set; }
  public string? Message { get; set; }
  
  public static LoginResponse GetSuccessfullLoginResponse(string refreshToken)
  {
   return new LoginResponse()
   {
    Message = "Login successful",
    Success = true,
    Token = refreshToken
   };
  }
  public static LoginResponse GetFailedLoginResponse()
  {
   return new LoginResponse()
   {
    Message = "Login failed",
    Success = false
   };
  }
  
 }   
}

