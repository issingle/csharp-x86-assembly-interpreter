using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;

// have individual classes for each command
// on startup, register these commands with the environment using a hashtable or something



namespace x86
{
    /// <summary>
    /// Basic x86 execution environment.
    /// </summary>
    [Serializable()]
    public class ExecutionEnvironment
    {
        bool Active = false;

        #region Constructor
        public ExecutionEnvironment(uint memoryBaseAddress, uint memorySize)
        {
            if (uint.MaxValue - memoryBaseAddress > memorySize)
            {
                BaseAddress = memoryBaseAddress;
            }
            else throw new ArgumentOutOfRangeException();

            if (memorySize > 256 && memorySize < 0x6400000)
            {
                Memory = new byte[memorySize];
                Esp.Value = memoryBaseAddress + memorySize;
            }
            else throw new ArgumentOutOfRangeException("Memory size must be between 256 bytes and 100 megabytes.");
        }
        #endregion

        #region Processor
        // general purpose registers
        public readonly Eax Eax = new Eax();
        public readonly Ebx Ebx = new Ebx();
        public readonly Ecx Ecx = new Ecx();
        public readonly Edx Edx = new Edx();
        public readonly Esi Esi = new Esi();
        public readonly Edi Edi = new Edi();
        public readonly Esp Esp = new Esp();
        public readonly Ebp Ebp = new Ebp();
        public uint Eip = 1;   // instead of addressing with eip, we are now working with lines
        public EFlags EFlags = EFlags.ZeroFlag;

        public Register GetRegisterByName(string reg)
        {
            switch (reg)
            {
                case "eax":
                case "ax":
                case "al":
                case "ah": return Eax;
                case "ebx":
                case "bx":
                case "bl":
                case "bh": return Ebx;
                case "ecx":
                case "cx":
                case "cl":
                case "ch": return Ecx;
                case "edx":
                case "dx":
                case "dl":
                case "dh": return Edx;
                case "esi":
                case "si": return Esi;
                case "edi":
                case "di": return Edi;
                case "esp":
                case "sp": return Esp;
                case "ebp":
                case "bp": return Ebp;
                default: throw new ArgumentException();
            }
        }

        public uint GetRegisterValueByName(string reg)
        {
            switch (reg)
            {
                case "eax": return Eax.Value;
                case "ax": return Eax.Ax;
                case "al": return Eax.Al;
                case "ah": return Eax.Ah;
                case "ebx": return Ebx.Value;
                case "bx": return Ebx.Bx;
                case "bl": return Ebx.Bl;
                case "bh": return Ebx.Bh;
                case "ecx": return Ecx.Value;
                case "cx": return Ecx.Cx;
                case "cl": return Ecx.Cl;
                case "ch": return Ecx.Ch;
                case "edx": return Edx.Value;
                case "dx": return Edx.Dx;
                case "dl": return Edx.Dl;
                case "dh": return Edx.Dh;
                case "esi": return Esi.Value;
                case "si": return Esi.Si;
                case "edi": return Edi.Value;
                case "di": return Edi.Di;
                case "esp": return Esp.Value;
                case "sp": return Esp.Sp;
                case "ebp": return Ebp.Value;
                case "bp": return Ebp.Bp;
                default: throw new ArgumentException();
            }
        }

        public void SetRegisterValueByName(string reg, uint value)
        {
            switch (reg)
            {
                case "eax": Eax.Value = (uint)value; break;
                case "ax": Eax.Ax = (ushort)value; break;
                case "al": Eax.Al = (byte)value; break;
                case "ah": Eax.Ah = (byte)value; break;
                case "ebx": Ebx.Value = (uint)value; break;
                case "bx": Ebx.Bx = (ushort)value; break;
                case "bl": Ebx.Bl = (byte)value; break;
                case "bh": Ebx.Bh = (byte)value; break;
                case "ecx": Ecx.Value = (uint)value; break;
                case "cx": Ecx.Cx = (ushort)value; break;
                case "cl": Ecx.Cl = (byte)value; break;
                case "ch": Ecx.Ch = (byte)value; break;
                case "edx": Edx.Value = (uint)value; break;
                case "dx": Edx.Dx = (ushort)value; break;
                case "dl": Edx.Dl = (byte)value; break;
                case "dh": Edx.Dh = (byte)value; break;
                case "esi": Esi.Value = (uint)value; break;
                case "si": Esi.Si = (ushort)value; break;
                case "edi": Edi.Value = (uint)value; break;
                case "di": Edi.Di = (ushort)value; break;
                case "esp": Esp.Value = (uint)value; break;
                case "sp": Esp.Sp = (ushort)value; break;
                case "ebp": Ebp.Value = (uint)value; break;
                case "bp": Ebp.Bp = (ushort)value; break;
                default: throw new ArgumentException();
            }
        }


