using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;

public class PurchaseService : IPurchaseService
{
    private readonly IPurchaseRepository _purchaseRepository;

    public PurchaseService(IPurchaseRepository purchaseRepository)
    {
        _purchaseRepository = purchaseRepository;
    }

    public async Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync()
    {
        return await _purchaseRepository.GetAllPurchasesAsync();
    }

    public async Task<PurchaseDto> GetPurchaseByIdAsync(int id)
    {
        return await _purchaseRepository.GetPurchaseByIdAsync(id);
    }

    //public async Task CreatePurchaseAsync(PurchaseDto purchase)
    //{
    //    purchase.GrossAmount = CalculateGrossAmount(purchase);
    //    purchase.NetAmount = CalculateNetAmount(purchase);
    //    await _purchaseRepository.AddPurchaseAsync(purchase);
    //}

    //public async Task UpdatePurchaseAsync(PurchaseDto purchase)
    //{
    //    purchase.GrossAmount = CalculateGrossAmount(purchase);
    //    purchase.NetAmount = CalculateNetAmount(purchase);
    //    await _purchaseRepository.UpdatePurchaseAsync(purchase);
    //}

    public async Task DeletePurchaseAsync(int id)
    {
        await _purchaseRepository.DeletePurchaseAsync(id);
    }

    //public async Task<IEnumerable<ItemDetail>> GetItemsByPurchaseIdAsync(int purchaseId)
    //{
    //    return await _purchaseRepository.GetItemsByPurchaseIdAsync(purchaseId);
    //}

    //public async Task AddItemToPurchaseAsync(int purchaseId, ItemDetail item)
    //{
    //    await _purchaseRepository.AddItemToPurchaseAsync(purchaseId, item);
    //}

    //public async Task UpdateItemInPurchaseAsync(int purchaseId, ItemDetail item)
    //{
    //    await _purchaseRepository.UpdateItemInPurchaseAsync(purchaseId, item);
    //}

    public async Task RemoveItemFromPurchaseAsync(int purchaseId, string itemName)
    {
        await _purchaseRepository.RemoveItemFromPurchaseAsync(purchaseId, itemName);
    }

    //public decimal CalculateGrossAmount(PurchaseDto purchase)
    //{
    //    return purchase.Items.Sum(item => item.);
    //}

    public decimal CalculateNetAmount(PurchaseDto purchase)
    {
        return (purchase.GrossAmount ?? 0) - (purchase.DiscountAmount ?? 0) + (purchase.TaxAmountTotal ?? 0) + (purchase.RoundOff ?? 0);
    }
}
