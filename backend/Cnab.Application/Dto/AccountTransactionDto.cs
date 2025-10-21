namespace Cnab.Application.Dto;

public class AccountTransactionDto
{
    public int Id { get; set; }
    public required string Type { get; set; }
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public decimal Balance { get; set; }
    public required string Cpf { get; set; }
    public required string Card { get; set; }
    public required string StoreOwner { get; set; }
    public required string StoreName { get; set; }
}
