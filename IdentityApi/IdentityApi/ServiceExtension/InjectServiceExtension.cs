using IdentityApplication.BusinessRule.Account;
using IdentityApplication.Service.Account;
using IdentityApplication.Service.ApiKey;
using IdentityApplication.ServiceInterface.Account;
using IdentityApplication.ServiceInterface.ApiKey;
using IdentityCore.BusinessRuleInterface.Account;
using IdentityCore.RepositoryInterface.Account;
using IdentityCore.RepositoryInterface.ApiKey;
using IdentityCore.RepositoryInterface.Generic;
using IdentityCore.RepositoryInterface.Role;
using IdentityCore.ServiceInterface.Audit;
using IdentityCore.ServiceInterface.Password;
using IdentityInfrastructure.Repository.Account;
using IdentityInfrastructure.Repository.ApiKey;
using IdentityInfrastructure.Repository.Generic;
using IdentityInfrastructure.Repository.Role;
using IdentityInfrastructure.Service.Audit;
using IdentityInfrastructure.Service.HashingPassword;

namespace Identity.ServiceExtension;

public static class InjectServiceExtension
{
    public static void AddInjectService(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IAccountRepository, AccountRepository>();

        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddScoped<IApiKeyRepository, ApiKeyRepository>();

        services.AddScoped<IApiKeyService, ApiKeyService>();

        services.AddScoped<IAccountService, AccountService>();

        services.AddScoped<IAuditService, AuditService>();

        services.AddScoped<IPasswordHashingService, PasswordHashingService>();

        services.AddScoped<IAccountBusinessRuleService, AccountBusinessRuleService>();
    }
}