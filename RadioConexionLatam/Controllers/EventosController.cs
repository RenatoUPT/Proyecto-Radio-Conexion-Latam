using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using RadioConexionLatam.Models;
using RadioConexionLatam.Models.Paypal_Order;
using RadioConexionLatam.Models.Paypal_Transaction;

namespace RadioConexionLatam.Controllers
{
    public class EventosController : Controller
    {
        private Model1 db = new Model1();

        // GET: Eventos
        public ActionResult Index()
        {
            var eventos = db.Eventos.Include(e => e.Categoria).ToList();
            return View(eventos);
        }

        

        // GET: Eventos/DetallesEvento/5
        public ActionResult DetallesEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // GET: Eventos/CrearEvento
        public ActionResult CrearEvento()
        {
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre");
            return View();
        }

        // POST: Eventos/CrearEvento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearEvento([Bind(Include = "idEvento,nombreEvento,descripcion,fechaEvento,lugar,organizador,estado,capacidad,idCategoria")] Eventos evento, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(ImagenFile.InputStream))
                    {
                        evento.Imagen = reader.ReadBytes(ImagenFile.ContentLength);
                    }
                }

                db.Eventos.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // GET: Eventos/EditarEvento/5
        public ActionResult EditarEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // POST: Eventos/EditarEvento/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEvento([Bind(Include = "idEvento,nombreEvento,descripcion,fechaEvento,lugar,organizador,estado,capacidad,idCategoria,Imagen")] Eventos evento, HttpPostedFileBase ImagenFile)
        {
            if (ModelState.IsValid)
            {
                if (ImagenFile != null && ImagenFile.ContentLength > 0)
                {
                    using (var reader = new System.IO.BinaryReader(ImagenFile.InputStream))
                    {
                        evento.Imagen = reader.ReadBytes(ImagenFile.ContentLength);
                    }
                }

                db.Entry(evento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idCategoria = new SelectList(db.Categorias, "idCategoria", "nombre", evento.idCategoria);
            return View(evento);
        }

        // GET: Eventos/EliminarEvento/5
        public ActionResult EliminarEvento(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Eventos evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        // POST: Eventos/EliminarEvento/5
        [HttpPost, ActionName("EliminarEvento")]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEventoConfirmado(int id)
        {
            Eventos evento = db.Eventos.Find(id);
            db.Eventos.Remove(evento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult VistaPublicitaria()
        {
            var eventosActivos = db.Eventos.Include(e => e.Categoria)
                                            .Where(e => e.estado == "A")
                                            .ToList();
            return View(eventosActivos);
        }

        //PAGO METODO PAYPAL

        [HttpPost]
        public async Task<JsonResult> Paypal(string precio, string producto)
        {
            bool status = false;
            string respuesta = string.Empty;

            using (var cliente = new HttpClient())
            {
                var username = ConfigurationManager.AppSettings["PAYPAL_CLIENT_ID"];
                var passwd = ConfigurationManager.AppSettings["PAYPAL_CLIENT_SECRET"];

                cliente.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{username}:{passwd}");
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var orden = new PaypalOrder()
                {
                    intent = "CAPTURE",
                    purchase_units = new List<Models.Paypal_Order.PurchaseUnit>()
                    {
                        new Models.Paypal_Order.PurchaseUnit()
                        {
                            amount = new Models.Paypal_Order.Amount()
                            {
                                currency_code = "USD",
                                value = precio
                            },
                            description = producto
                        }
                    },
                    application_context = new ApplicationContext()
                    {
                        brand_name = "PruebaTest",
                        landing_page = "NO_PREFERENCE",
                        user_action = "PAY_NOW",
                        return_url = "http://localhost:58332/Eventos/AprobacionPago",
                        cancel_url = "http://localhost:58332/Eventos/VistaPublicitaria"
                    }
                };

                var json = JsonConvert.SerializeObject(orden);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await cliente.PostAsync("/v2/checkout/orders", data);

                status = response.IsSuccessStatusCode;

                if (status)
                {
                    respuesta = response.Content.ReadAsStringAsync().Result;
                }
            }

            return Json(new { status = status, respuesta = respuesta }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> AprobacionPago()
        {
            string token = Request.QueryString["token"];

            bool status = false;

            using (var cliente = new HttpClient())
            {
                var username = ConfigurationManager.AppSettings["PAYPAL_CLIENT_ID"];
                var passwd = ConfigurationManager.AppSettings["PAYPAL_CLIENT_SECRET"];

                cliente.BaseAddress = new Uri("https://api-m.sandbox.paypal.com");

                var authToken = Encoding.ASCII.GetBytes($"{username}:{passwd}");
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                var data = new StringContent("{}", Encoding.UTF8, "application/json");
                HttpResponseMessage response = await cliente.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                status = response.IsSuccessStatusCode;

                ViewData["Status"] = status;

                if (status)
                {
                    var jsonRespuesta = response.Content.ReadAsStringAsync().Result;

                    PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonRespuesta);
                    ViewData["IdTransaccion"] = objeto.purchase_units[0].payments.captures[0].id;
                }
            }
            return View();
        }

    }
}