using Microsoft.AspNetCore.Mvc;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Utilities;

public class PurchaseController : Controller
{
    private readonly MyDbContext _context;

    public PurchaseController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult PurchaseEntryIndex()
    {
        var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
        if (loggedInUser == null)
        {
            return RedirectToAction("Login", "Authenticate");
        }

        // Pass user details to the view
        ViewBag.UserName = loggedInUser.userName;
        ViewBag.UserEmail = loggedInUser.Emailid;
        ViewBag.UserRole = loggedInUser.Role;
        //ViewBag.UserProfileImage = loggedInUser.ProfileImageUrl; // URL to the user's profile image
        var purchases = _context.PurchaseEntryEntity.ToList();
        return View(purchases);
    }

    [HttpPost]
    public IActionResult SavePurchase(PurchaseDto model)
    {
        var purchase = new PurchaseDetails
        {
            Branch = model.Branch,
           // In = model.InvoiceNo,
            InvoiceDate = model.InvoiceDate,
            ClientName = model.ClientName,
            Address = model.Address,
        };

        _context.PurchaseEntryEntity.Add(purchase);
        _context.SaveChanges();

        int id = purchase.Id;

        foreach (var item in model.Items)
        {
            purchaseitemdetailsEntity itemDetail = new purchaseitemdetailsEntity
            {
                PurchaseId = id,
                ItemDescription = item.ItemName,
                PurchaseAcc = item.PurchaseAcc,
                HSNSAC = item.HSNSAC,
                MRP = item.MRP,
                rate = item.Rate,
                discount = item.Discount,
                QTY = item.Qty,
                Freeqty = item.FreeQty,
                unit = item.Unit
            };

            _context.purchaseitemdetail.Add(itemDetail);
        }

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Purchase and items saved successfully!";
        return RedirectToAction("PurchaseEntryIndex");
    }

}
