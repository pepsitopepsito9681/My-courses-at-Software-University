
namespace LogisticsSystem.Services.Reports.Models
{
    public class ReportServiceModel
    {
        public int Id { get; set; }

        public string ReportKind { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public string LoadId { get; set; }

        public string LoadTitle { get; set; }

        public string PublishedOn { get; set; }
    }
}
