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
        public ActionResult Index()
        {
            return View();
        }
        // GET: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login model)
        {
            System.Diagnostics.Debug.WriteLine("Index POST method called");  // Log cuando se llama al método POST
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Model is valid");  // Log si el modelo es válido
                string connectionString = ConfigurationManager.ConnectionStrings["Model1"].ConnectionString;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "SELECT contrasena FROM Usuarios WHERE correo = @Email";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", model.Email);

                    con.Open();
                    var storedPassword = cmd.ExecuteScalar() as string;  // Obtiene la contraseña almacenada

                    if (storedPassword != null && VerifyPassword(model.Password, storedPassword))
                    {
                        System.Diagnostics.Debug.WriteLine("Password verified");  // Log si la contraseña es verificada
                        FormsAuthentication.SetAuthCookie(model.Email, false);  // Autentica al usuario
                        return RedirectToAction("PanelAdmin");  // Redirige al panel de administración
                    }
                    else
                    {
                        ModelState.AddModelError("", "Correo o contraseña incorrectos.");
                        System.Diagnostics.Debug.WriteLine("Invalid login attempt");  // Log si el intento de inicio de sesión es inválido
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Model is not valid");  // Log si el modelo no es válido
            }
            return View(model);  // Devuelve la vista con los errores de validación
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            return inputPassword == storedPassword;
        }



        public ActionResult PanelAdmin()
        {
            return View();  // Devuelve la vista del panel de administración
        }


    }

}