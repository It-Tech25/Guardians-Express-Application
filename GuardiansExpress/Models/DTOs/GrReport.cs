// Models/GrReportDTO.cs
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YourNamespace.Models
{
    public class GrReportRequestDto
    {
        public string Branch { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<string> SelectedFields { get; set; } = new List<string>();
    }
    public class GrTypeFieldDto
    {
        public string GR { get; set; }  // Database column name
        public string GrReport { get; set; } // User-friendly name
    }
    public class GrReportDataDto
    {
        // Dynamic properties will be handled via dictionary
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}