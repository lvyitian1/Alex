using System;
using System.Collections.Generic;
using System.Text;
using Alex.GuiDebugger.Common.Services;

namespace Alex.GuiDebugger.Services
{
    public interface IAlexPipeService
    {

        IGuiDebuggerService GuiDebuggerService { get; }

    }
}
