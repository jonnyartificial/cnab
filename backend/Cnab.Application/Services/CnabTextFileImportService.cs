using Cnab.Domain.Entities;
using Cnab.Domain.Interfaces;
using Cnab.Domain.ValueObjects;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Cnab.Application.Services;

public class CnabTextFileImportService : ITextFileImportService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionTypeRepository _transactionTypeRepository;
    private readonly IStoreRepository _storeRepository;
    private readonly IAccountTransactionRepository _accountTransactionRepository;

    private readonly DateTime _transactionDateTime;

    private IEnumerable<TransactionType> _transactionTypes;
    private IEnumerable<Store> _stores;

    public CnabTextFileImportService(
        IUnitOfWork unitOfWork,
        ITransactionTypeRepository transactionTypeRepository,
        IStoreRepository storeRepository,
        IAccountTransactionRepository accountTransactionRepository)
    {
        _unitOfWork = unitOfWork;
        _transactionTypeRepository = transactionTypeRepository;
        _storeRepository = storeRepository;
        _accountTransactionRepository = accountTransactionRepository;
        _transactionDateTime = DateTime.UtcNow;
    }

    public async Task<TextFileParseResult> ImportAsync(string content, CancellationToken cancellationToken)
    {
        //load transaction types
        _transactionTypes = await _transactionTypeRepository
            .GetAllTransactionTypesAsync(cancellationToken);

        //load stores
        _stores = await _storeRepository
            .GetAllStoresAsync(cancellationToken);

        var result = new TextFileParseResult { IsSuccess = true };

        var lines = content.Split(["\r\n", "\n", "\r"], StringSplitOptions.None);

        for (var i = 0; i < lines.Length - 1; i++)
        {
            result.LinesRead++;
            var importedItem = ParseLine(lines[i]);
            if (importedItem.IsSuccess == false)
            {
                result.Erros++;
                result.Messages.Add($"Line {i + 1}: {importedItem.Message ?? ""}");
            }
            else
            {
                var savedItem = await InsertRecordAsync(importedItem.Cnab!, cancellationToken);
                if (savedItem.IsSuccess == false)
                {
                    result.Erros++;
                    result.Messages.Add($"Line {i + 1}: {savedItem.Message ?? ""}");
                }
            }
        }

        return result;
    }

    private ImportResult ParseLine(string line)
    {
        if (line.Length != 80)
            return new ImportResult(false, "Invalid Length");

        var result = new Cnab();

        //parse Type
        var type = line.Substring(0, 1);
        if (int.TryParse(type, out var parsedType) == false)
            return new ImportResult(false, "Invalid Type field");
        result.Type = parsedType;

        //parse Date
        var dateTime = line.Substring(1, 8) + line.Substring(42, 6);
        if (DateTime.TryParseExact(dateTime, "yyyyMMddHHmmss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var parsedDateTime) == false)

            return new ImportResult(false, "Invalid Date/Time fields");
        result.Date = parsedDateTime;

        //parse Value
        var value = line.Substring(9, 10);
        if (Decimal.TryParse(value, out var parsedValue) == false)
            return new ImportResult(false, "Invalid Value field");
        result.Value = parsedValue / 100;

        //parse CPF
        //only checking if field is 11 numbers long, not validating cpf formula
        var cpf = line.Substring(19, 11);
        if (Regex.IsMatch(cpf, @"^\d{11}$") == false)
            return new ImportResult(false, "Invalid CPF field");
        result.Cpf = cpf;

        //parse Card
        result.Card = line.Substring(30, 12);

        result.StoreOwner = line.Substring(48, 14).Trim();
        result.StoreName = line.Substring(62, 18).Trim();

        return new ImportResult(true, cnab: result);
    }

    private async Task<ImportResult> InsertRecordAsync(Cnab record, CancellationToken cancellationToken)
    {
        //get type
        var type = GetTransactionType(record.Type);
        if (type == null)
            return new ImportResult(false, "Invalid Type");

        //get store
        var store = await GetStoreAsync(record.StoreName, cancellationToken);

        //create entity
        var accountTransaction = new AccountTransaction
        {
            TransactionTypeId = type.Id,
            StoreId = store.Id,
            Date = record.Date,
            Value = record.Value,
            Cpf = record.Cpf,
            Card = record.Card,
            StoreOwner = record.StoreOwner,
            CreatedDateTime = _transactionDateTime
        };

        //add entity to db
        await _accountTransactionRepository.AddAsync(accountTransaction, cancellationToken);

        try
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is SqlException sqlEx &&
            (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            //unique constraint violation - idempotence
            //this record was already imported and so cannot be imported again
            return new ImportResult(false, "This record was already imported");
        }
        catch (Exception ex)
        {
            return new ImportResult(false, "There was an error and this record cannot be imported");
        }

        return new ImportResult(true);
    }

    private TransactionType? GetTransactionType(int type)
    {
        return _transactionTypes.FirstOrDefault(p => p.Id == type);
    }

    private async Task<Store> GetStoreAsync(string name, CancellationToken cancellationToken)
    {
        //find if store already exist
        var store = _stores.FirstOrDefault(
            p =>
                string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase));

        //if store doesn't exist, create record and reload cached data
        if (store == null)
        {
            var result = await _storeRepository.Add(name, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _stores = await _storeRepository.GetAllStoresAsync(cancellationToken);
            return result;
        }

        return store;
    }

    private class Cnab
    {
        public int Type { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Cpf { get; set; } = string.Empty;
        public string Card { get; set; } = string.Empty;
        public string StoreOwner { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
    }

    private class ImportResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Cnab? Cnab { get; set; }

        public ImportResult(bool isSuccess, string? message = null, Cnab? cnab = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Cnab = cnab;
        }
    }
}