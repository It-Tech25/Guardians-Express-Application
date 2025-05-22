using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
 
    public class GRNoRepo: IGRNoRepo
    {
        private readonly MyDbContext _context;
        public GRNoRepo(MyDbContext context)
        {
            _context = context;
        }
        public List<GRNoEntity> List()
        {
            var result = _context.grnoEntity.Where(a => a.IsDeleted == false).ToList();
            return result;
        }
        public GenericResponse AddGRNo(GRNoEntity gr)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                gr.createdOn = DateTime.Now;
                gr.IsDeleted = false;
               
                _context.grnoEntity.Add(gr);
                _context.SaveChanges();
                response.statuCode = 1;
                response.Message = "Added Successfully";
            }
            catch(Exception ex)
            {
                response.statuCode = 0;
                response.Message = "Failed To Add GRNO";
            }

            return response;
        }
        public GenericResponse UpdateGRNo(GRNoEntity gr)
        {
            var res = _context.grnoEntity.Where(a => a.Grnoid == gr.Grnoid && a.IsDeleted == false).FirstOrDefault();
            GenericResponse response = new GenericResponse();
            GRNoEntity entity = new GRNoEntity();
            try
            {
                entity.Grnoid = gr.Grnoid;
                entity.GR = gr.GR;
                entity.createdOn = res.createdOn;
                entity.IsDeleted = false;
                entity.IsActive = gr.IsActive;
                entity.createdOn = res.createdOn;
                entity.UpdatedOn = DateTime.Now;
                _context.grnoEntity.Update(entity);
                _context.SaveChanges();
                response.statuCode = 1;
                response.Message = "Updated Successfully";
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.Message = "Failed To Update GRNO";
            }

            return response;
        }
        public GenericResponse DeleteGRno(int id)
        {
            var res = _context.grnoEntity.Where(a => a.Grnoid == id && a.IsDeleted == false).FirstOrDefault();
            GenericResponse response = new GenericResponse();
            GRNoEntity entity = new GRNoEntity();
            try
            {
                res.IsDeleted = true;
                res.IsActive = false;
                res.UpdatedOn = DateTime.Now;
                _context.grnoEntity.Update(res);
                _context.SaveChanges();
                response.statuCode = 1;
                response.Message = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                response.statuCode = 0;
                response.Message = "Failed To Delete GRNO";
            }

            return response;
        }
    }
}
