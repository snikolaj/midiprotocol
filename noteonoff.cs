// See https://aka.ms/new-console-template for more information

int[] noteNumber = new int[16];
bool[] noteOn = new bool[16];

int[] commandArray = new int[3];

int index = 0;

for(int i = 0; i < 16; i++)
{
    noteNumber[i] = 0;
    noteOn[i] = false;
}

string midiFunction()
{
    string response = "";
    if(index > 2)
    {
        switch (commandArray[0])
        {
            case 0x90:
                if(commandArray[2] == 0)
                {
                    response = turnNoteOff();
                }
                else
                {
                    response = turnNoteOn();
                }
                break;
            case 0x80:
                response = turnNoteOff();
                break;
        }

        index = 0;
    }
    
    
    return response;
}

string turnNoteOn()
{
    string response = "";
    for (int i = 0; i < 16; i++)
    {
        if (noteOn[i] == false && noteNumber[i] == 0)
        {
            noteNumber[i] = commandArray[1];
            noteOn[i] = true;
            response = String.Format("Note {0} turned on", commandArray[1]);
            break;
        }
    }

    for (int i = 0; i < 16; i++)
    {
        if (noteNumber[i] != 0)
        {
            Console.WriteLine("Note {0} {1}", noteNumber[i], (noteOn[i]) ? "on" : "off");
        }
    }

    return response;
}
string turnNoteOff()
{
    string response = "";
    for (int i = 15; i >= 0; i--)
    {
        if (noteOn[i] == true && noteNumber[i] == commandArray[1])
        {
            noteNumber[i] = 0;
            noteOn[i] = false;
            response = String.Format("Note {0} turned off", commandArray[1]);
            break;
        }
    }

    for (int i = 0; i < 16; i++)
    {
        if (noteNumber[i] != 0)
        {
            Console.WriteLine("Note {0} {1}", noteNumber[i], (noteOn[i]) ? "on" : "off");
        }
    }

    return response;
}


Console.WriteLine("Hello, World!");
beginning:

commandArray[index] = int.Parse(Console.ReadLine());
index++;
midiFunction();

goto beginning;
