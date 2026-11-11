# Microservicio: Inventario.Api

**Carrera:** Desarrollo de Software  
**Materia:** Aplicaciones Distribuidas – Unidad 1 – Práctica 1  
**Paralelo:** 4BM (Cuarto "B" Matutino)  
**Tema:** Diseño de Arquitecturas Distribuidas – Comercio Multibodega  

---

## 1. Descripción
Microservicio RESTful responsable de gestionar el almacenamiento físico, el control de stock, movimientos de inventario y reservas de mercancías a través de múltiples bodegas, desarrollado con **ASP.NET Core Web API (.NET 10)** y **Entity Framework Core**.

## 2. Base de Datos
* **Motor:** SQL Server (`(localdb)\MSSQLLocalDB`)
* **Base de Datos:** `InventarioDB`
* **Credenciales:** `reactUser` / `reactUser`

### Tablas:
* `Bodega` (`IdBodega`, `Nombre`, `Ciudad`, `Direccion`)
* `Existencia` (`IdExistencia`, `IdProducto`, `IdBodega`, `Stock`)
  * `IdBodega`: Llave Foránea Física a `Bodega`.
  * `IdProducto`: Referencia Lógica a `CatalogoDB.Producto`.
* `MovimientoInventario` (`IdMovimiento`, `IdProducto`, `IdBodega`, `Tipo`, `Cantidad`, `Fecha`)
  * Tipos: `ENTRADA`, `SALIDA`.
  * Actualización y validación automática de existencias.
* `ReservaStock` (`IdReserva`, `IdProducto`, `IdBodega`, `Cantidad`, `Estado`)
  * Estados: `PENDIENTE`, `CONFIRMADA`, `CANCELADA`.

## 3. Endpoints Disponibles
La API expone documentación interactiva Swagger en: `http://localhost:5226/swagger`

### Bodegas (`/api/bodegas`)
* `GET /api/bodegas`: Listar bodegas.
* `GET /api/bodegas/{id}`: Obtener bodega por ID.
* `POST /api/bodegas`: Registrar bodega.
* `PUT /api/bodegas/{id}`: Modificar bodega.
* `DELETE /api/bodegas/{id}`: Eliminar bodega.

### Existencias (`/api/existencias`)
* `GET /api/existencias`: Listar todas las existencias.
* `GET /api/existencias/{id}`: Obtener existencia por ID.
* `GET /api/existencias/producto/{idProducto}`: Consultar stock de un producto en todas las bodegas.
* `GET /api/existencias/bodega/{idBodega}`: Consultar existencias de una bodega específica.
* `POST /api/existencias`: Registrar existencia inicial.

### Movimientos (`/api/movimientos`)
* `GET /api/movimientos`: Historial general de movimientos.
* `GET /api/movimientos/producto/{idProducto}`: Movimientos por producto.
* `POST /api/movimientos`: Registrar entrada o salida (actualiza stock automáticamente y rechaza salidas sin stock suficiente con HTTP 400).

### Reservas (`/api/reservas`)
* `GET /api/reservas`: Listar reservas.
* `POST /api/reservas`: Crear reserva de stock.
* `PUT /api/reservas/{id}/estado?nuevoEstado=CONFIRMADA`: Actualizar estado de la reserva.
* `DELETE /api/reservas/{id}`: Eliminar reserva.

## 4. Pruebas con Postman
Se incluye la colección en formato JSON: `Inventario_Api.postman_collection.json`.
Variable de entorno base: `baseUrl = http://localhost:5226`.