        public void SetEFlags(EFlags flags)
        {
            EFlags = EFlags.Set(flags);
        }
        public void ClearEFlags(EFlags flags)
        {
            EFlags = EFlags.Clear(flags);
        }
        public void ClearEFlags()
        {
            EFlags = EFlags.Clear();
        }
        /// <summary>
        /// Updates the EFlags based on the result.
        /// </summary>
        /// <param name="result">The integer result. Uses unsigned 64-bit value as the accumulator to test for overflows.</param>
        /// <param name="size">Destination size of the result in bytes.</param>
        /// <param name="flags">Flags to be updated.</param>
        public void UpdateEFlags(ulong result, uint size, EFlags flags)
        {
            int val = (int)result;

            // todo: their might be instances where according to size value is zero but upper portion is nonzero, might need to mask out the rest...
            if (flags.HasFlag(EFlags.ZeroFlag))
            {
                if (result == 0) SetEFlags(EFlags.ZeroFlag);
                else ClearEFlags(EFlags.ZeroFlag);
            }

            // set if lowest byte contains an even number of bits set
            if (flags.HasFlag(EFlags.ParityFlag))
            {
                // http://graphics.stanford.edu/~seander/bithacks.html#ParityWith64Bits
                if (((((((ulong)result & 0xFF) * (ulong)0x0101010101010101) & 0x8040201008040201) % 0x1FF) & 1) == 0)
                {
                    SetEFlags(EFlags.ParityFlag);
                }
                else ClearEFlags(EFlags.ParityFlag);
            }

            if (flags.HasFlag(EFlags.SignFlag))
            {
                uint signFlag = (uint)(1 << (int)((size << 3) - 1));
                if (((result & signFlag) == signFlag))
                {
                    SetEFlags(EFlags.SignFlag);
                }
                else ClearEFlags(EFlags.SignFlag);
            }

            if (flags.HasFlag(EFlags.AdjustFlag))   // 4th bit
            {
                if ((result & 8) == 8) SetEFlags(EFlags.AdjustFlag);
                else ClearEFlags(EFlags.AdjustFlag);
            }

            if (flags.HasFlag(EFlags.OverflowFlag)) // signed overflow
            {
                long signedResult = (long)result;
                switch (size)
                {
                    case 1:
                        if (signedResult > sbyte.MaxValue || signedResult < sbyte.MinValue)
                            SetEFlags(EFlags.OverflowFlag);
                        else ClearEFlags(EFlags.OverflowFlag);
                        break;
                    case 2:
                        if (signedResult > short.MaxValue || signedResult < short.MinValue)
                            SetEFlags(EFlags.OverflowFlag);
                        else ClearEFlags(EFlags.OverflowFlag);
                        break;
                    case 4:
                        if (signedResult > int.MaxValue || signedResult < int.MinValue)
                            SetEFlags(EFlags.OverflowFlag);
                        else ClearEFlags(EFlags.OverflowFlag);
                        break;
                    default: throw new ArgumentException();
                }
            }

            if (flags.HasFlag(EFlags.CarryFlag))    // unsigned overflow
            {
                if (((result >> ((int)size << 3)) & 1) == 1)
                {
                    SetEFlags(EFlags.CarryFlag);
                }
                else ClearEFlags(EFlags.CarryFlag);
            }
        }


        #endregion

        #region Memory
        /// <summary>
        /// Memory base address.
        /// </summary>
        public readonly uint BaseAddress;

        /// <summary>
        /// Memory data.
        /// </summary>
        public byte[] Memory;

