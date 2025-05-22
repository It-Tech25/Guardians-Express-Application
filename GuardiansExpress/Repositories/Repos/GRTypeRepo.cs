using GuardiansExpress.Models.Entity;
using GuardiansExpress.Repositories.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GuardiansExpress.Repositories.Repos
{
    public class GRTypeRepo : IGRTypeRepo
    {
        private readonly MyDbContext _context;

        public GRTypeRepo(MyDbContext context)
        {
            _context = context;
        }

        //----------------------------- GR Type Master -------------------------------------------

        public IEnumerable<GRType> GetGRTypes(string searchTerm, int pageNumber, int pageSize)
        {
            var grTypes = _context.gRTypes.Where(g => g.IsDeleted == false)
                .ToList();
            return grTypes;
        }

        public GenericResponse CreateGRType(GRType grType)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                grType.IsDeleted = false;
                _context.gRTypes.Add(grType);
                _context.SaveChanges();
                response.statuCode = 1;
                response.message = "GR Type created successfully";
                response.currentId = grType.GRTypeId;
            }
            catch (Exception ex)
            {
                response.message = "Failed to save GR Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }

        public GRType GetGRTypeById(int id)
        {
            return _context.gRTypes.Find(id);
        }

        public GenericResponse UpdateGRType(GRType grType)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingGRType = _context.gRTypes.Where(g => g.GRTypeId == grType.GRTypeId).FirstOrDefault();
                if (existingGRType != null)
                {
                    grType.IsDeleted = false;
                    _context.Entry(existingGRType).CurrentValues.SetValues(grType);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "GR Type updated successfully";
                    response.currentId = grType.GRTypeId;
                }
                else
                {
                    response.statuCode = 0;
                    response.message = "GR Type not found";
                    response.currentId = 0;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.message = "Failed to update GR Type: " + ex.Message;
                response.currentId = 0;
                return response;
            }
        }

        public GenericResponse DeleteGRType(int id)
        {
            GenericResponse response = new GenericResponse();
            try
            {
                var existingGRType = _context.gRTypes.Where(g => g.GRTypeId == id).FirstOrDefault();
                if (existingGRType != null)
                {
                    existingGRType.IsDeleted = true;
                    _context.Update(existingGRType);
                    _context.SaveChanges();
                    response.statuCode = 1;
                    response.message = "GR Type deleted successfully";
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
                response.message = "Failed to delete GR Type: " + ex.Message;
                response.currentId = 0;
            }
            return response;
        }
    }
}

