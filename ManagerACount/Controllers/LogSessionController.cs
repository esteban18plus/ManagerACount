using ManagerACount.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerACount.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LogSessionController: Controller
    {
        protected readonly EfManagerAccountConfigurationsContext _contextConfiguration;

        public LogSessionController(EfManagerAccountConfigurationsContext contextConfiguration)
        {
            _contextConfiguration = contextConfiguration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var result =  _contextConfiguration.LogSession.ToList();
            return Json(result);
        }
    }
}
