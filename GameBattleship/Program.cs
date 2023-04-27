int[,] tablero_jugador;
int[,] tablero_maquina;
int dificultad = 0;
Random rnd = new Random();

void crear_tablero(int dificultad)
{
    int tablero_size = 0;

    switch (dificultad)
    {
        case 1:
            tablero_size = 6;
            break;
        case 2:
            tablero_size = 10;
            break;
        case 3:
            tablero_size = 20;
            break;
    }

    tablero_jugador = new int[tablero_size, tablero_size];
    tablero_maquina = new int[tablero_size, tablero_size];

    for (int f = 0; f < tablero_jugador.GetLength(0); f++)
    {
        for (int c = 0; c < tablero_jugador.GetLength(1); c++)
        {
            tablero_jugador[f, c] = 0;
            tablero_maquina[f, c] = 0;
        }
    }
}

bool verificar_posicion_disponible(
    int[,] tablero,
    int fila,
    int columna,
    int tamano_barco,
    bool es_horizontal
)
{
    int fila_inicio = fila;
    int columna_inicio = columna;
    int fila_fin = fila + (es_horizontal ? 0 : tamano_barco - 1);
    int columna_fin = columna + (es_horizontal ? tamano_barco - 1 : 0);

    // Verificar si la posición está dentro del tablero
    if (fila_fin >= tablero.GetLength(0) || columna_fin >= tablero.GetLength(1))
    {
        return false;
    }

    // Verificar si la posición está libre
    for (int f = fila_inicio; f <= fila_fin; f++)
    {
        for (int c = columna_inicio; c <= columna_fin; c++)
        {
            if (tablero[f, c] != 0)
            {
                return false;
            }
        }
    }

    return true;
}

void colocar_barco(int[,] tablero, int fila, int columna, int tamano_barco, bool es_horizontal)
{
    for (int i = 0; i < tamano_barco; i++)
    {
        if (es_horizontal && columna + i < tablero.GetLength(1))
        {
            tablero[fila, columna + i] = 1;
        }
        else if (!es_horizontal && fila + i < tablero.GetLength(0))
        {
            tablero[fila + i, columna] = 1;
        }
    }
}

void colocar_barcos()
{
    int[][] tamanos_barcos = new int[][]
    {
        new int[] { 2, 3, 4 }, // Fácil
        new int[] { 2, 3, 3, 4, 4, 1, 2 }, // Normal
        new int[] { 1, 2, 3, 3, 4, 5, 8, 6, 4 } // DarkSouls
    };

    // Colocar barcos para el jugador y para la máquina
    for (int i = 0; i < tamanos_barcos[dificultad - 1].Length; i++)
    {
        int tamano_barco = tamanos_barcos[dificultad - 1][i];
        bool posicion_disponible = false;
        int fila = 0;
        int columna = 0;
        bool es_horizontal = false;

        // Colocar barco para el jugador
        do
        {
            fila = rnd.Next(tablero_jugador.GetLength(0));
            columna = rnd.Next(tablero_jugador.GetLength(1));
            es_horizontal = rnd.Next(2) == 0;
            posicion_disponible = verificar_posicion_disponible(
                tablero_jugador,
                fila,
                columna,
                tamano_barco,
                es_horizontal
            );
        } while (!posicion_disponible);

        colocar_barco(tablero_jugador, fila, columna, tamano_barco, es_horizontal);

        // Colocar barco para la máquina
        do
        {
            fila = rnd.Next(tablero_maquina.GetLength(0));
            columna = rnd.Next(tablero_maquina.GetLength(1));
            es_horizontal = rnd.Next(2) == 0;
            posicion_disponible = verificar_posicion_disponible(
                tablero_maquina,
                fila,
                columna,
                tamano_barco,
                es_horizontal
            );
        } while (!posicion_disponible);

        colocar_barco(tablero_maquina, fila, columna, tamano_barco, es_horizontal);
    }
}

