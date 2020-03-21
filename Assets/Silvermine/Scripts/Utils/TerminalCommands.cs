using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandTerminal;

public class TerminalCommands
{
    [RegisterCommand(Help = "Gets data folder path", MinArgCount = 0, MaxArgCount = 0)]
    public static void GetApplicationDataPath(CommandArg[] args)
    {
        string path = ContentUtilities.GetApplicationDataPath();

        Terminal.Log("Data Path: {0}", path);
    }
}
