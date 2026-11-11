# Microservicio: Pedidos.Api

**Carrera:** Desarrollo de Software  
**Materia:** Aplicaciones Distribuidas – Unidad 1 – Práctica 1  
**Paralelo:** 4BM (Cuarto "B" Matutino)  
**Tema:** Diseño de Arquitecturas Distribuidas – Comercio Multibodega  

---

## 1. Descripción
Microservicio RESTful responsable del procesamiento de pedidos de clientes, incluyendo el registro de clientes, la creación de pedidos con detalle de productos y el cálculo automático del total. Desarrollado con **ASP.NET Core Web API (.NET 10)** y **Entity Framework Core**.

## 2. Base de Datos
* **Motor:** SQL Server (`(localdb)\MSSQLLocalDB`)
* **Base de Datos:** `PedidosDB`
* **Credenciales:** `reactUser` / `reactUser`

### Tablas:
* `Cliente` (`IdCliente`, `Nombres`, `Cedula` único, `Correo`)
* `Pedido` (`IdPedido`, `IdCliente`, `Fecha`, `Total`, `Estado`)
  * `IdCliente`: Llave Foránea Física a `Cliente`.
  * Estados: `PENDIENTE`, `COMPLETADO`, `CANCELADO`.
* `DetallePedido` (`IdDetalle`, `IdPedido`, `IdProducto`, `Cantidad`, `PrecioUnitario`)
  * `IdPedido`: Llave Foránea Física a `Pedido`.
  * `IdProducto`: Referencia Lógica a `CatalogoDB.Producto`.

## 3. Endpoints Disponibles
La API expone documentación interactiva Swagger en: `https://localhost:7003/swagger`

### Clientes (`/api/cliente`)
* `GET /api/clientes`: Listar clientes.
* `GET /api/clientes/{id}`: Obtener cliente por ID.
* `POST /api/cliente`: Registrar cliente (valida cédula única y formato de correo).
* `PUT /api/clientes/{id}`: Actualizar cliente.
* `DELETE /api/clientes/{id}`: Eliminar cliente.

### Pedidos (`/api/pedido`)
* `GET /api/pedidos`: Listar pedidos.
* `GET /api/pedidos/{id}`: Obtener pedido con sus detalles.
* `GET /api/pedidos/cliente/{idCliente}`: Pedidos de un cliente específico.
* `POST /api/pedido`: Crear pedido con detalles (calcula `Total` automáticamente).
* `PUT /api/pedidos/{id}/estado?nuevoEstado=COMPLETADO`: Cambiar estado del pedido.
* `DELETE /api/pedidos/{id}`: Eliminar pedido y sus detalles.

## 4. Pruebas con Postman
Se incluye la colección en formato JSON: `DISTR-4BM-B1-AD01-PostmanCollection.json`.
Variables de entorno: `pedidosUrl = https://localhost:7003`.

