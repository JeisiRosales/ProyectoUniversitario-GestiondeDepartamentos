using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

//Clases
public class Departamento
{
    public string Nombre;
    public string Codigo;
    public string Descripcion;

    public Departamento(string nombre, string codigo, string descripcion)
    {
        Nombre = nombre;
        Codigo = codigo;
        Descripcion = descripcion;
    }
}
public class Materia
{
    public string Departamento;
    public string Nombre;
    public string Codigo;
    public string Descripcion;

    public Materia(string departamento, string nombre, string codigo, string descripcion)
    {
        Departamento = departamento;
        Codigo = codigo;
        Nombre = nombre;
        Descripcion = descripcion;
    }
}
public class Estudiante
{
    public string Codigo;
    public string Nombre;
    public string Apellido;
    public string Cedula;
    public string Seccion;
    public string Periodo;
    public double Nota;

    public Estudiante(string codigo, string nombre, string apellido, string cedula, string seccion, string periodo, double nota)
    {
        Codigo = codigo;
        Nombre = nombre;
        Apellido = apellido;
        Cedula = cedula;
        Seccion = seccion;
        Periodo = periodo;
        Nota = nota;
    }
}
//Nodos
public class NodoEst
{
    public Estudiante Datos;
    public NodoEst Siguiente;

    public NodoEst(Estudiante datos)
    {
        Datos = datos;
        Siguiente = null!;
    }
}
public class NodoMat
{
    public Materia Datos;
    public NodoMat Siguiente;
    public NodoEst Siguiente_D;

    public NodoMat(Materia datos)
    {
        Datos = datos;
        Siguiente = null!;
        Siguiente_D = null!;
    }
}
public class NodoDepa
{
    public Departamento Datos;
    public NodoDepa Siguiente;
    public NodoDepa Anterior;
    public NodoMat Siguiente_D;

