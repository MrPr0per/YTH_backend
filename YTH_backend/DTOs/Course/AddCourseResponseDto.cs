namespace YTH_backend.DTOs.Course;

public record AddCourseResponseDto(Guid Id, string Name, string Description, string Link, DateTime CreatedAt, string? ImageUrl, decimal Price);