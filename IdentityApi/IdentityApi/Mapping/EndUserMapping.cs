using Identity.Request.User;
using IdentityCore.Model.Users;

namespace Identity.Mapping;

public static class EndUserMapping
{
    public static EndUserRegistration MapToUser(this EndUserRegisterRequest request)
    {
        return new EndUserRegistration
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