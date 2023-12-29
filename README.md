# Generales
Esto es un programa diseñado para migrar usuarios desde una instania de keycloak hacia 
otra utilizando las API de la herramienta.

Este programa no migra contraseñas, por lo que el usuario deberá recuperar su contraseña por lo medios correspondientes.

# Pre-requisitos

* Instalar visual studio code
* Instalar el .NET Framework 6
* C# Dev Kit (desde el market place)
* Seleccionar un directorio donde descargar el proyecto y dentro del directorio, 
usando la línea de comando, ejecutar: 
git clone git@gitlab.com:ingeniaca/demos/migracion-usuario-keycloaka-a-keycloakb.git

# Ejecución
Dentro del archivo KeycloakResponseObjets.cs, se encuentra una clase que contiene
las variables que nos permitirán conectarnos al keycloak. Solo debes colocar el 
nombre del realm, el url, client_id y cliente_secret. Con estos cuatro parámetros 
logramos conectarnos a keycloak.

    void InfoKeycloak_Origen()
    {
        this.realm = "ingenia1";
        this.url = "http://172.10.1.226:8081";
        this.client_id = "ingenia1";
        this.client_secret = "sMxdKvRfp8xNLlcHiV6yYK20qKYJ6I9c";
        .....
    }

    void InfoKeycloak_Destino()
    {
        this.realm = "ingenia2";
        this.url = "http://172.10.1.226:8082";
        this.client_id = "ingenia2";
        this.client_secret = "Z9Jj0A5TCmZTiXYFAX5cLsO3J6hRmQnB";
        .....
    }

## Windows

Puedes abrir el proyecto en visual studio code o visual studio y lo ejecutas.

## Ubuntu

* docker pull bitnami/dotnet-sdk:6.0.417-debian-11-r2
* docker images
* Toma el id de la imagen
* docker run -v /home/CARPETA_PROYECTO:/app -it ID_IMAGEN_DOCKER
* Puedes usar una ruta relavita: docker run -v .:/app -it ID_IMAGEN_DOCKER

