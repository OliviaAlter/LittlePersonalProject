using IdentityApplication.BusinessRule.User;
using IdentityApplication.Interface.User;
using IdentityApplication.Service.User;
using IdentityCore.BusinessRuleInterface.EndUser;
using IdentityCore.RepositoryInterface.Generic;
using IdentityCore.RepositoryInterface.Role;
using IdentityCore.RepositoryInterface.User;
using IdentityCore.ServiceInterface.Audit;
using IdentityCore.ServiceInterface.Password;
using IdentityInfrastructure.Repository.Generic;
using IdentityInfrastructure.Repository.Role;
using IdentityInfrastructure.Repository.User;
using IdentityInfrastructure.Service.Audit;
using IdentityInfrastructure.Service.HashingPassword;

namespace Identity.ServiceExtension;

public static class InjectServiceExtension
{
    public static void AddInjectService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IEndUserRepository, EndUserRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IEndUserService, EndUserService>();

        services.AddScoped<IAuditService, AuditService>();

        services.AddScoped<IPasswordHashingService, PasswordHashingService>();

        services.AddScoped<IEndUserBusinessRuleService, EndUserBusinessRuleService>();
    }
}