        /// <summary>
        /// Reads a byte of memory from the specified address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public byte ReadByteFromMemory(uint address)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length)
            {
                return Memory[offset];
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Reads a word of memory from the specified address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public unsafe ushort ReadWordFromMemory(uint address)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length - 1)
            {
                fixed (byte* p = Memory)
                {
                    return *(ushort*)(p + offset);
                }
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Reads a doubleword of memory from the specified address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public unsafe uint ReadDoublewordFromMemory(uint address)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length - 3)
            {
                fixed (byte* p = Memory)
                {
                    return *(uint*)(p + offset);
                }
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Writes a byte to the specified address in memory.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteByteToMemory(uint address, byte value)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length)
            {
                Memory[offset] = value;
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Writes a word to the specified address in memory.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public unsafe void WriteWordToMemory(uint address, ushort value)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length - 1)
            {
                fixed (byte* p = Memory)
                {
                    (*(ushort*)(p + offset)) = value;
                }
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Writes a doubleword to the specified address in memory.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public unsafe void WriteDoublewordToMemory(uint address, uint value)
        {
            uint offset = address - BaseAddress;
            if (offset < Memory.Length - 3)
            {
                fixed (byte* p = Memory)
                {
                    (*(uint*)(p + offset)) = value;
                }
            }
            else throw new IndexOutOfRangeException(GetValidMemoryRange());
        }

        /// <summary>
        /// Writes a value to the specified address in memory.  The value must either be 1, 2, or 4 bytes in length.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        public void WriteMemory(uint address, object value)
        {
            switch (Type.GetTypeCode(value.GetType()))
            {
                case TypeCode.Boolean:
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                    WriteByteToMemory(address, (byte)value);
                    break;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    WriteWordToMemory(address, (ushort)value);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Single:
                    WriteDoublewordToMemory(address, (uint)value);
                    break;
                default: throw new ArgumentException("Invalid object type.");
            }
        }

        /// <summary>
        /// Reads a value of specified size from the specified address in memory.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public uint ReadMemory(uint address, uint size)
        {
            switch (size)
            {
                case 1: return ReadByteFromMemory(address);
                case 2: return ReadWordFromMemory(address);
                case 4: return ReadDoublewordFromMemory(address);
                default: throw new ArgumentException("Invalid size.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetValidMemoryRange()
        {
            return string.Format("Valid memory range of 0x{0:X8}-0x{1:X8}", BaseAddress, BaseAddress + Memory.Length);
        }
        #endregion

        #region Execution
        // return whether or not it is finished with the instruction
        // used with repeat!
        // pass in entire code
        public bool Execute(string code)
        {
            string prefix = null;        // only support repeat for string move functions
            string mnemonic = null;

            // break down the code into lines
            // inefficient, but i don't really forsee large scripts being executed by this application anyways...
            string[] lines = code.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);

            // get the current instruction
            string instruction = NormalizeInstruction(lines[Eip - 1]);

            // skip whitespace and labels
            if (instruction.Length == 0 || instruction.EndsWith(":"))
            {
                Eip++;
                return true;
            }

            // extract prefix if present
            if (instruction.StartsWith("rep"))
            {
                int i = instruction.IndexOf(' ');
                if (i != -1)
                {
                    prefix = instruction.Substring(0, i);
                    instruction = instruction.Remove(0, i + 1);
                }
                else throw new Exception("Invalid prefix");
            }
            else if (instruction.StartsWith("lock"))
            {
                throw new NotSupportedException();
            }

            // extract mnemonic
            int nextSpace = instruction.IndexOf(' ');
            mnemonic = (nextSpace > -1) ? instruction.Substring(0, nextSpace) : instruction;

            // verify repeat prefix is only used with string move and store instructions
            if (prefix != null && !mnemonic.StartsWith("movs") && !mnemonic.StartsWith("stos"))
            {
                throw new NotSupportedException();
            }

            // jumps
            if (mnemonic[0] == 'j')
            {
                string label = instruction.Substring(instruction.IndexOf(' ') + 1);
                ExecuteJump(mnemonic, ref lines, label);
                return true;
            }
            else // regular instruction
            {
                // parse arguments
                List<InstructionArgument> arguments = new List<InstructionArgument>();
                string[] args = instruction.Replace(mnemonic, string.Empty).Trim().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    foreach (string arg in args)
                    {
                        arguments.Add(new InstructionArgument(this, arg));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Syntax error");
                }

                // verify argument types

                InstructionArgument dst = null;
                InstructionArgument src = null;
                InstructionArgument arg2 = null;   // todo: new name

                if (args.Length == 0)
                {

                }
                else if (args.Length == 1)
                {
                    dst = arguments[0];
                    src = arguments[0];
                }
                else if (args.Length == 2)
                {
                    dst = arguments[0];
                    src = arguments[1];
                }
                else if (args.Length == 3)
                {
                    arg2 = arguments[2];
                }
                else throw new ArgumentException(); // don't think there are any instructions with over 3 args

                // can push immediates
                if (args.Length == 2 && dst.Type == ArgumentType.Immediate) throw new ArgumentException();

                // todo: have custom exceptions
                // ex: EncodingException, InvalidSyntaxException etc...

                switch (mnemonic)
                {
                    case "nop": break;
                    case "mov":
                        if (args.Length != 2)
                        {
                            throw new ArgumentException("The mov instruction requires two arguments.");
                        }
                        else if (dst.Type == ArgumentType.Immediate)
                        {
                            throw new ArgumentException("Cannot have an immediate as the destination.");
                        }
                        else if (src.Type == ArgumentType.Immediate && src.Size > dst.Size)
                        {
                            // immediate won't fit in the destination register or memory address
                            throw new ArgumentException("Immediate size is greater than the destination size.");
                        }
                        else if (src.Type != ArgumentType.Immediate && src.Size != dst.Size)
                        {
                            // source and destination must be either registers or memory addresses, so sizes must match
                            throw new ArgumentException("Source and destination sizes must match.");
                        }
                        else dst.SetValue(src.GetValue());
                        break;

                    case "movzx":

                        if (args.Length != 2)
                        {
                            throw new ArgumentException();
                        }
                        else if (dst.Type != ArgumentType.Register)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Type == ArgumentType.Immediate)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Size >= dst.Size)
                        {
                            throw new ArgumentException();
                        }
                        else dst.SetValue(src.GetValue());
                        break;

                    case "movsb":
                        WriteByteToMemory(Edi.Value, ReadByteFromMemory(Esi.Value));
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value--;
                            Esi.Value--;
                        }
                        else
                        {
                            Edi.Value++;
                            Esi.Value++;
                        }
                        break;
                    case "movsw":
                        WriteWordToMemory(Edi.Value, ReadWordFromMemory(Esi.Value));
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value -= 2;
                            Esi.Value -= 2;
                        }
                        else
                        {
                            Edi.Value += 2;
                            Esi.Value += 2;
                        }
                        break;
                    case "movsd":
                        WriteDoublewordToMemory(Edi.Value, ReadDoublewordFromMemory(Esi.Value));
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value -= 4;
                            Esi.Value -= 4;
                        }
                        else
                        {
                            Edi.Value += 4;
                            Esi.Value += 4;
                        }
                        break;
                    case "stosb":
                        WriteByteToMemory(Edi.Value, Eax.Al);
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value--;
                            Esi.Value--;
                        }
                        else
                        {
                            Edi.Value++;
                            Esi.Value++;
                        }
                        break;
                    case "stosw":
                        WriteWordToMemory(Edi.Value, Eax.Ax);
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value -= 2;
                            Esi.Value -= 2;
                        }
                        else
                        {
                            Edi.Value += 2;
                            Esi.Value += 2;
                        }
                        break;
                    case "stosd":
                        WriteDoublewordToMemory(Edi.Value, Eax.Value);
                        Ecx.Value--;
                        if (EFlags.HasFlag(EFlags.DirectionFlag))
                        {
                            Edi.Value -= 4;
                            Esi.Value -= 4;
                        }
                        else
                        {
                            Edi.Value += 4;
                            Esi.Value += 4;
                        }
                        break;

