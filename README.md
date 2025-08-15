# PROYECTO UNIVERSITARIO
### Gestión de Departamentos

Esta aplicación de consola en C# es un sistema de gestión para una universidad o institución educativa. Está diseñada para manejar información relacionada con departamentos, materias y estudiantes. El programa utiliza listas enlazadas para almacenar y gestionar los datos de manera dinámica, lo que permite agregar, modificar y eliminar registros de forma eficiente.

El sistema está estructurado en tres clases principales: `Departamento`, `Materia` y `Estudiante`, cada una representando una entidad específica. Estas clases son gestionadas por sus correspondientes clases de listas enlazadas: `ListaDepartamentos`, `ListaMaterias` y `ListaEstudiantes`.

El programa ofrece un menú principal con cuatro secciones:

* **Gestión de Departamentos:** Esta sección permite a los usuarios realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) en los departamentos.
    * Agregar un nuevo departamento con un nombre, un código único de tres dígitos y una descripción.
    * Modificar el nombre y la descripción de un departamento existente utilizando su código.
    * Eliminar un departamento usando su código.
* **Gestión de Materias:** Esta sección se enfoca en la gestión de las materias.
    * Agregar una nueva materia, vinculándola a un departamento existente a través del código de este. A la materia se le asigna un código único que combina el código del departamento con un código de materia de cuatro dígitos.
    * Modificar el nombre y la descripción de una materia.
    * Eliminar una materia usando su código.
* **Gestión de Estudiantes:** Esta sección se encarga de los datos relacionados con los estudiantes.
    * Agregar un nuevo estudiante, vinculándolo a una materia a través de su código. Los detalles del estudiante incluyen nombre, apellido, cédula, sección y período.
    * Modificar la nota de un estudiante en una materia específica.
    * Eliminar un estudiante utilizando su cédula.
* **Reportes:** Esta sección ofrece diversas funcionalidades de reportes para analizar los datos almacenados.
    * Listar todos los estudiantes inscritos en una materia específica durante un período académico determinado.
    * Identificar la materia con el mayor número de estudiantes inscritos en un período dado.
    * Listar las materias que tienen una cantidad de créditos específica (basada en el último dígito del código de la materia).
    * Listar todas las materias de un departamento específico en un semestre determinado (basado en el penúltimo dígito del código de la materia).
    * Encontrar la materia con el promedio de notas más alto para un período dado.
    * Calcular y mostrar el promedio de notas general de un estudiante.
    * Identificar el departamento con el mayor número de estudiantes inscritos.

El programa incluye datos precargados de departamentos, materias y estudiantes para demostrar su funcionalidad inmediatamente después de la ejecución. También incorpora validación de entrada.
