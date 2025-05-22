using Azure;
using GuardiansExpress.Models.Entity;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace GuardiansExpress.Controllers
{
    public class GRNoController : Controller
    {
        private readonly IGRNoService _grnoService;
        public GRNoController(IGRNoService grnoService)
        {
            _grnoService = grnoService;
        }
        public IActionResult Index()
        {
            var res = _grnoService.List();
            return View(res);
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddGRNo(GRNoEntity model)
        {
            GenericResponse res = new GenericResponse();
            if (ModelState.IsValid)
            {
                res = _grnoService.AddGRNo(model);
                if (res.statuCode == 1)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = res.message });
                }
            }
            return Json(new { success = false, message = "Validation failed." });
        }
        [HttpPost]
        public IActionResult UpdateGRNo(GRNoEntity gr)
        {
            GenericResponse res = new GenericResponse();
            res = _grnoService.UpdateGRNo(gr);
            if (res.statuCode == 1)
            {
                return Json(new { success = true, message = res.message });
            }
            return Json(new { success = false, message = res.message });
        }

        [HttpPost]
        public IActionResult DeleteGrNo(int id)
        {
            GenericResponse res = new GenericResponse();
            res = _grnoService.DeleteGRno(id);
            if (res.statuCode == 1)
            {
                return Json(new { success = true, message = res.message });
            }
            return Json(new { success = false, message = res.message });
        }
    }
}
