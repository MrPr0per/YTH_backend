using YTH_backend.DTOs.User;

namespace YTH_backend.Tests.Common.Clients;

public class AuthClient(HttpClient client, string apiPrefix, string resourcePrefix)
    : BaseResourceClient(client, apiPrefix, resourcePrefix)
{
    public Task<HttpResponseMessage> SendVerificationEmailForRegistration(SendVerificationEmailRequestDto args) =>
        PostAsync("sendVerificationEmailForRegistration", args);

    public Task<HttpResponseMessage> Register(CreateUserRequestDto args) =>
        PostAsync("register", args);

    public Task<HttpResponseMessage> Login(LoginUserRequestDto args) =>
        PostAsync("login", args);

    public Task<HttpResponseMessage> Logout() =>
        PostAsync("logout");

    public Task<HttpResponseMessage> Refresh() =>
        PostAsync("refresh");

    public Task<HttpResponseMessage> ChangePassword(ChangePasswordRequestDto args) =>
        PostAsync("changePassword", args);

    public Task<HttpResponseMessage> SendVerificationEmailForResetPassword(ForgotPasswordRequestDto args) =>
        PostAsync("sendVerificationEmailForResetPassword", args);

    public Task<HttpResponseMessage> ResetPassword(ResetPasswordRequestDto args) =>
        PostAsync("resetPassword", args);
}