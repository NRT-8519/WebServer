namespace WebServer.Models.DTOs
{
    public class ClearanceDTO
    {
        public Guid? UUID { get; set; }
        public string ClearanceNumber { get; set; }
        public DateOnly BeginDate { get; set; }
        public DateOnly ExpiryDate { get; set;}
    }
}
