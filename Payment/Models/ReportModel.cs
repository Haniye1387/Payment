namespace Payment.Models
{
    public class ReportModel
    {
        public string UserName { get; set; } = "";
        public string Date { get; set; } = "";
        public decimal TotalPrice { get; set; }
    }
}