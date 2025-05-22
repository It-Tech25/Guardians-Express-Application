using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class VehicleMasterRepo :IVehicleMastersRepo
    {

        private readonly MyDbContext _context;

        public VehicleMasterRepo(MyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<VehicleMasterDTO> GetVehicleMaster(string searchTerm, int pageNumber, int pageSize)
        {
            var query = from vehicle in _context.VehicleMasters
                        join branch in _context.branch on vehicle.BranchId equals branch.id
                        join v in _context.VehicleTypeEntite on vehicle.VehicleTypeId equals v.Id
                        join b in _context.BodyTypes on vehicle.BodyTypeId equals b.Id
                        where vehicle.IsDeleted == false
                        select new VehicleMasterDTO
                        {
                            VehicleId = vehicle.VehicleId,
                            VehicleNo = vehicle.VehicleNo,
                            VehicleTypeId=vehicle.VehicleTypeId,
                            BodyTypeId=vehicle.BodyTypeId,
                            Bodytype=b.Bodytype,
                            DisplayVehicleNo=vehicle.DisplayVehicleNo,
                            Weight=vehicle.Weight,
                            OwnedBy=vehicle.OwnedBy,
                            Transporter=vehicle.Transporter,
                            MaxWeightAllowed=vehicle.MaxWeightAllowed,
                            Status=vehicle.Status,
                            StartDate=vehicle.StartDate,
                            DocumentType=vehicle.DocumentType,
                            ExpiryDate=vehicle.ExpiryDate,
                            BranchId = vehicle.BranchId,
                            BranchName = branch.BranchName, 
                            Amount=vehicle.Amount,
                            Remarks=vehicle.Remarks,
                            Uploads=vehicle.Uploads,
                            Docs=vehicle.Docs,
                            VehicleType=v.VehicleType
                                                            
                        };

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.VehicleNo.Contains(searchTerm) ||
                                         v.OwnedBy.Contains(searchTerm) ||
                                         v.Transporter.Contains(searchTerm) 
                                         
                                         );
            }

            return query.Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }

        public VehicleMasterEntity GetVehicleMasterById(int id)
        {

            return _context.VehicleMasters.Find(id);
        }
        public GenericResponse CreateVehicleMaster(VehicleMasterEntity Vehicle)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                Vehicle.IsDeleted = false;
                Vehicle.IsActive = true;
                _context.VehicleMasters.Add(Vehicle);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "VehicleMaster created successfully";
                response.currentId = Vehicle.VehicleId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save VehicleMaster: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
        public GenericResponse UpdateVehicleMaster(VehicleMasterEntity Vehicle)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var existing = _context.VehicleMasters.Where(c => c.VehicleId == Vehicle.VehicleId).FirstOrDefault();
                if (existing != null)
                {
                    Vehicle.IsActive = true;
                    Vehicle.IsDeleted = false;

                    Vehicle.UpdatedOn = DateTime.Now;

                    _context.Entry(existing).CurrentValues.SetValues(Vehicle);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "VehicleMaster updated successfully";
                    response.currentId = Vehicle.VehicleId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "VehicleMaster not found";
                    response.currentId = 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update VehicleMaster: " + ex.Message;
                response.currentId = 0;
                return response;
            }
        }
        public GenericResponse DeleteVehicleMaster(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {

                var existing = _context.VehicleMasters.Where(v => v.VehicleId == id).FirstOrDefault();
                if (existing != null)
                {

                    existing.IsDeleted = true;
                    _context.Update(existing);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = " VehicleMaster deleted successfully";
                    response.currentId = id;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "Delete Failed";
                    response.currentId = 0;
                }
            }
            catch (Exception ex)
            {
                response.message = "Failed to delete VehicleMaster: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

    }
}
