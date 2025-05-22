using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Services.Service
{
    public class GRService: IGRService
    {
        private readonly IGRRepo _grRepo;

        public GRService(IGRRepo grRepo)
        {
            _grRepo = grRepo;
        }
        public IEnumerable<GRDTOs> Getgrdetails()
        {
            return _grRepo.Getgrdetails();
        }
        public GenericResponse AddAsync(GRDTOs grDto, string serializedinvoiceData)
        {
            return _grRepo.AddAsync(grDto,serializedinvoiceData);
        }
        public GenericResponse UpdateAsync(GRDTOs grDto, string serializedinvoiceData)
        {
            return _grRepo.UpdateAsync(grDto,serializedinvoiceData);
        }
        public GenericResponse DeleteAsync(int id)
        {
            return _grRepo.DeleteAsync(id);
        }
    }
}
