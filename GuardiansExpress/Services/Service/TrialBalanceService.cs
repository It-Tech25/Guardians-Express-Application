using GuardiansExpress.Models.DTOs;

namespace GuardiansExpress.Services.Service
{
    public class TrialBalanceService : ITrialBalanceService
    {
        private readonly ITrialBalanceRepository _repository;

        public TrialBalanceService(ITrialBalanceRepository repository)
        {
            _repository = repository;
        }

        public List<TrialBalanceDTO> GetTrialBalance()
        {
            return _repository.GetTrialBalance();
        }
    }
}