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
            EnviarCorreo(correo, nombre, telefono, mensaje);

            // Redirigir al Index con un ancla para el apartado "CONTACTO"
            return RedirectToAction("Index", new RouteValueDictionary { { "scrollTo", "contacto" } });
        }

        private void EnviarCorreo(string correoDestino, string nombre, string telefono, string mensaje)
        {
            string correoOrigen = "rene230802@gmail.com"; // Cambiar por tu dirección de correo electrónico
            string contraseña = "Alex32&22rene"; // Cambiar por tu contraseña
            string servidorSmtp = "smtp.gmail.com"; // Cambiar si estás usando un servidor SMTP diferente
            int puertoSmtp = 587; // Cambiar el puerto si es necesario
            bool ssl = true;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(correoOrigen);
                mail.To.Add(correoDestino);
                mail.Subject = "Nuevo mensaje de contacto";
                mail.Body = $"Nombre: {nombre}\nCorreo: {correoDestino}\nTeléfono: {telefono}\nMensaje: {mensaje}";

                using (SmtpClient smtp = new SmtpClient(servidorSmtp, puertoSmtp))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(correoOrigen, contraseña);
                    smtp.EnableSsl = ssl;
                    smtp.Send(mail);
                }
            }
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

