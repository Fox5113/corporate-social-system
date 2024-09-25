using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.TimeTracker;
using Services.Repositories.Abstractions;

namespace Infrastructure.Repositories.Implementations
{
	/// <summary>
	/// Репозиторий работы с курсами.
	/// </summary>
	public class TimeTrackerRepository : Repository<TimeTracker, Guid>, ITimeTrackerRepository
	{
		public TimeTrackerRepository(DataContext context) : base(context)
		{
		}

		/// <summary>
		/// Получить сущность по Id.
		/// </summary>
		/// <param name="id"> Id сущности. </param>
		/// <param name="cancellationToken"></param>
		/// <returns> Тайм трекер. </returns>
		public override async Task<TimeTracker> GetAsync(Guid id, CancellationToken cancellationToken)
		{
			var query = Context.Set<TimeTracker>().AsQueryable();
			return await query.SingleOrDefaultAsync(
				c => c.Id == id,
				cancellationToken);
		}

		/// <summary>
		/// Получить постраничный список.
		/// </summary>
		/// <param name="filterDto"> ДТО фильтра. </param>
		/// <returns> Список тайм трекеров. </returns>
		public async Task<List<TimeTracker>> GetPagedAsync(TimeTrackerFilterDto filterDto)
		{
			var query = GetAll();
			//.Include(c => c.Lessons).AsQueryable();
			if (filterDto.ProjectId != default)
			{
				query = query.Where(c => c.ProjectId == filterDto.ProjectId);
			}

			if (filterDto.EmployeeId != default)
			{
				query = query.Where(c => c.EmployeeId == filterDto.EmployeeId);
			}

			query = query
				.Skip((filterDto.Page - 1) * filterDto.ItemsPerPage)
				.Take(filterDto.ItemsPerPage);

			return await query.ToListAsync();
		}
	}
}
