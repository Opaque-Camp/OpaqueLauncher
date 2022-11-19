namespace OpaqueCamp.Launcher.Application;

using Core;
using Application.Properties;
using System;

public sealed class SettingsCurrentAccountProvider : ICurrentAccountProvider
{
    private readonly IAccountRepository _accountRepository;

    public SettingsCurrentAccountProvider(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    /// <inheritdoc/>
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            Settings.Default.CurrentAccountId = value.Id;
            Settings.Default.Save();
        }
    }
}