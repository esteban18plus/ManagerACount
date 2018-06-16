using ManagerACount.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerACount.Filters;
using Microsoft.AspNetCore.Http;

namespace ManagerACount.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LogSessionController: Controller
    {
        protected readonly EfManagerAccountConfigurationsContext _contextConfiguration;
        protected IHttpContextAccessor _accessor;

        public LogSessionController(IHttpContextAccessor accessor, EfManagerAccountConfigurationsContext contextConfiguration)
        {
            _accessor = accessor;
            _contextConfiguration = contextConfiguration;
        }

        [HttpGet]
        [ValidateAccount(_ip = "0.0.0.0")]
        public JsonResult Get()
        {
            var result =  _contextConfiguration.LogSession.ToList();
            return Json(result);
        }
    }
}
