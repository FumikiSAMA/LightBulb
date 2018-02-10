using System.Runtime.InteropServices;

namespace LightBulb.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public class Rect
    {
        private readonly int _left;
        private readonly int _top;
        private readonly int _right;
        private readonly int _bottom;

        public int Left => _left;

        public int Top => _top;

        public int Right => _right;

        public int Bottom => _bottom;

        public int Height => _bottom - _top;

        public int Width => _right - _left;

        public Rect(int left, int top, int right, int bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }

        public override string ToString() => $"{_left}:{_top} x {_right}:{_bottom}";
    }
}