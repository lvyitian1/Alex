using Alex.Api;
using Alex.API.Graphics;
using Alex.API.Utils;
using Alex.Graphics.Models.Entity;
using Alex.ResourcePackLib;
using Alex.ResourcePackLib.Json.Models;
using Alex.ResourcePackLib.Json.Models.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alex.Graphics.Models.Items
{
    public interface IItemRenderer : IAttached
    {
        ResourcePackModelBase Model { get; }

     //   Vector3         Rotation          { get; set; }
      //  Vector3         Translation       { get; set; }
      //  Vector3         Scale             { get; set; }
        DisplayPosition DisplayPosition   { get; set; }
       // DisplayElement  ActiveDisplayItem { get; set; }

        //void Update(IUpdateArgs args, MCMatrix characterMatrix);

        //void Render(IRenderArgs args, bool mock, out int vertices);

        bool Cache(ResourceManager pack);

        IItemRenderer Clone();
    }
}