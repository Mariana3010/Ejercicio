using System;
using System.Data.SqlClient;

namespace Ejercicio1_Examen
{
    public class Program
    {
        static void Main(string[] args)
        {
            var  cadena = "Data Source=DESKTOP-EFTCLHO;Initial Catalog=EXAMEN_MSM;Integrated Security=True;";
            var continuar = true;

            while (continuar)
            {
                Console.WriteLine("----------------------Menú Principal:------------------------");
                Console.WriteLine("1. Clientes");
                Console.WriteLine("2. Mascotas");
                Console.WriteLine("3. Citas");
                Console.WriteLine("4. Salir");
                Console.WriteLine("\n");
                Console.Write("Elija una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        GestionarCliente(cadena);
                        break;
                    case "2":
                        GestionarMascotas(cadena);
                        break;
                    case "3":
                        GestionarCita(cadena);
                        break;
                    case "4":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta, Vuelva a intentarlo");
                        break;
                }
            }
        }

        static void GestionarMascotas(string cadena)
        {
            var volverMenuPrincipal = false;
            while (!volverMenuPrincipal)
            {
                Console.WriteLine("---------------------Menú de Mascotas:----------------------");
                Console.WriteLine("1. Agregar Mascota");
                Console.WriteLine("2. Ver Mascotas");
                Console.WriteLine("3. Editar Mascota");
                Console.WriteLine("4. Eliminar Mascota");
                Console.WriteLine("5. Volver al Menú Principal");
                Console.WriteLine("\n");
                Console.Write("Elija una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarMascota(cadena);
                        break;
                    case "2":
                        MostrarMascota(cadena);
                        break;
                    case "3":
                        EditarMascota(cadena);
                        break;
                    case "4":
                        EliminarMascota(cadena);
                        break;
                    case "5":
                        volverMenuPrincipal = true;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta, Vuelva a intentarlo");
                        break;
                }
            }
        }

