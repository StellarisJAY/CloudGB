﻿using CloudGB.Core.CPU;

namespace CloudGB.Core.Test
{
    public class Debugger
    {
        public enum ExecMode
        {
            Halt,
            SingleStep,
            Continue,
        }

        public delegate void CommandHandler(IEmulator emulator, string[] args);

        private Dictionary<string, CommandHandler> _handlers = [];

        public ExecMode Mode { get; set; } = ExecMode.SingleStep;

        private string[] _lastCommand = [];

        public Debugger()
        {
            _handlers["b"] = HandleSetBreakpoint;
            _handlers["c"] = HandleContinue;
            _handlers["s"] = HandleStep;
            _handlers["x"] = Disassemble;
            _handlers["reg"] = (emulator, args) => Console.WriteLine(emulator.CPU().DumpRegisters());
            _handlers["r"] = (emulator, args) =>
            {
                emulator.CPU().Reset();
                Mode = ExecMode.Halt;
            };
            _handlers["help"] = (emulator, args) =>
            {
                Console.WriteLine("b $address: create breakpoint at address");
                Console.WriteLine("c:          continue execution until reaching a breakpoint");
                Console.WriteLine("s:          single step forward");
                Console.WriteLine("x $address: disassemble instruction at address");
                Console.WriteLine("reg:        dump registers' values");
                Console.WriteLine("r:          restart program");
            };
        }

        public void Step(IEmulator emulator)
        {
            while(true)
            {
                if (Mode == ExecMode.Continue)
                {
                    bool hasNext = emulator.DebugStep(out int breakpoint);
                    if (breakpoint >= 0)
                    {
                        Mode = ExecMode.Halt;
                        Console.WriteLine($"Breakpoint {breakpoint} reached");
                    }
                    if (!hasNext)
                    {
                        Mode = ExecMode.Halt;
                    }
                }
                if (Mode == ExecMode.Halt || Mode == ExecMode.SingleStep)
                {
                    Console.Write(">>> ");
                    string? line = Console.ReadLine();
                    string[] parts;
                    if (string.IsNullOrEmpty(line))
                    {
                        parts = _lastCommand;
                    }else
                    {
                        parts = line.Split(' ');
                    }
                    _lastCommand = parts;
                    string command = parts[0];
                    string[] args = parts[1..];
                    if (command.Equals("quit")) break;
                    if (_handlers.ContainsKey(command))
                    {
                        _handlers[command](emulator, args);
                    }else
                    {
                        Console.WriteLine($"unknown command {command}");
                    }
                }
            }
        }
        /// <summary>
        ///     Create a breakpoint at given address.
        ///     Output the id of new breakpoint
        /// </summary>
        private void HandleSetBreakpoint(IEmulator emulator, string[] args)
        {
            if (args.Length > 0)
            {
                ushort address = Convert.ToUInt16(args[0], 16);
                int idx = emulator.CPU().SetBreakpoint(address);
                Console.WriteLine($"breakponit {idx} set");
            }else
            {
                Console.WriteLine("breakpoint's address was not provided");
            }
        }

        private void HandleContinue(IEmulator emulator, string[] args)
        {
            Mode = ExecMode.Continue;
        }

        private void HandleStep(IEmulator emulator, string[] args)
        {
            Mode = ExecMode.SingleStep;
            if (!emulator.DebugStep(out int breakpoint))
            {
                Mode = ExecMode.Halt;
            }
            if (breakpoint >= 0)
            {
                Console.WriteLine($"Breakpoint {breakpoint} reached");
            }
        }

        /// <summary>
        ///     Disassemble an instruction at given address
        ///     If no address arg was provided, disassemble next instruction at PC
        /// </summary>
        private void Disassemble(IEmulator emulator, string[] args)
        {
            Mode = ExecMode.Halt;
            if (args.Length == 0)
            {
                Console.WriteLine(emulator.CPU().Disassemble(emulator.CPU().PC()));
            }
        }
    }
}
