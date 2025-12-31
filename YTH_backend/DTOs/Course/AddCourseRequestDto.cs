namespace YTH_backend.DTOs.Course;

public record AddCourseRequestDto(string Name, string Description, string Link, string? ImageBase64, decimal Price);