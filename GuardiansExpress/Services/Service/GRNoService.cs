using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class GRNoService: IGRNoService
    {
        private readonly IGRNoRepo _grrepo;
        public GRNoService(IGRNoRepo grrepo)
        {
            _grrepo = grrepo;
        }
        public List<GRNoEntity> List()
        {
            var res = _grrepo.List();
            return res;

        }
        public GenericResponse AddGRNo(GRNoEntity gr)
        {
            var res = _grrepo.AddGRNo(gr);
            return res;
        }
        public GenericResponse UpdateGRNo(GRNoEntity gr)
        {
            var res = _grrepo.UpdateGRNo(gr);
            return res;
        }
        public GenericResponse DeleteGRno(int id)
        {
            var res = _grrepo.DeleteGRno(id);
            return res;
        }




    }
}
