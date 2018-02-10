using System;
using System.Text;
using System.Windows.Input;

namespace LightBulb.Models
{
    public partial class Hotkey : IEquatable<Hotkey>
    {
        public Key Key { get; }

        public ModifierKeys Modifiers { get; }

        public Hotkey(Key key, ModifierKeys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public bool Equals(Hotkey other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;

            return Key == other.Key && Modifiers == other.Modifiers;
        }

        public override bool Equals(object obj)
        {
            if (obj is Hotkey other)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) Key * 397) ^ (int) Modifiers;
            }
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
    }

    public partial class Hotkey
    {
        public static bool operator ==(Hotkey a, Hotkey b)
        {
            if (a is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Hotkey a, Hotkey b) => !(a == b);
    }
}