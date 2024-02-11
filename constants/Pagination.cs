namespace BelajarSharp.constants
{
    public class Pagination
    {
        int paging;
        int limit = 10;
        public Pagination(int page) {
            paging = page;
        }
        public int GetOffset() {
            return paging > 1 ? (paging - 1) * limit : 0;
        }

        public int GetLimit() {
            return limit;
        }
    }
}
