using GuardiansExpress.Models.DTOs;
using GuardiansExpress.Services.Interfaces;
using GuardiansExpress.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GuardiansExpress.Controllers
{
    public class AuthenticateController : Controller
    {

        private readonly IUserMgmtService userMgmtService;

        public AuthenticateController(IUserMgmtService _userService)
        {
            userMgmtService = _userService;
        }

        public IActionResult Login()
        {
            //LoginResponse lr = new LoginResponse();
            //lr = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            //if (lr == null)
            //{
            //    return RedirectToAction("Login", "Authenticate");
            //}
            LoginRequest res = new LoginRequest();

			return View(res);
        }

        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {
            var ipAddress = HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") ?? HttpContext.Connection.RemoteIpAddress?.ToString();
            var ipAddressWithoutPort = ipAddress?.Split(':')[0];

            //var ipApiResponse = await _ipApiClient.Get(ipAddressWithoutPort, ct);

            //var response = new
            //{
            //	IpAddress = ipAddressWithoutPort,
            //	Country = ipApiResponse?.country,
            //	Region = ipApiResponse?.regionName,
            //	City = ipApiResponse?.city,
            //	District = ipApiResponse?.district, 
            //	PostCode = ipApiResponse?.zip,
            //	Longitude = ipApiResponse?.lon.GetValueOrDefault(),
            //	Latitude = ipApiResponse?.lat.GetValueOrDefault(),
            //};

            try
            {
                if (ModelState.IsValid)
                {
                    var result = userMgmtService.LoginCheck(request);
                    if (result.statusCode == 0)
                    {
                        ViewBag.ErrorMessage = result.Message;
                    }
                    else
                    {
                        // store values / object in session

                        HttpContext.Session.SetObjectAsJson("loggedUser", result);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {


            }



            return View(request);
        }

    }
}
