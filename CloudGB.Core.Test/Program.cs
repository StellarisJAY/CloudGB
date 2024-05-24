using CloudGB.Core.CPU.Interpreter;
using CloudGB.Core.CPU;
using CloudGB.Core.Memory;
using System.CommandLine;
using CloudGB.Core.Test;
using CloudGB.Core;

var debugOption = new Option<bool>(name: "--debug", description: "start in debug mode");
var fileOption = new Option<string>(name: "--file", description: "target game file");
var traceOption = new Option<bool>(name: "--trace", description: "trace command execution");

var command = new RootCommand();
command.AddOption(debugOption);
command.AddOption(fileOption);
command.AddOption(traceOption);

command.SetHandler((debugMode, file, traceMode) =>
{
    byte[] data = File.ReadAllBytes(file);
    IEmulator emulator = new InterpreterEmulator(data, debugMode);
    emulator.Reset();
    if (debugMode)
    {
        Debugger debugger = new();
        debugger.Step(emulator);
    }else
    {
        emulator.Start();
    }
}, debugOption, fileOption, traceOption);

command.Invoke(args);

