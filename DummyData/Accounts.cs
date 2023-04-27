using XCoreAssignment.Models;

namespace XCoreAssignment.DummyData;

public static class Accounts
{
    public static List<AccountDTO> DummyAccounts { get; set; } = new(){
        new AccountDTO(1,"0011225333665"),
        new AccountDTO(2,"0065533554433"),
        new AccountDTO(3,"00668855441122"),
    };
}
