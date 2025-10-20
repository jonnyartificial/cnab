namespace Cnab.Domain.Entities;

public class TransactionType
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool IsIncome { get; set; }

    public ICollection<AccountTransaction> AccountTransactions { get; set; } = [];
}
