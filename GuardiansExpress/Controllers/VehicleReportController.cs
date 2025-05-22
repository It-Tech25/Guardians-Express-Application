using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using SkiaSharp;
using ClosedXML.Excel;
using Newtonsoft.Json;

public class VehicleReportController : Controller
{
    private readonly MyDbContext _context;

    public VehicleReportController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult VehicleReportIndex()
    {
        ViewBag.Branches = _context.branch.Select(x => new SelectListItem { Value = x.id.ToString(), Text = x.BranchName }).ToList();
        ViewBag.VehicleTypeList = _context.VehicleTypeEntite.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VehicleType }).ToList();
        ViewBag.VehicleGroupList = _context.vehicles.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VehicleGroup }).ToList();
        ViewBag.Users = _context.users.Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.UserName }).ToList();

        ViewBag.StatusList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Active", Text = "Active" },
            new SelectListItem { Value = "Inactive", Text = "Inactive" }
        };

        ViewBag.EMIStatusList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Paid", Text = "Paid" },
            new SelectListItem { Value = "Pending", Text = "Pending" }
        };

        ViewBag.TableColumns = GetColumnNames("VehicleMaster");

        return View();
    }

    public IActionResult ExportToPdf()
    {
        if (TempData["VehicleReportData"] != null)
        {
            var data = JsonConvert.DeserializeObject<List<VehicleReportDTO>>(TempData["VehicleReportData"].ToString());

            using (var stream = new MemoryStream())
            {
                var document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(document, stream);
                document.Open();

                var table = new PdfPTable(12)
                {
                    WidthPercentage = 100
                };

                string[] headers = new string[]
                {
                "Vehicle No", "Display Vehicle No", "Vehicle Type", "Body Type",
                "Weight", "Owned By", "Transporter", "Max Weight Allowed",
                "Status", "Branch", "Start Date", "Expiry Date"
                };

                foreach (var header in headers)
                {
                    table.AddCell(new PdfPCell(new Phrase(header)) { BackgroundColor = BaseColor.LightGray });
                }

                foreach (var item in data)
                {
                    table.AddCell(item.VehicleNo ?? "-");
                    table.AddCell(item.DisplayVehicleNo ?? "-");
                    table.AddCell(item.VehicleType ?? "-");
                    table.AddCell(item.Bodytype ?? "-");
                    table.AddCell(item.Weight?.ToString() ?? "-");
                    table.AddCell(item.OwnedBy ?? "-");
                    table.AddCell(item.Transporter ?? "-");
                    table.AddCell(item.MaxWeightAllowed?.ToString() ?? "-");
                   // table.AddCell(item.Status ? "Active" : "Inactive");
                    table.AddCell(item.BranchName ?? "-");
                    table.AddCell(item.StartDate?.ToString("yyyy-MM-dd") ?? "-");
                    table.AddCell(item.ExpiryDate?.ToString("yyyy-MM-dd") ?? "-");
                }

                document.Add(table);
                document.Close();

                var content = stream.ToArray();
                return File(content, "application/pdf", "VehicleReport.pdf");
            }
        }

        return RedirectToAction("Search");
    }

    public IActionResult ExportToExcel()
    {
        var data = JsonConvert.DeserializeObject<List<VehicleReportDTO>>(TempData["VehicleReportData"].ToString());

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Vehicle Report");
            var currentRow = 1;

            // Header titles
            string[] headers = new string[]
            {
            "Vehicle No", "Display Vehicle No", "Vehicle Type", "Body Type", "Weight",
            "Owned By", "Transporter", "Max Weight Allowed", "Status", "Branch", "Start Date", "Expiry Date"
            };

            // Set headers
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(currentRow, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Add data rows
            foreach (var item in data)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = item.VehicleNo;
                worksheet.Cell(currentRow, 2).Value = item.DisplayVehicleNo;
                worksheet.Cell(currentRow, 3).Value = item.VehicleTypeId;
                worksheet.Cell(currentRow, 4).Value = item.BodyTypeId;
                worksheet.Cell(currentRow, 5).Value = item.Weight;
                worksheet.Cell(currentRow, 6).Value = item.OwnedBy;
                worksheet.Cell(currentRow, 7).Value = item.Transporter;
                worksheet.Cell(currentRow, 8).Value = item.MaxWeightAllowed;
                worksheet.Cell(currentRow, 9).Value = item.Status.ToString();
                worksheet.Cell(currentRow, 10).Value = item.BranchId;
                worksheet.Cell(currentRow, 11).Value = item.StartDate?.ToString("yyyy-MM-dd");
                worksheet.Cell(currentRow, 12).Value = item.ExpiryDate?.ToString("yyyy-MM-dd");
            }

            // Auto adjust column widths
            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "VehicleReport.xlsx");
            }
        }
    }


    [HttpPost]
    public IActionResult Search()
    {
        var selectedColumns = Request.Form["SelectedColumns"].ToList();

        ViewBag.SelectedColumns = selectedColumns;
        ViewBag.VehicleTypeList = _context.VehicleTypeEntite.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VehicleType }).ToList();
        ViewBag.VehicleGroupList = _context.vehicles.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.VehicleGroup }).ToList();
        ViewBag.TableColumns = GetColumnNames("VehicleMaster");

        var vehicleNo = Request.Form["VehicleNo"];
        var vehicleType = Request.Form["VehicleType"];
        var branchId = Request.Form["Branch"];
        var status = Request.Form["Status"];

        var query = from vehicle in _context.VehicleMasters
                    join branch in _context.branch on vehicle.BranchId equals branch.id
                    join v in _context.VehicleTypeEntite on vehicle.VehicleTypeId equals v.Id
                    join b in _context.BodyTypes on vehicle.BodyTypeId equals b.Id
                    where vehicle.IsDeleted == false
                    select new VehicleReportDTO
                    {
                        VehicleId = vehicle.VehicleId,
                        VehicleNo = vehicle.VehicleNo,
                        VehicleTypeId = vehicle.VehicleTypeId,
                        Bodytype = b.Bodytype,
                        DisplayVehicleNo = vehicle.DisplayVehicleNo,
                        Weight = vehicle.Weight,
                        OwnedBy = vehicle.OwnedBy,
                        Transporter = vehicle.Transporter,
                        MaxWeightAllowed = vehicle.MaxWeightAllowed,
                        Status = vehicle.Status,
                        StartDate = vehicle.StartDate,
                        DocumentType = vehicle.DocumentType,
                        ExpiryDate = vehicle.ExpiryDate,
                        BranchId = vehicle.BranchId,
                        BranchName = branch.BranchName,
                        Amount = vehicle.Amount,
                        Remarks = vehicle.Remarks,
                        Uploads = vehicle.Uploads,
                        Docs = vehicle.Docs,
                        VehicleType = v.VehicleType
                    };

        if (!string.IsNullOrEmpty(vehicleNo))
            query = query.Where(v => v.VehicleNo.Contains(vehicleNo));

        if (!string.IsNullOrEmpty(branchId))
            query = query.Where(v => v.BranchId.ToString() == branchId);

        if (!string.IsNullOrEmpty(vehicleType))
            query = query.Where(v => v.VehicleTypeId.ToString() == vehicleType);

        if (!string.IsNullOrEmpty(status) && status != "All")
            query = query.Where(v => v.Status == (status == "Active"));

        var model = query.ToList();
        TempData["VehicleReportData"] = JsonConvert.SerializeObject(model); // store serialized

        return View("VehicleReportIndex", model);
    }

    public List<string> GetColumnNames(string tableName)
    {
        var columnNames = new List<string>();

        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";

            var param = command.CreateParameter();
            param.ParameterName = "@tableName";
            param.Value = tableName;
            command.Parameters.Add(param);

            _context.Database.OpenConnection();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    columnNames.Add(reader.GetString(0));
                }
            }

            _context.Database.CloseConnection();
        }

        return columnNames;
    }
}
