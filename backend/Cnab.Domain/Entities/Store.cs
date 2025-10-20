namespace Cnab.Domain.Entities;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<AccountTransaction> AccountTransactions { get; set; } = [];
}
