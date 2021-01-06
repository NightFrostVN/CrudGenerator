using System;

namespace DataAccess.DataModel
{
    public class WebsiteDetail
    {
        public int? Id { get; set; }
        public int? WebsiteId { get; set; }
        public string Detail { get; set; }
        public DateTime? PublishDate { get; set; }
    }
}

