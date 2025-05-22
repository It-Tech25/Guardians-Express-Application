using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Repositories.Repos
{
    public class CompanyConfigurationRepo : ICompanyConfigurationRepo
    {
        private readonly MyDbContext _context;
        public CompanyConfigurationRepo(MyDbContext dbContext)
        {
            _context = dbContext;
        }

        public List<CompanyConfigurationViewModel> CompanyConfigurationList(string searchTerm, int pageNumber, int pageSize)
        {
            var res = _context.CompanyConfigurations
                              .Where(x => x.IsDeleted == false)
                              .Select(x => new CompanyConfigurationViewModel
                              {
                                  CompanyId = x.CompanyId,
                                  Name = x.Name,
                                  Email = x.Email,
                                  InfoEmails = x.InfoEmails,
                                  Phone = x.Phone,
                                  TelNo = x.TelNo,
                                  Fax = x.Fax,
                                  Website = x.Website,
                                  PANNo = x.PANNo,
                                  GSTIN = x.GSTIN,
                                  CINNumber = x.CINNumber,
                                  HSCSSN = x.HSCSSN,


                                  CompanyLogoPath = x.CompanyLogoPath,
                                  LetterHeadImagePath = x.LetterHeadImagePath,
                                  EnableSMS = x.EnableSMS,
                                  EnableEmail = x.EnableEmail,
                                  IsDefault = x.IsDefault,
                                  ChangeTaxOnInvoice = x.ChangeTaxOnInvoice,
                                  ShowItemImageInLead = x.ShowItemImageInLead,
                                  ShowItemImageInPO = x.ShowItemImageInPO,
                                  EnableKYCVerification = x.EnableKYCVerification,
                                  CreatedOn = x.CreatedOn,
                                  UpdatedOn = x.UpdatedOn,
                                  CreatedBy = x.CreatedBy,
                                  UpdatedBy = x.UpdatedBy,
                                  IsDeleted = x.IsDeleted,
                                  AddressLine1 = x.AddressLine1,
                                  AddressLine2 = x.AddressLine2,
                                  City = x.City,
                                  State = x.State,
                                  Country = x.Country,
                                  Pincode = x.Pincode,
                                 
                                  RoundOffInvoice = x.RoundOffInvoice,
                                  RequiredTransportDetails = x.RequiredTransportDetails,
                                  ServiceIndustry = x.ServiceIndustry,
                                  DuplicateItemInBill = x.DuplicateItemInBill,
                                  ImportGoods = x.ImportGoods,
                                  BarcodeBilling = x.BarcodeBilling,
                                  BarcodeOnSrno = x.BarcodeOnSrno,
                                  ExportTCSApplicable = x.ExportTCSApplicable,
                                  ProvisionForBilling = x.ProvisionForBilling,
                                  ManualWtInBilling = x.ManualWtInBilling,
                                  ApprovedVoucherInLedger = x.ApprovedVoucherInLedger,
                                  ManualRate = x.ManualRate,
                                  DispatchWiseGR = x.DispatchWiseGR,
                                  TransShipment = x.TransShipment,
                                  ClubVoucherEntryTotal = x.ClubVoucherEntryTotal,
                                  ShowGroupInVoucherEntry = x.ShowGroupInVoucherEntry,
                                  DirectDebitDriverTransporter = x.DirectDebitDriverTransporter,
                                  ResetGRNoFinancialYearWise = x.ResetGRNoFinancialYearWise,
                                  UpcomingEMI = x.UpcomingEMI,
                                  LedgerWiseDetail = x.LedgerWiseDetail,
                                  LockEntryTill = x.LockEntryTill,
                                  TCSAmount = x.TCSAmount,
                                  InvoiceNoMinLength = x.InvoiceNoMinLength,
                                  ContractDueDays = x.ContractDueDays,
                                  EwayBillAmount = x.EwayBillAmount,
                                  ORCPercentage = x.ORCPercentage,
                                  AutoInvoiceTime = x.AutoInvoiceTime,
                                  PasswordResetDays = x.PasswordResetDays,
                                  URLExpireTime = x.URLExpireTime,
                                  Status = x.Status,
                                  WhiteListIP = x.WhiteListIP,
                                  InvoiceTemplate = x.InvoiceTemplate,
                                  DebitNoteTemplate = x.DebitNoteTemplate,
                                  LeadTemplate = x.LeadTemplate,
                                  GRTemplate = x.GRTemplate,
                                  GRReceiveTemplate = x.GRReceiveTemplate,
                                  BillTemplate = x.BillTemplate,
                                  BillDetails1Template = x.BillDetails1Template,
                                  BillDetails2Template = x.BillDetails2Template,
                                  HireTemplate = x.HireTemplate,
                                  HireTransporterTemplate = x.HireTransporterTemplate,
                                  SaleOrderTemplate = x.SaleOrderTemplate,
                                  DefaultTemplate = x.DefaultTemplate,
                                  LeadPDFTitle = x.LeadPDFTitle,
                                  SOTopHeight = x.SOTopHeight,
                                  SOHeaderHeight = x.SOHeaderHeight,
                                  SOFooterHeight = x.SOFooterHeight,
                                  LeadTopHeight = x.LeadTopHeight,
                                  LeadHeaderHeight = x.LeadHeaderHeight,
                                  LeadFooterHeight = x.LeadFooterHeight,
                                  InvTopHeight = x.InvTopHeight,
                                  InvHeaderHeight = x.InvHeaderHeight,
                                  InvFooterHeight = x.InvFooterHeight,
                                  GRTopHeight = x.GRTopHeight,
                                  GRHeaderHeight = x.GRHeaderHeight,
                                  GRFooterHeight = x.GRFooterHeight,
                                  BillTopHeight = x.BillTopHeight,
                                  BillHeaderHeight = x.BillHeaderHeight,
                                  BillFooterHeight = x.BillFooterHeight,
                                  Bill1TopHeight = x.Bill1TopHeight,
                                  Bill1HeaderHeight = x.Bill1HeaderHeight,
                                  Bill1FooterHeight = x.Bill1FooterHeight,
                                  Bill2TopHeight = x.Bill2TopHeight,
                                  Bill2HeaderHeight = x.Bill2HeaderHeight,
                                  Bill2FooterHeight = x.Bill2FooterHeight,
                                  HireTopHeight = x.HireTopHeight,
                                  HireHeaderHeight = x.HireHeaderHeight,
                                  HireFooterHeight = x.HireFooterHeight,
                                  HireTransporterTopHeight = x.HireTransporterTopHeight,
                                  HireTransporterHeaderHeight = x.HireTransporterHeaderHeight,
                                  HireTransporterFooterHeight = x.HireTransporterFooterHeight,
                                  CalDashEventCount = x.CalDashEventCount,
                                  EInvoiceUsername = x.EInvoiceUsername,
                                  EInvoicePassword = x.EInvoicePassword
                              })
                              .ToList();

            return res;
        }

        public CompanyConfigurationViewModel GetCompanyConfigurationById(int id)
        {
            var entity = _context.CompanyConfigurations
                .Where(x => x.CompanyId == id && x.IsDeleted == false)
                .Select(x => new CompanyConfigurationViewModel
                {
                    CompanyId = x.CompanyId,
                    Name = x.Name,
                    Email = x.Email,
                    InfoEmails = x.InfoEmails,
                    Phone = x.Phone,
                    TelNo = x.TelNo,
                    Fax = x.Fax,
                    Website = x.Website,
                    PANNo = x.PANNo,
                    GSTIN = x.GSTIN,
                    CINNumber = x.CINNumber,
                    HSCSSN = x.HSCSSN,


                    CompanyLogoPath = x.CompanyLogoPath,
                    LetterHeadImagePath = x.LetterHeadImagePath,
                    EnableSMS = x.EnableSMS,
                    EnableEmail = x.EnableEmail,
                    IsDefault = x.IsDefault,
                    ChangeTaxOnInvoice = x.ChangeTaxOnInvoice,
                    ShowItemImageInLead = x.ShowItemImageInLead,
                    ShowItemImageInPO = x.ShowItemImageInPO,
                    EnableKYCVerification = x.EnableKYCVerification,
                    CreatedOn = x.CreatedOn,
                    UpdatedOn = x.UpdatedOn,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    IsDeleted = x.IsDeleted,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    City = x.City,
                    State = x.State,
                    Country = x.Country,
                    Pincode = x.Pincode,
                   
                    RoundOffInvoice = x.RoundOffInvoice,
                    RequiredTransportDetails = x.RequiredTransportDetails,
                    ServiceIndustry = x.ServiceIndustry,
                    DuplicateItemInBill = x.DuplicateItemInBill,
                    ImportGoods = x.ImportGoods,
                    BarcodeBilling = x.BarcodeBilling,
                    BarcodeOnSrno = x.BarcodeOnSrno,
                    ExportTCSApplicable = x.ExportTCSApplicable,
                    ProvisionForBilling = x.ProvisionForBilling,
                    ManualWtInBilling = x.ManualWtInBilling,
                    ApprovedVoucherInLedger = x.ApprovedVoucherInLedger,
                    ManualRate = x.ManualRate,
                    DispatchWiseGR = x.DispatchWiseGR,
                    TransShipment = x.TransShipment,
                    ClubVoucherEntryTotal = x.ClubVoucherEntryTotal,
                    ShowGroupInVoucherEntry = x.ShowGroupInVoucherEntry,
                    DirectDebitDriverTransporter = x.DirectDebitDriverTransporter,
                    ResetGRNoFinancialYearWise = x.ResetGRNoFinancialYearWise,
                    UpcomingEMI = x.UpcomingEMI,
                    LedgerWiseDetail = x.LedgerWiseDetail,
                    LockEntryTill = x.LockEntryTill,
                    TCSAmount = x.TCSAmount,
                    InvoiceNoMinLength = x.InvoiceNoMinLength,
                    ContractDueDays = x.ContractDueDays,
                    EwayBillAmount = x.EwayBillAmount,
                    ORCPercentage = x.ORCPercentage,
                    AutoInvoiceTime = x.AutoInvoiceTime,
                    PasswordResetDays = x.PasswordResetDays,
                    URLExpireTime = x.URLExpireTime,
                    Status = x.Status,
                    WhiteListIP = x.WhiteListIP,
                    InvoiceTemplate = x.InvoiceTemplate,
                    DebitNoteTemplate = x.DebitNoteTemplate,
                    LeadTemplate = x.LeadTemplate,
                    GRTemplate = x.GRTemplate,
                    GRReceiveTemplate = x.GRReceiveTemplate,
                    BillTemplate = x.BillTemplate,
                    BillDetails1Template = x.BillDetails1Template,
                    BillDetails2Template = x.BillDetails2Template,
                    HireTemplate = x.HireTemplate,
                    HireTransporterTemplate = x.HireTransporterTemplate,
                    SaleOrderTemplate = x.SaleOrderTemplate,
                    DefaultTemplate = x.DefaultTemplate,
                    LeadPDFTitle = x.LeadPDFTitle,
                    SOTopHeight = x.SOTopHeight,
                    SOHeaderHeight = x.SOHeaderHeight,
                    SOFooterHeight = x.SOFooterHeight,
                    LeadTopHeight = x.LeadTopHeight,
                    LeadHeaderHeight = x.LeadHeaderHeight,
                    LeadFooterHeight = x.LeadFooterHeight,
                    InvTopHeight = x.InvTopHeight,
                    InvHeaderHeight = x.InvHeaderHeight,
                    InvFooterHeight = x.InvFooterHeight,
                    GRTopHeight = x.GRTopHeight,
                    GRHeaderHeight = x.GRHeaderHeight,
                    GRFooterHeight = x.GRFooterHeight,
                    BillTopHeight = x.BillTopHeight,
                    BillHeaderHeight = x.BillHeaderHeight,
                    BillFooterHeight = x.BillFooterHeight,
                    Bill1TopHeight = x.Bill1TopHeight,
                    Bill1HeaderHeight = x.Bill1HeaderHeight,
                    Bill1FooterHeight = x.Bill1FooterHeight,
                    Bill2TopHeight = x.Bill2TopHeight,
                    Bill2HeaderHeight = x.Bill2HeaderHeight,
                    Bill2FooterHeight = x.Bill2FooterHeight,
                    HireTopHeight = x.HireTopHeight,
                    HireHeaderHeight = x.HireHeaderHeight,
                    HireFooterHeight = x.HireFooterHeight,
                    HireTransporterTopHeight = x.HireTransporterTopHeight,
                    HireTransporterHeaderHeight = x.HireTransporterHeaderHeight,
                    HireTransporterFooterHeight = x.HireTransporterFooterHeight,
                    CalDashEventCount = x.CalDashEventCount,
                    EInvoiceUsername = x.EInvoiceUsername,
                    EInvoicePassword = x.EInvoicePassword
                })
                .FirstOrDefault();

            return entity;
        }

        public GenericResponse AddCompanyConfiguration(CompanyConfigurationViewModel req)
        {
            GenericResponse checkStatus = new GenericResponse();

            var companyExists = _context.CompanyConfigurations
                .Where(i => i.Name == req.Name && i.IsDeleted == false)
                .FirstOrDefault();

            if (companyExists != null)
            {
                checkStatus.statuCode = 0;
                checkStatus.message = "Company Configuration Already exists.";
                checkStatus.currentId = 0;
                return checkStatus;
            }

            try
            {
                CompanyConfigurationEntity response = new CompanyConfigurationEntity
                {
                    Name = req.Name,
                    Email = req.Email,
                    InfoEmails = req.InfoEmails,
                    Phone = req.Phone,
                    TelNo = req.TelNo,
                    Fax = req.Fax,
                    Website = req.Website,
                    PANNo = req.PANNo,
                    GSTIN = req.GSTIN,
                    CINNumber = req.CINNumber,
                    HSCSSN = req.HSCSSN,


                    CompanyLogoPath = req.CompanyLogoPath,
                    LetterHeadImagePath = req.LetterHeadImagePath,
                    EnableSMS = req.EnableSMS,
                    EnableEmail = req.EnableEmail,
                    IsDefault = req.IsDefault,
                    ChangeTaxOnInvoice = req.ChangeTaxOnInvoice,
                    ShowItemImageInLead = req.ShowItemImageInLead,
                    ShowItemImageInPO = req.ShowItemImageInPO,
                    EnableKYCVerification = req.EnableKYCVerification,
                    AddressLine1 = req.AddressLine1,
                    AddressLine2 = req.AddressLine2,
                    City = req.City,
                    State = req.State,
                    Country = req.Country,
                    Pincode = req.Pincode,
                   
                    RoundOffInvoice = req.RoundOffInvoice,
                    RequiredTransportDetails = req.RequiredTransportDetails,
                    ServiceIndustry = req.ServiceIndustry,
                    DuplicateItemInBill = req.DuplicateItemInBill,
                    ImportGoods = req.ImportGoods,
                    BarcodeBilling = req.BarcodeBilling,
                    BarcodeOnSrno = req.BarcodeOnSrno,
                    ExportTCSApplicable = req.ExportTCSApplicable,
                    ProvisionForBilling = req.ProvisionForBilling,
                    ManualWtInBilling = req.ManualWtInBilling,
                    ApprovedVoucherInLedger = req.ApprovedVoucherInLedger,
                    ManualRate = req.ManualRate,
                    DispatchWiseGR = req.DispatchWiseGR,
                    TransShipment = req.TransShipment,
                    ClubVoucherEntryTotal = req.ClubVoucherEntryTotal,
                    ShowGroupInVoucherEntry = req.ShowGroupInVoucherEntry,
                    DirectDebitDriverTransporter = req.DirectDebitDriverTransporter,
                    ResetGRNoFinancialYearWise = req.ResetGRNoFinancialYearWise,
                    UpcomingEMI = req.UpcomingEMI,
                    LedgerWiseDetail = req.LedgerWiseDetail,
                    LockEntryTill = req.LockEntryTill,
                    TCSAmount = req.TCSAmount,
                    InvoiceNoMinLength = req.InvoiceNoMinLength,
                    ContractDueDays = req.ContractDueDays,
                    EwayBillAmount = req.EwayBillAmount,
                    ORCPercentage = req.ORCPercentage,
                    AutoInvoiceTime = req.AutoInvoiceTime,
                    PasswordResetDays = req.PasswordResetDays,
                    URLExpireTime = req.URLExpireTime,
                    Status = req.Status,
                    WhiteListIP = req.WhiteListIP,
                    InvoiceTemplate = req.InvoiceTemplate,
                    DebitNoteTemplate = req.DebitNoteTemplate,
                    LeadTemplate = req.LeadTemplate,
                    GRTemplate = req.GRTemplate,
                    GRReceiveTemplate = req.GRReceiveTemplate,
                    BillTemplate = req.BillTemplate,
                    BillDetails1Template = req.BillDetails1Template,
                    BillDetails2Template = req.BillDetails2Template,
                    HireTemplate = req.HireTemplate,
                    HireTransporterTemplate = req.HireTransporterTemplate,
                    SaleOrderTemplate = req.SaleOrderTemplate,
                    DefaultTemplate = req.DefaultTemplate,
                    LeadPDFTitle = req.LeadPDFTitle,
                    SOTopHeight = req.SOTopHeight,
                    SOHeaderHeight = req.SOHeaderHeight,
                    SOFooterHeight = req.SOFooterHeight,
                    LeadTopHeight = req.LeadTopHeight,
                    LeadHeaderHeight = req.LeadHeaderHeight,
                    LeadFooterHeight = req.LeadFooterHeight,
                    InvTopHeight = req.InvTopHeight,
                    InvHeaderHeight = req.InvHeaderHeight,
                    InvFooterHeight = req.InvFooterHeight,
                    GRTopHeight = req.GRTopHeight,
                    GRHeaderHeight = req.GRHeaderHeight,
                    GRFooterHeight = req.GRFooterHeight,
                    BillTopHeight = req.BillTopHeight,
                    BillHeaderHeight = req.BillHeaderHeight,
                    BillFooterHeight = req.BillFooterHeight,
                    Bill1TopHeight = req.Bill1TopHeight,
                    Bill1HeaderHeight = req.Bill1HeaderHeight,
                    Bill1FooterHeight = req.Bill1FooterHeight,
                    Bill2TopHeight = req.Bill2TopHeight,
                    Bill2HeaderHeight = req.Bill2HeaderHeight,
                    Bill2FooterHeight = req.Bill2FooterHeight,
                    HireTopHeight = req.HireTopHeight,
                    HireHeaderHeight = req.HireHeaderHeight,
                    HireFooterHeight = req.HireFooterHeight,
                    HireTransporterTopHeight = req.HireTransporterTopHeight,
                    HireTransporterHeaderHeight = req.HireTransporterHeaderHeight,
                    HireTransporterFooterHeight = req.HireTransporterFooterHeight,
                    CalDashEventCount = req.CalDashEventCount,
                    EInvoiceUsername = req.EInvoiceUsername,
                    EInvoicePassword = req.EInvoicePassword,
                    IsDeleted = false,
                    CreatedBy = req.CompanyId,
                    CreatedOn = DateTime.Today,
                    UpdatedOn = DateTime.Today,
                    UpdatedBy = req.CompanyId,
                    
                   
                };

                _context.CompanyConfigurations.Add(response);
                _context.SaveChanges();

                checkStatus.statuCode = 1;
                checkStatus.message = "Add Successful.";
                checkStatus.currentId = response.CompanyId;
            }
            catch (Exception ex)
            {
                checkStatus.statuCode = 0;
                checkStatus.message = "Add failed";
                checkStatus.ErrorMessage = ex.Message;
            }

            return checkStatus;
        }

        public GenericResponse UpdateCompanyConfiguration(CompanyConfigurationViewModel req)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var entity = _context.CompanyConfigurations
                    .Where(a => a.CompanyId == req.CompanyId).FirstOrDefault();

                if (entity == null)
                {
                    response.message = "Record not found";
                    response.currentId = 0;
                    return response;
                }

                entity.Name = req.Name;
                entity.Email = req.Email;
                entity.InfoEmails = req.InfoEmails;
                entity.Phone = req.Phone;
                entity.TelNo = req.TelNo;
                entity.Fax = req.Fax;
                entity.Website = req.Website;
                entity.PANNo = req.PANNo;
                entity.GSTIN = req.GSTIN;
                entity.CINNumber = req.CINNumber;
                entity.HSCSSN = req.HSCSSN;
             
                entity.CompanyLogoPath = req.CompanyLogoPath;
                entity.LetterHeadImagePath = req.LetterHeadImagePath;
                entity.EnableSMS = req.EnableSMS;
                entity.EnableEmail = req.EnableEmail;
                entity.IsDefault = req.IsDefault;
                entity.ChangeTaxOnInvoice = req.ChangeTaxOnInvoice;
                entity.ShowItemImageInLead = req.ShowItemImageInLead;
                entity.ShowItemImageInPO = req.ShowItemImageInPO;
                entity.EnableKYCVerification = req.EnableKYCVerification;
                entity.AddressLine1 = req.AddressLine1;
                entity.AddressLine2 = req.AddressLine2;
                entity.City = req.City;
                entity.State = req.State;
                entity.Country = req.Country;
                entity.Pincode = req.Pincode;
               
                entity.RoundOffInvoice = req.RoundOffInvoice;
                entity.RequiredTransportDetails = req.RequiredTransportDetails;
                entity.ServiceIndustry = req.ServiceIndustry;
                entity.DuplicateItemInBill = req.DuplicateItemInBill;
                entity.ImportGoods = req.ImportGoods;
                entity.BarcodeBilling = req.BarcodeBilling;
                entity.BarcodeOnSrno = req.BarcodeOnSrno;
                entity.ExportTCSApplicable = req.ExportTCSApplicable;
                entity.ProvisionForBilling = req.ProvisionForBilling;
                entity.ManualWtInBilling = req.ManualWtInBilling;
                entity.ApprovedVoucherInLedger = req.ApprovedVoucherInLedger;
                entity.ManualRate = req.ManualRate;
                entity.DispatchWiseGR = req.DispatchWiseGR;
                entity.TransShipment = req.TransShipment;
                entity.ClubVoucherEntryTotal = req.ClubVoucherEntryTotal;
                entity.ShowGroupInVoucherEntry = req.ShowGroupInVoucherEntry;
                entity.DirectDebitDriverTransporter = req.DirectDebitDriverTransporter;
                entity.ResetGRNoFinancialYearWise = req.ResetGRNoFinancialYearWise;
                entity.UpcomingEMI = req.UpcomingEMI;
                entity.LedgerWiseDetail = req.LedgerWiseDetail;
                entity.LockEntryTill = req.LockEntryTill;
                entity.TCSAmount = req.TCSAmount;
                entity.InvoiceNoMinLength = req.InvoiceNoMinLength;
                entity.ContractDueDays = req.ContractDueDays;
                entity.EwayBillAmount = req.EwayBillAmount;
                entity.ORCPercentage = req.ORCPercentage;
                entity.AutoInvoiceTime = req.AutoInvoiceTime;
                entity.PasswordResetDays = req.PasswordResetDays;
                entity.URLExpireTime = req.URLExpireTime;
                entity.Status = req.Status;
                entity.WhiteListIP = req.WhiteListIP;
                entity.InvoiceTemplate = req.InvoiceTemplate;
                entity.DebitNoteTemplate = req.DebitNoteTemplate;
                entity.LeadTemplate = req.LeadTemplate;
                entity.GRTemplate = req.GRTemplate;
                entity.GRReceiveTemplate = req.GRReceiveTemplate;
                entity.BillTemplate = req.BillTemplate;
                entity.BillDetails1Template = req.BillDetails1Template;
                entity.BillDetails2Template = req.BillDetails2Template;
                entity.HireTemplate = req.HireTemplate;
                entity.HireTransporterTemplate = req.HireTransporterTemplate;
                entity.SaleOrderTemplate = req.SaleOrderTemplate;
                entity.DefaultTemplate = req.DefaultTemplate;
                entity.LeadPDFTitle = req.LeadPDFTitle;
                entity.SOTopHeight = req.SOTopHeight;
                entity.SOHeaderHeight = req.SOHeaderHeight;
                entity.SOFooterHeight = req.SOFooterHeight;
                entity.LeadTopHeight = req.LeadTopHeight;
                entity.LeadHeaderHeight = req.LeadHeaderHeight;
                entity.LeadFooterHeight = req.LeadFooterHeight;
                entity.InvTopHeight = req.InvTopHeight;
                entity.InvHeaderHeight = req.InvHeaderHeight;
                entity.InvFooterHeight = req.InvFooterHeight;
                entity.GRTopHeight = req.GRTopHeight;
                entity.GRHeaderHeight = req.GRHeaderHeight;
                entity.GRFooterHeight = req.GRFooterHeight;
                entity.BillTopHeight = req.BillTopHeight;
                entity.BillHeaderHeight = req.BillHeaderHeight;
                entity.BillFooterHeight = req.BillFooterHeight;
                entity.Bill1TopHeight = req.Bill1TopHeight;
                entity.Bill1HeaderHeight = req.Bill1HeaderHeight;
                entity.Bill1FooterHeight = req.Bill1FooterHeight;
                entity.Bill2TopHeight = req.Bill2TopHeight;
                entity.Bill2HeaderHeight = req.Bill2HeaderHeight;
                entity.Bill2FooterHeight = req.Bill2FooterHeight;
                entity.HireTopHeight = req.HireTopHeight;
                entity.HireHeaderHeight = req.HireHeaderHeight;
                entity.HireFooterHeight = req.HireFooterHeight;
                entity.HireTransporterTopHeight = req.HireTransporterTopHeight;
                entity.HireTransporterHeaderHeight = req.HireTransporterHeaderHeight;
                entity.HireTransporterFooterHeight = req.HireTransporterFooterHeight;
                entity.CalDashEventCount = req.CalDashEventCount;
                entity.EInvoiceUsername = req.EInvoiceUsername;
                entity.EInvoicePassword = req.EInvoicePassword;
                entity.IsDeleted = false;
                entity.UpdatedOn = DateTime.Now;
                entity.UpdatedBy = req.CompanyId;

                _context.CompanyConfigurations.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Success";
                response.currentId = req.CompanyId;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update: " + ex.Message;
                response.currentId = 0;
                return response;
            }
        }

        public GenericResponse DeleteCompanyConfiguration(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var entity = _context.CompanyConfigurations
                    .FirstOrDefault(x => x.CompanyId == id && x.IsDeleted == false);

                if (entity == null)
                {
                    response.statuCode = 0;
                    response.message = "Record not found";
                    return response;
                }

                entity.IsDeleted = true;
                _context.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Delete Successful.";
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Delete failed.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }

        public async Task<GenericResponse> SaveCompany(string companyLogoPath, string letterHeadImagePath)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var companyEntity = new CompanyConfigurationEntity
                {
                    CompanyLogoPath = companyLogoPath,
                    LetterHeadImagePath = letterHeadImagePath,
                    CreatedOn = DateTime.Today,
                    IsDeleted = false
                };

                _context.CompanyConfigurations.Add(companyEntity);
                await _context.SaveChangesAsync();

                response.statuCode = 1;
                response.message = "Company saved successfully.";
                response.currentId = companyEntity.CompanyId;
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Failed to save company.";
                response.ErrorMessage = ex.Message;
            }
            return response;
        }
    }
}