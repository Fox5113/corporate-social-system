using Domain.Entities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Services.Contracts.Project;
using Services.Repositories.Abstractions;

namespace Infrastructure.Repositories.Implementations
{
	/// <summary>
	/// Репозиторий работы с проектами.
	/// </summary>
	public class ProjectRepository : Repository<Project, Guid>, IProjectRepository
	{
		public ProjectRepository(DataContext context) : base(context)
		{
		}

		/// <summary>
		/// Получить сущность по Id.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns> Проект. </returns>
		public override async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken, bool asNoTracking = false)
		{
			var query = Context.Set<Project>().Include(c => c.TimeTrackers).AsQueryable();
			return await query.ToListAsync(cancellationToken);
		}

		/// <summary>
		/// Получить сущность по Id.
		/// </summary>
		/// <param name="id"> Id сущности. </param>
		/// <param name="cancellationToken"></param>
		/// <returns> Проект. </returns>
		public override async Task<Project> GetAsync(Guid id, CancellationToken cancellationToken)
		{
			var query = Context.Set<Project>().Include(c => c.TimeTrackers).AsQueryable();
			return await query.SingleOrDefaultAsync(
				c => c.Id == id,
				cancellationToken);
		}

		/// <summary>
		/// Получить постраничный список.
		/// </summary>
		/// <param name="filterDto"> ДТО фильтра. </param>
		/// <returns> Список проектов. </returns>
		public async Task<List<Project>> GetPagedAsync(ProjectFilterDto filterDto)
		{
			var query = GetAll().Include(c => c.TimeTrackers).AsQueryable();
			if (!string.IsNullOrWhiteSpace(filterDto.Code))
			{
				query = query.Where(c => c.Code == filterDto.Code);
			}

			if (!string.IsNullOrWhiteSpace(filterDto.Name))
			{
				query = query.Where(c => c.Name.Contains(filterDto.Name));
			}

			query = query
				.Skip((filterDto.Page - 1) * filterDto.ItemsPerPage)
				.Take(filterDto.ItemsPerPage);

			return await query.ToListAsync();
		}
	}
}
