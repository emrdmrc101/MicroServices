using Lesson.Domain.Interfaces.Repositories;
using Lesson.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lesson.Infrastructure.Repositories;

public class LessonRepository(LessonDbContext context) : Repository<Domain.Entities.Lesson>(context), ILessonRepository
{
    public async Task<List<Domain.Entities.Lesson>> GetMyLessons(Guid userId)
    {
        return  await context.Set<Domain.Entities.Lesson>()
            .Include(i => i.UserLessons)
            .Where(x => x.UserLessons.UserId == userId)
            .AsNoTracking().ToListAsync();
    }
}