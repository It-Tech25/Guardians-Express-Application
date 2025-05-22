public interface IPurchaseRepository
{
    // Purchase management
    Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync();
    Task<PurchaseDto> GetPurchaseByIdAsync(int id);
    Task AddPurchaseAsync(PurchaseDto purchase);
    Task UpdatePurchaseAsync(PurchaseDto purchase);
    Task DeletePurchaseAsync(int id);

    // Item management within a purchase
    //Task<IEnumerable<ItemDetail> GetItemsByPurchaseIdAsync(int purchaseId);
    //Task AddItemToPurchaseAsync(int purchaseId, ItemDetailsDto item);
    //Task UpdateItemInPurchaseAsync(int purchaseId, ItemDetailsDto item);
    Task RemoveItemFromPurchaseAsync(int purchaseId, string itemName);
}
