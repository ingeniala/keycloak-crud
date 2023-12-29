using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeycloakRestAPI
{
    public class InfoKeycloak
    {
        public string url = "";

        public string client_id = "";
        public string client_secret = "";

        public string grant_type = "client_credentials";
        public string token = "";
        public string realm = "";

        public string issuer = "";
        public string authorization_endpoint = "";
        public string token_endpoint = "";
        public string introspection_endpoint = "";
        public string userinfo_endpoint = "";
        public string end_session_endpoint = "";
        public string frontchannel_logout_session_supported = "";
        public string frontchannel_logout_supported = "";
        public string jwks_uri = "";
        public string check_session_iframe = "";

        public string first = "0";
        public string max = "200";

        /*
        API Keyclloak
        "issuer":"http://172.10.1.226:8081/realms/ingenia1",
        "authorization_endpoint":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/auth",
        "token_endpoint":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/token",
        "introspection_endpoint":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/token/introspect",
        "userinfo_endpoint":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/userinfo",
        "end_session_endpoint":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/logout",
        "frontchannel_logout_session_supported":true,
        "frontchannel_logout_supported":true,
        "jwks_uri":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/certs",
        "check_session_iframe":"http://172.10.1.226:8081/realms/ingenia1/protocol/openid-connect/login-status-iframe.html",
        */

        public InfoKeycloak(TipoKeycloak tipo)
        {
            switch (tipo)
            {
                case TipoKeycloak.Keycloak_Origen:
                    InfoKeycloak_Origen();
                    break;
                case TipoKeycloak.Keycloak_Destino:
                    InfoKeycloak_Destino();
                    break;
                default:
                    break;

            }
        }
        void InfoKeycloak_Origen()
        {
            this.realm = "ingenia1";
            this.url = "http://172.10.1.226:8081";
            this.client_id = "ingenia1";
            this.client_secret = "sMxdKvRfp8xNLlcHiV6yYK20qKYJ6I9c";

            issuer = url + "/realms/" + realm;
            authorization_endpoint = issuer + "/protocol/openid-connect/auth";
            token_endpoint = issuer + "/protocol/openid-connect/token";
            introspection_endpoint = issuer + "/protocol/openid-connect/token/introspect";
            userinfo_endpoint = issuer + "/protocol/openid-connect/userinfo";
            end_session_endpoint = issuer + "/protocol/openid-connect/logout";
            frontchannel_logout_session_supported = "true";
            frontchannel_logout_supported = "true";
            jwks_uri = issuer + "/protocol/openid-connect/certs";
            check_session_iframe = issuer + "/protocol/openid-connect/login-status-iframe.html";
        }
        void InfoKeycloak_Destino()
        {
            this.realm = "ingenia2";
            this.url = "http://172.10.1.226:8082";
            this.client_id = "ingenia2";
            this.client_secret = "Z9Jj0A5TCmZTiXYFAX5cLsO3J6hRmQnB";

            issuer = url + "/realms/" + realm;
            authorization_endpoint = issuer + "/protocol/openid-connect/auth";
            token_endpoint = issuer + "/protocol/openid-connect/token";
            introspection_endpoint = issuer + "/protocol/openid-connect/token/introspect";
            userinfo_endpoint = issuer + "/protocol/openid-connect/userinfo";
            end_session_endpoint = issuer + "/protocol/openid-connect/logout";
            frontchannel_logout_session_supported = "true";
            frontchannel_logout_supported = "true";
            jwks_uri = issuer + "/protocol/openid-connect/certs";
            check_session_iframe = issuer + "/protocol/openid-connect/login-status-iframe.html";
        }
    }

    public class AccessTokenObjet
    {
        public string access_token = "";
        public string expires_in = "";
        public string refresh_expires_in = "";
        public string token_type = "";

        [JsonProperty("not-before-policy")]
        public string NotBeforePolicy = "";
        // public string scope  = "";
    }

    public class User
    {
        public string id = "";
        public string username = "";
        public string enabled = "";
        public string emailVerified = "";
        public string firstName = "";
        public string lastName = "";
        public string email = "";
        public string createdTimestamp = "";
        
    }
    public enum TipoKeycloak
    {
        Keycloak_Origen = 1,
        Keycloak_Destino = 2
    }
    public class KeycloakResponseObjets
    {
        public AccessTokenObjet GetToken(InfoKeycloak objKeycloak)
        {
            string url = "";

            //Generando el token
            //POST
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();

                var parameters = new Dictionary<string, string>
                {
                    { "client_id", objKeycloak.client_id},
                    { "client_secret", objKeycloak.client_secret},
                    { "grant_type", objKeycloak.grant_type}
                };

                var HttpContet = new FormUrlEncodedContent(parameters);

                //string jsonstring = JObject.Parse(parametros).ToString();
                //var HttpContet = new StringContent(jsonstring, Encoding.UTF8, "application/x-www-form-urlencoded");
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authorization", "adasdasd");

                var response = client.PostAsync(objKeycloak.token_endpoint, HttpContet).Result;

                var res = response.Content.ReadAsStringAsync().Result;

                dynamic r = JObject.Parse(res);

                AccessTokenObjet _Token = JsonConvert.DeserializeObject<AccessTokenObjet>(res);

                //Console.WriteLine(r);
                //Console.WriteLine(JsonConvert.SerializeObject(parametros));
                //Console.WriteLine(r);

                return _Token;
            }
        }

        public List<User> GetUsers(InfoKeycloak objKeycloak)
        {
            string url = "";
            //Generando el token
            //POST
            using (var client = new HttpClient())
            {
                url = objKeycloak.url + "/admin/realms/" + objKeycloak.realm + "/users?first=" + objKeycloak.first + "&max=" + objKeycloak.max;

                client.DefaultRequestHeaders.Clear();

                var BearerToken = "Bearer " + objKeycloak.token;
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objKeycloak.token);
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                //string jsonstring = JObject.Parse(parametros).ToString();
                //var HttpContet = new StringContent(jsonstring, Encoding.UTF8, "application/x-www-form-urlencoded");
                //client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Authorization", "adasdasd");

                var response = client.GetAsync(url).Result;
                var res = response.Content.ReadAsStringAsync().Result;

                var UserList = JsonConvert.DeserializeObject<List<User>>(res);

                return UserList;
            }
        }

        public string CreateUser(InfoKeycloak objKeycloak, User usr)
        {
            string url = "";
            //Generando el token
            //POST
            using (var client = new HttpClient())
            {
                url = objKeycloak.url + "/admin/realms/" + objKeycloak.realm + "/users";
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objKeycloak.token);

                string jsonstring = JsonConvert.SerializeObject(usr);
                var HttpContet = new StringContent(jsonstring, Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, HttpContet).Result;

                var res = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "Created")
                {
                    JObject responseObject = JObject.Parse(res);
                    Console.WriteLine("el usuario [{0}] con el correo [{1}] no se pudo crear: => {2}", usr.username, usr.email, response.StatusCode.ToString());
                }

                return response.StatusCode.ToString();
            }
        }
        public string DeleteUser(InfoKeycloak objKeycloak, User usr)
        {
            string url = "";
            //Generando el token
            //POST
            using (var client = new HttpClient())
            {
                url = objKeycloak.url + "/admin/realms/" + objKeycloak.realm + "/users/" + usr.id;
                client.DefaultRequestHeaders.Clear();

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + objKeycloak.token);

                string jsonstring = JsonConvert.SerializeObject(usr);
                var HttpContet = new StringContent(jsonstring, Encoding.UTF8, "application/json");

                var response = client.DeleteAsync(url).Result;

                var res = response.Content.ReadAsStringAsync().Result;

                if (response.StatusCode.ToString() != "NoContent")
                {
                    JObject responseObject = JObject.Parse(res);
                    Console.WriteLine("el usuario [{0}] con el correo [{1}] no se pudo borrar: => {2}", usr.username, usr.email, response.StatusCode.ToString());
                }

                return response.StatusCode.ToString();
            }
        }
    }
}
