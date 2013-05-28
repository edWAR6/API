using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using EARTH.Jaguar.Models;
using EARTH.Jaguar.Security;

namespace EARTH.Jaguar.Controllers
{
    public class PublicMessagesController : ApiController
    {
        // GET api/public/messages/{last}
        [Authorize]        
        public IEnumerable<P_NotasPublicas> GetNewPublicMessages(int last)
        {            
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                SGA_DesarrolloEntities context = new SGA_DesarrolloEntities();
                context.Configuration.ProxyCreationEnabled = false;
                IEnumerable<P_NotasPublicas> notas;

                notas = (from pm in context.P_NotasPublicas
                         where pm.Activa == true && pm.idNotasPublicas > last
                         select pm).Take(50);
                return notas;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Impersonate.UndoImpersonation();
            }
        }

        // GET api/old/public/messages/{last}
        [Authorize]        
        public IEnumerable<P_NotasPublicas> GetOldPublicMessages(int last)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                SGA_DesarrolloEntities context = new SGA_DesarrolloEntities();
                context.Configuration.ProxyCreationEnabled = false;
                IEnumerable<P_NotasPublicas> notas;

                notas = (from pm in context.P_NotasPublicas
                         where pm.Activa == true && pm.idNotasPublicas <= last
                         select pm).Take(50); ;
                return notas;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Impersonate.UndoImpersonation();
            }
        }
    }
}