                    case "xchg":
                        if (args.Length != 2)
                        {
                            throw new ArgumentException("The xchg mnemonic only accepts two arguments");
                        }
                        else if (dst.Type != ArgumentType.Register || src.Type == ArgumentType.Immediate)
                        {
                            throw new ArgumentException("Destination must be a register and the source cannot be an immediate value.");
                        }
                        else if (src.Size != dst.Size)
                        {
                            throw new ArgumentException("Source and destination sizes must match.");
                        }
                        else
                        {
                            uint tmp = src.GetValue();
                            src.SetValue(dst.GetValue());
                            dst.SetValue(tmp);
                        }
                        break;

                    case "lea":
                        if (args.Length == 2 && dst.Type == ArgumentType.Register && src.Type == ArgumentType.Memory && src.Size > 1)
                        {
                            dst.SetValue(src.Address);
                        }
                        else throw new ArgumentException();
                        break;

                    case "inc":
                        if (args.Length == 1 && dst.Type != ArgumentType.Immediate)
                        {
                            ulong val = (ulong)dst.GetValue() + 1;
                            UpdateEFlags(val, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag);
                            dst.SetValue((uint)val);
                        }
                        else throw new ArgumentException();
                        break;

                    case "dec":
                        if (args.Length == 1 && dst.Type != ArgumentType.Immediate)
                        {
                            ulong val = (ulong)dst.GetValue() - 1;
                            UpdateEFlags(val, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag);
                            dst.SetValue((uint)val);
                        }
                        else throw new ArgumentException();
                        break;

