using ClosedXML.Excel;
using GuardiansExpress.Models.Entity;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Wordprocessing;

namespace GuardiansExpress.Controllers
{
    // Updated Controller
    public class LedgerReportController : Controller
    {
        private readonly MyDbContext _context;

        public LedgerReportController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult LedgerReportIndex()
        {
            var allColumns = GetColumnNames("LedgerMaster");
            ViewBag.TableColumns = allColumns;
            ViewBag.SelectedColumns = allColumns;
            return View();
        }

        [HttpPost]
        public IActionResult Search(string AccGroup, string Branch, string FromDate, string ToDate)
        {
            var selectedColumns = Request.Form["SelectedColumns"].ToList();
            ViewBag.TableColumns = GetColumnNames("LedgerMaster");
            ViewBag.SelectedColumns = selectedColumns.Count > 0 ? selectedColumns : ViewBag.TableColumns;

            DateTime? fromDate = string.IsNullOrEmpty(FromDate) ? null : DateTime.ParseExact(FromDate, "yyyy-MM-dd", null);
            DateTime? toDate = string.IsNullOrEmpty(ToDate) ? null : DateTime.ParseExact(ToDate, "yyyy-MM-dd", null);

            var query = from ledger in _context.ledgerEntity
                        join subgroup in _context.SubGroups on ledger.subgroupId equals subgroup.subgroupId into subgroupJoin
                        from subgroup in subgroupJoin.DefaultIfEmpty()
                        where ledger.IsDeleted == false && ledger.IsActive == true
                        select new LedgerMasterDTO
                        {
                            LedgerId = ledger.LedgerId,
                            subgroupName = subgroup != null ? subgroup.SubGroupName : null,
                            AccGroup = ledger.AccGroup,
                            AccHead = ledger.AccHead,
                            Status = ledger.Status,
                            Email = ledger.Email,
                            Mobile = ledger.Mobile,
                            BankAccount = ledger.BankAccount,
                            BalanceOpening = ledger.BalanceOpening,
                            BankName = ledger.BankName,
                            BranchName = ledger.BranchName,
                            BalanceIn = ledger.BalanceIn,
                            BankAccNo = ledger.BankAccNo,
                            BankBranch = ledger.BankBranch,
                            CCEmailId = ledger.CCEmailId,
                            OtherEmailId = ledger.OtherEmailId,
                            AAdharNumber = ledger.AAdharNumber,
                            AccHolderName = ledger.AccHolderName,
                            Address = ledger.Address,
                            Address1 = ledger.Address1,
                            Address2 = ledger.Address2,
                            GSTIN = ledger.GSTIN,
                            PANNo = ledger.PANNo,
                            IFSCCode = ledger.IFSCCode,
                            RefID = ledger.RefID,
                            RegistrationType = ledger.RegistrationType,
                            City = ledger.City,
                            State = ledger.State,
                            Pincode = ledger.Pincode,
                            Country = ledger.Country,
                            ContactPerson = ledger.ContactPerson,
                            PartyType = ledger.PartyType,
                            PaymentTerm = ledger.PaymentTerm,
                            Password = ledger.Password,
                            UserName = ledger.UserName,
                            Agent = ledger.Agent,
                            WalkinLedger = ledger.WalkinLedger,
                            OpeningBalance = _context.financialLedgers
                                .Where(a => a.LedgerId == ledger.LedgerId)
                                .Select(a => a.OpeningBalance)
                                .FirstOrDefault(),
                            CityStatePincode = ledger.CityStatePincode,
                            NameAddressMobile = ledger.NameAddressMobile,
                            DueDays = ledger.DueDays,
                            TelNo = ledger.TelNo,
                            AltMobile = ledger.AltMobile,
                            Taxable = ledger.Taxable,
                            TaxLedger = ledger.TaxLedger,
                            VehicleExpense = ledger.VehicleExpense,
                            TDSPercent = ledger.TDSPercent,
                            IsActive = ledger.IsActive,
                            IsDeleted = ledger.IsDeleted,
                            CreatedOn = ledger.CreatedOn,
                            UpdatedOn = ledger.UpdatedOn,
                            UpdatedBy = ledger.UpdatedBy,
                            CreatedBy = ledger.CreatedBy
                        };

            // Apply Filters
            if (!string.IsNullOrEmpty(AccGroup))
            {
                query = query.Where(x => x.AccGroup != null &&
                                        x.AccGroup.ToLower().Contains(AccGroup.ToLower()));
            }

            if (!string.IsNullOrEmpty(Branch))
            {
                query = query.Where(x => x.BranchName != null &&
                                        x.BranchName.ToLower().Contains(Branch.ToLower()));
            }

            if (fromDate.HasValue)
            {
                query = query.Where(x => x.CreatedOn.HasValue &&
                                       x.CreatedOn.Value.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.CreatedOn.HasValue &&
                                       x.CreatedOn.Value.Date <= toDate.Value.Date);
            }

            var results = query.ToList();

            return View("LedgerReportIndex", results);
        }

