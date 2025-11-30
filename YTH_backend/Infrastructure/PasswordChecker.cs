using Zxcvbn;
namespace YTH_backend.Infrastructure;

public static class PasswordChecker
{
    public static bool IsPasswordStrong(string password)
    {
        var evaluator = new Zxcvbn.Zxcvbn();
        var result = evaluator.EvaluatePassword(password);

        return result.Score >= 1;
    }
}