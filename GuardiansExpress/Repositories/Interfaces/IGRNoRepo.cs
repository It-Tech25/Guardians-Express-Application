using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Interfaces
{
    public interface IGRNoRepo
    {
        List<GRNoEntity> List();
        GenericResponse AddGRNo(GRNoEntity gr);
        GenericResponse UpdateGRNo(GRNoEntity gr);
        GenericResponse DeleteGRno(int id);
    }
}
