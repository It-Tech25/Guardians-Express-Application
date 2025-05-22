using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

public interface IBranchMasterService
{
    IEnumerable<BranchMasterEntity> BranchMasterss();
    IEnumerable<BranchMasterDTO> BranchMaster(string searchTerm, int pageNumber, int pageSize);
    public BranchMasterEntity BranchMasterId(int id);
    public GenericResponse CreateBranchMaster(BranchMasterEntity res);
    public GenericResponse UpdateBranchMaster(BranchMasterEntity req);
    public GenericResponse DeleteBranchMaster(int id);


}
