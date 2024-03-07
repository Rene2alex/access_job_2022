using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Access_Job.Controllers
{
    public class HomeController : Controller
    {
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

        [HttpPost]
        public ActionResult SaveContact(string nombre, string correo, string telefono, string mensaje)
        {
            string connectionString = "Server=localhost;Port=3308;Database=formulario_contacto;Uid=root;Pwd=;";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO 0 (nombre, correo, telefono, mensaje) VALUES (@Nombre, @Correo, @Telefono, @Mensaje)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", nombre);
                        command.Parameters.AddWithValue("@Correo", correo);
                        command.Parameters.AddWithValue("@Telefono", telefono);
                        command.Parameters.AddWithValue("@Mensaje", mensaje);

                        int rowsAffected = command.ExecuteNonQuery();
                        ViewBag.SuccessMessage = "¡Tu mensaje ha sido enviado con éxito!";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ha ocurrido un error al enviar el mensaje: " + ex.Message;
                Console.WriteLine(ex.ToString()); // Agregar esta línea para imprimir el mensaje de error en la consola
            }


            return View("Index");
        }
    }
}
