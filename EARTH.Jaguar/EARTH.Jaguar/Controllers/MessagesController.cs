using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EARTH.Jaguar.Models;
using EARTH.Jaguar.Security;

namespace EARTH.Jaguar.Controllers
{
    public class MessagesController : ApiController
    {
        // GET api/{userName}/messages/{last}
        [Authorize]
        public IEnumerable<P_Notas> GetNewUserMessages(string userName, int last)
        {
            try
            {                
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                IEnumerable<P_Notas> messages;
                SGA_DesarrolloEntities context = new SGA_DesarrolloEntities();
                context.Configuration.ProxyCreationEnabled = false;
                messages = (from n in context.P_Notas
                            join p in context.P_Personas
                            on n.IdPersona equals p.IdPersona
                            where p.login_red == userName && n.idNota > last
                            select n).Take(50);
                return messages;
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

        // GET api/user/{userName}/old/messages/{last}
        [Authorize]
        public IEnumerable<P_Notas> GetOldUserMessages(string userName, int last)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                IEnumerable<P_Notas> messages;
                SGA_DesarrolloEntities context = new SGA_DesarrolloEntities();
                context.Configuration.ProxyCreationEnabled = false;
                messages = (from n in context.P_Notas
                            join p in context.P_Personas
                            on n.IdPersona equals p.IdPersona
                            where p.login_red == userName && n.idNota <= last
                            select n).Take(50);
                return messages;
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
