using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class AccountsWindowFactory
{
    private readonly IAccountRepository _accountRepository;

    public AccountsWindowFactory(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public AccountsWindow Create()
    {
        return new(new AccountsViewModel(_accountRepository));
    }
}