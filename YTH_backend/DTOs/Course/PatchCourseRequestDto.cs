namespace YTH_backend.DTOs.Course;

public record PatchCourseRequestDto(string? Name, string? Description, string? Link);