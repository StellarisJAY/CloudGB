using System.CommandLine;
using CloudGB.Core.Test;
using CloudGB.Core;
using CloudGB.Core.CPU.Interpreter.OpCode;

var debugOption = new Option<bool>(name: "--debug", description: "start in debug mode");
var fileOption = new Option<string>(name: "--file", description: "target game file");
var traceOption = new Option<bool>(name: "--trace", description: "trace command execution");
var benchmarkOption = new Option<bool>(name: "--benchmark", description: "benchmark cpu");

var command = new RootCommand();
command.AddOption(debugOption);
command.AddOption(fileOption);
command.AddOption(traceOption);
command.AddOption(benchmarkOption);

command.SetHandler((debugMode, file, traceMode, benchmark) =>
{
    if (benchmark)
    {
        OpCodes.BenchmarkOpCodes(file);
        return;
    }
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
}, debugOption, fileOption, traceOption, benchmarkOption);

command.Invoke(args);

