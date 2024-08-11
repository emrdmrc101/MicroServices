using Lesson.Domain.Interfaces.Repositories;
using Lesson.Infrastructure.Data;

namespace Lesson.Infrastructure.Repositories;

public class UserLessonRepository(LessonDbContext context):Repository<Domain.Entities.UserLesson>(context), IUserLessonRepository
{
    
}