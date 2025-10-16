namespace YTH_backend.DTOs.Course;

public record GetCoursesResponseDto(string Name, string? Description, string? Link, DateTime CreatedAt);

public record AddCourseRequestDto(string Name, string? Description, string? Link, DateTime CreatedAt);

public record PatchCourseRequestDto(string? Name, string? Description, string? Link, DateTime? CreatedAt);









