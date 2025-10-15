namespace YTH_backend.DTOs.User;

//TODO что делать с админами
public record AddAdminRequest(Guid UserId);

public record RemoveAdminRequest(string Reason);

// public record GetAppointeeAdmins();
//
// public record GetRevokedAdmins();

