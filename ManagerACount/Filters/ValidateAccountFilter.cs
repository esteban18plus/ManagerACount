using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagerACount.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;

namespace ManagerACount.Filters
{
    public class ValidateAccountAttribute : Attribute, IActionFilter
    {

        protected StringValues _token;
        public String _ip;
        protected StringValues _authorization;
        protected readonly EfManagerAccountConfigurationsContext _contextConfiguration;

        public ValidateAccountAttribute() : base()
        {

        }

        public ValidateAccountAttribute(EfManagerAccountConfigurationsContext contextConfiguration)
        {
            _contextConfiguration = contextConfiguration;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            var headers = context.HttpContext.Request.Headers;

            headers.TryGetValue("Authorization", out _authorization);
            var str = _authorization.ToString();
            _token = str.Replace("Bearer", "");

            if (ValidateToken(_token,_ip))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool ValidateToken(string token, string IP)
        {

            var result = _contextConfiguration.LogSession.First(l => l.Token.Equals(token) && l.IP.Equals(IP) && l.State == true);

            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
