using System;
using System.Linq;

namespace Application.Utility;

public class AccountDTO
{
    public AccountDTO(int id, string account)
    {
        if (id == 0 || id < 0)
            throw new Exception("Account id cannot be 0 or less than 0");

        if (string.IsNullOrWhiteSpace(account))
            throw new Exception("Account value cannot be null or empty");

        bool isAllDigitsInAcc = account.Length == account.Where(x => char.IsDigit(x)).Count();
        if (!isAllDigitsInAcc)
            throw new Exception("Invalid value for account. Not all chars in account are digits");

        Id = id;
        Account = account;
    }

    public int Id { get; set; }
    public string Account { get; set; }
}
