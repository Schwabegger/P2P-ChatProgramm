// Copyright ©️ Schwabegger Moritz. All Rights Reserved
// Supporters:
// ඞ Hackl Tobias
// ඞ Ratzenböck Peter

using System;

namespace Basics.Interfaces // Interface used to Close other windows
{
    internal interface ICloseWindow
    {
        Action CloseAction { get; set; }
    }
}