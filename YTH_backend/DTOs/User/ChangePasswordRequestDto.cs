namespace YTH_backend.DTOs.User;

public record ChangePasswordRequestDto(string OldPassword, string NewPassword);