    public NodoDepa(Departamento datos)
    {
        Datos = datos;
        Siguiente = null!;
        Anterior = null!;
        Siguiente_D = null!;
    }
}
//Listas y funciones
public class ListaEstudiantes
{
    public NodoEst Cabeza;
    public ListaEstudiantes()
    {
        Cabeza = null!;
    }
    public void agregarEstudiante(Estudiante estudiante, ListaMaterias materias)
    {
        NodoEst nuevoNodo = new NodoEst(estudiante);
        NodoMat materia = materias.Cabeza;
        bool materiaEncontrada = false;

        while (materia != null)
        {
            if (estudiante.Codigo == materia.Datos.Codigo)
            {
                materiaEncontrada = true;
                if (Cabeza == null)
                {
                    Cabeza = nuevoNodo;
                }
                else
                {
                    NodoEst actual = Cabeza;
                    while (actual.Siguiente != null)
                    {
                        actual = actual.Siguiente;
                    }
                    actual.Siguiente = nuevoNodo;
                }
                Console.WriteLine("Estudiante agregado exitosamente.");
                
                return; // Salir del método una vez que se ha agregado el estudiante
            }
            materia = materia.Siguiente;
        }

        if (!materiaEncontrada)
        {
            Console.WriteLine("ERROR: La materia asociada al código del estudiante no existe.");
        }
    }
    public void EliminarEstudiante(string Cedula, ListaEstudiantes estudiantes)
    {
        if (Cabeza == null) return;
        NodoEst actual = Cabeza;
        bool encontrado = false;
        while(actual != null)
        {
            if(actual.Datos.Cedula == Cedula)
            {
                encontrado = true;
                if (Cabeza.Datos.Cedula == Cedula)
                {
                    Cabeza = Cabeza.Siguiente;
                    Console.WriteLine("Estudiante eliminado con exito.");
                    return;
                }

                NodoEst actual2 = Cabeza;
                while (actual2.Siguiente != null)
                {
                    if (actual2.Siguiente.Datos.Cedula == Cedula)
                    {
                        actual2.Siguiente = actual2.Siguiente.Siguiente;
                        Console.WriteLine("Estudiante eliminado con exito.");
                        return;
                    }
                    actual2 = actual2.Siguiente;
                }
            }       
            actual = actual.Siguiente;
        }
        if(!encontrado)
        {
            Console.WriteLine("ERROR: La cedula del estudiante no ha sido encontrado.");
        }
    }
    public static bool ValidarEntrada(string entrada)
    {
        string patron = @"^(I|II|III)-\d{4}$";
        Regex regex = new Regex(patron);
        return regex.IsMatch(entrada);
    }
    public void ModificarEstudiante(string Cedula,ListaEstudiantes estudiantes, ListaMaterias materias)
    {
        NodoEst estudiante = estudiantes.Cabeza;
        bool encontrado = false;
        while(estudiante != null)
        {
            if(estudiante.Datos.Cedula == Cedula)
            {
                encontrado = true;
                double nuevaNota;
                while (true)
                {
                    NodoMat materia = materias.Cabeza;
                    while(materia != null)
                    {
                        if(estudiante.Datos.Codigo == materia.Datos.Codigo)
                        {
                            Console.Write($"Ingrese la nueva nota del estudiante en la materia '{materia.Datos.Nombre}': ");
                        }
                        materia = materia.Siguiente;
                    }
                    string input = Console.ReadLine() ?? "";
                    if (!double.TryParse(input, out nuevaNota))
                    {
                        Console.WriteLine("ERROR: Por favor ingrese un valor numérico válido para la nota.");
                    }
                    else if (nuevaNota > 10 || nuevaNota < 0)
                    {
                        Console.WriteLine("ERROR: La nota debe ser entre 0 y 10");
                    }
                    else
                    {
                        estudiante.Datos.Nota = nuevaNota;
                        Console.WriteLine("Nota actualizada exitosamente.");
                        break;
                    }
                }
            }
            estudiante = estudiante.Siguiente;
        }
        if(!encontrado)
        {
            Console.Write("ERROR: la cedula ingresada no pertenece a ningun estudiante.");
        }
    }
    public void promedioNotasEstudiante(ListaMaterias listaMaterias, ListaEstudiantes listaEstudiantes, string cedula)
    {
        NodoMat actualMateria = listaMaterias.Cabeza;
        NodoEst actualEstudiante = listaEstudiantes.Cabeza;
        double notas = 0.0;
        int cont = 0;
        double promedio = 0.0;
        while(actualMateria != null)
        {
            actualEstudiante = listaEstudiantes.Cabeza;
            while(actualEstudiante != null)
            {
                if(actualEstudiante.Datos.Cedula == cedula)
                {
                    notas = notas + actualEstudiante.Datos.Nota;
                    cont = cont + 1;
                }
                actualEstudiante = actualEstudiante.Siguiente;
            }
            actualMateria = actualMateria.Siguiente;
        }
        promedio = notas / cont;
        actualEstudiante = listaEstudiantes.Cabeza;
        while(actualEstudiante != null)
        {
            if(actualEstudiante.Datos.Cedula == cedula)
            {
                Console.WriteLine("El alumno " + actualEstudiante.Datos.Nombre + " " + actualEstudiante.Datos.Apellido + " tiene un promedio de notas de: " + promedio);
                break;
            }
            actualEstudiante = actualEstudiante.Siguiente;
        }
        if(cont == 0)
        {
            Console.WriteLine("No se encontro al estudiante");
        }

    }
}
public class ListaMaterias
{
    public NodoMat Cabeza;
    public ListaMaterias()
    {
        Cabeza = null!;
    }
    public void agregarMateria(Materia newMateria, ListaDepartamentos departamentos)
    {
        string codigoCompleto = newMateria.Departamento + newMateria.Codigo;
        if (newMateria.Codigo.Length != 4)
        {
            Console.WriteLine("ERROR: El código debe tener 4 dígitos.");
            return;
        }
        NodoDepa actualDepartamento = departamentos.Cabeza;
        bool departamentoExiste = false;
        while (actualDepartamento != null)
        {
            if (actualDepartamento.Datos.Codigo == newMateria.Departamento)
            {
                departamentoExiste = true;
                break;
            }
            actualDepartamento = actualDepartamento.Siguiente;
        }

        if (!departamentoExiste)
        {
            Console.WriteLine("ERROR: El código ingresado del departamento no existe.");
            return;
        }
        NodoMat nuevoNodo = new NodoMat(newMateria);
        nuevoNodo.Datos.Codigo = codigoCompleto;
        if (Cabeza == null)
        {
            Cabeza = nuevoNodo;
        }
        else
        {
            NodoMat actual1 = Cabeza;
            while (actual1.Siguiente != null)
            {
                if (actual1.Datos.Departamento.ToLower().Replace(" ", "") == nuevoNodo.Datos.Departamento.ToLower().Replace(" ", "") &&
                    actual1.Datos.Nombre.ToLower().Replace(" ", "") == nuevoNodo.Datos.Nombre.ToLower().Replace(" ", ""))
                {
                    Console.WriteLine("Ya existe una materia con el mismo nombre en este departamento.");
                    return;
                }
                actual1 = actual1.Siguiente;
            }
            if (actual1.Datos.Departamento.ToLower().Replace(" ", "") == nuevoNodo.Datos.Departamento.ToLower().Replace(" ", "") &&
                actual1.Datos.Nombre.ToLower().Replace(" ", "") == nuevoNodo.Datos.Nombre.ToLower().Replace(" ", ""))
            {
                Console.WriteLine("Ya existe una materia con el mismo nombre en este departamento.");
                return;
            }
            actual1.Siguiente = nuevoNodo;
        }
        Console.WriteLine("\nMateria agregada exitosamente:");
        Console.WriteLine($"Nombre de la materia: {newMateria.Nombre}\nCodigo de la materia: {codigoCompleto}\nDescripcion de la materia: {newMateria.Descripcion}");
    }
    public void modificarMateria(string codigo, ListaDepartamentos departamentos, ListaMaterias materias)
    {
        bool encontrado = false;
        NodoMat actual = Cabeza;
        bool validacion = true;
        while(actual != null)
        {
            if(actual.Datos.Codigo == codigo)
            {
                string newNombre = departamentos.ValidarEntrada("Ingrese el nombre modificado de la materia: ");
                string newDescripcion = departamentos.ValidarEntrada("Ingrese la descripción modificada de la materia: ");
                
                
                // Verificar que el nuevo nombre no exista en la misma materia o en el mismo departamento
                NodoMat temp = Cabeza;
                while (temp != null)
                {
                    if (temp != actual && temp.Datos.Departamento == actual.Datos.Departamento &&
                        temp.Datos.Nombre.ToLower().Replace(" ", "") == newNombre.ToLower().Replace(" ", ""))
                    {
                        Console.WriteLine("ERROR: El nombre ingresado ya existe para otra materia en el mismo departamento.");
                        validacion = false;
                        break;
                    }
                    temp = temp.Siguiente;
                }

                if (validacion)
                {
                    actual.Datos.Nombre = newNombre;
                    actual.Datos.Descripcion = newDescripcion;
                    Console.WriteLine("\nMateria modificada exitosamente:");
                    Console.WriteLine($"Nuevo nombre de la materia: {actual.Datos.Nombre}\nNueva descripcion de la materia: {actual.Datos.Descripcion}");
                    encontrado = true;
                }
                break;
            }
            actual = actual.Siguiente;
        }

        if (!encontrado && validacion)
        {
            Console.WriteLine("ERROR: El código de la materia no ha sido encontrado.");
        }
    }
    public void EliminarMateria(string codigo)
    {
        if (Cabeza == null) return;

        if (Cabeza.Datos.Codigo == codigo)
        {
            Cabeza = Cabeza.Siguiente;
            Console.WriteLine("Materia eliminado con exito.");
            return;
        }

        NodoMat actual = Cabeza;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Datos.Codigo == codigo)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                Console.WriteLine("Materia eliminado con exito.");
                return;
            }
            actual = actual.Siguiente;
        }

        Console.WriteLine("ERROR: El codigo de la materia no ha sido encontrado.");
    }
    public void estudiantesInscritosMateria(ListaMaterias listaMaterias, string codigo, ListaEstudiantes listaEstudiantes, string periodo)
    {
        NodoMat actualMateria = listaMaterias.Cabeza;
        bool encontrado = false;
        while (actualMateria != null && actualMateria.Datos.Codigo != codigo)
        {
            actualMateria = actualMateria.Siguiente;
        }
        if(actualMateria == null)
        {
            Console.WriteLine("No se encontro la materia. Verifique el codigo ingresado");
        }else
        {
            NodoEst actualEstudiante = listaEstudiantes.Cabeza;
            int num = 1;
            while(actualEstudiante != null)
            {
                if(actualEstudiante.Datos.Periodo == periodo && actualEstudiante.Datos.Codigo == codigo)
                {
                    encontrado = true;
                    Console.WriteLine("");
                    Console.WriteLine("Estudiante #" + num);
                    Console.WriteLine("-------------------------------");
                    Console.WriteLine("Nombre: " + actualEstudiante.Datos.Nombre);
                    Console.WriteLine("Apellido: " + actualEstudiante.Datos.Apellido);
                    Console.WriteLine("Cedula: " + actualEstudiante.Datos.Cedula);
                    Console.WriteLine("Seccion: " + actualEstudiante.Datos.Seccion);
                    num = num + 1;
                }
                actualEstudiante = actualEstudiante.Siguiente;
            }
            if(!encontrado)
            {
                Console.WriteLine("No hay ningun estudiante registrado en " + actualMateria.Datos.Nombre + ", en el periodo " + periodo);
            }
        }
    }
    public void materiaConMasEstudiantes(ListaMaterias listaMaterias, string periodo, ListaEstudiantes listaEstudiantes)
    {
        NodoMat actualMateria = listaMaterias.Cabeza;
        int mayorCantEstudiantes = 0;
        string codigoMat = "";
        while(actualMateria != null)
        {
            NodoEst actualEstudiante = listaEstudiantes.Cabeza;
            int cantEstudiantes = 0;
            while(actualEstudiante != null)
            {
                if(actualEstudiante.Datos.Periodo == periodo && actualMateria.Datos.Codigo == actualEstudiante.Datos.Codigo)
                {
                    cantEstudiantes = cantEstudiantes + 1;
                }
                actualEstudiante = actualEstudiante.Siguiente;
            }
            if(cantEstudiantes >= mayorCantEstudiantes)
            {
                mayorCantEstudiantes = cantEstudiantes;
                codigoMat = actualMateria.Datos.Codigo;
            }
            actualMateria = actualMateria.Siguiente;
            
        }
        actualMateria = listaMaterias.Cabeza;
        while(actualMateria != null)
        {
            if(actualMateria.Datos.Codigo == codigoMat)
            {
                Console.WriteLine(actualMateria.Datos.Nombre + " es la materia con mas estudiantes inscritos, con la cantidad de: " + mayorCantEstudiantes);
            }
            actualMateria = actualMateria.Siguiente;
        }
    }
    public void materiaConMayorCreditos(ListaMaterias listaMaterias, string credito)
    {
        NodoMat actualMateria = listaMaterias.Cabeza;
        char num =  credito[0];
        int i = 1;
        while(actualMateria != null)
        {
            char ultimoDigito = actualMateria.Datos.Codigo[actualMateria.Datos.Codigo.Length - 1];
            if(ultimoDigito == num)
            {
                Console.WriteLine("");
                Console.WriteLine("Materia #" + i);
                Console.WriteLine("----------------------");
                Console.WriteLine("Nombre: " + actualMateria.Datos.Nombre);
                Console.WriteLine("Codigo: " + actualMateria.Datos.Codigo);
                Console.WriteLine("Creditos: " + ultimoDigito);
                Console.WriteLine("Descripcion: " + actualMateria.Datos.Descripcion);
                i = i + 1;
            }
            actualMateria = actualMateria.Siguiente;       
        }
        if(i == 1)
        {
            Console.WriteLine("No se encontraron materias con esa cantidad de creditos");
        }
    }
    public void materiasSemestre (ListaMaterias listaMaterias, string codigo, string semestre, ListaDepartamentos listaDepartamentos)
    {
        NodoDepa actualDepartamento = listaDepartamentos.Cabeza;
        NodoMat actualMateria = listaMaterias.Cabeza;
        int i = 1;
        char num = semestre[0];
        while(actualDepartamento.Datos.Codigo != codigo)
        {
            actualDepartamento = actualDepartamento.Siguiente;
        }
        while(actualMateria != null)
        {
            string codigoMat1 = actualMateria.Datos.Codigo.Substring(0,3);
            char codigoMat2 = actualMateria.Datos.Codigo[actualMateria.Datos.Codigo.Length - 2];
            if(actualDepartamento.Datos.Codigo == codigoMat1 && num == codigoMat2)
            {
                Console.WriteLine("");
                Console.WriteLine("Materia #" + i);
                Console.WriteLine("----------------------");
                Console.WriteLine("Nombre: " + actualMateria.Datos.Nombre);
                Console.WriteLine("Codigo: " + actualMateria.Datos.Codigo);
                Console.WriteLine("Semestre: " + num);
                Console.WriteLine("Descripcion: " + actualMateria.Datos.Descripcion);
                i = i + 1;
            }
            actualMateria = actualMateria.Siguiente;
        }
        if(i == 1)
        {
            Console.WriteLine("Para ese departamento y semestre, no hay materias registradas");
        }
    }
    public void materiaMayorPromedio(ListaMaterias listaMaterias, string periodo, ListaEstudiantes listaEstudiantes)
    {
        NodoMat actualMateria = listaMaterias.Cabeza;
        double mayorProm = 0.0;
        string codigo = "";
        while(actualMateria != null)
        {
            int cont = 0;
            double prom = 0.0;
            double total = 0.0;
           NodoEst actualEstudiante = listaEstudiantes.Cabeza;
           while(actualEstudiante != null)
           {
                if(actualEstudiante.Datos.Periodo == periodo && actualEstudiante.Datos.Codigo == actualMateria.Datos.Codigo)
                {
                    prom = prom + actualEstudiante.Datos.Nota;
                    cont = cont + 1;
                }
                actualEstudiante = actualEstudiante.Siguiente;
           }
           total = prom / cont;
           if (total > mayorProm)
           {
                mayorProm = total;
                codigo = actualMateria.Datos.Nombre;
           }
           actualMateria = actualMateria.Siguiente;
        }
        Console.WriteLine("La materia que posee un mayor promedio es " + codigo + ", con un promedio de " + mayorProm);  
        }
}
public class ListaDepartamentos
{
    public NodoDepa Cabeza;
    public ListaDepartamentos()
    {
        Cabeza = null!;
    }
    public void agregarDepartamento(Departamento newDepartamento)
    {
        NodoDepa nuevoNodo = new NodoDepa(newDepartamento);
        if(Cabeza == null)
        {
            Cabeza = nuevoNodo;
        }
        else
        {  
            NodoDepa actual = Cabeza;
            while(actual.Siguiente != null)
            {
                if(actual.Datos.Codigo.ToLower().Replace(" ", "") == newDepartamento.Codigo.ToLower().Replace(" ", "") || actual.Siguiente.Datos.Codigo.ToLower().Replace(" ", "") == newDepartamento.Codigo.ToLower().Replace(" ", ""))
                {
                    Console.Write("ERROR: Codigo existente.");
                    return;
                }
                if (actual.Datos.Nombre.ToLower().Replace(" ", "") == newDepartamento.Nombre.ToLower().Replace(" ", "") || actual.Siguiente.Datos.Nombre.ToLower().Replace(" ", "") == newDepartamento.Nombre.ToLower().Replace(" ", ""))
                {
                    Console.Write("ERROR: Nombre existente.");
                    return;
                }
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo; 
            Console.WriteLine("\nDepartamento Agregado con exito:");
            Console.WriteLine($"Nombre del departamento: {newDepartamento.Nombre}\nCodigo del departamento: {newDepartamento.Codigo}\nDescripcion del depratamento: {newDepartamento.Descripcion}");
        }
    }
    public string ValidarEntrada(string mensaje)
    {
        string entrada = "";
        while (string.IsNullOrWhiteSpace(entrada))
        {
            Console.Write(mensaje);
            entrada = Console.ReadLine() ?? "";
            if (string.IsNullOrWhiteSpace(entrada))
            {
                Console.WriteLine("ERROR: Este campo no puede estar vacío. Por favor, ingrese un valor.");
            }
        }
        return entrada;
    }
    public void ModificarDepartamento(string codigo)
    {
        NodoDepa newDepartamento = Cabeza;
        bool encontrado = false;
        while(newDepartamento != null)
        {
            if(newDepartamento.Datos.Codigo == codigo)
            {
                string newNombre = ValidarEntrada("Ingrese el nombre modificado del departamento: ");
                string newDescripcion = ValidarEntrada("Ingrese la descripcion del departamento: ");
                bool validacion = true;
                NodoDepa actual = Cabeza;
                while(actual.Siguiente != null)
                {
                    if (actual.Datos.Nombre.ToLower().Replace(" ", "") == newNombre.ToLower().Replace(" ", "") || actual.Siguiente.Datos.Nombre.ToLower().Replace(" ", "") == newNombre.ToLower().Replace(" ", ""))
                    {
                        Console.Write("ERROR: Nombre existente.");
                        return;
                    }
                    actual = actual.Siguiente;
                }
                if(validacion)
                {
                    newDepartamento.Datos.Nombre = newNombre;
                    newDepartamento.Datos.Descripcion = newDescripcion;
                    Console.WriteLine("\nDepartamento modificado con exito:");
                    Console.WriteLine($"Nuevo nombre del departamento: {newDepartamento.Datos.Nombre}\nNueva descripcion del departamento: {newDepartamento.Datos.Descripcion}");
                    encontrado = true;
                }
                break;
            }
            newDepartamento = newDepartamento.Siguiente;
        }
        if(!encontrado)
        {
            Console.WriteLine("ERROR: El codigo del departamento no ha sido encontrado.");
        }
    }
    public void EliminarDepartamento(string codigo)
    {
        if (Cabeza == null) return;

        if (Cabeza.Datos.Codigo == codigo)
        {
            Cabeza = Cabeza.Siguiente;
            Console.WriteLine("Departamento eliminado con exito.");
            return;
        }

        NodoDepa actual = Cabeza;
        while (actual.Siguiente != null)
        {
            if (actual.Siguiente.Datos.Codigo == codigo)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                Console.WriteLine("Departamento eliminado con exito.");
                return;
            }
            actual = actual.Siguiente;
        }

        Console.WriteLine("ERROR: El codigo del departamento no ha sido encontrado.");
    }
    public static bool EsNumerico(string input)
    {
        return Regex.IsMatch(input, @"^\d+$");
    }
    public void MostrarDepartamento(ListaDepartamentos departamentos, ListaEstudiantes estudiantes, ListaMaterias materias)
    {
        NodoDepa departamento = departamentos.Cabeza;
        NodoEst estudiante = estudiantes.Cabeza;
        NodoMat materia = materias.Cabeza;
        int departamentoMayor = 0;
        string dptName = "";

        while(departamento != null)
        {
            int cantEstudiantes = 0;
            while(materia != null)
            {
                string codigo = materia.Datos.Codigo.Substring(0, 3);
                if(codigo == departamento.Datos.Codigo)
                {
                    while(estudiante != null)
                    {
                        if(estudiante.Datos.Codigo == materia.Datos.Codigo)
                        {
                            cantEstudiantes++;
                        }
                        estudiante = estudiante.Siguiente;
                    }
                }
                materia = materia.Siguiente;
            }
            if(cantEstudiantes > departamentoMayor)
            {
                departamentoMayor = cantEstudiantes;
                dptName = departamento.Datos.Nombre;
            }
            departamento = departamento.Siguiente;
        }
        Console.WriteLine($"El departamento con mas estudinates inscritos es: {dptName} con {departamentoMayor} estudiantes");
    }
}
//Main
class Program
{
    public static void Main()
    {
        //Departamentos pre-cargados
        ListaDepartamentos listaDepartamentos = new ListaDepartamentos();
        listaDepartamentos.agregarDepartamento(new Departamento("Informatica", "230", "Departamento de EICA"));
        listaDepartamentos.agregarDepartamento(new Departamento("Cursos Basicos", "008", "Materias Básicas"));
        listaDepartamentos.agregarDepartamento(new Departamento("Estadistica", "220", "Ciencias estadísticas"));

        //Materias pre-cargadas
        ListaMaterias listaMaterias = new ListaMaterias();
        listaMaterias.agregarMateria(new Materia("230", "Algoritmo y Estructuras de datos II","1324","Estructuras de datos"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("230", "Organizacion y Sistemas","1723","Cuestiones de empresas"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("230", "Fundamentos de Electricidad","2534","Fisica"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("008", "Matematicas I","1214","Calculo Basico"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("008", "Lingüstica","2413","Elementos del castellano"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("008", "Deporte","3314","Actividad deportiva"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("220", "Computacion I","1213","Principios de computacion"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("220", "Matematicas III","2134","Calculo multivariable"), listaDepartamentos);
        listaMaterias.agregarMateria(new Materia("220", "Muestreo II","3463","Tecnicas de recoleccion"), listaDepartamentos);

        //Estudiantes pre-cargados
        ListaEstudiantes listaEstudiantes = new ListaEstudiantes();
        listaEstudiantes.agregarEstudiante(new Estudiante("2301324", "Alan", "Rivas", "16803161", "0620", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2301324", "Diego", "Rodriguez", "16823162", "0620", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2301324", "Eric", "Rondon", "16043166", "0621", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2301324", "Jesus", "Salazar", "16081090", "0621", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2201213", "Gonzalez", "Cristian", "16166662", "0820", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2201213", "Gonzalez", "Francisco", "14202614", "0820", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2201213", "Gonzalez", "Valeria", "16216622", "0821", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2201213", "Lopez", "Jesus", "14218088", "0821", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2202134", "Prieto", "Diego", "14820148", "0520", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2202134", "Quijada", "Jorge", "14688162", "0520", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2202134", "Suarez", "Karlismar", "14688649", "0521", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2302534", "Joscar", "Gomez", "14168103", "0520", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2302534", "Yudeimis", "Guerra", "18169493", "0520", "II-2023", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2302534", "Jade", "Hajjar", "16820222", "0521", "II-2022", 0), listaMaterias);
        listaEstudiantes.agregarEstudiante(new Estudiante("2302534", "Maria", "Henriquez", "16248093", "0521", "II-2022", 0), listaMaterias);
        while(true)
        {
            Console.Clear();
            Console.WriteLine("Sistema de gestion de departamentos:");
            Console.WriteLine("1. Gestionar departamentos");
            Console.WriteLine("2. Gestionar materias");
            Console.WriteLine("3. Gestionar estudiantes");
            Console.WriteLine("4. Reportes");
            Console.WriteLine("5. Salir");
            Console.Write("Escoga una opcion: ");
            ConsoleKeyInfo opcion = Console.ReadKey(intercept: true);

            if(opcion.KeyChar == '1')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Gestion de Departamentos");
                    Console.WriteLine("1. Agregar un departamento");
                    Console.WriteLine("2. Modificar un departamento");
                    Console.WriteLine("3. Eliminar un departamento");
                    Console.WriteLine("4. Salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        string nombre = listaDepartamentos.ValidarEntrada("Ingrese el nombre del departamento: ");
                        string codigo = "";
                        while(true)
                        {
                            codigo = listaDepartamentos.ValidarEntrada("Ingrese el codigo del departamento: ");
                            if(!ListaDepartamentos.EsNumerico(codigo))
                            {
                                Console.WriteLine("ERROR: El codigo del departamento solo debe contener números.");
                            }
                            else if(codigo.Length != 3)
                            {
                                Console.WriteLine("ERROR: El codigo del departamento debe estar conformado por 3 digitos.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        string descripcion = listaDepartamentos.ValidarEntrada("Ingrese la descripcion del departamento: ");
                        Departamento newDepartamento = new Departamento(nombre, codigo, descripcion);
                        listaDepartamentos.agregarDepartamento(newDepartamento);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo del departamento que desea modificar: ");
                        string codigo = Console.ReadLine() ?? "";                      
                        listaDepartamentos.ModificarDepartamento(codigo);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo del departamento que desea eliminar: ");
                        string codigo = Console.ReadLine() ?? "";                      
                        listaDepartamentos.EliminarDepartamento(codigo);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        break;
                    }
                }
            }
            else if(opcion.KeyChar == '2')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Gestion de Materias");
                    Console.WriteLine("1. Agregar una materia");
                    Console.WriteLine("2. Modificar una materia");
                    Console.WriteLine("3. Eliminar una materia");
                    Console.WriteLine("4. Salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        string departamento = "";
                        while(true)
                        {
                            departamento = listaDepartamentos.ValidarEntrada("Ingrese el codigo del departamento al que pertenece esta materia: ");
                            if(!ListaDepartamentos.EsNumerico(departamento))
                            {
                                Console.WriteLine("ERROR: El codigo departamento solo debe contener números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        string nombre = listaDepartamentos.ValidarEntrada("Ingrese el nombre de la materia: ");
                        string codigo = "";
                        while(true)
                        {
                            codigo = listaDepartamentos.ValidarEntrada("Ingrese los ultimos 4 digitos del codigo de la materia: ");
                            if(!ListaDepartamentos.EsNumerico(codigo))
                            {
                                Console.WriteLine("ERROR: El codigo solo debe contener números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        string descripcion = listaDepartamentos.ValidarEntrada("Ingrese la descripcion de la materia: ");
                        Materia newMateria = new Materia(departamento, nombre, codigo, descripcion);
                        listaMaterias.agregarMateria(newMateria, listaDepartamentos);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo de la materia que desea modificar: ");
                        string codigo = Console.ReadLine() ?? "";                      
                        listaMaterias.modificarMateria(codigo, listaDepartamentos, listaMaterias);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();                    
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        Console.Write("Indique el codigo de la materia que desea eliminar: ");
                        string codigo = Console.ReadLine() ?? "";                      
                        listaMaterias.EliminarMateria(codigo);

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        break;
                    }
                }

            }
            else if(opcion.KeyChar == '3')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Gestion de Estudiantes");
                    Console.WriteLine("1. Agregar un estudiante");
                    Console.WriteLine("2. Modificar un estudiante (para actualizar su nota)");
                    Console.WriteLine("3. Eliminar un estudiante");
                    Console.WriteLine("4. Salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        string Codigo = listaDepartamentos.ValidarEntrada("Ingrese el codigo de la materia: ");
                        string Nombre = listaDepartamentos.ValidarEntrada("Ingrese el nombre del estudiante: ");
                        string Apellido = listaDepartamentos.ValidarEntrada("Ingrese el apellido del estudiante: ");
                        string Cedula = "";
                        while(true)
                        {
                            Cedula = listaDepartamentos.ValidarEntrada("Ingrese la cedula del estudiante: ");
                            if(!ListaDepartamentos.EsNumerico(Cedula))
                            {
                                Console.WriteLine("ERROR: La cédula solo debe contener números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                        string Seccion = "";
                        while(true)
                        {
                            Seccion = listaDepartamentos.ValidarEntrada("Ingrese la seccion del estudiante: ");
                            if(!ListaDepartamentos.EsNumerico(Seccion))
                            {
                                Console.WriteLine("ERROR: La sección solo debe contener números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        while(true)
                        {
                            string Periodo = listaDepartamentos.ValidarEntrada("Ingrese el periodo del estudiante: ");
                            if(!ListaEstudiantes.ValidarEntrada(Periodo))
                            {
                                Console.WriteLine("ERROR: La entrada no es validada. Asegurese de escribir correctamente el periodo (Ejemplo: II-2023)");
                            }else
                            {
                                Estudiante nuevoEstudiante = new Estudiante(Codigo, Nombre, Apellido, Cedula, Seccion, Periodo, 0);
                                listaEstudiantes.agregarEstudiante(nuevoEstudiante, listaMaterias);
                                break;
                            }
                        }

                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        string Cedula = listaDepartamentos.ValidarEntrada("Ingrese la cedula del estudiante que quiera modificar: ");
                        listaEstudiantes.ModificarEstudiante(Cedula, listaEstudiantes, listaMaterias);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        string Cedula = "";
                        while(true)
                        {
                            Cedula = listaDepartamentos.ValidarEntrada("Ingrese la cedula del estudiante: ");
                            if(!ListaDepartamentos.EsNumerico(Cedula))
                            {
                                Console.WriteLine("ERROR: La cédula solo debe contener números.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        listaEstudiantes.EliminarEstudiante(Cedula, listaEstudiantes);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        break;
                    }
                }
            }
            else if(opcion.KeyChar == '4')
            {
                while(true)
                {
                    Console.Clear();
                    Console.WriteLine("Reportes");
                    Console.WriteLine("1. Lista de estudiantes inscritos en una materia");
                    Console.WriteLine("2. Lista de una materia con más estudiantes inscritos");
                    Console.WriteLine("3. Lista de materias que poseen creditos segun un determinado numero");
                    Console.WriteLine("4. Lista de materias de un departamento segun un semestre");
                    Console.WriteLine("5. Materia con mayor promedio de notas");
                    Console.WriteLine("6. Promedio de notas general de un estudiante");
                    Console.WriteLine("7. Departamento que cuenta con más estudiantes inscritos");
                    Console.WriteLine("8. Salir");
                    Console.Write("Escoga una opcion: ");
                    ConsoleKeyInfo tecla = Console.ReadKey(intercept: true);

                    if(tecla.KeyChar == '1')
                    {
                        Console.Clear();
                        string codigo = listaDepartamentos.ValidarEntrada("Indique el codigo de la materia: ");
                        string periodo = listaDepartamentos.ValidarEntrada("Indique el periodo: ");
                        if(!ListaEstudiantes.ValidarEntrada(periodo))
                        {
                            Console.WriteLine("La entrada no es valida. Asegurese de escribir correctamente el periodo (Ejemplo: II-2023)");
                        }else
                        {
                            listaMaterias.estudiantesInscritosMateria(listaMaterias, codigo, listaEstudiantes, periodo);
                        }
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '2')
                    {
                        Console.Clear();
                        string periodo = listaDepartamentos.ValidarEntrada("Indique el periodo:");
                        if(!ListaEstudiantes.ValidarEntrada(periodo))
                        {
                            Console.WriteLine("La entrada no es valida. Asegurese de escribir correctamente el periodo (Ejemplo: II-2023)");
                        }
                        listaMaterias.materiaConMasEstudiantes(listaMaterias, periodo, listaEstudiantes);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '3')
                    {
                        Console.Clear();
                        string credito = listaDepartamentos.ValidarEntrada("Indique el numero de creditos: ");
                        if (credito.Length != 1 || !char.IsDigit(credito[0]))
                        {
                            Console.WriteLine("El valor de crédito debe ser un solo dígito.");
                        }else
                        {
                        listaMaterias.materiaConMayorCreditos(listaMaterias, credito);
                        }
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '4')
                    {
                        Console.Clear();
                        while(true)
                        {
                            string codigo = listaDepartamentos.ValidarEntrada("Indique el codigo del departamento: ");
                            if(codigo.Length < 3 || codigo.Length > 3)
                            {
                                Console.WriteLine("Ingrese correctamente el codigo del departamento. Debe ser 3 caracteres");
                                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                                Console.ReadKey();
                                break;
                            }
                            string semestre = listaDepartamentos.ValidarEntrada("Indique el numero del semestre: ");
                            if (semestre.Length != 1 || !char.IsDigit(semestre[0]))
                            {
                                Console.WriteLine("El valor de crédito debe ser un solo dígito.");
                                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                                Console.ReadKey();
                                break;
                            }else
                            {
                                listaMaterias.materiasSemestre(listaMaterias, codigo, semestre, listaDepartamentos);
                                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                                Console.ReadKey();
                                break;
                            }
                        }
                    }
                    else if(tecla.KeyChar == '5')
                    {
                        Console.Clear();
                        string periodo = listaDepartamentos.ValidarEntrada("Indique el periodo:");
                        if(!ListaEstudiantes.ValidarEntrada(periodo))
                        {
                            Console.WriteLine("La entrada no es valida. Asegurese de escribir correctamente el periodo (Ejemplo: II-2023)");
                        }
                        listaMaterias.materiaMayorPromedio(listaMaterias, periodo, listaEstudiantes);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '6')
                    {
                        Console.Clear();
                        string cedula = listaDepartamentos.ValidarEntrada("Indique la cedula del estudiante: ");
                        listaEstudiantes.promedioNotasEstudiante(listaMaterias, listaEstudiantes, cedula);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();                     
                    }
                    else if(tecla.KeyChar == '7')
                    {
                        Console.Clear();
                        listaDepartamentos.MostrarDepartamento(listaDepartamentos, listaEstudiantes, listaMaterias);
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                    else if(tecla.KeyChar == '8')
                    {
                        break;
                    }
                }
            }
            else if(opcion.KeyChar == '5')
            {
                break;
            }
            else
            {
                Console.WriteLine("\nSeleccione una opcion valida. Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }
}