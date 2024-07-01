using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using RadioConexionLatam.Models;

namespace RadioConexionLatam.Controllers
{
    public class LoginController : Controller
    {
        private Model1 db = new Model1();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT contrasena, nombre, apellido, idRol FROM Usuarios WHERE correo = @Email";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", model.Email);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedPassword = reader["contrasena"].ToString();
                            string userName = reader["nombre"].ToString();
                            string userLastName = reader["apellido"].ToString();
                            string userRole = reader["idRol"].ToString();

                            if (VerifyPassword(model.Password, storedPassword))
                            {
                                FormsAuthentication.SetAuthCookie(model.Email, false);
                                Session["UserName"] = userName;
                                Session["UserLastName"] = userLastName;
                                Session["UserRole"] = userRole;

                                return RedirectToAction("PanelAdmin");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                        }
                    }
                }
            }

            return View(model);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword.Trim() == storedPassword.Trim();
        }

        public ActionResult PanelAdmin()
        {
            // Obtener el nombre del usuario actual y el rol desde la sesión
            var userName = Session["UserName"] as string;
            var userLastName = Session["UserLastName"] as string;
            var userRole = Session["UserRole"] as string;

            // Contar anuncios y eventos activos
            var activeAnunciosCount = db.Anuncios.Count(a => a.estado == "A");
            var activeEventosCount = db.Eventos.Count(e => e.estado == "A");

            // Pasar los datos a la vista
            ViewBag.UserName = userName;
            ViewBag.UserLastName = userLastName;
            ViewBag.UserRole = userRole;
            ViewBag.ActiveAnunciosCount = activeAnunciosCount;
            ViewBag.ActiveEventosCount = activeEventosCount;

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
