using System;

namespace Application.Utility;

public class CreditorRefenceAndModelDTO
{
    public CreditorRefenceAndModelDTO(string creditorReference, string creditorReferenceModel)
    {
        if (string.IsNullOrWhiteSpace(creditorReference))
            throw new Exception("Creditor reference cannot be null or empty");

        if (string.IsNullOrWhiteSpace(creditorReferenceModel))
            throw new Exception("Creditor reference model cannot be null or empty");

        CreditorReference = creditorReference;
        CreditorReferenceModel = creditorReferenceModel;
    }

    public string CreditorReference { get; }
    public string CreditorReferenceModel { get; }
}
