using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

public interface IBranchMasterRepo
{
    //IEnumerable<BranchMasterDTO> BranchMaster();
    IEnumerable<BranchMasterEntity> BranchMasterss();
    IEnumerable<BranchMasterDTO> BranchMaster(string searchTerm, int pageNumber, int pageSize);
    BranchMasterEntity BranchMasterId(int id);
    GenericResponse CreateBranchMaster(BranchMasterEntity res);
    GenericResponse UpdateBranchName(BranchMasterEntity req);
    GenericResponse DeleteBranchName(int id);
}