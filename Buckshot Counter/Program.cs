//Shotgun Roulette Calculator

List<int> determined = new();
List<int> ShowDetermined = new();
int unknownblank = 0;
int unknownlive = 0;
bool exit = false;
int currentshell = 0;
int numberOfShells = 0;
bool unknownflip = false;
int chancetop = 0;
int chancebottom = 0;

reset();

while (!exit)
{    
    exit = main();
}

void reset()
{
    Console.Clear();
    currentshell = 0;
    determined.Clear();
    unknownflip = false;
    ShowDetermined.Clear();
    chancetop = 0;
    chancebottom = 0;
    Console.WriteLine("Enter Live Count");
    unknownlive = int.Parse(Console.ReadLine());
    Console.WriteLine("Enter Blank Count");
    unknownblank = int.Parse(Console.ReadLine());


    //Makes sure there are no negatives
    if (unknownblank < 0)
    {
        unknownblank = 0;
    } else if (unknownlive < 0)
    {
        unknownlive = 0;
    }

    int total = unknownblank + unknownlive;
    numberOfShells = total;

    for (int i = 0; i < total; i++) //Initializes lists
    {
        determined.Add(0);
        ShowDetermined.Add(0);
    }

    if (unknownlive == 0) // Sets all to unknownblank if unknownlive count is zero
    {
        for (int i = 0; i < determined.Count(); i++)
        {
            determined[i] = 2;
            ShowDetermined[i] = 2;
        }
    } else if (unknownblank == 0) // Sets all to unknownlive if unknownblank coubnt is zero
    {
        for (int i = 0; i < determined.Count(); i++)
        {
            determined[i] = 1;
            ShowDetermined[i] = 1;
        }
    }
    return;
}

bool main()
{
    if (currentshell >= determined.Count)
    {
        Console.WriteLine("You have used all rounds in the chamber. Press anybutton to reset");
        Console.ReadKey(true);
        reset();
    }
    Console.Clear();
    Console.WriteLine($"Current Live/Blank --> {unknownlive}/{unknownblank}");

    //Outputs Determined Shell List
    List<char> output = new();
    for (int i=0; i<ShowDetermined.Count(); i++)
    {
        int current = ShowDetermined[i];
        if (current == 0)
        {
            output.Add('?');
        } else if (current == 1)
        {
            output.Add('L');
        } else if (current == 2)
        {
            output.Add('B');
        }
    }

    //Sets Chance top and bottom of current shell being live
    if (determined[currentshell] != 0) //If the current shell is known
    {
        if (determined[currentshell] == 1)
        {
            chancetop = 1;
            chancebottom = 1;
        } else
        {
            chancetop = 0;
            chancebottom = 1;
        }
    } else //If the current shell is unknown
    {
        chancetop = unknownlive;
        chancebottom = determined.Count(x => x == 0);
    }

    Console.WriteLine($"Debug Shells: {string.Join("/", determined)}");
    Console.WriteLine($"Determined Shells: {string.Join("/", output)}");
    Console.WriteLine($"Chance of being live round: {chancetop}/{chancebottom}  |  {(int)((double)chancetop/(double)chancebottom*100.0)}%");

    //Gets Current Command And Sends To Calculator if applicable
    Console.WriteLine("Commands F --> Live, S --> Blank, R --> Reset, E --> Flip Current, D --> Cell Info, C --> Beer, V --> Magnifying Glass, O --> Quit");
    Console.WriteLine("Enter Command");
    char input = '=';
    char[] allowed = ['f','s','r','e','d','c','v','o'];
    while (!allowed.Contains(input))
    {
        input = char.ToLower(Console.ReadKey(true).KeyChar);
    }
    if (input == 'r') {reset(); return false;}
    if (input == 'o') {return true;}

    Dictionary<char,int> inputToOpperation = new Dictionary<char, int>
    {
        {'f',1}, //Fire
        {'s',2}, //Blank
        {'e',3}, //Flip Current
        {'d',4}, //Cell Info
        {'c',5}, //Beer
        {'v',6}
    };

    UpdateFromMove(inputToOpperation[input]);

    return false;
}

