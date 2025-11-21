namespace YTH_backend.DTOs.Course;

public record AddCourseToUserResponseDto(Guid Id, DateTime CreatedAt, Guid UserId, Guid CourseId);