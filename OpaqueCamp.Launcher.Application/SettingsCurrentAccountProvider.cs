using System;
using OpaqueCamp.Launcher.Application.Properties;
using OpaqueCamp.Launcher.Core;

namespace OpaqueCamp.Launcher.Application;

public sealed class SettingsCurrentAccountProvider : ICurrentAccountProvider
{
    private readonly IAccountRepository _accountRepository;

    public SettingsCurrentAccountProvider(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    /// <inheritdoc />
    public Account? CurrentAccount
    {
        get
        {
            try
            {
                return _accountRepository.GetAccountById(Settings.Default.CurrentAccountId);
            }
            catch
            {
                return null;
            }
        }

        set
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            Settings.Default.CurrentAccountId = value.Id;
            Settings.Default.Save();
        }
    }
}