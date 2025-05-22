using GuardiansExpress.Models.Entity;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Interfaces
{
    public interface IGRNoService
    {
        List<GRNoEntity> List();
        GenericResponse AddGRNo(GRNoEntity gr);
        GenericResponse UpdateGRNo(GRNoEntity gr);
        GenericResponse DeleteGRno(int id);
    }
}
