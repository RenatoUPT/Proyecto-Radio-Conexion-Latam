using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using RadioConexionLatam.Models;

namespace RadioConexionLatam.Controllers
{
    public class LoginController : Controller
    {
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
                    System.Diagnostics.Debug.WriteLine("Conexión establecida con la base de datos.");
                    string query = "SELECT contrasena FROM Usuarios WHERE correo = @Email";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", model.Email);

                    con.Open();
                    System.Diagnostics.Debug.WriteLine("Conexión abierta.");
                    var storedPassword = cmd.ExecuteScalar() as string;

                    if (storedPassword != null)
                    {
                        System.Diagnostics.Debug.WriteLine("Usuario encontrado, verificando contraseña.");
                        if (VerifyPassword(model.Password, storedPassword))
                        {
                            System.Diagnostics.Debug.WriteLine("Contraseña verificada correctamente.");
                            FormsAuthentication.SetAuthCookie(model.Email, false);
                            return RedirectToAction("PanelAdmin");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Error de contraseña.");
                            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("No se encontró el usuario.");
                        ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Modelo no válido.");
            }
            return View(model);
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword.Trim() == storedPassword.Trim();
        }


        public ActionResult PanelAdmin()
        {
            return View();
        }
    }
}
