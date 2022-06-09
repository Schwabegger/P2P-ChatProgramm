using System;

namespace Basics.Interfaces // Interface used to Close other windows
{
    internal interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}