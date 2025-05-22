using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GuardiansExpress.Services.Implementation
{
    public class GRReportService : IGRReportService
    {
        private readonly IGRReportRepository _grReportRepository;
        private readonly MyDbContext _context;

        public GRReportService(IGRReportRepository grReportRepository,MyDbContext context)
        {
            _grReportRepository = grReportRepository;
            _context = context;
        }

        //public IEnumerable<GRDTOs> GetAllGRReports()
        //{
        //    return _grReportRepository.GetAllGRReports();
        //}

        public IEnumerable<GRDTOs> Getgrdetails(
     int? branchId,
     string fromDate,
     string toDate,
     string fromGRNo,
     string toGRNo,
     string status)
        {
            DateTime? parsedFromDate = null;
            DateTime? parsedToDate = null;

            // Parse dates if provided
            if (!string.IsNullOrEmpty(fromDate)
                && DateTime.TryParse(fromDate, out var tempFromDate))
            {
                parsedFromDate = tempFromDate;
            }

            if (!string.IsNullOrEmpty(toDate)
                && DateTime.TryParse(toDate, out var tempToDate))
            {
                parsedToDate = tempToDate;
            }

            var res = _grReportRepository.Getgrdetails();

            // Filter by branch
            if (branchId.HasValue)
            {
                var branchName = _context.branch
                    .FirstOrDefault(b => b.id == branchId)?.BranchName;
                res = res.Where(x => x.Branch == branchName);
            }

            // Apply date filters
            if (parsedFromDate.HasValue)
                res = res.Where(x => x.GRDate >= parsedFromDate.Value);

            if (parsedToDate.HasValue)
                res = res.Where(x => x.GRDate <= parsedToDate.Value);

            // Filter by GRNo range (string comparison)
            if (!string.IsNullOrEmpty(fromGRNo))
                res = res.Where(x => x.GRNo.CompareTo(fromGRNo) >= 0);

            if (!string.IsNullOrEmpty(toGRNo))
                res = res.Where(x => x.GRNo.CompareTo(toGRNo) <= 0);

            // Filter by status
            if (!string.IsNullOrEmpty(status) && status != "All")
                res = res.Where(x => x.IsActive == (status == "Active"));

            return res;
        }
    }
}