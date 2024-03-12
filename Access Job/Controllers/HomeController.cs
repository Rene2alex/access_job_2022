using System;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Net;
using System.Text;

namespace Access_Job.Controllers
{
    public class HomeController : Controller
    {
        // Cadena de conexión a tu base de datos SQL Server
        string connectionString = "Server=localhost:3308;Database=formulario_contacto;Trusted_Connection=True;";

        public ActionResult Index()
        {
            return View();
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

        // Nuevo método para enviar una solicitud HTTP POST
        [HttpPost]
        public ActionResult SendData(string nombre, string correo, string telefono, string mensaje)
        {
            try
            {
                // Insertar los datos en la tabla "contactos" de la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO contactos (nombre, correo, telefono, mensaje) VALUES (@nombre, @correo, @telefono, @mensaje)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@telefono", telefono);
                    command.Parameters.AddWithValue("@mensaje", mensaje);
                    command.ExecuteNonQuery();
                }

                // Envía la solicitud HTTP POST
                SendHttpPost(nombre, correo, telefono, mensaje);

                ViewBag.ResponseMessage = "Datos enviados y guardados correctamente.";
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante el proceso
                ViewBag.ResponseMessage = "Error: " + ex.Message;
            }

            // Redirigir de vuelta a la página de inicio
            return View("Index");
        }

        // Método para enviar la solicitud HTTP POST
        private void SendHttpPost(string nombre, string correo, string telefono, string mensaje)
        {
            // URL del script PHP en tu servidor XAMPP que recibirá los datos
            string url = "http://localhost/insertar_contacto.php";

            // Construir los datos a enviar
            string postData = $"nombre={nombre}&correo={correo}&telefono={telefono}&mensaje={mensaje}";

            // Codificar los datos para enviarlos en la solicitud
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Crear una solicitud HTTP POST
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            // Escribir los datos en el cuerpo de la solicitud
            using (var stream = request.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            // Obtener la respuesta del servidor
            using (WebResponse response = request.GetResponse())
            {
                // No es necesario procesar la respuesta aquí, pero podrías hacerlo si lo necesitas
            }
        }
    }
}
