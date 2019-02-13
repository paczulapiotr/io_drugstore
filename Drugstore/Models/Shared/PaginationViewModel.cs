using Newtonsoft.Json;

namespace Drugstore.Models
{
    public class PaginationViewModel
    {
        [JsonIgnore]
        private readonly string _requestTemplate;
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public PaginationViewModel(string requestTemplate, int totalPages, int currentPage)
        {
            _requestTemplate = requestTemplate;
            TotalPages = totalPages;
            CurrentPage = currentPage;
        }

        public string GetPage(int page)
        {
            return string.Format(_requestTemplate, page);
        }

        public string GetNextPage()
        {
            return GetPage(CurrentPage + 1);
        }

        public string GetPreviousPage()
        {
            return GetPage(CurrentPage - 1);
        }
    }
}
