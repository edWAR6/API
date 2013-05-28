using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using EARTH.Jaguar.Models;
using System.DirectoryServices;
using System.Data.EntityClient;
using System.Security.Principal;
using EARTH.Jaguar.Security;
using System.Configuration;

namespace EARTH.Jaguar.Controllers
{
    [AllowAnonymous]
    public class LoginController : ApiController
    {
        //POST api/login
        [AllowAnonymous]
        public HttpResponseMessage Post(LogOnModel user)
        {
            try
            {
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user.UserName, user.Password);
                if (this.ValidateUser(user))
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, true);                    
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                return Request.CreateResponse(HttpStatusCode.OK, false);
            }
            catch (DirectoryServicesCOMException dse)
            {
                HttpError error = new HttpError(dse.Message.Replace("\n", " ").Replace("\r", " ").Replace("\t", " "));
                return Request.CreateResponse(HttpStatusCode.Unauthorized, error);
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

        private bool ValidateUser(LogOnModel model)
        {
            bool valid = false;
            
            using (SGA_DesarrolloEntities context = new SGA_DesarrolloEntities())
            {
                valid = context.P_Personas.Count(p => p.login_red == model.UserName) > 0;
                if (valid)
                {
                    DirectoryEntry entry = new DirectoryEntry("LDAP://" + ConfigurationManager.AppSettings["Domain"].ToString(), model.UserName, model.Password);
                    //TODO: uncomment this line in server.
                    object nativeObject = entry.NativeObject;
                }
            }
            return valid;
        }
    }
}