                    case "not":
                        if (args.Length == 1 && dst.Type != ArgumentType.Immediate)
                        {
                            dst.SetValue(~dst.GetValue());
                        }
                        else throw new ArgumentException();
                        break;
                    case "neg":
                        if (args.Length == 1 && dst.Type != ArgumentType.Immediate)
                        {
                            ulong val = (ulong)(-(long)dst.GetValue());
                            UpdateEFlags(val, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            dst.SetValue((uint)val);
                        }
                        else throw new ArgumentException();
                        break;

                    case "bswap":

                        if (args.Length != 1 || dst.Type != ArgumentType.Register || dst.Size != 4)
                        {
                            throw new ArgumentException("Source operand must be a 32-bit register.");
                        }
                        else
                        {
                            uint dval = dst.GetValue();
                            dst.SetValue((uint)((dval << 24) | ((dval & 0xFF00) << 8) | ((dval & 0xFF0000) >> 8) | (dval >> 24)));
                        }
                        break;

                    case "add":
                        if (args.Length == 2)
                        {
                            ulong val = (ulong)dst.GetValue() + src.GetValue();
                            UpdateEFlags(val, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            dst.SetValue((uint)val);
                        }
                        else throw new ArgumentException();
                        break;

                    case "sub":
                        if (args.Length == 2)
                        {
                            ulong val = (ulong)dst.GetValue() - (ulong)src.GetValue();
                            UpdateEFlags(val, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            dst.SetValue((uint)val);
                        }
                        else throw new ArgumentException();
                        break;

                    case "mul": // unsigned multiply

                        // the OF and CF flags are set to 0 if the upper half of the result is 0, otherwise they are set to 1

                        if (args.Length != 1)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Type == ArgumentType.Immediate)
                        {
                            throw new ArgumentException();
                        }
                        else
                        {
                            switch (src.Size)
                            {
                                case 1:
                                    ushort r16 = (ushort)(Eax.Al * src.GetValue());
                                    Eax.Ax = r16;
                                    if (r16 >> 8 == 0)
                                    {
                                        ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    else
                                    {
                                        SetEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    break;
                                case 2:
                                    uint r32 = Eax.Ax * src.GetValue();
                                    Edx.Dx = (ushort)(r32 >> 16);    // high order
                                    Eax.Ax = (ushort)(r32 & 0xFFFF); // low order
                                    if (Edx.Dx == 0)
                                    {
                                        ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    else
                                    {
                                        SetEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    break;
                                case 4:
                                    ulong r64 = Eax.Ax * src.GetValue();
                                    Edx.Value = (uint)(r64 >> 32);    // high order
                                    Eax.Value = (uint)(r64 & 0xFFFFFFFF); // low order
                                    if (Edx.Value == 0)
                                    {
                                        ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    else
                                    {
                                        SetEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                                    }
                                    break;
                            }
                        }
                        break;

                    case "div":

                        if (src.Type == ArgumentType.Immediate)
                        {
                            throw new ArgumentException("Source operand must be a register or value from memory.");
                        }

                        ulong dividend, quotient;
                        uint divisor = src.GetValue();
                        if (divisor == 0)
                        {
                            throw new ArgumentException("Cannot divide by zero.");
                        }

                        if (src.Size == 1)
                        {
                            dividend = Eax.Ax;
                            quotient = dividend / divisor;
                            if (quotient > 0xFF) throw new ArgumentException("Divide Error");
                            Eax.Al = (byte)quotient;
                            Eax.Ah = (byte)(dividend % divisor);
                        }
                        else if (src.Size == 2)
                        {
                            dividend = ((uint)Edx.Dx << 16) & Eax.Ax;
                            quotient = dividend / divisor;
                            if (quotient > 0xFFFF) throw new ArgumentException("Divide Error");
                            Eax.Ax = (ushort)quotient;
                            Edx.Dx = (ushort)(dividend % divisor);
                        }
                        else if (src.Size == 4)
                        {
                            dividend = ((ulong)Edx.Value << 32) & Eax.Value;
                            quotient = dividend / divisor;
                            if (quotient > 0xFFFFFFFF) throw new ArgumentException("Divide Error");
                            Eax.Value = (uint)quotient;
                            Edx.Value = (uint)(dividend % divisor);
                        }

                        break;

                    //case "imul":    // todo: signed multiply
                    //    break;

                    //case "idiv":    // todo: signed divide
                    //    break;

                    case "and":
                        if (args.Length == 2)
                        {
                            uint result = dst.GetValue() & src.GetValue();
                            dst.SetValue(result);
                            ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                            UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag);
                        }
                        else throw new ArgumentException();
                        break;

                    case "or":
                        if (args.Length == 2)
                        {
                            uint result = dst.GetValue() | src.GetValue();
                            dst.SetValue(result);
                            ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                            UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag);
                        }
                        else throw new ArgumentException();

                        break;
                    case "xor":
                        if (args.Length == 2)
                        {
                            uint result = dst.GetValue() ^ src.GetValue();
                            dst.SetValue(result);
                            ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                            UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag);
                        }
                        else throw new ArgumentException();
                        break;

                    case "push":

                        // todo: check for stack overflow (stack pointer < base address)
                        if (args.Length == 1)
                        {
                            uint s = dst.Size;
                            if (s == 2 || s == 4)
                            {
                                WriteMemory(Esp - s, dst.GetValue());
                                Esp.Value -= s;
                            }
                            else throw new ArgumentException();
                        }
                        else throw new ArgumentException();
                        break;

                    case "pop":

                        // todo: check for stack overflow (stack pointer > base address + memory size)
                        if (args.Length == 1 && dst.Type != ArgumentType.Immediate)
                        {
                            uint s = dst.Size;
                            if (s == 2 || s == 4)
                            {
                                dst.SetValue(ReadMemory(Esp, s));
                                Esp.Value += s;
                            }
                            else throw new ArgumentException();
                        }
                        else throw new ArgumentException();
                        break;

                    case "pusha":
                        if (args.Length == 0)
                        {
                            WriteMemory(Esp - 2, Eax.Ax);
                            WriteMemory(Esp - 4, Ecx.Cx);
                            WriteMemory(Esp - 6, Edx.Dx);
                            WriteMemory(Esp - 8, Ebx.Bx);
                            WriteMemory(Esp - 10, Esp.Sp);
                            WriteMemory(Esp - 12, Ebp.Bp);
                            WriteMemory(Esp - 14, Esi.Si);
                            WriteMemory(Esp - 16, Edi.Di);
                            Esp.Value -= 16;
                        }
                        else throw new Exception();
                        break;

                    case "pushad":
                        if (args.Length == 0)
                        {
                            WriteMemory(Esp - 4, Eax.Value);
                            WriteMemory(Esp - 8, Ecx.Value);
                            WriteMemory(Esp - 12, Edx.Value);
                            WriteMemory(Esp - 16, Ebx.Value);
                            WriteMemory(Esp - 20, Esp.Value);
                            WriteMemory(Esp - 24, Ebp.Value);
                            WriteMemory(Esp - 28, Esi.Value);
                            WriteMemory(Esp - 32, Edi.Value);
                            Esp.Value -= 32;
                        }
                        else throw new Exception();
                        break;

                    case "popa":
                        if (args.Length == 0)
                        {
                            Edi.Di = ReadWordFromMemory(Esp);
                            Esi.Si = ReadWordFromMemory(Esp + 2);
                            Ebp.Bp = ReadWordFromMemory(Esp + 4);
                            //Esp.Sp = ReadWord(Esp + 6);    // ignored
                            Ebx.Bx = ReadWordFromMemory(Esp + 8);
                            Edx.Dx = ReadWordFromMemory(Esp + 10);
                            Ecx.Cx = ReadWordFromMemory(Esp + 12);
                            Eax.Ax = ReadWordFromMemory(Esp + 14);
                            Esp.Value += 16;
                        }
                        else throw new ArgumentException();
                        break;

                    case "popad":

                        if (args.Length == 0)
                        {
                            Edi.Value = ReadDoublewordFromMemory(Esp);
                            Esi.Value = ReadDoublewordFromMemory(Esp + 4);
                            Ebp.Value = ReadDoublewordFromMemory(Esp + 8);
                            //Esp.Value = ReadDoubleword(Esp + 12);    // ignored
                            Ebx.Value = ReadDoublewordFromMemory(Esp + 16);
                            Edx.Value = ReadDoublewordFromMemory(Esp + 20);
                            Ecx.Value = ReadDoublewordFromMemory(Esp + 24);
                            Eax.Value = ReadDoublewordFromMemory(Esp + 28);
                            Esp.Value += 32;
                        }
                        else throw new ArgumentException();
                        break;

                        // todo: pushf and popf, and effects on eflags

                    case "stc": // set carry flag
                        SetEFlags(EFlags.CarryFlag);
                        break;
                    case "std": // set direction flag
                        SetEFlags(EFlags.DirectionFlag);
                        break;
                    case "clc": // clear carry flag
                        ClearEFlags(EFlags.CarryFlag);
                        break;
                    case "cld": // clear direction flag
                        ClearEFlags(EFlags.DirectionFlag);
                        break;
                    case "shr": // shift right

                        if (args.Length != 2)
                        {
                            throw new ArgumentException();
                        }
                        else if (dst.Type == ArgumentType.Immediate || src.Type == ArgumentType.Memory)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Type == ArgumentType.Register && src.RegisterName != "cl")
                        {
                            throw new ArgumentException("Source register must be CL.");
                        }
                        else
                        {
                            uint result = dst.GetValue() >> (byte)src.GetValue();
                            dst.SetValue(result);

                            if (src.Type == ArgumentType.Immediate && src.GetValue() == 1)
                            {
                                UpdateEFlags(result, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag  | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                            else
                            {
                                UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                        }
                        break;

                    case "shl": // shift left
                    case "sal": // shift arithmetic left (signed)

                        if (args.Length != 2)
                        {
                            throw new ArgumentException();
                        }
                        else if (dst.Type == ArgumentType.Immediate || src.Type == ArgumentType.Memory)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Type == ArgumentType.Register && src.RegisterName != "cl")
                        {
                            throw new ArgumentException("Source register must be CL.");
                        }
                        else
                        {
                            ulong result = (ulong)dst.GetValue() << (byte)src.GetValue();
                            dst.SetValue((uint)result);

                            if (src.Type == ArgumentType.Immediate && src.GetValue() == 1)
                            {
                                UpdateEFlags(result, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                            else
                            {
                                UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                        }
                        break;

                    case "sar": // shift arithmetic right (signed)

                        if (args.Length != 2)
                        {
                            throw new ArgumentException();
                        }
                        else if (dst.Type == ArgumentType.Immediate || src.Type == ArgumentType.Memory)
                        {
                            throw new ArgumentException();
                        }
                        else if (src.Type == ArgumentType.Register && src.RegisterName != "cl")
                        {
                            throw new ArgumentException("Source register must be CL.");
                        }
                        else
                        {
                            uint result = (uint)((int)dst.GetValue() >> (byte)src.GetValue());
                            dst.SetValue(result);

                            if (src.Type == ArgumentType.Immediate && src.GetValue() == 1)
                            {
                                UpdateEFlags(result, dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                            else
                            {
                                UpdateEFlags(result, dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                            }
                        }
                        break;

                    case "ror": // rotate right

                        // bits >> n | (bits << (32 - n));
                        uint data = dst.GetValue();
                        uint bitsToShift = src.GetValue();
                        data = data >> (int)bitsToShift | (data << (32 - (int)bitsToShift));
                        dst.SetValue(data);

                        if (src.Type == ArgumentType.Immediate && src.GetValue() == 1)
                        {
                            UpdateEFlags(data, dst.Size, EFlags.OverflowFlag | EFlags.CarryFlag);
                        }
                        else
                        {
                            UpdateEFlags(data, dst.Size, EFlags.CarryFlag);
                        }
                        break;

                    case "rol": // rotate left

                        // bits << n | (bits >> (32 - n));
                        uint data2 = dst.GetValue();
                        uint bitsToShift2 = src.GetValue();
                        data = data2 << (int)bitsToShift2 | (data2 >> (32 - (int)bitsToShift2));
                        dst.SetValue(data);

                        if (src.Type == ArgumentType.Immediate && src.GetValue() == 1)
                        {
                            UpdateEFlags(data, dst.Size, EFlags.OverflowFlag | EFlags.CarryFlag);
                        }
                        else
                        {
                            UpdateEFlags(data, dst.Size, EFlags.CarryFlag);
                        }
                        break;


                    //case "call":

                    //    break;
                    //case "ret":
                    //    Eip = ReadMemory(Esp, 4);
                    //    Esp.Value += 4;
                    //    break;

                    // xadd
                    // loop

                    case "cmp":
                        UpdateEFlags((ulong)dst.GetValue() - (ulong)src.GetValue(), dst.Size, EFlags.OverflowFlag | EFlags.SignFlag | EFlags.ZeroFlag | EFlags.AdjustFlag | EFlags.ParityFlag | EFlags.CarryFlag);
                        break;
                    case "test":
                            ClearEFlags(EFlags.OverflowFlag | EFlags.CarryFlag);
                            UpdateEFlags(dst.GetValue() & src.GetValue(), dst.Size, EFlags.SignFlag | EFlags.ZeroFlag | EFlags.ParityFlag);
                        break;

                    default: throw new NotSupportedException("Instruction unknown or not supported.");
                }

                bool isFinished = true;
                if (prefix != null)
                {
                    if (prefix == "rep")
                    {
                        // repeat until ecx=0
                        isFinished = Ecx.Value == 0;
                    }
                    else if (prefix == "repe" || prefix == "repz")
                    {
                        // repeat until ecx=0 or zf=0
                        isFinished = Ecx.Value == 0 || !EFlags.HasFlag(EFlags.ZeroFlag);
                    }
                    else if (prefix == "repne" || prefix == "repnz")
                    {
                        // repeat until ecx=0 or zf=1
                        isFinished = Ecx.Value == 0 || EFlags.HasFlag(EFlags.ZeroFlag);
                    }
                    else throw new NotSupportedException("Invalid prefix.");
                }

                if (isFinished)
                {
                    bool stopExecution = !GoTo(ref lines, (int)Eip);

                }

                // todo: if finished and no more lines detected, stop execution
                // just update the environment state, and let the application do the rest

                return isFinished;
            }
        }

        private void ExecuteJump(string mnemonic, ref string[] instructions, string label)
        {
            bool shouldJump = false;

            // handle jumps
            if (mnemonic == "jmp")
            {
                shouldJump = true;
            }
            else if (mnemonic == "je" || mnemonic == "jz")
            {
                // Jump short if equal (ZF=1).
                shouldJump = EFlags.HasFlag(EFlags.ZeroFlag);
            }
            else if (mnemonic == "jne" || mnemonic == "jnz")
            {
                // Jump short if not equal (ZF=0).
                shouldJump = !EFlags.HasFlag(EFlags.ZeroFlag);  
            }
            else if (mnemonic == "ja" || mnemonic == "jnbe")    // unsigned comparisons
            {
                // Jump short if not below or equal (CF=0 and ZF=0).
                shouldJump = !EFlags.HasFlag(EFlags.CarryFlag) && !EFlags.HasFlag(EFlags.ZeroFlag);  
            }
            else if (mnemonic == "jb" || mnemonic == "jc" || mnemonic == "jnae")    // unsigned comparisons
            {
                // Jump short if not above or equal (CF=1). 
                shouldJump = EFlags.HasFlag(EFlags.CarryFlag);  
            }
            else if (mnemonic == "jng" || mnemonic == "jle")   // signed comparisons 
            {
                // Jump short if not greater (ZF=1 or SF != OF).
                shouldJump = EFlags.HasFlag(EFlags.ZeroFlag) || (EFlags.HasFlag(EFlags.SignFlag) != EFlags.HasFlag(EFlags.OverflowFlag));  
            }
            else if (mnemonic == "jnc" || mnemonic == "jnb" || mnemonic == "jae")  // unsigned comparisons 
            {
                // Jump short if not carry (CF=0). 
                shouldJump = !EFlags.HasFlag(EFlags.CarryFlag);  
            }
            else if (mnemonic == "jna" || mnemonic == "jbe")   // unsigned comparisons
            {
                // Jump short if not above (CF=1 or ZF=1).
                shouldJump = EFlags.HasFlag(EFlags.CarryFlag) || EFlags.HasFlag(EFlags.ZeroFlag);
            }
            else if (mnemonic == "jge" || mnemonic == "jnl")     // signed comparison
            {
                // Jump short if greater or equal (SF=OF).
                shouldJump = EFlags.HasFlag(EFlags.SignFlag) == EFlags.HasFlag(EFlags.OverflowFlag);  
            }
            else if (mnemonic == "jg" || mnemonic == "jnle")       // signed comparison
            {
                // Jump short if greater (ZF=0 and SF=OF).
                shouldJump = !EFlags.HasFlag(EFlags.ZeroFlag) && (EFlags.HasFlag(EFlags.SignFlag) == EFlags.HasFlag(EFlags.OverflowFlag)); 
            }
            else if (mnemonic == "jl" || mnemonic == "jnge")       // signed comparison
            {
                // Jump short if less (SF != OF). 
                shouldJump = EFlags.HasFlag(EFlags.SignFlag) != EFlags.HasFlag(EFlags.OverflowFlag);
            }
            else if (mnemonic == "jecxz")   
            {
                // Jump short if ECX register is 0. 
                shouldJump = Ecx.Value == 0;
            }
            else if (mnemonic == "jcxz")
            {
                // Jump short if CX register is 0. 
                shouldJump = Ecx.Cx == 0;
            }
            else if (mnemonic == "jno")
            {
                // Jump short if not overflow (OF=0)
                shouldJump = !EFlags.HasFlag(EFlags.OverflowFlag);
            }
            else if (mnemonic == "jnp" || mnemonic == "jpo")
            {
                // Jump short if not parity (PF=0)
                shouldJump = !EFlags.HasFlag(EFlags.ParityFlag);
            }
            else if (mnemonic == "jns")
            {
                // Jump short if not sign (SF=0)
                shouldJump = !EFlags.HasFlag(EFlags.SignFlag);
            }
            else if (mnemonic == "jo")
            {
                // Jump short if overflow (OF=1)
                shouldJump = !EFlags.HasFlag(EFlags.OverflowFlag);
            }
            else if (mnemonic == "jp" || mnemonic == "jpe")
            {
                // Jump short if parity (PF=1)
                shouldJump = !EFlags.HasFlag(EFlags.ParityFlag);
            }
            else if (mnemonic == "js")
            {
                // Jump short if sign (SF=1)
                shouldJump = !EFlags.HasFlag(EFlags.SignFlag);
            }
            else
            {
                throw new Exception("Invalid jump type detected.");
            }

            if (shouldJump) GoTo(ref instructions, label);
            else Eip++;
        }
        
        // todo: needs fixing, might not be a next instruction, and only whitespace
        /// <summary>
        /// Attempts to jump to the next instruction in the stream after the specified line number.
        /// </summary>
        /// <param name="instructions">The lines of instructions in the editor window.</param>
        /// <param name="line">The line (starting from 1) to jump to.</param>
        /// <returns>Whether or not a next instruction was found.</returns>
        public bool GoTo(ref string[] instructions, int line)
        {
            for (int i = line; i < instructions.Length; i++)
            {
                string instruction = NormalizeInstruction(instructions[i]);
                if (instruction.Length > 0 && !instruction.EndsWith(":"))   // skip empty lines and labels
                {
                    Eip = (uint)i + 1;
                    return true;
                }
            }
            Eip = (uint)line + 1;
            return false;
        }

        /// <summary>
        /// Attempts to jump to the instruction immediately following the specified label.
        /// </summary>
        /// <param name="instructions">The lines of instructions in the editor window.</param>
        /// <param name="label">The label to jump to.</param>
        /// <returns>Whether or not a next instruction was found.</returns>
        public bool GoTo(ref string[] instructions, string label)
        {
            bool foundLabel = false;
            int labelIndex = 0;
            for (; labelIndex < instructions.Length; labelIndex++)
            {
                if (NormalizeInstruction(instructions[labelIndex]).Equals(label + ":"))
                {
                    foundLabel = true;
                    break;
                }
            }

            if (foundLabel)
            {
                return GoTo(ref instructions, labelIndex + 1);
            }
            else return false;
        }

        /// <summary>
        /// Resets the execution state.
        /// </summary>
        public void Reset()
        {
            Eax.Value = 0;
            Ebx.Value = 0;
            Ecx.Value = 0;
            Edx.Value = 0;
            Esi.Value = 0;
            Edi.Value = 0;
            Esp.Value = BaseAddress + (uint)Memory.Length;
            Ebp.Value = 0;
            Eip = 1;
            EFlags = EFlags.ZeroFlag;
            Memory = new byte[Memory.Length];
        }
        #endregion

        private string NormalizeInstruction(string instruction)
        {
            // strip comments
            int commentIndex = instruction.IndexOf(';');
            if (commentIndex > -1)
            {
                instruction = instruction.Remove(commentIndex);
            }

            // convert to lowercase and remove any unnecessary whitespace
            instruction = Regex.Replace(instruction.ToLowerInvariant(), @"\s+", " ").Trim();

            // check for bad characters
            if (Regex.IsMatch(instruction, @"[!@#$%^&()={}\\|/?<>.~`""'_]"))
            {
                throw new InvalidOperationException(string.Format("Invalid character on line {0}.", Eip));
            }

            return instruction;
        }
    }
}