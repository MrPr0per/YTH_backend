namespace YTH_backend.DTOs.Course;

public record GetUserCourseByIdResponseDto(Guid Id, DateTime CreatedAt, Guid UserId, Guid CourseId);