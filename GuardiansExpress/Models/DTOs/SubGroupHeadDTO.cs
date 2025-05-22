namespace GuardiansExpress.Models.DTOs
{
    public class SubGroupHeadDTO
    {
        public int subgroupId { get; set; }

        public string? SubGroupName { get; set; }
        public string? Behaviour { get; set; }
        public string? Group { get; set; }
        public int? GroupId { get; set; }

        public bool? Detailed { get; set; }

        public bool? AcceptAddress { get; set; }

        public bool? Employee { get; set; }

        public bool? BalanceDashboard { get; set; }

        public string? orderin { get; set; }

    }
}
