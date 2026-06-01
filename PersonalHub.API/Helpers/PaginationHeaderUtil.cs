using PersonalHub.API.Dtos.Pagination;
using System.Text.Json;

namespace PersonalHub.API.Helpers
{
    public static class PaginationHeaderUtil
    {
        public static HttpContext AddPaginationHeader<T>(this HttpContext context, PagedList<T> pagedList) where T : class
        {
            var responseHeaders = context.Response.Headers;
            string headerKey = "X-Pagination";
            if (!responseHeaders.ContainsKey(headerKey))
            {
                var totalPages = pagedList.TotalPages;
                var currentPage = pagedList.CurrentPage;
                var pageSize = pagedList.PageSize;
                var totalCount = pagedList.TotalCount;
                var hasPrevious = pagedList.HasPrevious;
                var hasNext = pagedList.HasNext;

                responseHeaders.Append(headerKey, JsonSerializer.Serialize(new
                {
                    totalPages,
                    currentPage,
                    pageSize,
                    totalCount,
                    hasPrevious,
                    hasNext
                }));
            }
            return context;
        }
    }
}
