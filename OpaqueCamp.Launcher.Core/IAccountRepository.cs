namespace OpaqueCamp.Launcher.Core;

public interface IAccountRepository
{
    /// <summary>
    ///     Returns all saved accounts.
    /// </summary>
    IEnumerable<Account> GetAccounts();

    /// <summary>
    ///     Finds an account by its ID.
    /// </summary>
    /// <exception cref="AccountNotFoundException">
    ///     thrown when the account with the specified ID was not found.
    /// </exception>
    Account GetAccountById(Guid id);

    /// <summary>
    ///     Saves the specified account.
    /// </summary>
    void AddAccount(Account account);

    /// <summary>
    ///     Updates the specified existing account.
    /// </summary>
    /// <exception cref="AccountNotFoundException">
    ///     thrown if the specified account was not saved by calling <see cref="AddAccount" /> before updating.
    /// </exception>
    void UpdateAccount(Account account);

    /// <summary>
    ///     Deletes the specified account.
    /// </summary>
    void DeleteAccount(Account account);
}