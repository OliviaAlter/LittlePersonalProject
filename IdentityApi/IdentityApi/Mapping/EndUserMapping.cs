using Identity.Request.User;
using IdentityCore.ModelValidation.UserModel;

namespace Identity.Mapping;

public static class EndUserMapping
{
    public static UserRegistration MapToUser(this EndUserRegisterRequest request)
    {
        return new UserRegistration
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber
        };
    }
}