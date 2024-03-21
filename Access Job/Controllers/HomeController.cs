using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;




using MySql.Data.MySqlClient;

namespace Access_Job.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {

          
            return View();
        }

        [HttpPost]
        public ActionResult Index(string nombre, string correo, string telefono, string mensaje)
        {
            string connectionString = "Server=localhost;Port=3308;Database=formulariop;Uid=root;Pwd=;";

            // Insertar en la base de datos
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO contactos (nombre, correo, telefono, mensaje) VALUES (@Nombre, @Correo, @Telefono, @Mensaje)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", nombre);
                command.Parameters.AddWithValue("@Correo", correo);
                command.Parameters.AddWithValue("@Telefono", telefono);
                command.Parameters.AddWithValue("@Mensaje", mensaje);
                connection.Open();
                command.ExecuteNonQuery();
            }

            // Enviar correo electrónico
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("alfredodanielramos123456@gmail.com"); // Coloco la dirección de correo electrónico quien lo envia
            mail.To.Add("rene230802@gmail.com"); // Coloco el corrego de quien lo resibe 
            mail.Subject = "Nuevo mensaje desde el formulario"; // en mensaje 
            mail.Body = $"Nombre: {nombre}\nCorreo: {correo}\nTeléfono: {telefono}\nMensaje: {mensaje}"; // los datos que se mostraran

            // configuraccion del servidor 
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("alfredodanielramos123456@gmail.com", "cxrqgqhylkwprumv"); // Coloca tus credenciales
            smtp.EnableSsl = true;
            smtp.Send(mail);

            // Redirigir al Index con un ancla para el apartado "CONTACTO"
            return RedirectToAction("Index", new RouteValueDictionary { { "scrollTo", "contacto" } });
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

      
        }
    }

