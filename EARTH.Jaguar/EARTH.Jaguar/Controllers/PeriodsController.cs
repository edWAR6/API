using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using EARTH.Jaguar.Models;
using EARTH.Jaguar.Security;

namespace EARTH.Jaguar.Controllers
{
    public class PeriodsController : ApiController
    {
        private SGA_DesarrolloEntities db;

        // GET api/Periods
        [Authorize]
        public IEnumerable<VR_Rendimiento_Periodos> GetVR_Rendimiento_Periodos()
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                return db.VR_Rendimiento_Periodos.AsEnumerable();
            }
            catch (System.Exception)
            {                
                throw;
            }
            finally
            {
                Impersonate.UndoImpersonation();
            }            
        }

        // GET api/Periods/{id}
        [Authorize]
        public VR_Rendimiento_Periodos GetVR_Rendimiento_Periodos(int id)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                VR_Rendimiento_Periodos vr_rendimiento_periodos = db.VR_Rendimiento_Periodos.Find(id);
                if (vr_rendimiento_periodos == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return vr_rendimiento_periodos;
            }
            catch (System.Exception)
            {                
                throw;
            }
            finally
            {
                Impersonate.UndoImpersonation();
            }
        }

        // GET api/{userName}/{year}/Periods
        [Authorize]
        public IEnumerable<VR_Rendimiento_Periodos> GetPeriodsByYearAndUser(string userName, int year)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                var periods = (from p in db.VR_Rendimiento_Periodos
                               join e in db.R_Estudiantes
                               on p.Estudiante equals e.IdPersona
                               join pe in db.P_Personas
                               on e.IdPersona equals pe.IdPersona
                               where pe.login_red == userName
                               && p.A_Adem == year
                               select p
                );
                return periods;
            }
            catch (System.Exception)
            {                
                throw;
            }
            finally
            {
                Impersonate.UndoImpersonation();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}