        static void AgregarMascota(string cadenaConexion)
        {
            Console.Write("Nombre de la Mascota: ");
            var nombre = Console.ReadLine();
            Console.Write("Raza de la Mascota: ");
            var raza = Console.ReadLine();
            Console.Write("Fecha de Nacimiento de la Mascota: ");
            var fechaNacimiento = Console.ReadLine();

            
            MostrarCliente(cadenaConexion);

            
            Console.Write("Ingrese el Id del Cliente que desea agregar): ");
            var clienteId = int.Parse(Console.ReadLine());

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var registroMascota = "INSERT INTO Mascota (ClienteId, Nombre, Raza, FechaNacimiento) VALUES (@ClienteId, @Nombre, @Raza, @FechaNacimiento)";
                using (SqlCommand insertar = new SqlCommand(registroMascota, conexion))
                {
                    insertar.Parameters.AddWithValue("@ClienteId", clienteId);
                    insertar.Parameters.AddWithValue("@Nombre", nombre);
                    insertar.Parameters.AddWithValue("@Raza", raza);
                    insertar.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

                    var respuesta = insertar.ExecuteNonQuery();
                    if (respuesta > 0)
                    {
                        Console.WriteLine("Mascota registrada correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Error al registrar la mascota");
                    }
                }
            }
        }
        static void MostrarMascota(string cadenaConexion)
        {
            Console.WriteLine("Lista de Mascotas:");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var consultaCliente = "SELECT MascotaId, ClienteId, Nombre, Raza FROM Mascota";
                using (SqlCommand consultar = new SqlCommand(consultaCliente, conexion))
                {
                    using (SqlDataReader mostrar = consultar.ExecuteReader())
                    {
                        while (mostrar.Read())
                        {
                            var mascotaId = (int)mostrar["MascotaId"];
                            var clienteId = (int)mostrar["ClienteId"];
                            var nombre = (string)mostrar["Nombre"];
                            var raza = (string)mostrar["Raza"];

                            Console.WriteLine($"Id: {mascotaId}, ClienteId: {clienteId}, Nombre: {nombre}, Raza: {raza}");
                        }
                    }
                }
            }
        }
        static void EditarMascota(string cadenaConexion)
        {
            MostrarMascota(cadenaConexion);
            Console.Write("Ingrese el Id de la Mascota que desea actualizar: ");
            var mascotaId = int.Parse(Console.ReadLine());

            Console.WriteLine("¿Que dato desea actualizar? (clienteid/ nombre / raza / fechanacimiento): ");
            var opcion = Console.ReadLine().ToLower();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var actualizarMascota = "";
                SqlCommand actualizar = new SqlCommand();

                switch (opcion)
                {
                    case "clienteid":
                        Console.Write("Nuevo ClienteId: ");
                        var nuevoCliente = Console.ReadLine();
                        actualizarMascota = "UPDATE Mascota SET ClienteId = @ClienteId WHERE MascotaId = @MascotaId";
                        actualizar = new SqlCommand(actualizarMascota, conexion);
                        actualizar.Parameters.AddWithValue("@ClienteId", nuevoCliente);
                        break;

                    case "nombre":
                        Console.Write("Nuevo Nombre: ");
                        var nuevoNombre = Console.ReadLine();
                        actualizarMascota = "UPDATE Mascota SET Nombre = @Nombre WHERE MascotaId = @MascotaId";
                        actualizar = new SqlCommand(actualizarMascota, conexion);
                        actualizar.Parameters.AddWithValue("@Nombre", nuevoNombre);
                        break;

                    case "raza":
                        Console.Write("Nueva Raza: ");
                        var nuevaRaza = Console.ReadLine();
                        actualizarMascota = "UPDATE Mascota SET Raza = @Raza WHERE MascotaId = @MascotaId";
                        actualizar = new SqlCommand(actualizarMascota, conexion);
                        actualizar.Parameters.AddWithValue("@Raza", nuevaRaza);
                        break;
                    case "fechanacimiento":
                        Console.Write("Nueva Fecha de Nacimiento: ");
                        var nuevaFecha = Console.ReadLine();
                        actualizarMascota = "UPDATE Mascota SET FechaNacimiento = @FechaNacimiento WHERE MascotaId = @MascotaId";
                        actualizar = new SqlCommand(actualizarMascota, conexion);
                        actualizar.Parameters.AddWithValue("@Raza", nuevaFecha);
                        break;

                    default:
                        Console.WriteLine("Opción no válida");
                        return;
                }

                actualizar.Parameters.AddWithValue("@MascotaId", mascotaId);
                var respuesta = actualizar.ExecuteNonQuery();

                if (respuesta > 0)
                {
                    Console.WriteLine("Mascota actualizada correctamente");
                }
                else
                {
                    Console.WriteLine("Error al actualizar a la Mascota");
                }
            }
        }
        static void EliminarMascota(string cadenaConexion)
        {
            MostrarMascota(cadenaConexion);
            Console.Write("Ingrese el Id de la Mascota que desea eliminar: ");
            var mascotaId = int.Parse(Console.ReadLine());

            Console.Write("¿Está seguro de que desea eliminar a la Mascota? (S/N): ");
            var confirmacion = Console.ReadLine().ToUpper();
            if (confirmacion == "S")
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    var eliminarMascota = "DELETE FROM Mascota WHERE MascotaId = @MascotaId";
                    using (SqlCommand eliminar = new SqlCommand(eliminarMascota, conexion))
                    {
                        eliminar.Parameters.AddWithValue("@MascotaId", mascotaId);
                        var respuesta = eliminar.ExecuteNonQuery();
                        if (respuesta > 0)
                        {
                            Console.WriteLine("Mascota eliminada correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al eliminar a la Mascota");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Eliminación cancelada.");
            }
        }
        static void GestionarCliente(string cadena)
        {
            var volverMenuPrincipal = false;
            while (!volverMenuPrincipal)
            {
                Console.WriteLine("---------------------------Menú de Clientes:----------------------");
                Console.WriteLine("1. Agregar Cliente");
                Console.WriteLine("2. Ver Clientes");
                Console.WriteLine("3. Editar Cliente");
                Console.WriteLine("4. Eliminar Cliente");
                Console.WriteLine("5. Volver al Menú Principal");
                Console.WriteLine("\n");
                Console.Write("Elija una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarCliente(cadena);
                        break;
                    case "2":
                        MostrarCliente(cadena);
                        break;
                    case "3":
                        EditarCliente(cadena);
                        break;
                    case "4":
                        EliminarCliente(cadena);
                        break;
                    case "5":
                        volverMenuPrincipal = true;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta, Vuelva a intentarlo");
                        break;
                }
            }
        }
        static void AgregarCliente(string cadenaConexion)
        {
            Console.Write("Nombre del Cliente: ");
            var nombre = Console.ReadLine();
            Console.Write("Correo Electrónico: ");
            var correo = Console.ReadLine();
            Console.Write("Teléfono: ");
            var telefono = Console.ReadLine();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var registroCliente = "INSERT INTO Cliente (Nombre, Correo, Telefono) VALUES (@Nombre, @Correo, @Telefono)";
                using (SqlCommand insertar = new SqlCommand(registroCliente, conexion))
                {
                    insertar.Parameters.AddWithValue("@Nombre", nombre);
                    insertar.Parameters.AddWithValue("@Correo", correo);
                    insertar.Parameters.AddWithValue("@Telefono", telefono);
                    var respuesta = insertar.ExecuteNonQuery();
                    if (respuesta > 0)
                    {
                        Console.WriteLine("Cliente registrado correctamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al registrar al cliente");
                    }
                }
            }
        }
        static void MostrarCliente(string cadenaConexion)
        {
            Console.WriteLine("Lista de Clientes:");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var consultaCliente = "SELECT ClienteId, Nombre, Correo, Telefono FROM Cliente";
                using (SqlCommand consultar = new SqlCommand(consultaCliente, conexion))
                {
                    using (SqlDataReader mostrar = consultar.ExecuteReader())
                    {
                        while (mostrar.Read())
                        {
                            var clienteId = (int)mostrar["ClienteId"];
                            var nombre = (string)mostrar["Nombre"];
                            var correo = (string)mostrar["Correo"];
                            var telefono = (string)mostrar["Telefono"];

                            Console.WriteLine($"Id: {clienteId}, Nombre: {nombre}, Correo: {correo}, Teléfono: {telefono}");
                        }
                    }
                }
            }
        }
        static void EditarCliente(string cadenaConexion)
        {
            MostrarCliente(cadenaConexion);
            Console.Write("Ingrese el Id del cliente que desea actualizar: ");
            var clienteId = int.Parse(Console.ReadLine());

            Console.WriteLine("¿Que dato desea actualizar? (nombre / correo / telefono): ");
            var opcion = Console.ReadLine().ToLower();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var actualizarCliente = "";
                SqlCommand actualizar = new SqlCommand();

                switch (opcion)
                {
                    case "nombre":
                        Console.Write("Nuevo Nombre: ");
                        var nuevoNombre = Console.ReadLine();
                        actualizarCliente = "UPDATE Cliente SET Nombre = @Nombre WHERE ClienteId = @ClienteId";
                        actualizar = new SqlCommand(actualizarCliente, conexion);
                        actualizar.Parameters.AddWithValue("@Nombre", nuevoNombre);
                        break;

                    case "correo":
                        Console.Write("Nuevo Correo Electrónico: ");
                        var nuevoCorreo = Console.ReadLine();
                        actualizarCliente = "UPDATE Cliente SET Correo = @Correo WHERE ClienteId = @ClienteId";
                        actualizar = new SqlCommand(actualizarCliente, conexion);
                        actualizar.Parameters.AddWithValue("@Correo", nuevoCorreo);
                        break;

                    case "telefono":
                        Console.Write("Nuevo Teléfono: ");
                        var nuevoTelefono = Console.ReadLine();
                        actualizarCliente = "UPDATE Cliente SET Telefono = @Telefono WHERE ClienteId = @ClienteId";
                        actualizar = new SqlCommand(actualizarCliente, conexion);
                        actualizar.Parameters.AddWithValue("@Telefono", nuevoTelefono);
                        break;

                    default:
                        Console.WriteLine("Opción no válida");
                        return;
                }

                actualizar.Parameters.AddWithValue("@ClienteId", clienteId);
                var respuesta = actualizar.ExecuteNonQuery();

                if (respuesta > 0)
                {
                    Console.WriteLine("Cliente actualizado correctamente");
                }
                else
                {
                    Console.WriteLine("Error al actualizar al cliente");
                }
            }
        }
        static void EliminarCliente(string cadenaConexion)
        {
            MostrarCliente(cadenaConexion);
            Console.Write("Ingrese el Id del cliente que desea eliminar: ");
            var clienteId = int.Parse(Console.ReadLine());

            Console.Write("¿Está seguro de que desea eliminar al cliente? (S/N): ");
            var confirmacion = Console.ReadLine().ToUpper();
            if (confirmacion == "S")
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    var eliminarCliente = "DELETE FROM Cliente WHERE ClienteId = @ClienteId";
                    using (SqlCommand eliminar = new SqlCommand(eliminarCliente, conexion))
                    {
                        eliminar.Parameters.AddWithValue("@ClienteId", clienteId);
                        var respuesta = eliminar.ExecuteNonQuery();
                        if (respuesta > 0)
                        {
                            Console.WriteLine("Cliente eliminado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al eliminar al cliente");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Eliminación cancelada.");
            }
        }
        static void GestionarCita(string cadena)
        {
            var volverMenuPrincipal = false;
            while (!volverMenuPrincipal)
            {
                Console.WriteLine("---------------------------Menú de Citas:----------------------");
                Console.WriteLine("1. Agregar Cita");
                Console.WriteLine("2. Ver Citas");
                Console.WriteLine("3. Editar Cita");
                Console.WriteLine("4. Eliminar Cita");
                Console.WriteLine("5. Volver al Menú Principal");
                Console.WriteLine("\n");
                Console.Write("Elija una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarCita(cadena);
                        break;
                    case "2":
                        MostrarCita(cadena);
                        break;
                    case "3":
                        EditarCita(cadena);
                        break;
                    case "4":
                        EliminarCita(cadena);
                        break;
                    case "5":
                        volverMenuPrincipal = true;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Opcion incorrecta, Vuelva a intentarlo");
                        break;
                }
            }
        }
        static void AgregarCita(string cadenaConexion)
        {
            MostrarCliente(cadenaConexion);

            Console.Write("Ingrese el Id del Cliente que desea agregar): ");
            var clienteId = int.Parse(Console.ReadLine());

            MostrarMascota(cadenaConexion);

            Console.Write("Ingrese el Id de la Mascota que desea agregar): ");
            var mascotaId = int.Parse(Console.ReadLine());

            Console.Write("Ingrese la Fecha de la cita: ");
            var fecha = Console.ReadLine();

            Console.Write("Ingrese el Motivo de la cita: ");
            var motivo = Console.ReadLine();

            Console.Write("Notas: ");
            var nota = Console.ReadLine();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var registroCliente = "INSERT INTO Cita (ClienteId, MascotaId, Fecha, Motivo, Notas) VALUES (@ClienteId, @MascotaId, @Fecha, @Motivo, @Notas)";
                using (SqlCommand insertar = new SqlCommand(registroCliente, conexion))
                {
                    insertar.Parameters.AddWithValue("@ClienteId", clienteId);
                    insertar.Parameters.AddWithValue("@MascotaId", mascotaId);
                    insertar.Parameters.AddWithValue("@Fecha", fecha);
                    insertar.Parameters.AddWithValue("Motivo", motivo);
                    insertar.Parameters.AddWithValue("@Notas", nota);

                    var respuesta = insertar.ExecuteNonQuery();
                    if (respuesta > 0)
                    {
                        Console.WriteLine("Cita registrada correctamente");
                    }
                    else
                    {
                        Console.WriteLine("Error al registrar la cita");
                    }
                }
            }
        }
        static void MostrarCita(string cadenaConexion)
        {
            Console.WriteLine("Lista de Citas:");

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var consultaCliente = "SELECT CitaId, ClienteId, MascotaId, Fecha, Notas FROM Cita";
                using (SqlCommand consultar = new SqlCommand(consultaCliente, conexion))
                {
                    using (SqlDataReader mostrar = consultar.ExecuteReader())
                    {
                        while (mostrar.Read())
                        {
                            var citaId = (int)mostrar["CitaId"];
                            var clienteId = (int)mostrar["ClienteId"];
                            var mascotaId = (int)mostrar["MascotaId"];
                            var fecha = (DateTime)mostrar["Fecha"];
                            var nota = (string)mostrar["Notas"];

                            Console.WriteLine($"Id: {citaId}, ClienteId: {clienteId}, MascotaId: {mascotaId}, Fecha: {fecha}, Notas: {nota}");
                        }
                    }
                }
            }
        }
        static void EditarCita(string cadenaConexion)
        {
            MostrarCita(cadenaConexion);
            Console.Write("Ingrese el Id de la cita que desea actualizar: ");
            var citaId = int.Parse(Console.ReadLine());

            Console.WriteLine("¿Que dato desea actualizar? (clienteid / mascotaid / fecha/ notas): ");
            var opcion = Console.ReadLine().ToLower();

            using (SqlConnection conexion = new SqlConnection(cadenaConexion))
            {
                conexion.Open();
                var actualizarCita = "";
                SqlCommand actualizar = new SqlCommand();

                switch (opcion)
                {
                    case "clienteid":
                        Console.Write("Nuevo ClienteId: ");
                        var nuevoCliente = Console.ReadLine();
                        actualizarCita = "UPDATE Cita SET ClienteId = @ClienteId WHERE CitaId = @CitaId";
                        actualizar = new SqlCommand(actualizarCita, conexion);
                        actualizar.Parameters.AddWithValue("@ClienteId", nuevoCliente);
                        break;

                    case "mascotaid":
                        Console.Write("Nuevo MascotaId: ");
                        var nuevaMascota = Console.ReadLine();
                        actualizarCita = "UPDATE Cita SET MascotaId = @MascotaId WHERE CitaId = @CitaId";
                        actualizar = new SqlCommand(actualizarCita, conexion);
                        actualizar.Parameters.AddWithValue("@MascotaId", nuevaMascota);
                        break;

                    case "fecha":
                        Console.Write("Nueva Fecha: ");
                        var nuevaFecha = Console.ReadLine();
                        actualizarCita = "UPDATE Cita SET Fecha = @Fecha WHERE CitaId = @CitaId";
                        actualizar = new SqlCommand(actualizarCita, conexion);
                        actualizar.Parameters.AddWithValue("@Fecha", nuevaFecha);
                        break;
                    case "notas":
                        Console.Write("Nueva Nota: ");
                        var nuevaNota = Console.ReadLine();
                        actualizarCita = "UPDATE Cita SET Notas = @Notas WHERE CitaId = @CitaId";
                        actualizar = new SqlCommand(actualizarCita, conexion);
                        actualizar.Parameters.AddWithValue("@Notas", nuevaNota);
                        break;

                    default:
                        Console.WriteLine("Opción no válida");
                        return;
                }

                actualizar.Parameters.AddWithValue("@CitaId", citaId);
                var respuesta = actualizar.ExecuteNonQuery();

                if (respuesta > 0)
                {
                    Console.WriteLine("Cita actualizada correctamente");
                }
                else
                {
                    Console.WriteLine("Error al actualizar la cita");
                }
            }
        }
        static void EliminarCita(string cadenaConexion)
        {
            MostrarCita(cadenaConexion);
            Console.Write("Ingrese el Id de la cita que desea eliminar: ");
            var citaId = int.Parse(Console.ReadLine());

            Console.Write("¿Está seguro de que desea eliminar la cita? (S/N): ");
            var confirmacion = Console.ReadLine().ToUpper();
            if (confirmacion == "S")
            {
                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    var eliminarCita = "DELETE FROM Cita WHERE CitaId = @CitaId";
                    using (SqlCommand eliminar = new SqlCommand(eliminarCita, conexion))
                    {
                        eliminar.Parameters.AddWithValue("@CitaId", citaId);
                        var respuesta = eliminar.ExecuteNonQuery();
                        if (respuesta > 0)
                        {
                            Console.WriteLine("Cita eliminado correctamente");
                        }
                        else
                        {
                            Console.WriteLine("Error al eliminar la cita");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Eliminación cancelada.");
            }
        }
    }
}
