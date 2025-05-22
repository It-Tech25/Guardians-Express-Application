using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace GuardiansExpress.Repositories.Repos
{
    public class LedgerMasterRepo : ILedgerMasterRepo
    {

        private readonly MyDbContext _context;
        public LedgerMasterRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<LedgerMasterDTO> ledgerEntity(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from ledger in _context.ledgerEntity
                        join subgroup in _context.SubGroups on ledger.subgroupId equals subgroup.subgroupId into subgroupjoin
                        from subgroup in subgroupjoin.DefaultIfEmpty()
                        where (ledger == null || (ledger.IsDeleted == false && ledger.IsActive == true))
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
                            OpeningBalance=_context.financialLedgers.Where(a=>a.LedgerId==ledger.LedgerId).Select(a=>a.OpeningBalance).FirstOrDefault(),
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

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.AccGroup.Contains(searchTerm) ||
                                          (t.subgroupName != null && t.subgroupName.Contains(searchTerm)) ||
                                          t.Status.Contains(searchTerm));
            }

            var pagedQuery = query
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

            return pagedQuery.ToList();
        }
        public LedgerMasterDTO LedgerMasterById(int id)
        {
            var query = (from ledger in _context.ledgerEntity
                         join subgroup in _context.SubGroups on ledger.subgroupId equals subgroup.subgroupId into subgroupjoin
                         from subgroup in subgroupjoin.DefaultIfEmpty()
                         where (ledger == null || (ledger.IsDeleted == false && ledger.IsActive == true))
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
                             CINNo=ledger.CINNo,
                             VendorCode = ledger.VendorCode,
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
                             OpeningBalance = _context.financialLedgers.Where(a => a.LedgerId == ledger.LedgerId).Select(a => a.OpeningBalance).FirstOrDefault(),
                             UserName = ledger.UserName,
                             Agent = ledger.Agent,
                             WalkinLedger = ledger.WalkinLedger,
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
                         }).FirstOrDefault();

            return query;
        }
        public GenericResponse CreateLedgerMaster(LedgerMasterEntity res)
        {
            GenericResponse response = new GenericResponse();

            int count = _context.ledgerEntity.Where(a => a.AccHead == res.AccHead && a.IsDeleted == false).Count();
            if (count < 1)
            {
                try
                {
                    var subgroupId = _context.SubGroups
                        .Where(a => a.SubGroupName == res.AccGroup && a.IsDeleted == false)
                        .Select(a => a.subgroupId)
                        .FirstOrDefault();

                    if (subgroupId == 0)  // Ensure subgroup exists
                    {
                        response.message = "Error: Selected Account Group (SubGroup) does not exist.";
                        return response;
                    }
                    bool tl, t, ve, ba, wl;
                    if (res.TaxLedger == null)
                    {
                        tl = false;
                    }
                    else { tl = true; }
                    if (res.Taxable == null)
                    {
                        t = false;
                    }
                    else { t = true; }
                    if (res.BankAccount == null)
                    {
                        ba = false;
                    }
                    else { ba = true; }
                    if (res.VehicleExpense == null)
                    {
                        ve = false;
                    }
                    else { ve = true; }
                    if (res.WalkinLedger == null)
                    {
                        wl = false;
                    }
                    else { wl = true; }
                    LedgerMasterEntity T = new LedgerMasterEntity
                    {
                        subgroupId = subgroupId,
                        AccGroup = res.AccGroup,
                        AccHead = res.AccHead,
                        Status = res.Status,
                        Email = res.Email,
                        BankAccount = ba,
                        Taxable = t,
                        TaxLedger = tl,
                        VehicleExpense = ve,
                      
                        TDSPercent = res.TDSPercent,
                        BalanceIn = res.BalanceIn,
                        ContactPerson = res.ContactPerson,
                        State = res.State,
                        Country = res.Country,
                        CCEmailId = res.CCEmailId,
                        RefID = res.RefID,
                        IFSCCode = res.IFSCCode,
                        AAdharNumber = res.AAdharNumber,
                        AltMobile = res.AltMobile,
                        Address1 = res.Address1,
                        Address = res.Address,
                        Address2 = res.Address2,
                        TelNo = res.TelNo,
                        PANNo = res.PANNo,
                        PartyType = res.PartyType,
                        RegistrationType = res.RegistrationType,
                        VendorCode = res.VendorCode,
                        Pincode = res.Pincode,
                        GSTIN = res.GSTIN,
                        CINNo= res.CINNo,
                       
                        AccHolderName = res.AccHolderName,
                        BankAccNo = res.BankAccNo,
                        BankBranch = res.BankBranch,
                        BankName = res.BankName,
                        DueDays = res.DueDays,
                        Agent = res.Agent,
                        UserName = res.UserName,
                        Password = res.Password,
                        WalkinLedger = res.WalkinLedger,
                        NameAddressMobile = res.NameAddressMobile,
                        CityStatePincode = res.CityStatePincode,
                        OtherEmailId = res.OtherEmailId,
                        BalanceOpening = res.BalanceOpening,
                        OpeningBalance = res.OpeningBalance,
                    
                        City = res.City,

                        Mobile = res.Mobile,
                        IsDeleted = false,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        PaymentTerm = res.PaymentTerm,
                        BranchName = res.BranchName,

                        CreatedBy = 1
                    };

                    _context.ledgerEntity.Add(T);
                    _context.SaveChanges();
   
                    // ✅ Make sure to return the LedgerId after saving
                    response.statuCode = 1;
                    response.message = "Ledger created successfully";
                    response.currentId = T.LedgerId;  // ✅ Return the correct LedgerId

                }
                catch (Exception ex)
                {
                    response.message = "Failed to save LedgerMaster: " + ex.Message;
                }
            }
            else
            {
                response.message = "LedgerMaster already exists.";
            }
            return response;
        }


        public GenericResponse UpdateLedgerMaster(LedgerMasterEntity req, List<AddressDetailsEntity> updatedAddresses)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var u = _context.ledgerEntity.FirstOrDefault(a => a.LedgerId == req.LedgerId);
                var subgroupId = _context.SubGroups
                       .Where(a => a.SubGroupName == req.AccGroup && a.IsDeleted == false)
                       .Select(a => a.subgroupId)
                       .FirstOrDefault();


                if (u == null)
                {
                    response.message = "Record not found";
                    response.currentId = 0;
                    return response;
                }

                bool tl, t, ve, ba, wl;
                if (u.TaxLedger == null || req.TaxLedger == null)
                {
                    tl = false;
                }

                else { tl = true; }
                if (u.Taxable == null || req.Taxable == null)
                {
                    t = false;
                }
                else { t = true; }
                if (u.BankAccount == null || req.BankAccount == null)
                {
                    ba = false;
                }
                else { ba = true; }
                if (u.VehicleExpense == null || req.VehicleExpense == null)
                {
                    ve = false;
                }
                else { ve = true; }
                if (u.WalkinLedger == null || req.WalkinLedger == null)
                {
                    wl = false;
                }
                else { wl = true; }

                req.TaxLedger = tl;
                req.BankAccount = ba;
                req.Taxable = t;
                req.WalkinLedger = wl;
                req.VehicleExpense = ve;
                req.IsActive = true;
                req.IsDeleted = false;
                req.CreatedOn = u.CreatedOn;
                req.UpdatedOn = DateTime.Now;
                req.UpdatedBy = req.LedgerId;
                req.subgroupId = subgroupId;

                _context.Entry(u).CurrentValues.SetValues(req);
                _context.SaveChanges();
                var existingAddresses = _context.address.Where(a => a.LedgerId == req.LedgerId).ToList();
                _context.address.RemoveRange(existingAddresses);

                // Add new/updated addresses
                _context.address.AddRange(updatedAddresses);

                _context.SaveChanges();
       
                response.statuCode = 1;
                response.message = "Success";
                response.currentId = req.LedgerId;
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update: " + ex.Message;
                response.currentId = 0;
                return response;
            }
        }

        public GenericResponse DeleteLedgerMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var entity = _context.ledgerEntity.FirstOrDefault(x => x.LedgerId == id);

                entity.IsDeleted = true;
                entity.IsActive = false;
                _context.Update(entity);
                _context.SaveChanges();

                response.statuCode = 1;
                response.message = "Delete Successful.";

            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.message = "Delete failed.";
                //response.ErrorMessage = ex.Message;
            }
            return response;
        }
        public IEnumerable<FinancialLedgerDTO> FinancialLedger(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from finance in _context.financialLedgers
                        join ledger in _context.ledgerEntity on finance.LedgerId equals ledger.LedgerId into financejoin
                        from ledger in financejoin.DefaultIfEmpty()
                        where (finance == null || (finance.IsDeleted == false && finance.IsActive == true))
                        select new FinancialLedgerDTO
                        {
                            Id = finance.Id,
                            AccHead = ledger != null ? ledger.AccHead : null,
                            AccountHead=finance.AccountHead,
                            OpeningBalance=finance.OpeningBalance, 
                            BalanceIn = finance.BalanceIn, 
                            IsActive = finance.IsActive,
                            IsDeleted = finance.IsDeleted,
                           
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(t => t.AccountHead.Contains(searchTerm) ||
                                          (t.AccHead != null && t.AccHead.Contains(searchTerm)));
            }

            var pagedQuery = query
                             .Skip((pageNumber - 1) * pageSize)
                             .Take(pageSize);

            return pagedQuery.ToList();
        }

    }
}


