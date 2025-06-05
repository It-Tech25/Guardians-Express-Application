

using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

//using DinkToPdf.Contracts;
//using DinkToPdf;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories;
using GuardiansExpress.Repositories.Implementations;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Repositories.Repos;
using GuardiansExpress.Repositories.Services;
using GuardiansExpress.Repository;
using GuardiansExpress.Repository.Interfaces;
using GuardiansExpress.Services;
using GuardiansExpress.Services.Implementation;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Services.Service;
using GuardiansExpress.Services.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(
           options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")), ServiceLifetime.Transient);

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddCors();

builder.Services.AddMvc().AddNewtonsoftJson(o =>
{
    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});
double sessionTimeout = Convert.ToDouble(builder.Configuration["sessionTimeOut"]);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUserMgmtService, UserMgmtService>();
builder.Services.AddScoped<IUserMgmtRepo, UserMgmtRepo>();
//builder.Services.AddScoped<ICommonService, CommonService>()
builder.Services.AddScoped<ILedgerMasterRepo, LedgerMasterRepo>();
builder.Services.AddScoped<ILedgerMasterService, LedgerMasterService>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeService>();
builder.Services.AddScoped<IVehicleTypeRepo, VehicleTypeRepo>();
builder.Services.AddScoped<IVehicleGroupService, VehicleGroupService>();
builder.Services.AddScoped<IVehicleGroupRepo, VehicleGroupRepo>();
builder.Services.AddScoped<ITaxMaster, TaxMaster>();
builder.Services.AddScoped<IBranchMasterRepo, BranchMasterRepo>();
builder.Services.AddScoped<IBranchMasterService, BranchMasterServices>();
builder.Services.AddScoped<ICountryAndStateRepo, CountryAndStateRepo>();
builder.Services.AddScoped<ICountryAndStateService, CountryAndStateService>();
builder.Services.AddScoped<IGRNoRepo, GRNoRepo>();
builder.Services.AddScoped<IGRNoService, GRNoService>();
builder.Services.AddScoped<IGRService, GRService>();
builder.Services.AddScoped<IExpcredit, ExpCreditNote>();
builder.Services.AddScoped<IGRService, GRService>();
//builder.Services.AddScoped<IExpcredit, ExpCreditNoteService>();
builder.Services.AddScoped<IFinanceService, FinancialService>();
builder.Services.AddScoped<IFinancialRepo, FinancialYearRepo>();
builder.Services.AddScoped<IGroupHeadRepo,GroupHeadRepo>();
builder.Services.AddScoped<IExpCreditNoteRepo, ExpCreditNoteRepo>();
builder.Services.AddScoped<IGroupHeadService,GroupHeadService>();
builder.Services.AddScoped<IBillTypeRepo,BillTypeRepo>();
builder.Services.AddScoped<IBillTypeService, BillTypeService>();
builder.Services.AddScoped<IBillTypeRepo, BillTypeRepo>();
builder.Services.AddScoped<ISubGroupHeadRepo,SubGroupHeadRepo>();
builder.Services.AddScoped<ISubGroupHeadService,SubGroupHeadService>();
builder.Services.AddScoped<ICompanySetupRepo,CompanySetupRepo>();
builder.Services.AddScoped<ICompanySetupService,CompanySetupService>();
builder.Services.AddScoped<ICompanyConfigurationRepo,CompanyConfigurationRepo>();
builder.Services.AddScoped<ICompanyConfigurationService,CompanyConfigurationService>();
builder.Services.AddScoped<IBodyTypeRepo,BodyTypeRepo>();
builder.Services.AddScoped<IBodyTypeService,BodyTypeService>();
builder.Services.AddScoped<IInvoiceTypeService, InvoiceTypeService>();
builder.Services.AddScoped<IInvoiceTypeRepo, InvoiceTypeRepo>();
builder.Services.AddScoped<IContractRepo,ContractRepo>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IGroupSummaryRepository,GroupSummaryRepository>();
builder.Services.AddScoped<IGroupSummaryReportService,GroupSummaryService>();

builder.Services.AddScoped<IPurchaseDetailsRepo, PurchaseDetailsRepo>();
builder.Services.AddScoped<IPurchaseDetailsService, PurchaseDetailsService>();
 
builder.Services.AddScoped<IPurchaseRepository, PurchaseRepository>();
builder.Services.AddScoped<IPurchaseService, PurchaseService>();

builder.Services.AddScoped<IAddressBookMasterService, AddressBookMasterService>();
builder.Services.AddScoped<IAddressBookMaster, AddressBookMasterRepo>();
builder.Services.AddScoped<IGRRepo, GRRepo>();

builder.Services.AddScoped<IVehicleStatusService, VehicleStatusService>();
builder.Services.AddScoped<IVehicleStatusRepo, VehicleStatusRepo>();
builder.Services.AddScoped<IGRTypeService,GRTypeService>();
builder.Services.AddScoped<IGRTypeRepo,GRTypeRepo>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IPlaceRepo, PlaceRepo>();
builder.Services.AddScoped<IRoutesRepo, Routesrepo>();
builder.Services.AddScoped<IRoutesService, RoutesService>(); 
builder.Services.AddScoped<ITaxCategoryService, TaxCategoryService>();
builder.Services.AddScoped<ITaxCategoryRepo, TaxCategoryRepo>();
builder.Services.AddScoped<IVehicleMastersRepo, VehicleMasterRepo>();
builder.Services.AddScoped<IVehicleMasterService, VehicleMasterService>();
builder.Services.AddScoped<ICreditNoteRepo,CreditNoteRepo>();
builder.Services.AddScoped<ICreditNoteService,CreditNoteService>();

builder.Services.AddScoped<ITaxMasterService, TaxMasterService>();

builder.Services.AddScoped<IInvoiceService,InvoiceService>();
builder.Services.AddScoped<IInvoiceRepo, InvoiceRepo>();
builder.Services.AddScoped<IBillAdjustmentRepo, BillAdjustmentRepo>();
builder.Services.AddScoped<IBillAdjustmentService, BillAdjustmentService>();

builder.Services.AddScoped<IInvoiceRepo, InvoiceRepo>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IVoucherService, VoucherService>();

builder.Services.AddScoped<IUserTypeMasterRepo, UserTypeMasterRepo>();
builder.Services.AddScoped<IUserTypeMasterService, UserTypeMasterService>();

builder.Services.AddScoped<ITaxMasterService, TaxMasterService>();

builder.Services.AddScoped<IPurchaseOrderRepo, PurchaseOrderRepo>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();

builder.Services.AddScoped<IBankReconciliationRepository, BankReconciliationRepository>();
builder.Services.AddScoped<IBankReconciliationService, BankReconciliationService>();

builder.Services.AddScoped<IInvoiceReturnRepo,InvoiceReturnRepo>();
builder.Services.AddScoped<IInvoiceReturnService,InvoiceReturnService>();

builder.Services.AddScoped<IBillSubmissionRepositorys, BillSubmissionRepository>();
builder.Services.AddScoped<IBillSubmissionService, BillSubmissionService>();

builder.Services.AddScoped<IBillSubmissionReportRepository, BillSubmissionReportRepository>();
builder.Services.AddScoped<IBillSubmissionReportService, BillSubmissionReportService>();


builder.Services.AddScoped<IGRReportRepository, GRReportRepository>();
builder.Services.AddScoped<IGRReportService, GRReportService>();

builder.Services.AddScoped<IBalanceSheetRepository, BalanceSheetRepository>();
builder.Services.AddScoped<IBalanceSheetService, BalanceSheetService>();

builder.Services.AddScoped<ITrialBalanceRepository, TrialBalanceRepository>();
builder.Services.AddScoped<ITrialBalanceService, TrialBalanceService>();

builder.Services.AddScoped<IProfitAndLossRepository, ProfitAndLossRepository>();
builder.Services.AddScoped<IProfitAndLossService, ProfitAndLossService>();



builder.Services.AddScoped<IBankRecoRepository, BankRecoRepository>();
builder.Services.AddScoped<IBankRecoService, BankRecoService>();

builder.Services.AddScoped<IDayBookService, DayBookService>();
builder.Services.AddScoped<IDayBookRepository, DayBookRepository>();

builder.Services.AddScoped<IExpCreditNoteService, ExpCreditNotesService>();
builder.Services.AddScoped<IExpCreditNoteRepository, ExpCreditNoteRepository>();

builder.Services.AddScoped<IInvoiceRegisterRepository, InvoiceRegisterRepository>();
builder.Services.AddScoped<IInvoiceRegisterService, InvoiceRegisterService>();

builder.Services.AddScoped<ICashReceiptRepository, CashReceiptRegisterRepository>();
builder.Services.AddScoped<ICashReceiptService, CashReceiptService>();


builder.Services.AddScoped<IFinancialLedgerRepository, FinancialLedgerRepository>();
//builder.Services.AddScoped<IProfitLossService, ProfitLossService>();

builder.Services.AddScoped<IDebitNoteService, DebitNoteService>();
builder.Services.AddScoped<IDebitNoteRepository, DebitNoteRepository>();

builder.Services.AddScoped<ICashPaymentRepository, CashPaymentRepository>();
builder.Services.AddScoped<ICashPaymentService, CashPaymentService>();

builder.Services.AddScoped<IBankReceiptRepository, BankReceiptRepository>();
builder.Services.AddScoped<IBankReceiptService, BankReceiptService>();

builder.Services.AddScoped<IBankPaymentRepository, BankPaymentRepository>();
builder.Services.AddScoped<IBankPaymentService, BankPaymentService>();

builder.Services.AddScoped<IContractReportRepository, ContractReportRepository>();
builder.Services.AddScoped<IContractReportService, ContractReportService>();



//var context = new CustomAssemblyLoadContext();
//var path = Path.Combine(Directory.GetCurrentDirectory(), "wkhtmltox", "libwkhtmltox.dll");
//context.LoadUnmanagedLibrary(path);
//builder.Services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
var frontImagePath = Path.Combine(app.Environment.WebRootPath, "UserImageAadharFront");
var backImagePath = Path.Combine(app.Environment.WebRootPath, "UserImageAadharBack");

if (!Directory.Exists(frontImagePath))
{
    Directory.CreateDirectory(frontImagePath);
}

if (!Directory.Exists(backImagePath))
{
    Directory.CreateDirectory(backImagePath);
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()); 
builder.Services.AddSession(); // Add this in services configuration
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authenticate}/{action=Login}/{id?}");

app.Run();

