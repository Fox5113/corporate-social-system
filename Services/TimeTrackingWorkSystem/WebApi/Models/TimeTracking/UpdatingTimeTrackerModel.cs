namespace WebApi.Models.TimeTracking
{
    public class UpdatingTimeTrackerModel
    {
		/// <summary>
		/// Описание.
		/// </summary>
		public string? Description { get; init; }

		/// <summary>
		/// Кол-во часов
		/// </summary>
		public int TimeAtWork { get; init; }
	}
}
