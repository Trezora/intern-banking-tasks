using BankingApp.Domain.ValueObjects.Emails;

namespace BankingApp.Domain.Services;

public interface IEmailUniquenessChecker
{
    bool IsEmailUnique(Email email);
}