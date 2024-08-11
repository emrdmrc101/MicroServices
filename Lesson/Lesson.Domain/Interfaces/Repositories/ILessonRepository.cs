using Lesson.Domain.Entities;

namespace Lesson.Domain.Interfaces.Repositories;

public interface ILessonRepository : IRepository<Entities.Lesson>
{
    Task<List<Domain.Entities.Lesson>> GetMyLessons(Guid userId);
}