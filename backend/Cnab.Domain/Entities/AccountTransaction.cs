namespace Cnab.Domain.Entities;

public class AccountTransaction
{
    public int Id { get; set; }
    public int TransactionTypeId { get; set; }
    public TransactionType TransactionType { get; set; }
    public int StoreId { get; set; }
    public Store Store { get; set; }
    public DateTime Date { get; set; }
    public Decimal Value { get; set; }
    public required string Cpf { get; set; }
    public required string Card { get; set; }
    public required string StoreOwner { get; set; }
    public required DateTime CreatedDateTime { get; set; }
}
