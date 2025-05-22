using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;

namespace GuardiansExpress.Repositories.Repos
{
    public class TaxCategoryRepo: ITaxCategoryRepo
    {
        private readonly MyDbContext _context;

        public TaxCategoryRepo(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaxCategoryEntity> GetTaxCategory(string searchTerm, int pageNumber, int pageSize)
        {
            
            var tax = _context.taxCategory.Where(c => c.IsDeleted == false).ToList();
            return tax;
        }

        public TaxCategoryEntity GetTaxCategoryById(int id)
        {
           
            return _context.taxCategory.Find(id);
        }

        public GenericResponse CreateTaxCategory(TaxCategoryEntity tax,
     List<int> TaxMasterID,
     List<decimal> Value,
     List<string> TaxFor)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                tax.IsDeleted = false;
                tax.IsActive = true;
                _context.taxCategory.Add(tax); 
                _context.SaveChanges();  
                response.statuCode = 1;  
                response.message = "Tax Category created successfully"; 
                response.currentId = tax.ID;
                int taxCategoryId = response.currentId; 

                
                List<TaxDetailsTableEntity> Entries = new List<TaxDetailsTableEntity>();

                for (int i = 0; i < Value.Count; i++)
                {
                    var Entry = new TaxDetailsTableEntity
                    {
                        id = tax.ID,
                        Value = Value[i],
                        TaxFor = TaxFor[i],
                        TaxMasterID = TaxMasterID[i],
                        IsActive = true, 
                        IsDeleted = false,
                        CreatedOn = DateTime.Now, 
                        CreatedBy = "Admin" 
                    };

                    Entries.Add(Entry);
                }

                
                _context.TaxDetailsTableEntitys.AddRange(Entries);
                _context.SaveChanges();

            }
            catch (Exception ex)
            {
                response.message = "Failed to save Tax Category: " + ex.Message; 
                response.currentId = 0; 
            }
            return response;
        }

        public GenericResponse UpdateTaxCategory(TaxCategoryEntity tax)
        {
            GenericResponse response = new GenericResponse();
            try
            {
         
                var existing = _context.taxCategory.Where(c => c.ID == tax.ID).FirstOrDefault();
                if (existing != null)
                {
                    tax.IsActive = true;
                    tax.IsDeleted = false;

                    tax.UpdatedOn = DateTime.Now;


                    _context.SaveChanges();

                    _context.Entry(existing).CurrentValues.SetValues(tax);
                    _context.SaveChanges(); 
                    response.statuCode = 1;  
                    response.message = "Tax Category updated successfully"; 
                    response.currentId = tax.ID; 
                }
                else
                {
                    response.statuCode = 0; 
                    response.message = "Tax Category not found";
                    response.currentId = 0; 
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update Tax Category: " + ex.Message; 
                response.currentId = 0;  
                return response;
            }
        }

        public GenericResponse DeleteTax(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
               
                var existing = _context.taxCategory.Where(c => c.ID == id).FirstOrDefault();
                if (existing != null)
                {
                  
                    existing.IsDeleted = true;
                    _context.Update(existing); 
                    _context.SaveChanges(); 
                    response.statuCode = 1; 
                    response.message = "Tax Category deleted successfully";
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
                response.message = "Failed to delete Tax Category: " + ex.Message; 
                response.currentId = 0;  
            }
            return response;
        }

    }
}
