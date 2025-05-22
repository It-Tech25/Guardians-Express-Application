using GuardiansExpress.Models.Entity;

using GuardiansExpress.Repository;

using GuardiansExpress.Repositories.Interfaces;

namespace GuardiansExpress.Services.Service
{
    public class BillSubmissionService : IBillSubmissionService
    {
        private readonly IBillSubmissionRepositorys _repository;

        public BillSubmissionService(IBillSubmissionRepositorys repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BillSubmissionEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<BillSubmissionEntity?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(CreateBillSubmissionDTO createDto, int userId)
        {
            var entity = new BillSubmissionEntity
            {
                ClientName = createDto.ClientName,
                BillNo = createDto.BillNo,
                BillSubDate = createDto.BillSubDate,
                BillSubmissionBy = createDto.BillSubmissionBy,
                ReceivedBy = createDto.ReceivedBy,
                HandedOverBy = createDto.HandedOverBy,
                DocketNo = createDto.DocketNo,
                CourierName = createDto.CourierName,
                IsActive = true,
                IsDelete = false,
                CreatedOn = DateTime.Now,
                CreatedBy = userId
            };

            return await _repository.CreateAsync(entity);
        }

        public async Task<BillSubmissionEntity?> UpdateAsync(UpdateBillSubmissionDTO updateDto)
        {
            var entity = new BillSubmissionEntity
            {
                BillSubmissionId = updateDto.BillSubmissionId,
                ClientName = updateDto.ClientName,
                BillNo = updateDto.BillNo,
                BillSubDate = updateDto.BillSubDate,
                BillSubmissionBy = updateDto.BillSubmissionBy,
                ReceivedBy = updateDto.ReceivedBy,
                HandedOverBy = updateDto.HandedOverBy,
                DocketNo = updateDto.DocketNo,
                CourierName = updateDto.CourierName,
                IsActive = updateDto.IsActive,
                UpdatedOn = DateTime.Now
            };

            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
