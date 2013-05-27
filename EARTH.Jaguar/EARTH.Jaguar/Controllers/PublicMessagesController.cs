using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using EARTH.Jaguar.Models;

namespace EARTH.Jaguar.Controllers
{
    public class PublicMessagesController : ApiController
    {
        // GET api/public/messages/{last}
        //[Authorize]
        //[InitializeSimpleMembership]
        public IEnumerable<P_NotasPublicas> GetNewPublicMessages(int last)
        {            
            try
            {                
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
        }

        // GET api/old/public/messages/{last}
        // [Authorize]
        //[InitializeSimpleMembership]
        public IEnumerable<P_NotasPublicas> GetOldPublicMessages(int last)
        {
            try
            {
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
        }
    }
}
