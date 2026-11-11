# INSTITUTO SUPERIOR TECNOLÓGICO CORDILLERA
## CARRERA DE DESARROLLO DE SOFTWARE
### APLICACIONES DISTRIBUIDAS – UNIDAD 1 – PRÁCTICA 1
**Paralelo:** 4BM (Cuarto "B" Matutino)  
**Actividad:** DISTR-4BM-B1-AD01 - Diseño de Arquitecturas Distribuidas (Comercio Multibodega)  
**Estudiante:** Flores Rodriguez Jose Mauricio  
**Fecha:** Noviembre 2026  

---

# PLANTILLA DE DECLARACIÓN DE USO DE INTELIGENCIA ARTIFICIAL (IA)

En concordancia con las políticas éticas y académicas institucionales respecto al uso de herramientas basadas en Inteligencia Artificial Generativa (IAG), declaro lo siguiente:

### 1. Herramientas de IA Utilizadas
* **Nombre de la herramienta:** Antigravity / Gemini 3.8
* **Propósito del uso:** Asistencia en la generación de código base siguiendo la plantilla arquitectónica del proyecto de clase (`LibroB`), estructuración de modelos con Data Annotations, creación de controladores RESTful con validaciones de negocio, y diseño de la colección de pruebas en formato Postman v2.1.

### 2. Áreas y Fases donde se aplicó Asistencia de IA
* [x] **Diseño de Modelos y Mapeo:** Generación de clases de entidad (`Categoria`, `Marca`, `Producto`, `Bodega`, `Existencia`, `MovimientoInventario`, `ReservaStock`, `Cliente`, `Pedido`, `DetallePedido`) acordes a los esquemas SQL provistos.
* [x] **Configuración de Dependencias y Pipeline:** Configuración de paquetes NuGet en los tres microservicios (`Microsoft.EntityFrameworkCore.SqlServer`, `Swashbuckle.AspNetCore`, etc.) y registro en `Program.cs`.
* [x] **Controladores y Lógica de Negocio:** Implementación de controladores con verificación de reglas (unicidad de SKU y cédula, validación de stock y actualización de existencias, cálculo automático del total de pedidos).
* [x] **Verificación y Pruebas:** Ejecución de pruebas unitarias/de integración automatizadas y generación de la colección unificada de Postman con casos de éxito (`200`, `201`) y error (`400`, `404`).

### 3. Declaración de Autoría y Comprensión Intelectual
Declaro bajo mi responsabilidad que:
1. Comprendo la arquitectura distribuida desarrollada y la separación de dominios de negocio entre `Catalogo.Api`, `Inventario.Api` y `Pedidos.Api`.
2. Entiendo y puedo justificar técnicamente la diferencia entre las **llaves foráneas físicas** (dentro del mismo microservicio y base de datos) y las **llaves foráneas lógicas** (referencias cruzadas entre microservicios, como `IdProducto` en `Existencia` o en `DetallePedido`).
3. He revisado, validado y probado personalmente cada endpoint de los tres microservicios en Visual Studio y Postman, garantizando su correcto funcionamiento.

---

**Firma del Estudiante:**  
_____________________________________  
**Nombre:** Flores Rodriguez Jose Mauricio  
**C.I.:** ___________________________

