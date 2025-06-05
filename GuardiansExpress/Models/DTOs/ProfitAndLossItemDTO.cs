using System.Collections.Generic;

namespace GuardiansExpress.Models.DTOs
{
    public class ProfitAndLossItemDTO
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public List<ProfitAndLossItemDTO> SubItems { get; set; } = new List<ProfitAndLossItemDTO>();
    }
}