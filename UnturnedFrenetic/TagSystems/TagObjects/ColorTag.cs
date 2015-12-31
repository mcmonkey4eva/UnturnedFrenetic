using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnityEngine;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ColorTag : TemplateObject
    {
        // <--[object]
        // @Type ColorTag
        // @SubType TextTag
        // @Group Mathematics
        // @Description Represents the color of text.
        // -->

        public Color Internal;

        public ColorTag(Color color)
        {
            Internal = color;
        }

        public static ColorTag For(string nameorrgba)
        {
            string[] split = nameorrgba.Split(',');
            if (split.Length == 4)
            {
                float red = Utilities.StringToFloat(split[0]);
                float green = Utilities.StringToFloat(split[1]);
                float blue = Utilities.StringToFloat(split[2]);
                float alpha = Utilities.StringToFloat(split[3]);
                return new ColorTag(new Color(red, green, blue, alpha));
            }
            else
            {
                switch (nameorrgba.ToLower())
                {
                    case "black":
                        return new ColorTag(Color.black);
                    case "blue":
                        return new ColorTag(Color.blue);
                    case "clear":
                        return new ColorTag(Color.clear);
                    case "cyan":
                        return new ColorTag(Color.cyan);
                    case "gray":
                        return new ColorTag(Color.gray);
                    case "green":
                        return new ColorTag(Color.green);
                    case "grey":
                        return new ColorTag(Color.grey);
                    case "magenta":
                        return new ColorTag(Color.magenta);
                    case "red":
                        return new ColorTag(Color.red);
                    case "white":
                        return new ColorTag(Color.white);
                    case "yellow":
                        return new ColorTag(Color.yellow);
                }
            }
            return null;
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name ColorTag.red
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the red value of this color.
                // @Example "0.1,0.2,0.3,1.0" .red returns "0.1".
                // -->
                case "red":
                    return new NumberTag(Internal.r).Handle(data.Shrink());
                // <--[tag]
                // @Name ColorTag.green
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the green value of this color.
                // @Example "0.1,0.2,0.3,1.0" .green returns "0.2".
                // -->
                case "greem":
                    return new NumberTag(Internal.g).Handle(data.Shrink());
                // <--[tag]
                // @Name ColorTag.blue
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the blue value of this color.
                // @Example "0.1,0.2,0.3,1.0" .red returns "0.3".
                // -->
                case "blue":
                    return new NumberTag(Internal.b).Handle(data.Shrink());
                // <--[tag]
                // @Name ColorTag.alpha
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the alpha value of this color.
                // @Example "0.1,0.2,0.3,1.0" .red returns "1.0".
                // -->
                case "alpha":
                    return new NumberTag(Internal.a).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.r + "," + Internal.g + "," + Internal.b + "," + Internal.a;
        }
    }
}
