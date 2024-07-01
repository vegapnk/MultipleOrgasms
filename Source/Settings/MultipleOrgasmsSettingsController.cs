using rjw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Multipleorgasm.Settings
{
    public class MultipleOrgasmsSettingsController :Mod
    {
        public MultipleOrgasmsSettingsController(ModContentPack content) : base(content)
        {
            GetSettings<MultipleOrgasmsSettings>();
        }

        public override string SettingsCategory()
        {
            return "MultipleOrgasmsSettings";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            MultipleOrgasmsSettings.DoWindowContents(inRect);
        }

    }
}
