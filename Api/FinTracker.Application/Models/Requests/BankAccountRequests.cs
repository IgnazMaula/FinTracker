namespace FinTracker.Application.Models.Requests;

public class CreateBankAccountRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AccountNo { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
    public string Currency { get; set; } = string.Empty;
    public decimal InitialBalance { get; set; }
}

public class UpdateBankAccountRequest
{
    public string Name { get; set; } = string.Empty;
    public string AccountNo { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
}
