// ProfitLossController.cs
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GuardiansExpress.Controllers
{
    public class ProfitLossController : Controller
    {
        private readonly IProfitLossService _plService;

        public ProfitLossController(IProfitLossService plService)
        {
            _plService = plService;
        }

        public IActionResult ProfitLossIndex()
        {
            return View(new ProfitLossDTO()); // Initial empty view
        }


        [HttpPost]
        public IActionResult GenerateReport(DateTime startDate, DateTime endDate, string branch = "Head Office")
        {
            var pnl = _plService.GenerateProfitLossReport(startDate, endDate, branch);
            return View("ProfitLossIndex", pnl);
        }
    }
}