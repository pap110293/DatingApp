namespace DatingApp.API.Helpers
{
    public class PagingParams
    {
        protected int MaxPageSize = 50;
        protected int pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
    }
}