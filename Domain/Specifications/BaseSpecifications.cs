using System.Linq.Expressions;


namespace Domain.Specifications
{
    public abstract class BaseSpecifications<T> where T : class
    {
        public Expression<Func<T, bool>> Criteria { get; private set; }

        public List<Expression<Func<T, object>>> Includes { get; private set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; private set; }
        public Expression<Func<T, object>> OrderByDescending { get; private set; }
        public int Take { get; private set; }

        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        public bool IsTotalCountEnable { get; private set; }

        protected void AddInclude(Expression<Func<T, object>> incldueExpression)
        {
            Includes.Add(incldueExpression);
        }
        protected void AddCriteria(Expression<Func<T, bool>> incldueExpression)
        {
            if (Criteria == null)
                Criteria = incldueExpression!;
        }
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        protected void ApplyPaging(int PageSize, int PageIndex)
        {
            Skip = PageSize * (PageIndex - 1);
            Take = PageSize;
            IsPagingEnabled = true;
            EnableTotalCount();
        }
        protected void ApplyPagingWithMaxRecords(int PageSize, int PageIndex, int MaxRecords)
        {
            int recordsToSkip = (PageIndex - 1) * PageSize;

            recordsToSkip = Math.Min(recordsToSkip, MaxRecords);

            Skip = recordsToSkip;
            Take = Math.Min(PageSize, MaxRecords - recordsToSkip);
            IsPagingEnabled = true;
            EnableTotalCount();
        }
        protected void EnableTotalCount()
        {
            IsTotalCountEnable = true;
        }
    }
}