void imprimir_tablero(int[,] tablero, bool es_tablero_jugador)
{
    string caracter_a_imprimir = "";

    Dictionary<int, string> simbolos_jugador = new Dictionary<int, string>()
    {
        { 0, "󰖌" },
        { 1, "󰠳" },
        { -1, "" },
        { -2, "󱎘" },
    };

    Dictionary<int, string> simbolos_maquina = new Dictionary<int, string>()
    {
        { 0, "󰖌" },
        { 1, "󰖌" },
        { -1, "" },
        { -2, "󱎘" },
    };


    //Iconos friendly

    //Dictionary<int, string> simbolos_jugador = new Dictionary<int, string>()
    //{
    // { 0, "~" },
    // { 1, "B" },
    //  { -1, "*" },
    //   { -2, "x" },
    // };

    // Dictionary<int, string> simbolos_maquina = new Dictionary<int, string>()
    // {
    //   { 0, "~" },
    //  { 1, "~" },
    //  { -1, "*" },
    //  { -2, "x" },
    // };


    Console.Write("  ");
    for (int c = 0; c < tablero.GetLength(1); c++)
    {
        Console.Write(c + " ");
    }
    Console.WriteLine();

    for (int f = 0; f < tablero.GetLength(0); f++)
    {
        Console.Write(f + " ");
        for (int c = 0; c < tablero.GetLength(1); c++)
        {
            if (es_tablero_jugador)
            {
                caracter_a_imprimir = simbolos_jugador[tablero[f, c]];
            }
            else
            {
                caracter_a_imprimir = simbolos_maquina[tablero[f, c]];
            }
            Console.Write(caracter_a_imprimir + " ");
        }
        Console.WriteLine();
    }
}

void ingreso_coordenadas()
{
    bool control = false,
        repetir_ataque_jugador = false;

    while (!repetir_ataque_jugador)
    {
        uint fila = 0,
            columna = 0; // inicializar fila y columna con valor cero

        repetir_ataque_jugador = false;
        control = false;

        Console.Clear();
        imprimir_tablero(tablero_maquina, false);

        while (!control)
        {
            do
            {
                Console.Write("\nIngresa la fila: ");
            } while (
                !uint.TryParse(Console.ReadLine(), out fila)
                || (fila < 0)
                || (fila > tablero_maquina.GetLength(0) - 1)
            );

            do
            {
                Console.Write("\nIngresa la columna: ");
            } while (
                !uint.TryParse(Console.ReadLine(), out columna)
                || (columna < 0)
                || (columna > tablero_maquina.GetLength(1) - 1)
            );

            if ((tablero_maquina[fila, columna] == -1) || (tablero_maquina[fila, columna] == -2))
            {
                control = false;
                Console.WriteLine("\n!!Ya se usó esta casilla!!");
            }
            else
            {
                control = true;
            }
        }
        Console.Clear();

        if (tablero_maquina[fila, columna] == 1)
        {
            tablero_maquina[fila, columna] = -1;

            Console.Beep();
            imprimir_tablero(tablero_maquina, false);
            Console.WriteLine("\nLe diste a un barco!");
            Console.Write("\nUn turno mas pa ti... ");
            Console.ReadKey();
        }
        else
        {
            tablero_maquina[fila, columna] = -2;

            imprimir_tablero(tablero_maquina, false);
            Console.WriteLine("\nCon suerte le diste a un pescado");

            repetir_ataque_jugador = true;
        }
    }

    Console.Write("\nPulsa una tecla para darle el turno a la maquina... ");
    Console.ReadKey();
    Console.Clear();
}

void ataque_maquina()
{
    bool control = false,
        repetir_ataque = false;
    Console.Clear();

    while (!repetir_ataque)
    {
        int fila = 0,
            columna = 0; // inicializar fila y columna con valor cero
        repetir_ataque = false;
        Console.Clear();

        while (!control)
        {
            fila = rnd.Next(tablero_jugador.GetLength(0));
            columna = rnd.Next(tablero_jugador.GetLength(1));

            if ((tablero_jugador[fila, columna] == -1) || (tablero_jugador[fila, columna] == -2))
            {
                control = false;
            }
            else
            {
                control = true;
            }
        }
        if (tablero_jugador[fila, columna] == 1)
        {
            tablero_jugador[fila, columna] = -1;

            Console.Beep();
            imprimir_tablero(tablero_jugador, true);
            Console.WriteLine("\nTe dio el vato :(");
            Console.WriteLine("Un turno mas para el... \n");
            Console.ReadKey();
        }
        else
        {
            tablero_jugador[fila, columna] = -2;

            imprimir_tablero(tablero_jugador, true);
            Console.WriteLine("\nNo te dio el pibe");

            repetir_ataque = true;
        }
    }

    Console.Write("\nPulsa una tecla para tomar tu turno... ");
    Console.ReadKey();
    Console.Clear();
}

