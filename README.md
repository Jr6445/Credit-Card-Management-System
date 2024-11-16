# **Credit Card Management System**

Este repositorio contiene la solución completa para el proyecto de gestión de tarjetas de crédito, que incluye el código fuente del frontend y backend, así como los scripts de base de datos necesarios para configurar la aplicación.

## **Índice**

- [Descripción](#descripción)
- [Arquitectura](#arquitectura)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Instalación](#instalación)
- [Scripts de Base de Datos](#scripts-de-base-de-datos)
- [Probar la Aplicación](#probar-la-aplicación)
- [Autor](#autor)

---

## **Descripción**

El sistema permite a los usuarios:
- Consultar el estado de cuenta de una tarjeta de crédito, mostrando información como límite de crédito, saldo total, interés bonificable, y más.
- Registrar nuevas transacciones (compras y pagos) asociadas a una tarjeta.
- Visualizar el historial de transacciones mensuales.
- Exportar el estado de cuenta en formato PDF.

Incluye cálculos automáticos como:
- Interés bonificable basado en un porcentaje configurable.
- Cuota mínima a pagar.
- Total con intereses.

---

## **Arquitectura**

La solución se divide en los siguientes componentes:

1. **CreditCardAPI**: 
   - Backend REST API desarrollado con **ASP.NET Core**.
   - Controladores, servicios, y repositorios para interactuar con la base de datos.
   - Documentación de API con Swagger.
   - Manejo de validaciones y excepciones globales.

2. **CreditCardMVC**:
   - Frontend desarrollado con **ASP.NET MVC** utilizando **Razor Views**.
   - Interfaz de usuario estilizada con **Bootstrap**.
   - Integración con la API para consumir servicios y manejar formularios.

---

## **Tecnologías Utilizadas**

- **Lenguaje**: C#
- **Frameworks**:
  - ASP.NET Core (.NET 6)
  - ASP.NET MVC
- **Base de Datos**: SQL Server
- **Librerías y Herramientas**:
  - AutoMapper
  - FluentValidation
  - Swagger (Documentación de API)
  - DinkToPdf (Generación de PDF)
  - Bootstrap (Estilo de la interfaz)
  - MediatR (Implementación de CQRS)
- **Herramientas**:
  - Visual Studio
  - Postman

---

## **Instalación**

1. **Clonar el Repositorio**:
   ```bash
   git clone https://github.com/tu-usuario/credit-card-management.git
2. **Ejecutar los scripts del archivo scriptDatabase.sql **:
3. **Ve a CreditCardAPI/appsettings.json y actualiza la cadena de conexión**:
      ```bash
   "ConnectionStrings": { "DefaultConnection": "Data Source=TU_SERVIDOR;Initial Catalog=CreditCardAccount;User ID=TU_USUARIO;Password=TU_PASSWORD;"}


