# CoWork API

Una API en .NET 9.0 para gestionar reservas de espacios de coworking. Este sistema permite a los usuarios registrarse, iniciar sesión, gestionar salas y manejar reservas en un entorno de trabajo compartido.

## 🛠 Stack Tecnológico

- **.NET 9.0**
- **Entity Framework Core** - ORM para operaciones con base de datos
- **MediatR** - Para implementar el patrón CQRS
- **JWT Authentication** - Para acceso seguro a la API
- **SQLite** - Base de datos
- **xUnit** - Framework de pruebas

## Resumen
El proyecto incluye:
- Pruebas unitarias
- Pruebas de integración
- Pruebas con base de datos en memoria

## 📝 Registro de Actividad

El sistema incluye registro completo a través de ILogService:
- Método HTTP
- Endpoint
- Código de estado
- ID de usuario (cuando está autenticado)

## ⚡ Consideraciones de Rendimiento

- Utiliza patrones async/await para operaciones no bloqueantes
- Implementa CQRS para mejor separación de operaciones de lectura/escritura
- Usa Entity Framework con indexación adecuada

## 🔒 Características de Seguridad

- Encriptación de contraseñas usando BCrypt
- Autenticación mediante tokens JWT
- Autorización basada en roles
- Validación y sanitización de entradas

## 🚨 Manejo de Errores

La API implementa un manejo consistente de errores con los siguientes códigos de estado:

- 200: Operación exitosa
- 201: Recurso creado exitosamente
- 400: Solicitud incorrecta
- 401: No autorizado
- 403: Prohibido
- 404: Recurso no encontrado
- 409: Conflicto (por ejemplo, en reservas superpuestas)

## 🚀 Comenzando

#### Prerequisitos

- SDK de .NET 9.0
- Un IDE (Visual Studio, VS Code o similar)

#### Instalación

1. Clonar el repositorio

```bash
git clone https://github.com/aicemaria112/coworkingCS.git
```

#### 2. Navegar al directorio del proyecto
```bash
cd coworkingCS/CoWorkApi #for api implementation
cd coworkingCS/CoWorkApi.Tests #for api tests
```

#### 3. Instalar dependencias

```bash
dotnet restore
```

#### 4 Correr las migraciones

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
##### Note:
```bash
 #En Algunas configuraciones según la version de .NET instalada y las dependencias donet-ef puede no estar instalado para resolver el problema ejecutar:
 dotnet tool install --global dotnet-ef
```

#### 5. Correr el proyecto

```bash
dotnet run
```

La API estará disponible en `http://localhost:5259`

## 🏗 Arquitectura

Este proyecto sigue los principios de Arquitectura Limpia con el patrón CQRS:

- **Controladores**: Manejan las peticiones y respuestas HTTP
- **Commands/Queries**: Implementan el patrón CQRS usando MediatR
- **Domain**: Contiene las entidades y lógica de negocio
- **Infrastructure**: Contexto de base de datos e implementaciones de servicios externos

## 📡 Endpoints de la API

### Autenticación

#### Registrar Usuario

```bash
curl --request POST \
  --url http://127.0.0.1:5259/api/auth/register \
  --header 'Content-Type: application/json' \
  --data '{
  "username": "testuser2",
  "password": "password123",
  "email": "testuser2@example.com",
	"name" : "Usuairo de Prueba"
}
'
```
#### Iniciar Sesión

```bash
curl --request POST \
  --url http://127.0.0.1:5259/api/auth/login \
  --header 'Content-Type: application/json' \
  --data '{
  "username": "testuser2",
  "password": "password123"
}
```

### Salas

#### Obtener Salas Disponibles

```bash
curl --request GET \
  --url 'http://localhost:5259/api/rooms/available?minimumCapacity=78&location=efgv&nameContains=w4dfsf'
```

#### Crear Sala (Solo Admin)

```bash
# POST /api/rooms
curl --request POST \
  --url http://localhost:5259/api/rooms \
  --header 'Content-Type: application/json' \
  --data '{
  "name": "Sala de Prueba",
  "capacity": 10,
  "location": "Ciudad de México",
  "description": "Sala de prueba para pruebas",
  "isAvailable": true
}'
```
### Reservas

#### Crear Reserva

```bash
curl --request POST \
  --url 'http:///127.0.0.1:5259/api/reservations?d='\''%3Cscript%3E'\''' \
  --header 'Authorization: Bearer {Token} \
  --header 'Content-Type: application/json' \
  --data '{
	"roomId":1,
	"Quantity":5,
	"startTime":"2024-01-19T11:00:00",
	"endTime":"2024-01-25T15:00:00"
}'
```


#### Obtener Reservas del Usuario

```bash
curl --request GET \
  --url http://localhost:5259/api/reservations \
  --header 'Authorization: Bearer {Token}' \
  --header 'Content-Type: application/x-www-form-urlencoded'
```

#### Actualizar Reserva
```bash
curl --request PUT \
  --url http:///127.0.0.1:5259/api/reservations/1 \
  --header 'Authorization: Bearer {Token}' \
  --header 'Content-Type: application/json' \
  --data '{
	"roomId":1,
	"Quantity":5,
	"startTime":"2024-01-10T11:00:00",
	"endTime":"2024-01-15T15:00:00"
}'
```

#### Eliminar Reserva
```bash
curl --request DELETE \
  --url http:///127.0.0.1:5259/api/reservations/1 \
  --header 'Authorization: Bearer {Token}'
```


## Swagger UI para entorno de desarrollo

```bash
http://localhost:5259/swagger/index.html
```


## 🔐 Autenticación

La API utiliza tokens JWT para la autenticación. Incluir el token en el encabezado Authorization:

```bash
Authorization: Bearer {Token}
```
## 👥 Roles de Usuario

- **Usuario**: Puede realizar y gestionar sus propias reservas
- **Admin**: Puede gestionar salas, ver todas las reservas y modificar roles de usuario

## 🧪 Pruebas

Ejecutar las pruebas usando:

```bash
dotnet test
```



Este README proporciona una visión completa del sistema en español, manteniendo un formato limpio y profesional. Puedes personalizar secciones específicas según características o requisitos adicionales de tu implementación.