    [HttpGet]
       public IActionResult ExportToExcel()
        {
            var data = _context.ledgerEntity.Select(x => new LedgerMasterDTO
            {
                LedgerId = x.LedgerId,
                subgroupName = _context.SubGroups.Where(a => a.subgroupId == x.subgroupId).Select(a => a.SubGroupName).FirstOrDefault(),
                AccHead = x.AccHead,
                Email = x.Email,
                Mobile = x.Mobile,
                BalanceOpening = x.BalanceOpening,
                AccGroup = x.AccGroup,
                Status = x.Status,
                IsActive = x.IsActive,
                BankAccount = x.BankAccount,
                TaxLedger = x.TaxLedger,
                Taxable = x.Taxable,
                VehicleExpense = x.VehicleExpense,
                TDSPercent = x.TDSPercent,
                BalanceIn = x.BalanceIn,
                OpeningBalance = x.OpeningBalance,
                UserName = x.UserName,
                ContactPerson = x.ContactPerson,
                AltMobile = x.AltMobile,
                TelNo = x.TelNo,
                RefID = x.RefID,
                CCEmailId = x.CCEmailId,
                OtherEmailId = x.OtherEmailId,
                VendorCode = x.VendorCode,
                Address1 = x.Address1,
                Address2 = x.Address2,
                City = x.City,
                State = x.State,
                Country = x.Country,
                RegistrationType = x.RegistrationType,
                PartyType = x.PartyType,
                CINNo = x.CINNo,
                GSTIN = x.GSTIN,
                PANNo = x.PANNo,
                AAdharNumber = x.AAdharNumber,
                Pincode = x.Pincode,
                AccHolderName = x.AccHolderName,
                BankName = x.BankName,
                BankAccNo = x.BankAccNo,
                BankBranch = x.BankBranch,
                IFSCCode = x.IFSCCode,
                PaymentTerm = x.PaymentTerm,
                DueDays = x.DueDays,
                Agent = x.Agent,
                Password = x.Password,
                BranchName = x.BranchName,
                NameAddressMobile = x.NameAddressMobile,
                Address = x.Address,
                CityStatePincode = x.CityStatePincode,
                WalkinLedger = x.WalkinLedger
            }).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ledger");

                var headers = new string[]
                {
            "LedgerId", "SubgroupName", "AccHead", "Email", "Mobile", "BalanceOpening", "AccGroup", "Status",
            "IsActive", "BankAccount", "TaxLedger", "Taxable", "VehicleExpense", "TDSPercent", "BalanceIn",
            "OpeningBalance", "UserName", "ContactPerson", "AltMobile", "TelNo", "RefID", "CCEmailId", "OtherEmailId",
            "VendorCode", "Address1", "Address2", "City", "State", "Country", "RegistrationType", "PartyType",
            "CINNo", "GSTIN", "PANNo", "AAdharNumber", "Pincode", "AccHolderName", "BankName", "BankAccNo",
            "BankBranch", "IFSCCode", "PaymentTerm", "DueDays", "Agent", "Password", "BranchName", "NameAddressMobile",
            "Address", "CityStatePincode", "WalkinLedger"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.LedgerId;
                    worksheet.Cell(row, 2).Value = item.subgroupName;
                    worksheet.Cell(row, 3).Value = item.AccHead;
                    worksheet.Cell(row, 4).Value = item.Email;
                    worksheet.Cell(row, 5).Value = item.Mobile;
                    worksheet.Cell(row, 6).Value = item.BalanceOpening;
                    worksheet.Cell(row, 7).Value = item.AccGroup;
                    worksheet.Cell(row, 8).Value = item.Status;
                    worksheet.Cell(row, 9).Value = item.IsActive.ToString();
                    worksheet.Cell(row, 10).Value = item.BankAccount.ToString();
                    worksheet.Cell(row, 11).Value = item.TaxLedger.ToString();
                    worksheet.Cell(row, 12).Value = item.Taxable.ToString();
                    worksheet.Cell(row, 13).Value = item.VehicleExpense.ToString();
                    worksheet.Cell(row, 14).Value = item.TDSPercent;
                    worksheet.Cell(row, 15).Value = item.BalanceIn;
                    worksheet.Cell(row, 16).Value = item.OpeningBalance;
                    worksheet.Cell(row, 17).Value = item.UserName;
                    worksheet.Cell(row, 18).Value = item.ContactPerson;
                    worksheet.Cell(row, 19).Value = item.AltMobile;
                    worksheet.Cell(row, 20).Value = item.TelNo;
                    worksheet.Cell(row, 21).Value = item.RefID;
                    worksheet.Cell(row, 22).Value = item.CCEmailId;
                    worksheet.Cell(row, 23).Value = item.OtherEmailId;
                    worksheet.Cell(row, 24).Value = item.VendorCode;
                    worksheet.Cell(row, 25).Value = item.Address1;
                    worksheet.Cell(row, 26).Value = item.Address2;
                    worksheet.Cell(row, 27).Value = item.City;
                    worksheet.Cell(row, 28).Value = item.State;
                    worksheet.Cell(row, 29).Value = item.Country;
                    worksheet.Cell(row, 30).Value = item.RegistrationType;
                    worksheet.Cell(row, 31).Value = item.PartyType;
                    worksheet.Cell(row, 32).Value = item.CINNo;
                    worksheet.Cell(row, 33).Value = item.GSTIN;
                    worksheet.Cell(row, 34).Value = item.PANNo;
                    worksheet.Cell(row, 35).Value = item.AAdharNumber;
                    worksheet.Cell(row, 36).Value = item.Pincode;
                    worksheet.Cell(row, 37).Value = item.AccHolderName;
                    worksheet.Cell(row, 38).Value = item.BankName;
                    worksheet.Cell(row, 39).Value = item.BankAccNo;
                    worksheet.Cell(row, 40).Value = item.BankBranch;
                    worksheet.Cell(row, 41).Value = item.IFSCCode;
                    worksheet.Cell(row, 42).Value = item.PaymentTerm;
                    worksheet.Cell(row, 43).Value = item.DueDays;
                    worksheet.Cell(row, 44).Value = item.Agent;
                    worksheet.Cell(row, 45).Value = item.Password;
                    worksheet.Cell(row, 46).Value = item.BranchName;
                    worksheet.Cell(row, 47).Value = item.NameAddressMobile;
                    worksheet.Cell(row, 48).Value = item.Address;
                    worksheet.Cell(row, 49).Value = item.CityStatePincode;
                    worksheet.Cell(row, 50).Value = item.WalkinLedger.ToString();
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LedgerMasterReport.xlsx");
                }
            }
        }
        [HttpGet]
        public IActionResult ExportToPdf()
        {
            var data = _context.ledgerEntity
                .Select(x => new LedgerMasterDTO
                {
                    LedgerId = x.LedgerId,
                    subgroupName = _context.SubGroups.Where(a=>a.subgroupId==x.subgroupId).Select(a=>a.SubGroupName).FirstOrDefault(),
                    AccHead = x.AccHead,
                    Email = x.Email,
                    Mobile = x.Mobile,
                    BalanceOpening = x.BalanceOpening,
                    AccGroup = x.AccGroup,
                    Status = x.Status,
                    IsActive = x.IsActive,
                    BankAccount = x.BankAccount,
                    TaxLedger = x.TaxLedger,
                    Taxable = x.Taxable,
                    VehicleExpense = x.VehicleExpense,
                    TDSPercent = x.TDSPercent,
                    BalanceIn = x.BalanceIn,
                    OpeningBalance = x.OpeningBalance,
                    UserName = x.UserName,
                    ContactPerson = x.ContactPerson,
                    AltMobile = x.AltMobile,
                    TelNo = x.TelNo,
                    RefID = x.RefID,
                    CCEmailId = x.CCEmailId,
                    OtherEmailId = x.OtherEmailId,
                    VendorCode = x.VendorCode,
                    Address1 = x.Address1,
                    Address2 = x.Address2,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    RegistrationType = x.RegistrationType,
                    PartyType = x.PartyType,
                    CINNo = x.CINNo,
                    GSTIN = x.GSTIN,
                    PANNo = x.PANNo,
                    AAdharNumber = x.AAdharNumber,
                    Pincode = x.Pincode,
                    AccHolderName = x.AccHolderName,
                    BankName = x.BankName,
                    BankAccNo = x.BankAccNo,
                    BankBranch = x.BankBranch,
                    IFSCCode = x.IFSCCode,
                    PaymentTerm = x.PaymentTerm,
                    DueDays = x.DueDays,
                    Agent = x.Agent,
                    Password = x.Password,
                    BranchName = x.BranchName,
                    NameAddressMobile = x.NameAddressMobile,
                    Address = x.Address,
                    CityStatePincode = x.CityStatePincode,
                    WalkinLedger = x.WalkinLedger,
                    CreatedOn = x.CreatedOn,
                    CreatedBy = x.CreatedBy,
                    UpdatedOn = x.UpdatedOn,
                    UpdatedBy = x.UpdatedBy,
                    IsDeleted = x.IsDeleted
                })
                .ToList();

            using (var ms = new MemoryStream())
            {
                var customPageSize = new Rectangle(2000, 1400); // width x height in points
                var document = new iTextSharp.text.Document(customPageSize);

                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Create table with as many columns as needed (54 here)
                var table = new PdfPTable(54)
                {
                    WidthPercentage = 100,
                    HeaderRows = 1
                };

                string[] headers = new string[]
                {
            "LedgerId", "SubgroupName", "AccHead", "Email", "Mobile", "BalanceOpening", "AccGroup", "Status",
            "IsActive", "BankAccount", "TaxLedger", "Taxable", "VehicleExpense", "TDSPercent", "BalanceIn",
            "OpeningBalance", "UserName", "ContactPerson", "AltMobile", "TelNo", "RefID", "CCEmailId", "OtherEmailId",
            "VendorCode", "Address1", "Address2", "City", "State", "Country", "RegistrationType", "PartyType",
            "CINNo", "GSTIN", "PANNo", "AAdharNumber", "Pincode", "AccHolderName", "BankName", "BankAccNo",
            "BankBranch", "IFSCCode", "PaymentTerm", "DueDays", "Agent", "Password", "BranchName", "NameAddressMobile",
            "Address", "CityStatePincode", "WalkinLedger", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy", "IsDeleted"
                };

                // Add headers
                foreach (var header in headers)
                {
                    table.AddCell(new PdfPCell(new Phrase(header, FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 6)))
                    {
                        BackgroundColor = new BaseColor(220, 220, 220)
                    });
                }
                // Add data rows
                foreach (var item in data)
                {
                    table.AddCell(item.LedgerId.ToString());
                    table.AddCell(item.subgroupName ?? "");
                    table.AddCell(item.AccHead ?? "");
                    table.AddCell(item.Email ?? "");
                    table.AddCell(item.Mobile ?? "");
                    table.AddCell(item.BalanceOpening?.ToString("F2") ?? "0.00");
                    table.AddCell(item.AccGroup ?? "");
                    table.AddCell(item.Status ?? "");
                    table.AddCell(item.IsActive?.ToString() ?? "");
                    table.AddCell(item.BankAccount?.ToString() ?? "");
                    table.AddCell(item.TaxLedger?.ToString() ?? "");
                    table.AddCell(item.Taxable?.ToString() ?? "");
                    table.AddCell(item.VehicleExpense?.ToString() ?? "");
                    table.AddCell(item.TDSPercent?.ToString("F2") ?? "");
                    table.AddCell(item.BalanceIn ?? "");
                    table.AddCell(item.OpeningBalance?.ToString("F2") ?? "");
                    table.AddCell(item.UserName ?? "");
                    table.AddCell(item.ContactPerson ?? "");
                    table.AddCell(item.AltMobile ?? "");
                    table.AddCell(item.TelNo ?? "");
                    table.AddCell(item.RefID ?? "");
                    table.AddCell(item.CCEmailId ?? "");
                    table.AddCell(item.OtherEmailId ?? "");
                    table.AddCell(item.VendorCode ?? "");
                    table.AddCell(item.Address1 ?? "");
                    table.AddCell(item.Address2 ?? "");
                    table.AddCell(item.City ?? "");
                    table.AddCell(item.State ?? "");
                    table.AddCell(item.Country ?? "");
                    table.AddCell(item.RegistrationType ?? "");
                    table.AddCell(item.PartyType ?? "");
                    table.AddCell(item.CINNo ?? "");
                    table.AddCell(item.GSTIN ?? "");
                    table.AddCell(item.PANNo ?? "");
                    table.AddCell(item.AAdharNumber ?? "");
                    table.AddCell(item.Pincode?.ToString() ?? "");
                    table.AddCell(item.AccHolderName ?? "");
                    table.AddCell(item.BankName ?? "");
                    table.AddCell(item.BankAccNo ?? "");
                    table.AddCell(item.BankBranch ?? "");
                    table.AddCell(item.IFSCCode ?? "");
                    table.AddCell(item.PaymentTerm ?? "");
                    table.AddCell(item.DueDays?.ToString() ?? "");
                    table.AddCell(item.Agent ?? "");
                    table.AddCell(item.Password ?? "");
                    table.AddCell(item.BranchName ?? "");
                    table.AddCell(item.NameAddressMobile ?? "");
                    table.AddCell(item.Address ?? "");
                    table.AddCell(item.CityStatePincode ?? "");
                    table.AddCell(item.WalkinLedger?.ToString() ?? "");
                    table.AddCell(item.CreatedOn?.ToString("dd-MM-yyyy") ?? "");
                    table.AddCell(item.CreatedBy?.ToString() ?? "");
                    table.AddCell(item.UpdatedOn?.ToString("dd-MM-yyyy") ?? "");
                    table.AddCell(item.UpdatedBy?.ToString() ?? "");
                    table.AddCell(item.IsDeleted?.ToString() ?? "");
                }

                document.Add(table);
                document.Close();
                return File(ms.ToArray(), "application/pdf", "LedgerMasterReport.pdf");
            }
        }
        private List<string> GetColumnNames(string tableName)
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
}