void jugar()
{
    crear_tablero(dificultad);
    colocar_barcos();

    bool jugador_gana = false;
    bool maquina_gana = false;

    Console.Clear();
    Console.WriteLine("Bienvenido al juego, este es tu tablero: \n");
    imprimir_tablero(tablero_jugador, true);
    Console.Write("\n\nPulse una tecla para continuar... ");
    Console.ReadKey();
    Console.Clear();

    while (!jugador_gana && !maquina_gana)
    {
        Console.WriteLine("TU TURNO\n");
        ingreso_coordenadas();
        jugador_gana = verificar_ganador(tablero_maquina);

        Console.WriteLine("TURNO DE LA MAQUINA\n");
        ataque_maquina();
        maquina_gana = verificar_ganador(tablero_jugador);
    }

    if (jugador_gana)
    {
        Console.WriteLine(
            @"\n\n
▒█░░▒█ ▒█▀▀▀█ ▒█░▒█ 　 ▒█░░▒█ ▀█▀ ▒█▄░▒█ 　 ▄ ▀▄ 
▒█▄▄▄█ ▒█░░▒█ ▒█░▒█ 　 ▒█▒█▒█ ▒█░ ▒█▒█▒█ 　 ░ ░█ 
░░▒█░░ ▒█▄▄▄█ ░▀▄▄▀ 　 ▒█▄▀▄█ ▄█▄ ▒█░░▀█ 　 ▀ ▄▀"
        );
    }
    else
    {
        Console.WriteLine(
            @"\n\n
▒█░░▒█ ▒█▀▀▀█ ▒█░▒█ 　 ▒█░░░ ▒█▀▀▀█ ▒█▀▀▀█ ▒█▀▀▀█ 　 ▄ ▄▀ 
▒█▄▄▄█ ▒█░░▒█ ▒█░▒█ 　 ▒█░░░ ▒█░░▒█ ░▀▀▀▄▄ ░▀▀▀▄▄ 　 ░ █░ 
░░▒█░░ ▒█▄▄▄█ ░▀▄▄▀ 　 ▒█▄▄█ ▒█▄▄▄█ ▒█▄▄▄█ ▒█▄▄▄█ 　 ▀ ▀▄"
        );
    }

    bool verificar_ganador(int[,] tablero)
    {
        for (int f = 0; f < tablero.GetLength(0); f++)
        {
            for (int c = 0; c < tablero.GetLength(1); c++)
            {
                if (tablero[f, c] != 0 && tablero[f, c] != -1)
                {
                    return false;
                }
            }
        }

        return true;
    }
}

