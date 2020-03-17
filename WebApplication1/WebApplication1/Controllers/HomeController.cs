using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using BLL;
using Common.Extension;
using Model.Entity;
using Common.Data;
using Common.AutoMapper;
using ModelDto;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private BLL.T_UserServer _user;
        private BLL.T_SystemServer _system;
        private BLL.BaseTransactionHelper _transaction;
        public HomeController(BLL.T_UserServer user, BLL.T_SystemServer system, BLL.BaseTransactionHelper transaction)
        {
            _user = user;
            _system = system;
            _transaction = transaction;
        }

        public IActionResult Index()
        {
            //BaseHelper.Current().BeginOrUseTransaction();
            try
            {
                var model = _user.GetList().FirstOrDefault().MapTo<UserDto>();
                ViewBag.Name = model.Name;
            }
            catch (Exception ex)
            {
                //BaseHelper.Current().Rollback();
            }
            //BaseHelper.Current().Rollback();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
