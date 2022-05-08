// See https://aka.ms/new-console-template for more information

int[] noteNumber = new int[16];
bool[] noteOn = new bool[16];

int[] commandArray = new int[3];

int index = 0;
int midiCommandBytes = 3;

for(int i = 0; i < 16; i++)
{
    noteNumber[i] = 0;
    noteOn[i] = false;
}

string midiFunction(int lastByte)
{
    // if the byte is > 127, then it is a status byte, where the message begins
    if(lastByte > 127)
    {
        // can be replaced with array or map?
        switch (lastByte)
        {
            case 0x90:
                midiCommandBytes = 3;
                break;
            case 0x80:  
                midiCommandBytes = 3;
                break;
            case 0xD0:
                midiCommandBytes = 2;
                break;
        }
        commandArray[0] = lastByte;
        index = 1;
    }
    // else it is a data byte, and enter it normally
    else 
    {
        commandArray[index - 1] = lastByte;
    }
    string response = "";
    // if the buffer's index matches the expected number of data bytes from the message call the associated function
    if (index >= midiCommandBytes)
    {
        switch (commandArray[0])
        {
            case 0x90:
                response = turnNoteOn();
                break;
            case 0x80:
                response = turnNoteOff();
                break;
            case 0xD0:
                response = afterTouch();
                break;
        }
    }

    DebugOut();
    
    return response;
}

string turnNoteOn()
{
    string response = "";
    // handle note on with 0 velocity = note off
    if (commandArray[2] == 0)
    {
        response = turnNoteOff();
    }
    else
    {
        for (int i = 0; i < 16; i++)
        {
            // if the associated note position is off, fill it up
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
                //Console.WriteLine("Note {0} {1}", noteNumber[i], (noteOn[i]) ? "on" : "off");
            }
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
            //Console.WriteLine("Note {0} {1}", noteNumber[i], (noteOn[i]) ? "on" : "off");
        }
    }

    return response;
}
string afterTouch()
{
    return String.Format("Global aftertouch set to {0}", commandArray[1]);
}
void DebugOut()
{
    Console.WriteLine("\nDebug out:\nMIDI array: ");
    for(int i = 0; i < 3; i++)
    {
        Console.Write(commandArray[i] + " ");
    }
    Console.WriteLine("\nIndex {0} and specific command index {1}", index, midiCommandBytes);
}

int lastByte;

Console.WriteLine("Hello, World!");
beginning:

lastByte = int.Parse(Console.ReadLine());

index++;
Console.WriteLine("\n" + midiFunction(lastByte));

goto beginning;