void UpdateFromMove(int opperation)
{
    switch (opperation)
    {
        case 1: //Fire
            firecommand();
            break;
        
        case 2: //Blank
            blankcommand();
            break;
        
        case 3: //Flip Current
            flipcommand();
            break;
        
        case 4: //Cell Phone Info
            cellphonecommand();
            break;
        
        case 5: //Beer
            beercommand();
            break;

        case 6: //Mag Glass
            magglasscommand();
            break;
    }
    

    Calculate();

    //Update ShowDetermined
    for (int i = 0; i < ShowDetermined.Count; i++)
    {
        int offset = numberOfShells - ShowDetermined.Count();
        ShowDetermined[i] = determined[i+offset];
    }

    void firecommand()
    {
        //if (unknownlive == 0) {return;}
        if (determined[currentshell] == 0)
        {
        if (unknownflip == true) {unknownflip = false; unknownlive++; unknownblank--;}
        unknownlive--;
        } else
        {
            unknownflip = false;
        }
        determined[currentshell] = 1;
        ShowDetermined.RemoveAt(ShowDetermined.Count-1);
        currentshell++;
    }

    void blankcommand()
    {
        //if (unknownblank == 0) {return;}
        if (determined[currentshell] == 0)
        {
        if (unknownflip == true) {unknownflip = false; unknownlive--; unknownblank++;}
        unknownblank--;
        } else
        {
            unknownflip = false;
        }
        determined[currentshell] = 2;       
        ShowDetermined.RemoveAt(ShowDetermined.Count-1);
        currentshell++;
    }

    void flipcommand()
    {
        if (unknownflip == true) {unknownflip = false; return;}
        if (determined[currentshell] == 0)
        {
            unknownflip = true;
            return;
        }
        else if (determined[currentshell] == 1)
        {
            determined[currentshell] = 2;
        } else
        {
            determined[currentshell] = 1;
        }
    }

    void cellphonecommand()
    {
        Console.WriteLine("Position?");
        int position = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("Result? Live --> F, Blank --> S");
        if (char.ToLower(Console.ReadKey(true).KeyChar) == 'f') // Is Live
        {
            if (determined[position] == 0) {unknownlive--;}
            determined[position] = 1;
            
        } else // Is Blank
        {
            if (determined[position] == 0) {unknownblank--;}
            determined[position] = 2;
            
        }
        return;
    }

    void beercommand()
    {
        Console.WriteLine("Result? Live --> F, Blank --> S");
        if (char.ToLower(Console.ReadKey(true).KeyChar) == 'f') // Is Live
        {
            if (unknownflip == true && determined[currentshell] == 0) {unknownlive++; unknownblank--; unknownflip = false;}
            if (determined[currentshell] == 0) {unknownlive--;}
            determined[currentshell] = 1;
        } else // Is Blank
        {
            if (unknownflip == true && determined[currentshell] == 0) {unknownlive--; unknownblank++; unknownflip = false;}
            if (determined[currentshell] == 0) {unknownblank--;}
            determined[currentshell] = 2;
        }
        currentshell++;
        ShowDetermined.RemoveAt(ShowDetermined.Count-1);
        return;
    }

    void magglasscommand()
    {
        Console.WriteLine("Result? Live --> F, Blank --> S");
        if (char.ToLower(Console.ReadKey(true).KeyChar) == 'f') // Is Live
        {
            if (unknownflip == true && determined[currentshell] == 0) {unknownlive++; unknownblank--; unknownflip = false;}
            if (determined[currentshell] == 0) {unknownlive--;}
            determined[currentshell] = 1;
        } else // Is Blank
        {
            if (unknownflip == true && determined[currentshell] == 0) {unknownlive--; unknownblank++; unknownflip = false;}
            if (determined[currentshell] == 0) {unknownblank--;}
            determined[currentshell] = 2;
        }
        return;
    }

}

void Calculate()
{
    int unknownCount = determined.Count(x => x == 0);
    if (unknownblank == unknownCount && unknownlive == 0 && unknownCount != 0)
    {
        for (int i = 0; i < determined.Count; i++)
        {
            if (determined[i] == 0) {determined[i] = 2;}
        }
        unknownblank = 0;

    } else if (unknownlive == unknownCount && unknownblank == 0 && unknownCount != 0)
    {
       for (int i = 0; i < determined.Count; i++)
        {
            if (determined[i] == 0) {determined[i] = 1;}
        }
        unknownlive = 0;
    }
}
