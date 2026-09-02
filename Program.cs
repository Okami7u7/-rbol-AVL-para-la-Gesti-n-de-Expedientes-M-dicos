using System;
using System.Text.RegularExpressions;

namespace AVL
{
    // Esta clase representa cada expediente que vamos guardando
    class Nodo
    {
        public string NumeroExpediente;
        public string NombrePaciente;
        public int Edad;
        public string TipoSangre;

        public Nodo Izquierdo;
        public Nodo Derecho;

        public int Altura;

        public Nodo(string numero, string nombre, int edad, string sangre)
        {
            NumeroExpediente = numero;
            NombrePaciente = nombre;
            Edad = edad;
            TipoSangre = sangre;

            Izquierdo = null;
            Derecho = null;

            Altura = 1;
        }
    }

    class ArbolAVL
    {
        private Nodo raiz;

        // Devuelve la altura de un nodo
        private int ObtenerAltura(Nodo nodo)
        {
            if (nodo == null)
                return 0;

            return nodo.Altura;
        }

        // Compara las dos alturas para actualizar la del nodo
        private int Mayor(int numero1, int numero2)
        {
            return numero1 > numero2 ? numero1 : numero2;
        }

        // Calcula que tan inclinado esta el nodo
        private int FactorBalance(Nodo nodo)
        {
            if (nodo == null)
                return 0;

            return ObtenerAltura(nodo.Izquierdo) -
                   ObtenerAltura(nodo.Derecho);
        }

        // Rotacion hacia la derecha
        private Nodo RotarDerecha(Nodo nodo)
        {
            Nodo nuevoPadre = nodo.Izquierdo;
            Nodo auxiliar = nuevoPadre.Derecho;

            nuevoPadre.Derecho = nodo;
            nodo.Izquierdo = auxiliar;

            nodo.Altura = 1 + Mayor(
                ObtenerAltura(nodo.Izquierdo),
                ObtenerAltura(nodo.Derecho));

            nuevoPadre.Altura = 1 + Mayor(
                ObtenerAltura(nuevoPadre.Izquierdo),
                ObtenerAltura(nuevoPadre.Derecho));

            return nuevoPadre;
        }

        // Rotacion hacia la izquierda
        private Nodo RotarIzquierda(Nodo nodo)
        {
            Nodo nuevoPadre = nodo.Derecho;
            Nodo auxiliar = nuevoPadre.Izquierdo;

            nuevoPadre.Izquierdo = nodo;
            nodo.Derecho = auxiliar;

            nodo.Altura = 1 + Mayor(
                ObtenerAltura(nodo.Izquierdo),
                ObtenerAltura(nodo.Derecho));

            nuevoPadre.Altura = 1 + Mayor(
                ObtenerAltura(nuevoPadre.Izquierdo),
                ObtenerAltura(nuevoPadre.Derecho));

            return nuevoPadre;
        }

        // Inserta un nuevo expediente y revisa si hace falta balancear
        private Nodo Insertar(Nodo nodo, Nodo nuevo)
        {
            if (nodo == null)
                return nuevo;

            if (string.Compare(nuevo.NumeroExpediente, nodo.NumeroExpediente) < 0)
            {
                nodo.Izquierdo = Insertar(nodo.Izquierdo, nuevo);
            }
            else if (string.Compare(nuevo.NumeroExpediente, nodo.NumeroExpediente) > 0)
            {
                nodo.Derecho = Insertar(nodo.Derecho, nuevo);
            }
            else
            {
                return nodo;
            }

            nodo.Altura = 1 + Mayor(
                ObtenerAltura(nodo.Izquierdo),
                ObtenerAltura(nodo.Derecho));

            int balance = FactorBalance(nodo);

            // Caso LL
            if (balance > 1 &&
                string.Compare(nuevo.NumeroExpediente, nodo.Izquierdo.NumeroExpediente) < 0)
            {
                return RotarDerecha(nodo);
            }

            // Caso RR
            if (balance < -1 &&
                string.Compare(nuevo.NumeroExpediente, nodo.Derecho.NumeroExpediente) > 0)
            {
                return RotarIzquierda(nodo);
            }

            // Caso LR
            if (balance > 1 &&
                string.Compare(nuevo.NumeroExpediente, nodo.Izquierdo.NumeroExpediente) > 0)
            {
                nodo.Izquierdo = RotarIzquierda(nodo.Izquierdo);
                return RotarDerecha(nodo);
            }

            // Caso RL
            if (balance < -1 &&
                string.Compare(nuevo.NumeroExpediente, nodo.Derecho.NumeroExpediente) < 0)
            {
                nodo.Derecho = RotarDerecha(nodo.Derecho);
                return RotarIzquierda(nodo);
            }

            return nodo;
        }

