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
    public class GradesController : ApiController
    {
        private SGA_DesarrolloEntities db;

        // GET api/Grades
        [Authorize]        
        public IEnumerable<R_RegistroNotas> GetR_RegistroNotas()
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                var r_registronotas = db.R_RegistroNotas.Include(r => r.R_Estudiantes);
                return r_registronotas.AsEnumerable();
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

        // GET api/Grades/{id}
        [Authorize]        
        public R_RegistroNotas GetR_RegistroNotas(int id)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                R_RegistroNotas r_registronotas = db.R_RegistroNotas.Find(id);
                if (r_registronotas == null)
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
                }

                return r_registronotas;
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

        // GET api/{userName}/{year}/{period}/grades
        [Authorize]        
        public IEnumerable<R_RegistroNotas> GetGrades(string userName, int year, string period)
        {
            try
            {
                NameValueCollection user = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                Impersonate.ImpersonateUser(ConfigurationManager.AppSettings["Domain"].ToString(), user["UserName"], user["Password"]);
                this.db = new SGA_DesarrolloEntities();
                db.Configuration.ProxyCreationEnabled = false;

                var grades = (from g in db.R_RegistroNotas.Include(g => g.R_Cursos)
                              join s in db.R_Estudiantes
                                  on g.Estudiante equals s.IdPersona
                              where s.usuario == userName
                              && g.A_Adem == year
                              && g.Trimestre == period
                              select g);

                return grades;
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
