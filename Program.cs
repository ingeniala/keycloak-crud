using System;
using System.Collections.Generic;
using KeycloakRestAPI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Keycloak
{
    internal class Program
    {
        public static InfoKeycloak KC_Origen = new InfoKeycloak(TipoKeycloak.Keycloak_Origen);
        public static InfoKeycloak KC_Destino = new InfoKeycloak(TipoKeycloak.Keycloak_Destino);
        public static KeycloakResponseObjets keycloakApi = new KeycloakResponseObjets();

        static void Main(string[] args)
        {
            try
            {
                KC_Origen.token = keycloakApi.GetToken(KC_Origen).access_token;
                KC_Destino.token = keycloakApi.GetToken(KC_Destino).access_token;

                while (true)
                {
                    Console.WriteLine("Seleccione una opción:");
                    Console.WriteLine("1. Crear lote de usuarios de prueba en el Realm A");
                    Console.WriteLine("2. Listar usuarios del Realm A");
                    Console.WriteLine("3. Borrar usuarios del Realm A");
                    Console.WriteLine("4. Copiar usuarios del Realm A al Realm B");
                    Console.WriteLine("5. Listar usuarios del Realm B");
                    Console.WriteLine("6. Borrar usuarios del Realm B");
                    Console.WriteLine("7. Limpiar Pantalla");
                    Console.WriteLine("8. Cerrar la aplicación");

                    var option = Console.ReadLine();
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();

                    switch (option)
                    {
                        case "1":
                            CrearLoteUsuariosRealmA(1500);
                            break;
                        case "2":
                            ListarUsuariosRealmA();
                            break;
                        case "3":
                            BorrarUsuariosRealmA();
                            break;
                        case "4":
                            CopiarUsuariosRealmAToB();
                            break;
                        case "5":
                            ListarUsuariosRealmB();
                            break;
                        case "6":
                            BorrarUsuariosRealmB();
                            break;
                        case "7":
                            Console.Clear();
                            break;
                        case "8":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Opción no válida. Intente de nuevo.");
                            break;
                    }

                    Console.WriteLine("-----------------------------------------------\n");
                    stopwatch.Stop();
                    TimeSpan tiempoTranscurrido = stopwatch.Elapsed;
                    Console.WriteLine($"Tiempo transcurrido: {tiempoTranscurrido.Minutes} minutos, {tiempoTranscurrido.Seconds} segundos, {tiempoTranscurrido.Milliseconds} milisegundos");
                    Console.WriteLine("-----------------------------------------------\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CrearLoteUsuariosRealmA(int cantidadUsuarios)
        {
            HashSet<int> numerosGenerados = new HashSet<int>();
            Random random = new Random();

            int numeroAleatorio = 0;

            Console.WriteLine("Creando lote de usuarios en el Realm A...");

            for (int i = 1; i <= cantidadUsuarios; i++)
            {
                numeroAleatorio = random.Next(1, 1000000);

                if (!numerosGenerados.Contains(numeroAleatorio))
                {
                    var user = new User
                    {
                        username = "Usr_demo_" + numeroAleatorio.ToString(),
                        enabled = "true",
                        emailVerified = "true",
                        firstName = "Well",
                        lastName = "Well",
                        email = "Usr_demo_" + numeroAleatorio.ToString() + "@marvel.com"
                    };
                    var response = keycloakApi.CreateUser(KC_Origen, user);

                    if (response == "Unauthorized")
                    {
                        break;
                    }

                    numerosGenerados.Add(numeroAleatorio);
                }
                else
                {
                    i -= 1;
                }
            }

            Console.WriteLine("Se han creando {0} usuarios", numerosGenerados.Count.ToString());

            Console.WriteLine("Actividad finalizada\n");
        }

        private static List<User> ListarUsuariosRealmA()
        {
            int Contador = 0;
            List<User> ListadoUsuarios = new List<User>();

            var Listado = keycloakApi.GetUsers(KC_Origen);

            if (Listado.Count == 0)
            {
                return ListadoUsuarios;
            }

            Console.WriteLine("Listando usuarios del Realm A...");

            for (int i = 0; i <= 1000000; i++)
            {
                Listado = keycloakApi.GetUsers(KC_Origen);

                ListadoUsuarios.AddRange(Listado);

                Contador += Listado.Count;
                KC_Origen.first = Contador.ToString();

                if (Listado.Count == 0)
                {
                    break;
                }
            }

            Console.WriteLine("\n\nSe han encontrado [{0}] usuario(s).", Contador);

            Console.WriteLine("Actividad finalizada");

            return ListadoUsuarios;
        }

        private static void BorrarUsuariosRealmA()
        {
            int CantidadUsuariosBorrados = 0;

            var listadoUsuarios = ListarUsuariosRealmA();

            if (listadoUsuarios.Count ==0)
            {
                return;
            }

            Console.WriteLine("Borrando usuarios del Realm A...");

            while (listadoUsuarios.Count() > 0)
            {
                foreach (User user in listadoUsuarios)
                {
                    var response = keycloakApi.DeleteUser(KC_Origen, user);
                }

                listadoUsuarios = keycloakApi.GetUsers(KC_Origen);
                CantidadUsuariosBorrados += listadoUsuarios.Count();
            }

            Console.WriteLine("Se han borrado {0} usuarios", CantidadUsuariosBorrados.ToString());

            Console.WriteLine("Actividad finalizada\n");
        }

        private static void CopiarUsuariosRealmAToB()
        {
            int CantidadUsuariosCopiados = 0;

            var listaUsuarios = ListarUsuariosRealmA();

            if (listaUsuarios.Count == 0)
            {
                return;
            }

            Console.WriteLine("Copiando usuarios del Realm A al Realm B...");

            foreach (User user in listaUsuarios)
            {
                var response = keycloakApi.CreateUser(KC_Destino, user);

                CantidadUsuariosCopiados += 1;
            }

            Console.WriteLine("\nSe han creando {0} usuarios", CantidadUsuariosCopiados.ToString());

            Console.WriteLine("Actividad finalizada\n");
        }

        private static List<User> ListarUsuariosRealmB()
        {
            int Contador = 0;
            List<User> ListadoUsuarios = new List<User>();

            var Listado = keycloakApi.GetUsers(KC_Destino);

            if (Listado.Count == 0)
            {
                return ListadoUsuarios;
            }

            Console.WriteLine("Listando usuarios del Realm B...");

            for (int i = 0; i <= 1000000; i++)
            {
                Listado = keycloakApi.GetUsers(KC_Destino);

                ListadoUsuarios.AddRange(Listado);

                Contador += Listado.Count;
                KC_Origen.first = Contador.ToString();

                if (Listado.Count == 0)
                {
                    break;
                }
            }

            Console.WriteLine("\n\nSe han encontrado [{0}] usuario(s).", Contador);

            Console.WriteLine("Actividad finalizada");

            return ListadoUsuarios;
        }
        private static void BorrarUsuariosRealmB()
        {
            int CantidadUsuariosBorrados = 0;

            var listadoUsuarios = ListarUsuariosRealmB();

            if (listadoUsuarios.Count == 0)
            {
                return;
            }

            Console.WriteLine("Borrando usuarios del Realm B...");

            while (listadoUsuarios.Count() > 0)
            {
                foreach (User user in listadoUsuarios)
                {
                    var response = keycloakApi.DeleteUser(KC_Destino, user);
                }

                listadoUsuarios = keycloakApi.GetUsers(KC_Destino);
                CantidadUsuariosBorrados += listadoUsuarios.Count();
            }

            Console.WriteLine("Se han borrado {0} usuarios", CantidadUsuariosBorrados.ToString());

            Console.WriteLine("Actividad finalizada\n");
        }
    }
}
