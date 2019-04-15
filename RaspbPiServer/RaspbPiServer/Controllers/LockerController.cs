using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RaspbPiServer.Controllers
{
    using Enums;
    using Interfaces;
    using LockerApp.ServiceModels;
    using Microsoft.AspNetCore.Authorization;

    [Route("api/[controller]")]
    [ApiController]
    public class LockerController : ControllerBase
    {
        private readonly ILockerManager lockerManager;

        public LockerController(ILockerManager manager)
        {
            this.lockerManager = manager;
        }

        [HttpGet, AllowAnonymous, Route("GetPowerStatus")]
        // GET: Gets all the employees
        public IActionResult GetPowerStatus()
        {
            var res = new ResponseData<PowerTypeEnum?>()
            {
                HasBeenSuccessful = false
            };


            try
            {
                var managerResult = this.lockerManager.GetCurrentPowerType();
                if (managerResult != null)
                {
                    res.Content = managerResult;
                    res.HasBeenSuccessful = true;
                    res.Error = null;
                    return Ok(res);
                }

                res.Content = null;
                res.HasBeenSuccessful = false;
                res.Error = null;
                return Ok(res);
            }
            catch (Exception e)
            {
                res.Content = null;
                res.HasBeenSuccessful = false;
                res.Error = e.Message;
               return Ok(res);
            }
        }

        [HttpPut, AllowAnonymous, Route("CloseLocker")]
        // GET: Gets all the employees
        public IActionResult CloseLocker()
        {
            var res = new ResponseBase()
            {
                HasBeenSuccessful = false
            };


            try
            {
                var managerResult = this.lockerManager.Close();
                if (managerResult)
                {
                    res.HasBeenSuccessful = true;
                    res.Error = null;
                    return Ok(res);
                }

                res.HasBeenSuccessful = false;
                res.Error = null;
                return Ok(res);
            }
            catch (Exception e)
            {
                res.HasBeenSuccessful = false;
                res.Error = e.Message;
                return Ok(res);
            }
        }

        [HttpPut, AllowAnonymous, Route("CheckApi")]
        // GET: Gets all the employees
        public IActionResult CheckApiRunning()
        {
            return Ok("API is working as expected ");
        }


        [HttpPut, AllowAnonymous, Route("OpenLocker")]
        // GET: Gets all the employees
        public IActionResult OpenLocker()
        {
            var res = new ResponseBase()
            {
                HasBeenSuccessful = false
            };


            try
            {
                var managerResult = this.lockerManager.Open();
                if (managerResult)
                {
                    
                    res.HasBeenSuccessful = true;
                    res.Error = null;
                    return Ok(res);
                }

                res.HasBeenSuccessful = false;
                res.Error = null;
                return Ok(res);
            }
            catch (Exception e)
            {
                res.HasBeenSuccessful = false;
                res.Error = e.Message;
                return Ok(res);
            }
        }

    }
}