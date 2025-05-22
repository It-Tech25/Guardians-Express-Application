using GuardiansExpress.Models.Entity;

public interface IPurchaseService
{
    Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync();
    Task<PurchaseDto> GetPurchaseByIdAsync(int id);
    //Task CreatePurchaseAsync(PurchaseDto purchase);
    //Task UpdatePurchaseAsync(PurchaseDto purchase);
    Task DeletePurchaseAsync(int id);

    //Task<IEnumerable<ItemDetail>> GetItemsByPurchaseIdAsync(int purchaseId);
    //Task AddItemToPurchaseAsync(int purchaseId, ItemDetail item);
    //Task UpdateItemInPurchaseAsync(int purchaseId, ItemDetail item);
    //Task RemoveItemFromPurchaseAsync(int purchaseId, string itemName);

   // decimal CalculateGrossAmount(PurchaseDto purchase);
    decimal CalculateNetAmount(PurchaseDto purchase);
}
