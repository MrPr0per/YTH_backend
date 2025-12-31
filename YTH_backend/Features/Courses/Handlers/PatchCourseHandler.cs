using MediatR;
using YTH_backend.Data;
using YTH_backend.DTOs.Course;
using YTH_backend.Features.Courses.Commands;
using YTH_backend.Infrastructure.Exceptions;
using YTH_backend.Infrastructure.Object_storage;

namespace YTH_backend.Features.Courses.Handlers;

public class PatchCourseHandler(AppDbContext dbContext, ImageAdder imageAdder, IStorageService storageService) : IRequestHandler<PatchCourseCommand>
{
    public async Task Handle(PatchCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses.FindAsync([request.CourseId], cancellationToken);
        
        if (course == null)
            throw new EntityNotFoundException($"Course with id {request.CourseId} not found");
        
        
        
        var dto = new PatchCourseRequestDto(course.Name, course.Description, course.Link, course.ImageUrl, course.Price);
        
        request.Patch.ApplyTo(dto);
        
        if (dto.Price < 0)
            throw new InvalidOperationException("Price must be greater than or equal to 0");
        
        if (dto.Name is not null)
            course.Name = dto.Name;
        if (dto.Description is not null)
            course.Description = dto.Description;
        course.Link = dto.Link;
        if (dto.Price is not null)
            course.Price = dto.Price.Value;
        
        if (Base64Helper.IsBase64String(dto.ImageBase64!) || dto.ImageBase64 is null)
        {
            if (course.ImageUrl is not null)
                await storageService.DeleteByUrlAsync(course.ImageUrl, cancellationToken);
            
            var url = null as string;
            
            if (dto.ImageBase64 is not null)
                url = await imageAdder.AddImageToObjectStorage(dto.ImageBase64, $"course_{course.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}", true);
            
            course.ImageUrl = url;
        }
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