        // Esta funcion es la que usamos desde el menu para agregar expedientes
        public bool AgregarExpediente(string numero, string nombre, int edad, string sangre)
        {
            if (Buscar(numero) != null)
                return false;

            Nodo nuevo = new Nodo(numero, nombre, edad, sangre);

            raiz = Insertar(raiz, nuevo);

            return true;
        }

        // Busca un expediente directamente dentro del arbol
        public Nodo Buscar(string numero)
        {
            Nodo actual = raiz;

            while (actual != null)
            {
                int comparacion = string.Compare(numero, actual.NumeroExpediente);

                if (comparacion == 0)
                    return actual;

                if (comparacion < 0)
                    actual = actual.Izquierdo;
                else
                    actual = actual.Derecho;
            }

            return null;
        }

        // Recorrido Inorden: izquierda, raiz y derecha
        public void Inorden(Nodo nodo)
        {
            if (nodo == null)
                return;

            Inorden(nodo.Izquierdo);
            MostrarExpediente(nodo);
            Inorden(nodo.Derecho);
        }

        // Recorrido Preorden: raiz, izquierda y derecha
        public void Preorden(Nodo nodo)
        {
            if (nodo == null)
                return;

            MostrarExpediente(nodo);
            Preorden(nodo.Izquierdo);
            Preorden(nodo.Derecho);
        }

        // Recorrido Postorden: izquierda, derecha y raiz
        public void Postorden(Nodo nodo)
        {
            if (nodo == null)
                return;

            Postorden(nodo.Izquierdo);
            Postorden(nodo.Derecho);
            MostrarExpediente(nodo);
        }

        // Muestra los datos de un expediente de una forma mas ordenada
        private void MostrarExpediente(Nodo nodo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(
                $"{nodo.NumeroExpediente} | {nodo.NombrePaciente} | " +
                $"Edad: {nodo.Edad} | Sangre: {nodo.TipoSangre}");

            Console.ResetColor();
        }

        // Devuelve la raiz para poder realizar los recorridos
        public Nodo ObtenerRaiz()
        {
            return raiz;
        }

        // Obtiene la altura actual del arbol
        public int ObtenerAltura()
        {
            return ObtenerAltura(raiz);
        }
    }

    class Program
    {
        static ArbolAVL arbol = new ArbolAVL();

