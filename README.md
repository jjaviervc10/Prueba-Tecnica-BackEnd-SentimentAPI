
README – SentimentAPI


SentimentAPI es una API REST desarrollada en ASP.NET Core que permite almacenar
comentarios de usuarios y realizar un análisis básico de sentimiento sobre el
texto del comentario (positivo, negativo o neutral).

El proyecto fue desarrollado como parte de una prueba técnica backend, siguiendo
buenas prácticas de arquitectura, Dockerización, manejo de errores y pruebas.

--------------------------------------------------
Tecnologías utilizadas
--------------------------------------------------

- ASP.NET Core (.NET 9)
- Minimal APIs
- SQL Server 2022
- Microsoft.Data.SqlClient
- xUnit (pruebas unitarias)
- Docker
- Docker Compose
- Postman

--------------------------------------------------
Clonar el repositorio
--------------------------------------------------

1. Clonar el repositorio:

   git clone https://github.com/USUARIO/SentimentAPI.git
   cd SentimentAPI

--------------------------------------------------
Ejecución con Docker (recomendado)
--------------------------------------------------

La aplicación puede levantarse junto con SQL Server usando Docker Compose.

1. Construir y levantar los contenedores:

   docker-compose up --build

2. La API estará disponible en:

   http://localhost:5055

3. SQL Server estará disponible en:

   localhost:1433
   Usuario: sa
   Password: SqlServer@2025

4. Para detener los contenedores:

   docker-compose down

--------------------------------------------------
Ejecución sin Docker (opcional)
--------------------------------------------------

1. Asegurarse de tener .NET 9 instalado.
2. Configurar una instancia de SQL Server local.
3. Ajustar la cadena de conexión en appsettings.json.
4. Ejecutar:

   dotnet restore
   dotnet run

--------------------------------------------------
Endpoints de la API
--------------------------------------------------

1. Verificación de estado

   GET /

   Respuesta:
   "SentimentAPI running OK"

--------------------------------------------------

2. Crear comentario

   POST /api/comments

   Body (JSON):
   {
     "productId": "P001",
     "userId": "U123",
     "commentText": "El producto es excelente"
   }

   Respuesta (201 Created):
   {
     "id": 1,
     "sentiment": "positivo"
   }

--------------------------------------------------

3. Obtener comentarios

   GET /api/comments

   Parámetros opcionales:
   - productId
   - sentiment

   Ejemplo:
   GET /api/comments?productId=P001&sentiment=positivo

--------------------------------------------------

4. Resumen de sentimientos

   GET /api/sentiment-summary

   Respuesta:
   {
     "total_comments": 10,
     "sentiment_counts": {
       "positivo": 5,
       "negativo": 3,
       "neutral": 2
     }
   }

--------------------------------------------------
Esquema de la base de datos
--------------------------------------------------

Base de datos: SentimentDb

Tabla: Comments

- Id (INT, PK, Identity)
- ProductId (NVARCHAR(50), NOT NULL)
- UserId (NVARCHAR(50), NOT NULL)
- CommentText (NVARCHAR(MAX), NOT NULL)
- Sentiment (NVARCHAR(20), NOT NULL)
- CreatedAt (DATETIME, NOT NULL)

--------------------------------------------------
Pruebas
--------------------------------------------------

El proyecto incluye un proyecto de pruebas independiente:

- SentimentAPI.Tests

Tipos de pruebas implementadas:
- Pruebas unitarias para la lógica de análisis de sentimiento.
- Pruebas de integración para validar el flujo POST y GET contra la base de datos.

Ejecutar pruebas:

   dotnet test

--------------------------------------------------
Decisiones de diseño
--------------------------------------------------

- Se utilizó Minimal API para mantener el código simple y enfocado.
- El análisis de sentimiento se implementó como un servicio independiente.
- La lógica de acceso a datos se separó en un repositorio.
- Se implementó un middleware global para manejo centralizado de errores.
- Docker Compose se utiliza para facilitar el levantamiento del entorno completo.
- Swagger UI no se incluyó intencionalmente para cumplir con los requerimientos.

--------------------------------------------------
Notas finales
--------------------------------------------------

Este proyecto fue desarrollado con enfoque en claridad, mantenibilidad y buenas
prácticas backend, priorizando la correcta separación de responsabilidades y
la facilidad de despliegue.