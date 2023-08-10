namespace WakaWaka.API.DataAccessLayer.Interfaces
{
    public interface IReviewRepository<T>
    {
        Task<T> GetReviewAsync(int id, int parentEntityId);
        Task<T> CreateReviewAsync(T entity);
        Task<T> UpdateReviewAsync(T entity, int reviewId);
        Task<bool> DeleteReviewAsync(int reviewId, int parentEntityId);
        Task <IQueryable<T>> GetAllReviewsAsync(int parentEntityId);
    }
}
