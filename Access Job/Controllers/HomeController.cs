using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.IO;
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
            mail.From = new MailAddress("infoacccesjobsofhubmx@gmail.com"); // Coloco la dirección de correo electrónico quien lo envia
            mail.To.Add("infoacccesjobsofhubmx@gmail.com"); // Coloco el corrego de quien lo resibe 
            mail.Subject = "Nuevo mensaje desde el formulario"; // en mensaje 
            mail.Body = $"Nombre: {nombre}\nCorreo: {correo}\nTeléfono: {telefono}\nMensaje: {mensaje}"; // los datos que se mostraran

            // configuraccion del servidor 
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("infoacccesjobsofhubmx@gmail.com", "lrknfabqqtufjmms"); // Coloca tus credenciales
            smtp.EnableSsl = true;
            smtp.Send(mail);

            // Redirigir al Index con un ancla para el apartado "CONTACTO"
            return RedirectToAction("Index", new RouteValueDictionary { { "scrollTo", "contacto" } });
        }


        // Método para abrir el documento 1 en una nueva pestaña del navegador
        public ActionResult ManualdeUsuario()
        {
            string rutaDocumento1 = Server.MapPath("~/Content/Documentos/Manual de Usuario App.pdf");  // Ruta absoluta del Documento 1
            return MostrarArchivoEnNuevaPestana(rutaDocumento1);
        }

        // Método para mostrar un archivo en una nueva pestaña del navegador dado una ruta
      private ActionResult MostrarArchivoEnNuevaPestana(string rutaArchivo)
{
    // Verificar si el archivo existe en la ruta especificada
    if (System.IO.File.Exists(rutaArchivo))
    {
        // Leer los bytes del archivo
        byte[] fileBytes = System.IO.File.ReadAllBytes(rutaArchivo);
        // Obtener el nombre del archivo
        string fileName = Path.GetFileName(rutaArchivo);

        // Establecer el tipo de contenido y el encabezado Content-Disposition para el archivo
        Response.AddHeader("Content-Disposition", "inline; filename=" + fileName);
        Response.AddHeader("Content-Type", "application/pdf");

        // Agregar la etiqueta link para el icono de la pestaña del navegador
        // La ruta a la imagen debe ser una URL válida accesible desde el navegador
        // Aquí, "~/Content/Imagenes/accessJob_logo.png" es una ruta relativa al proyecto ASP.NET MVC
        // Pero para que el navegador pueda acceder a ella, necesitas convertirla a una URL absoluta
        // Si tienes una carpeta de imágenes accesible en tu proyecto, puedes usar una ruta absoluta o relativa según tu estructura de archivos
        // En este caso, vamos a suponer que la carpeta de imágenes está en la raíz del sitio web
        string iconURL = Url.Content("~/Content/Imagenes/accessJob_logo.png");
        Response.Write($"<link rel='icon' type='image/png' href='{iconURL}'>");

        // Devolver el archivo como resultado de la acción
        return File(fileBytes, "application/pdf");
    }
    else
    {
        // Manejar el caso donde el archivo no existe
        return HttpNotFound();
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