        static void Main(string[] args)
        {
            Console.Title = "Sistema de Expedientes - Arbol AVL";

            bool continuar = true;

            while (continuar)
            {
                MostrarMenu();

                Console.Write("Seleccione una opcion: ");
                string opcion = Console.ReadLine();

                Console.Clear();

                switch (opcion)
                {
                    case "1":
                        RegistrarExpediente();
                        break;

                    case "2":
                        BuscarExpediente();
                        break;

                    case "3":
                        MostrarInorden();
                        break;

                    case "4":
                        MostrarPreorden();
                        break;

                    case "5":
                        MostrarPostorden();
                        break;

                    case "6":
                        MostrarAltura();
                        break;

                    case "7":
                        continuar = false;

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Programa finalizado. Hasta luego.");
                        Console.ResetColor();
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("La opcion ingresada no es valida.");
                        Console.ResetColor();
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione ENTER para regresar al menu...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
        }

        static void MostrarMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("==============================================");
            Console.WriteLine("       SISTEMA DE EXPEDIENTES MEDICOS");
            Console.WriteLine("              ARBOL AVL");
            Console.WriteLine("          . . .");
            Console.WriteLine("         .        .  .     ..    .");
            Console.WriteLine("      .                 .         .  .");
            Console.WriteLine("                      .");
            Console.WriteLine("                    .                ..");
            Console.WriteLine("   .          .            .              .");
            Console.WriteLine("   .            '.,        .               .");
            Console.WriteLine("   .              'b      *");
            Console.WriteLine("    .              '$    #.                ..");
            Console.WriteLine("   .    .           $:   #:               .");
            Console.WriteLine("       ..      .  ..*#  @):        .   . .");
            Console.WriteLine("              .     :@,@):   ,.**:'   .");
            Console.WriteLine("  .      .,         :@@*: ..**'      .   .");
            Console.WriteLine("           '#o.    .:(@'.@*'  .");
            Console.WriteLine("   .  .       'bq,..:,@@*'   ,*      .  .");
            Console.WriteLine("            , p$q8,:@)'  .p*'      .");
            Console.WriteLine("     .     '  . '@@Pp@@*'    .  .");
            Console.WriteLine("      .  . ..    Y7'.'     .  .");
            Console.WriteLine("               :@):.");
            Console.WriteLine("              .:@:'.");
            Console.WriteLine("            .::(@:. ");
            Console.WriteLine("==============================================");

            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("1. Registrar expediente");
            Console.WriteLine("2. Buscar expediente");
            Console.WriteLine("3. Mostrar recorrido Inorden");
            Console.WriteLine("4. Mostrar recorrido Preorden");
            Console.WriteLine("5. Mostrar recorrido Postorden");
            Console.WriteLine("6. Mostrar altura del arbol");
            Console.WriteLine("7. Salir");

            Console.WriteLine();
        }

        static void RegistrarExpediente()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- REGISTRAR EXPEDIENTE ----------");
            Console.ResetColor();

            string numero;

            // Aqui revisamos que el codigo tenga el formato que pide el laboratorio
            while (true)
            {
                Console.Write("Numero de expediente (ej. EXP0069): ");
                numero = Console.ReadLine().ToUpper();

                if (Regex.IsMatch(numero, @"^EXP\d{4}$"))
                {
                    break;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("El numero debe tener el formato EXP0000.");
                Console.WriteLine("Ejemplo: EXP0069");
                Console.ResetColor();

                Console.WriteLine();
            }

            Console.Write("Nombre del paciente: ");
            string nombre = Console.ReadLine();

            Console.Write("Edad: ");
            int edad;

            while (!int.TryParse(Console.ReadLine(), out edad) || edad < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Ingrese una edad valida: ");
                Console.ResetColor();
            }

            Console.Write("Tipo de sangre: ");
            string sangre = Console.ReadLine().ToUpper();

            bool agregado = arbol.AgregarExpediente(
                numero,
                nombre,
                edad,
                sangre);

            Console.WriteLine();

            if (agregado)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("El expediente fue registrado correctamente.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ese numero de expediente ya existe.");
            }

            Console.ResetColor();
        }

        static void BuscarExpediente()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- BUSCAR EXPEDIENTE ----------");
            Console.ResetColor();

            Console.Write("Ingrese el numero de expediente: ");
            string numero = Console.ReadLine().ToUpper();

            Nodo encontrado = arbol.Buscar(numero);

            Console.WriteLine();

            if (encontrado != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Expediente encontrado:");
                Console.ResetColor();

                Console.WriteLine("Numero: " + encontrado.NumeroExpediente);
                Console.WriteLine("Paciente: " + encontrado.NombrePaciente);
                Console.WriteLine("Edad: " + encontrado.Edad);
                Console.WriteLine("Tipo de sangre: " + encontrado.TipoSangre);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No se encontro un expediente con ese numero.");
                Console.ResetColor();
            }
        }

        static void MostrarInorden()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- RECORRIDO INORDEN ----------");
            Console.ResetColor();

            if (arbol.ObtenerRaiz() == null)
            {
                Console.WriteLine("El arbol esta vacio.");
                return;
            }

            arbol.Inorden(arbol.ObtenerRaiz());
        }

        static void MostrarPreorden()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- RECORRIDO PREORDEN ----------");
            Console.ResetColor();

            if (arbol.ObtenerRaiz() == null)
            {
                Console.WriteLine("El arbol esta vacio.");
                return;
            }

            arbol.Preorden(arbol.ObtenerRaiz());
        }

        static void MostrarPostorden()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- RECORRIDO POSTORDEN ----------");
            Console.ResetColor();

            if (arbol.ObtenerRaiz() == null)
            {
                Console.WriteLine("El arbol esta vacio.");
                return;
            }

            arbol.Postorden(arbol.ObtenerRaiz());
        }

        static void MostrarAltura()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("---------- ALTURA DEL ARBOL ----------");
            Console.ResetColor();

            int altura = arbol.ObtenerAltura();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("La altura actual del arbol es: " + altura);
            Console.ResetColor();
        }
    }
}