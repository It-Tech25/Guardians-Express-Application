

using GuardiansExpress.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;

namespace GuardiansExpress.Models.Entity
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<UserMaster>().ToTable("UsersMaster"); 
            modelBuilder.Entity<CountryEntity>().ToTable("Country");
            modelBuilder.Entity<StateEntity>().ToTable("State");
            modelBuilder.Entity<UserTypeMaster>().ToTable("UserTypeMaster");
            modelBuilder.Entity<VehicleGroupEntity>().ToTable("VehicleGroup");
            modelBuilder.Entity<VehicleTypeEntity>().ToTable("VehicleTypeMaster");
            modelBuilder.Entity<GroupHeadEntity>().ToTable("GroupHead");
            modelBuilder.Entity<BillEntity>().ToTable("Bills");
            modelBuilder.Entity<SubGroupEntity>().ToTable("SubGroup");
            modelBuilder.Entity<CompanySetupMasterEntity>().ToTable("CompanyBillDetails");
          //  modelBuilder.Entity<CompanySettingsEntity>().ToTable("CompanyConfiguration");
            modelBuilder.Entity<BodyTypeEntity>().ToTable("BodyType");
           
            //modelBuilder.Entity<CashOutPositions_V2>().HasNoKey();
            modelBuilder.Entity<LedgerMasterEntity>().ToTable("LedgerMaster");
            modelBuilder.Entity<InvoiceTypeMasterEntity>().ToTable("InvoiceTypeMaster");
            modelBuilder.Entity<FinancialyearEntity>().ToTable("FinancialYear");
            modelBuilder.Entity<FinancialInvoiceEntity>().ToTable("Invoice");
            modelBuilder.Entity<FinancialBillEntity>().ToTable("Bill");
            

            modelBuilder.Entity<AddressBookMasterEntity>().ToTable("AddressBookMaster ");
 
            modelBuilder.Entity<BranchMasterEntity>().ToTable("BranchMaster");
            modelBuilder.Entity<BehaviourEntity>().ToTable("Behaviour");
            modelBuilder.Entity<VehicleStatusEntity>().ToTable("VehicleStatus");
            modelBuilder.Entity<GRType>().ToTable("GRTypes");
            modelBuilder.Entity<Place>().ToTable("Place");

            modelBuilder.Entity<PurchaseDetails>().ToTable("Purchase");
            modelBuilder.Entity<AddressDetailsEntity>().ToTable("AddressTable");
            modelBuilder.Entity<TaxCategoryEntity>().ToTable("TaxCategory");
            modelBuilder.Entity<VehicleMasterEntity>().ToTable("VehicleMaster");
            modelBuilder.Entity<TaxMasterEntity>().ToTable("TaxMaster");
            modelBuilder.Entity<TaxDetailsEntity>().ToTable("TaxDetails");
            modelBuilder.Entity<Routes>().ToTable("Routes");
            modelBuilder.Entity<CompanyConfigurationEntity>().ToTable("CompanyConfiguration");
            modelBuilder.Entity<GREntity>().ToTable("GR");
            modelBuilder.Entity<EXP_CREDITNoteEntity>().ToTable("EXP_CREDITNote");
            modelBuilder.Entity<EXP_CreditLedgerEntity>().ToTable("EXP_CreditLedger");
            modelBuilder.Entity<CreditNoteEntity>().ToTable("CreditNotes");
            modelBuilder.Entity<InvoiceEntity>().ToTable("SaleInvoice");
            modelBuilder.Entity<VehicleDocumentsEntity>().ToTable("VehicleDocuments");
            modelBuilder.Entity<TaxDetailsTableEntity>().ToTable("TaxDetailsTable");
            modelBuilder.Entity<PurchaseDetailsEntity>().ToTable("PurchaseDetails");
            modelBuilder.Entity<salesdetailsEntity>().ToTable("salesdetails");
            modelBuilder.Entity<BillAdjustmentEntity>().ToTable("BillAdjustment");
 
            modelBuilder.Entity<purchaseitemdetailsEntity>().ToTable("purchaseitemdetails");
            modelBuilder.Entity<Voucher>().ToTable("Voucher");
            modelBuilder.Entity<VoucherDetail>().ToTable("VoucherDetails");

            modelBuilder.Entity<PurchaseOrderEntity>().ToTable("PurchaseOrder");
            modelBuilder.Entity<PurchaseOrderItemEntity>().ToTable("PurchaseOrderitem");
            modelBuilder.Entity<ContractEntity>().ToTable("Contracts");
            modelBuilder.Entity<ContractItemEntity>().ToTable("ContractItems");
            modelBuilder.Entity<BankReconciliationEntity>().ToTable("BankReconciliation");
            modelBuilder.Entity<BillAdjustmentDetailsEntity>().ToTable("BillAdjustmentDetails");
            modelBuilder.Entity<InvoiceReturnEntity>().ToTable("InvoiceReturn");
            modelBuilder.Entity<InvoiceReturnDetailsEntity>().ToTable("InvoiceReturnDetails");

            modelBuilder.Entity<FinancialLedgerEntity>().ToTable("FinancialLedger");
            modelBuilder.Entity<InvoiceGrEntity>().ToTable("InvoiceGr");
            modelBuilder.Entity<BillSubmissionEntity>().ToTable("BilSubmission");
            modelBuilder.Entity<GRNoEntity>().ToTable("GRNoEntity");
           



        }
        public DbSet<UserMaster> users { get; set; }
        public DbSet<UserTypeMaster> userTypes { get; set; }
        public DbSet<CountryEntity> countryEntities { get; set; }
        public DbSet<StateEntity> stateEntities { get; set; }
    
        public DbSet<VehicleTypeEntity> VehicleTypeEntite { get; set; }
        public DbSet<LedgerMasterEntity> ledgerEntity { get; set; }
        public DbSet<GroupHeadEntity> GroupHeads { get; set; }
        public DbSet<SubGroupEntity> SubGroups { get; set; }
        public DbSet<BillEntity> Bill { get; set; }

        public DbSet<VehicleGroupEntity> vehicles { get; set; }
        public DbSet<CompanySetupMasterEntity> CompanySetups { get; set; }
        public DbSet<BodyTypeEntity> BodyTypes { get; set; }    
        public DbSet<FinancialyearEntity> finance { get; set; }
        public DbSet<FinancialInvoiceEntity> finvoice { get; set; }
        public DbSet<FinancialBillEntity> fBill { get; set; }
        public DbSet<InvoiceTypeMasterEntity> invoice { get; set; }

       public DbSet<AddressBookMasterEntity> AddressBookMaster { get; set; }
 
        public DbSet<BranchMasterEntity> branch { get; set; }
        public DbSet<BehaviourEntity> behaviour { get; set; }

        public DbSet<GRType> gRTypes { get; set; }
        public DbSet<Place> placeEntity { get; set; }
        public DbSet<PurchaseDetails> PurchaseEntryEntity { get; set; }

        public DbSet<purchaseitemdetailsEntity> purchaseitemdetail { get; set; }
        public DbSet<TaxCategoryEntity> taxCategory { get; set; }

        public DbSet<TaxMasterEntity> taxMaster { get; set; }
        public DbSet<TaxDetailsEntity> taxDetails { get; set; }
        public DbSet<VehicleStatusEntity> vehicleStatuses { get; set; }
        public DbSet<Routes> routes { get; set; }
        public DbSet<VehicleMasterEntity> VehicleMasters { get; set; }
        public DbSet<GREntity> grDetails { get; set; }
        public DbSet<CompanyConfigurationEntity> CompanyConfigurations { get; set; }
        public DbSet<EXP_CREDITNoteEntity>creditnote{ get; set; }
        public DbSet<EXP_CreditLedgerEntity>creditledger { get; set; }
        public DbSet<CreditNoteEntity> creditNotes { get; set; }

        public DbSet<InvoiceEntity> Invoices { get; set; }
        public DbSet<VehicleDocumentsEntity> VehicleDocumentsEntitys { get; set; }
        public DbSet<TaxDetailsTableEntity> TaxDetailsTableEntitys { get; set; }
        public DbSet<AddressDetailsEntity> address { get; set; }
        public DbSet<FinancialLedgerEntity> financialLedgers { get; set; }
        public DbSet<PurchaseDetailsEntity> PurchaseDetails { get; set; }
        public DbSet<salesdetailsEntity> salesdetailsEntitys { get; set; }
        public DbSet<BillAdjustmentEntity> BillAdjustments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<VoucherDetail> voucherDetails { get; set; }


        public DbSet<PurchaseOrderEntity> purchaseOrders { get; set; }
        public DbSet<PurchaseOrderItemEntity> purchaseOrderItems { get; set; }
        public DbSet<ContractEntity> contractEntities { get; set; }

        public DbSet<ContractItemEntity> contractItemEntities { get; set; }
        public DbSet<BankReconciliationEntity> Banks { get; set; }
        public DbSet<BillAdjustmentDetailsEntity> billAdjustmentDetails { get; set; }
        public DbSet<InvoiceReturnEntity> invoiceReturns { get; set; }
        public DbSet<InvoiceReturnDetailsEntity> invoiceReturnDetails{ get; set; }

        public DbSet<InvoiceGrEntity> invoicegr { get; set; }
        public DbSet<BillSubmissionEntity> BilSubmissions { get; set; }
        public DbSet<GRNoEntity> grnoEntity { get; set; }


    }
}
