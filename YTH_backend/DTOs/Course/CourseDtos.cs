namespace YTH_backend.DTOs.Course;

public record GetCoursesResponse(string Name, string? Description, string? Link, DateTime CreatedAt);

public record AddCourseRequest(string Name, string? Description, string? Link, DateTime CreatedAt);











