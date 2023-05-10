namespace Domain.Entities;


/*
 assuming this will later be saved in table in database
that is why it is located insied Entities folder
 */
public class Account
{
    public Account(int id, string number)
    {
        Id = id;
        Number = number;
    }

    public int Id { get; set; }
    public string Number { get; set; }
}
