using CloudGB.Core.CPU;

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

        public delegate void CommandHandler(IProcessor cpu, string[] args);

        private Dictionary<string, CommandHandler> _handlers = [];

        public ExecMode Mode { get; set; } = ExecMode.SingleStep;

        private string[] _lastCommand = [];

        public Debugger()
        {
            _handlers["b"] = HandleSetBreakpoint;
            _handlers["c"] = HandleContinue;
            _handlers["s"] = HandleStep;
            _handlers["x"] = Disassemble;
            _handlers["r"] = (cpu, args) => Console.WriteLine(cpu.DumpRegisters());
        }

        public void Step(IProcessor cpu)
        {
            while(true)
            {
                if (Mode == ExecMode.Continue)
                {
                    cpu.Step(out int cycles, out int breakpoint);
                    if (breakpoint >= 0)
                    {
                        Mode = ExecMode.Halt;
                        Console.WriteLine($"Breakpoint {breakpoint} reached");
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
                    if (_handlers.ContainsKey(command))
                    {
                        _handlers[command](cpu, args);
                    }
                }
            }
        }
        /// <summary>
        ///     Create a breakpoint at given address.
        ///     Output the id of new breakpoint
        /// </summary>
        private void HandleSetBreakpoint(IProcessor cpu, string[] args)
        {
            if (args.Length > 0)
            {
                ushort address = Convert.ToUInt16(args[0], 16);
                int idx = cpu.SetBreakpoint(address);
                Console.WriteLine($"breakponit {idx} set");
            }else
            {
                Console.WriteLine("breakpoint's address was not provided");
            }
        }

        private void HandleContinue(IProcessor cpu, string[] args)
        {
            Mode = ExecMode.Continue;
        }

        private void HandleStep(IProcessor cpu, string[] args)
        {
            Mode = ExecMode.SingleStep;
            if (!cpu.Step(out int cycles, out int breakpoint))
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
        private void Disassemble(IProcessor cpu, string[] args)
        {
            Mode = ExecMode.Halt;
            if (args.Length == 0)
            {
                Console.WriteLine(cpu.Disassemble(cpu.PC()));
            }
        }
    }
}
