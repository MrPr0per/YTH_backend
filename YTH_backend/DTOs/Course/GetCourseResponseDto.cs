namespace YTH_backend.DTOs.Course;

public record GetCourseResponseDto(Guid Id, string Name, string? Description, string? Link, DateTime CreatedAt, string? ImageUrl, decimal Price);













