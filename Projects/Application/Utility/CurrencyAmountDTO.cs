using System;

namespace Application.Utility;

public class CurrencyAmountDTO
{
    public CurrencyAmountDTO(string currencyCode, string amount)
    {
        if (string.IsNullOrWhiteSpace(currencyCode))
            throw new Exception("Currency code cannot be null or empty");

        if (currencyCode.Length != 3)
            throw new Exception("Invalid format for currency code. Currency code has to have 3 digits");

        if (string.IsNullOrWhiteSpace(amount))
            throw new Exception("Amount cannot be null or empty");

        CurrencyCode = currencyCode;
        Amount = amount;
    }

    public string CurrencyCode { get; }
    public string Amount { get; }
}
