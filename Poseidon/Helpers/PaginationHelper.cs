namespace Poseidon.Helpers
{
    public class PaginationHelper
    {
        public static IQueryable<T> Paginate<T>(IQueryable<T> source, int page, int pageSize)
        {
            return source.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
