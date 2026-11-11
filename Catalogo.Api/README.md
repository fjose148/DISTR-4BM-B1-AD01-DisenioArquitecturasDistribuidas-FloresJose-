# Microservicio: Catalogo.Api

**Carrera:** Desarrollo de Software  
**Materia:** Aplicaciones Distribuidas – Unidad 1 – Práctica 1  
**Paralelo:** 4BM (Cuarto "B" Matutino)  
**Tema:** Diseño de Arquitecturas Distribuidas – Comercio Multibodega  

---

## 1. Descripción
Microservicio RESTful responsable de la administración de la información comercial de los productos, incluyendo categorías y marcas, desarrollado con **ASP.NET Core Web API (.NET 10)** y **Entity Framework Core**.

## 2. Base de Datos
* **Motor:** SQL Server (`(localdb)\MSSQLLocalDB`)
* **Base de Datos:** `CatalogoDB`
* **Credenciales:** `reactUser` / `reactUser`

### Tablas:
* `Categoria` (`IdCategoria`, `Nombre`, `Descripcion`)
* `Marca` (`IdMarca`, `Nombre`, `PaisOrigen`)
* `Producto` (`IdProducto`, `IdCategoria`, `IdMarca`, `Nombre`, `SKU`, `Precio`, `Estado`)
  * Llaves foráneas físicas a `Categoria` y `Marca`.
  * Restricción de `SKU` único y `Precio >= 0`.

## 3. Endpoints Disponibles
La API expone documentación interactiva Swagger en: `http://localhost:5292/swagger`

### Categorías (`/api/categorias`)
* `GET /api/categorias`: Listar todas las categorías.
* `GET /api/categorias/{id}`: Obtener categoría por ID.
* `POST /api/categorias`: Registrar una nueva categoría.
* `PUT /api/categorias/{id}`: Actualizar una categoría existente.
* `DELETE /api/categorias/{id}`: Eliminar una categoría.

### Marcas (`/api/marcas`)
* `GET /api/marcas`: Listar todas las marcas.
* `GET /api/marcas/{id}`: Obtener marca por ID.
* `POST /api/marcas`: Registrar una nueva marca.
* `PUT /api/marcas/{id}`: Actualizar una marca existente.
* `DELETE /api/marcas/{id}`: Eliminar una marca.

### Productos (`/api/productos`)
* `GET /api/productos`: Listar todos los productos.
* `GET /api/productos/{id}`: Obtener producto por ID.
* `POST /api/productos`: Registrar un nuevo producto con validación de existencia de marca y categoría, y SKU único.
* `PUT /api/productos/{id}`: Actualizar producto.
* `DELETE /api/productos/{id}`: Eliminar producto.

## 4. Pruebas con Postman
Se incluye la colección en formato JSON: `Catalogo_Api.postman_collection.json`.
Variable de entorno base: `baseUrl = http://localhost:5292`.

