using FinTracker.Application.Interfaces;
using FinTracker.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FinTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IBankAccountService, BankAccountService>();
        services.AddScoped<IBankTransactionService, BankTransactionService>();
        services.AddScoped<ICashflowService, CashflowService>();
        services.AddScoped<IDCACalculatorService, DCACalculatorService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