void menu()
{
    uint menu_op = 1;
    Console.Clear();

    Console.WriteLine(
        @"
        ██████╗░░█████╗░████████╗████████╗██╗░░░░░███████╗░██████╗██╗░░██╗██╗██████╗░
        ██╔══██╗██╔══██╗╚══██╔══╝╚══██╔══╝██║░░░░░██╔════╝██╔════╝██║░░██║██║██╔══██╗
        ██████╦╝███████║░░░██║░░░░░░██║░░░██║░░░░░█████╗░░╚█████╗░███████║██║██████╔╝
        ██╔══██╗██╔══██║░░░██║░░░░░░██║░░░██║░░░░░██╔══╝░░░╚═══██╗██╔══██║██║██╔═══╝░ 
        ██████╦╝██║░░██║░░░██║░░░░░░██║░░░███████╗███████╗██████╔╝██║░░██║██║██║░░░░░
        ╚═════╝░╚═╝░░╚═╝░░░╚═╝░░░░░░╚═╝░░░╚══════╝╚══════╝╚═════╝░╚═╝░░╚═╝╚═╝╚═╝░░░░░"
    );

    Console.WriteLine("\n1. Iniciar juego");
    Console.WriteLine("2. Salir");

    do
    {
        Console.Write("\n> ");
    } while (!uint.TryParse(Console.ReadLine(), out menu_op) || ((menu_op == 0) || (menu_op > 2)));

    switch (menu_op)
    {
        case 1:

            Console.Clear();

            Console.WriteLine(
                @"
                ██████╗░██╗███████╗██╗░█████╗░██╗░░░██╗██╗░░░░░████████╗░█████╗░██████╗░
                ██╔══██╗██║██╔════╝██║██╔══██╗██║░░░██║██║░░░░░╚══██╔══╝██╔══██╗██╔══██╗
                ██║░░██║██║█████╗░░██║██║░░╚═╝██║░░░██║██║░░░░░░░░██║░░░███████║██║░░██║
                ██║░░██║██║██╔══╝░░██║██║░░██╗██║░░░██║██║░░░░░░░░██║░░░██╔══██║██║░░██║
                ██████╔╝██║██║░░░░░██║╚█████╔╝╚██████╔╝███████╗░░░██║░░░██║░░██║██████╔╝
                ╚═════╝░╚═╝╚═╝░░░░░╚═╝░╚════╝░░╚═════╝░╚══════╝░░░╚═╝░░░╚═╝░░╚═╝╚═════╝░"
            );

            Console.WriteLine("\n1. Fácil");
            Console.WriteLine("2. Normal");
            Console.WriteLine("3. DarkSouls");

            do
            {
                Console.Write("\n> ");
            } while (
                !int.TryParse(Console.ReadLine(), out dificultad)
                || ((dificultad == 0) || (dificultad > 3))
            );

            Console.Clear();

            jugar();

            break;

        case 2:

            Console.Clear();
            Console.WriteLine("\nGracias por usar el programa - Vuelve Pronto\n");
            Console.WriteLine(
                @"
          ⡠⠲⠠⠀⢠⠂⡴⠀⡴⠂⡔⠀⠀⠀⠀⠀⢀⠀⠀⠀⢆⠀⠀⠀⡆⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⣐⠪⣁⠣⠤⣇⡮⢁⣺⠋⡐⠄⠀⠀⠀⢀⢀⢲⠀⠀⠀⠈⣆⠀⠀⠈⡀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢠⠃⢣⡽⢀⡛⣬⢋⣾⡍⣰⡉⠀⠀⠀⠠⢎⠚⣸⠀⠀⠀⠰⢸⡀⠀⠀⡇⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠈⢰⡈⡇⢸⢷⣿⣾⣜⡳⢩⠁⠀⢀⠝⡭⠉⢩⣿⠀⠀⠐⠒⣼⡋⣴⢶⡧⡁
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠸⣷⣹⢸⣸⣿⣿⣿⠃⣿⠀⡠⢊⠼⣤⣴⣼⣇⡇⣧⠀⠀⣿⢫⢭⡿⡇⠁
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡿⣮⣼⠛⡛⡻⠟⠠⣈⠜⢁⠊⢰⣿⣿⣿⣿⢱⡌⠀⠸⣏⢷⡷⠁⠃⠀
⠀⠀⠀⢠⠀⠒⠒⠀⠤⠄⠿⣟⡘⠀⠀⠁⠀⠀⠏⠀⠀⠀⠘⡉⡽⡻⢃⡾⠀⢀⢻⣽⠞⠘⠀⢰⠀
⠀⠀⠀⡌⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠁⠀⠒⠂⠠⠤⠀⢀⣀⡁⠠⡺⠁⠠⢑⡲⠁⠀⠀⡇⠈⡀
⠀⠀⢀⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⢠⣶⢫⣛⠀⠀⠀⢸⡀⠇
⠀⠀⡈⠀⠀⡶⠶⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣈⣋⠶⣏⡄⠀⠀⠸⣧⠀
⠠⠩⡙⠀⢰⣇⣠⡼⠀⠀⢀⡴⣿⠀⠀⣰⠀⢀⣤⠀⠀⠀⠀⠀⠀⢀⠀⠻⣼⠣⡽⣡⠀⠀⠀⡴⡆
⠐⠠⠀⠀⣾⣀⣉⡷⠀⣤⠿⠤⣿⠀⢀⣯⠶⠋⠀⠀⢀⣴⣿⠀⠀⢠⠈⠷⣧⣛⠴⣫⠆⠀⠀⢠⠃
⠈⡇⠀⠀⠈⠉⠉⠀⠘⠃⠀⠀⢿⠀⣸⠛⢧⡀⠀⣠⠾⠥⣿⠀⠀⢰⡁⠄⢗⠻⣩⣓⣻⡄⠀⠠⠆
⢰⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠀⠈⠙⠘⠋⠀⠀⢿⠀⠀⢀⡆⠀⢸⣏⡳⣆⠧⢿⣆⠀⠀
⡌⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢘⢷⠀⠸⠽⣺⡌⣏⠶⣯⣆⠀
⠑⠒⠀⠠⠄⠀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢟⡌⠛⡋⠍⡽⡿⣌⢳⡏⣼⡇
⠀⠀⠀⠀⠀⠀⠀⠀⠈⠝⠀⠒⠂⡤⠤⠀⣀⡀⠀⠀⠀⠀⠀⠀⢰⡶⣬⣆⠱⢨⣟⠶⣌⢳⠇⠡⡇
⠀⠀⠀⠀⠀⠀⢀⣔⣃⡀⠀⠀⠀⡇⢀⠀⠀⠺⣈⣍⣄⣶⣒⣠⡤⣦⣐⣠⣃⣹⣉⣺⣬⡟⠀⣰⠀
                "
            );
            Environment.Exit(0);

            break;
    }
}

menu();
