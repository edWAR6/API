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
    public class YearsController : ApiController
    {
        private SGA_DesarrolloEntities db;

        // GET api/Years
        [Authorize]
        public IEnumerable<VR_Rendimiento_A_Academ> GetVR_Rendimiento_A_Academ()
        {
            try
            {                
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                return db.VR_Rendimiento_A_Academ.AsEnumerable();
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

        // GET api/{userName}/years
        [Authorize]
        public IEnumerable<VR_Rendimiento_A_Academ> GetYearsByUser(string userName)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                var years = (from y in db.VR_Rendimiento_A_Academ
                             join e in db.R_Estudiantes
                             on y.Estudiante equals e.IdPersona
                             join p in db.P_Personas
                             on e.IdPersona equals p.IdPersona
                             orderby y.A_Adem
                             where p.login_red == userName
                             select y
                );
                return years;
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

        // GET api/Years/5
        [Authorize]
        public VR_Rendimiento_A_Academ GetVR_Rendimiento_A_Academ(int id)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                VR_Rendimiento_A_Academ vr_rendimiento_a_academ = db.VR_Rendimiento_A_Academ.Find(id);
                if (vr_rendimiento_a_academ == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return vr_rendimiento_a_academ;
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
