using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

public class BranchMasterServices : IBranchMasterService
{
    private readonly IBranchMasterRepo repo;
    private bool _disposed;

    public BranchMasterServices(IBranchMasterRepo _repo)
    {
        repo = _repo;

    }
    public IEnumerable<BranchMasterEntity> BranchMasterss()
    {
        return repo.BranchMasterss();
    }

    public IEnumerable<BranchMasterDTO> BranchMaster(string searchTerm, int pageNumber, int pageSize)

    {
        return repo.BranchMaster(searchTerm, pageNumber, pageSize);
    }
    public BranchMasterEntity BranchMasterId(int id)
    {
        return repo.BranchMasterId(id);
    }
    public GenericResponse CreateBranchMaster(BranchMasterEntity res)
    {
        return repo.CreateBranchMaster(res);
    }
    public GenericResponse UpdateBranchMaster(BranchMasterEntity req)
    {
        return repo.UpdateBranchName(req);
    }
    public GenericResponse DeleteBranchMaster(int id)
    {
        return repo.DeleteBranchName(id);
    }
}
