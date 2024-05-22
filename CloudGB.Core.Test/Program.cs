using CloudGB.Core.CPU.Interpreter;
using CloudGB.Core.CPU;
using CloudGB.Core.Memory;
using System.CommandLine;
using CloudGB.Core.Test;

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
    IMemoryMap memory = new DefaultMemoryMap(data[0..0x4000], data[0x4000..0x8000]);

    IProcessor cpu = new InterpreterProcessor(memory, traceMode | debugMode);
    cpu.Reset();

    if (debugMode)
    {
        Debugger debugger = new();
        debugger.Step(cpu);
    }else
    {
        while (cpu.Step(out int cycles, out int breakpoint))
        {}
    }
}, debugOption, fileOption, traceOption);

await command.InvokeAsync(args);

