using Domain.Entities;

namespace XCoreAssignment.DummyData;

public static class Accounts
{
    public static List<Account> DummyAccounts { get; set; } = new(){
        new Account(1,"0011225333665"),
        new Account(2,"0065533554433"),
        new Account(3,"00668855441122"),
    };
}
