using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;

public class PurchaseRepository : IPurchaseRepository
{
    private readonly List<PurchaseDto> _purchases = new();

    public async Task<IEnumerable<PurchaseDto>> GetAllPurchasesAsync()
    {
        return await Task.FromResult(_purchases);
    }

    public async Task<PurchaseDto> GetPurchaseByIdAsync(int id)
    {
        return await Task.FromResult(_purchases.FirstOrDefault(p => p.Id == id));
    }

    public async Task AddPurchaseAsync(PurchaseDto purchase)
    {
        _purchases.Add(purchase);
        await Task.CompletedTask;
    }

    public async Task UpdatePurchaseAsync(PurchaseDto purchase)
    {
        var existingPurchase = _purchases.FirstOrDefault(p => p.Id == purchase.Id);
        if (existingPurchase != null)
        {
            _purchases.Remove(existingPurchase);
            _purchases.Add(purchase);
        }
        await Task.CompletedTask;
    }

    public async Task DeletePurchaseAsync(int id)
    {
        var purchase = _purchases.FirstOrDefault(p => p.Id == id);
        if (purchase != null)
        {
            _purchases.Remove(purchase);
        }
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<ItemDetail>> GetItemsByPurchaseIdAsync(int purchaseId)
    {
        var purchase = _purchases.FirstOrDefault(p => p.Id == purchaseId);
        return await Task.FromResult(purchase?.Items ?? new List<ItemDetail>());
    }

    public async Task AddItemToPurchaseAsync(int purchaseId, ItemDetail item)
    {
        var purchase = _purchases.FirstOrDefault(p => p.Id == purchaseId);
        purchase?.Items.Add(item);
        await Task.CompletedTask;
    }

    public async Task UpdateItemInPurchaseAsync(int purchaseId, ItemDetail item)
    {
        var purchase = _purchases.FirstOrDefault(p => p.Id == purchaseId);
        if (purchase != null)
        {
            var existingItem = purchase.Items.FirstOrDefault(i => i.ItemName == item.ItemName);
            if (existingItem != null)
            {
                purchase.Items.Remove(existingItem);
                purchase.Items.Add(item);
            }
        }
        await Task.CompletedTask;
    }

    public async Task RemoveItemFromPurchaseAsync(int purchaseId, string itemName)
    {
        var purchase = _purchases.FirstOrDefault(p => p.Id == purchaseId);
        var item = purchase?.Items.FirstOrDefault(i => i.ItemName == itemName);
        if (item != null)
        {
            purchase.Items.Remove(item);
        }
        await Task.CompletedTask;
    }
}
