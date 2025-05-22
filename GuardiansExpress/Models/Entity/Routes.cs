using System.ComponentModel.DataAnnotations;

namespace GuardiansExpress.Models.Entity
{
    public class Routes
    {
        [Key]
        public int id { get; set; }

        public string? from_place { get; set; }

        public string? to_place { get; set; }

        public string ?vehicle_type { get; set; }

        public decimal? advance { get; set; }

        public int? kilometers { get; set; }

        public int? trip_time_hours { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
