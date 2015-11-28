using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    class PlayerTag : TemplateObject
    {
        public string Name;

        public PlayerTag(string name)
        {
            Name = name;
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                case "name":
                    return new TextTag(Name).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
