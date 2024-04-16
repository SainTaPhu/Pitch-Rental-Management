namespace UnityComplex.ViewModels
{
    public class OrderProduct
    {
        public List<int?> ProId { get; set; }
        public int IdField { get; set; }
        public int IdUser { get; set; }
        public string? Description { get; set; }

        public DateTime SelectedDate { get; set; }
        public TimeSpan Slot { get; set; }
        public List<int?> Quantity { get; set; }
        public int TotalPrice { get; set; }

        public string? myCheckboxBook { get; set; }


    }
}
