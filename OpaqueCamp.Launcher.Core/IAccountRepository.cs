namespace OpaqueCamp.Launcher.Core;

public interface IAccountRepository
{
    /// <summary>
    ///     Returns all saved accounts.
    /// </summary>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when an error occurs while reading the accounts.
    ///     The inner exception may contain the actual error.
    /// </exception>
    IEnumerable<Account> GetAccounts();

    /// <summary>
    ///     Finds an account by its ID.
    /// </summary>
    /// <exception cref="AccountNotFoundException">
    ///     thrown when the account with the specified ID was not found.
    /// </exception>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when an error occurs while reading the accounts.
    ///     The inner exception may contain the actual error.
    /// </exception>
    Account GetAccountById(Guid id);

    /// <summary>
    ///     Saves the specified account.
    /// </summary>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when an error occurs while saving the accounts.
    ///     The inner exception may contain the actual error.
    /// </exception>
    /// <exception cref="AccountAlreadyExistsException">
    ///     thrown when the previously added account has the same ID as the specified account.
    /// </exception>
    void AddAccount(Account account);

    /// <summary>
    ///     Updates the specified existing account.
    /// </summary>
    /// <exception cref="AccountNotFoundException">
    ///     thrown if the specified account was not saved by calling <see cref="AddAccount" /> before updating.
    /// </exception>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when an error occurs while reading or saving the accounts.
    ///     The inner exception may contain the actual error.
    /// </exception>
    void UpdateAccount(Account account);

    /// <summary>
    ///     Deletes the specified account.
    /// </summary>
    /// <exception cref="AccountRepositoryException">
    ///     thrown when an error occurs while reading or saving the accounts.
    ///     The inner exception may contain the actual error.
    /// </exception>
    /// <exception cref="AccountNotFoundException">
    ///     thrown if the specified account was not saved by calling <see cref="AddAccount" /> before deleting.
    /// </exception>
    void DeleteAccount(Account account);
}