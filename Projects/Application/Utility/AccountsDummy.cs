using System.Collections.Generic;

namespace Application.Utility;

public static class AccountsDummy
{
    public static List<AccountDTO> Accounts { get; set; } = new(){
        new AccountDTO(1,"0011225333665"),
        new AccountDTO(2,"0065533554433"),
        new AccountDTO(3,"00668855441122"),
    };
}
