using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewControls
{
    public class NewControls
    {
        public static readonly string Ver = "v2.0";
        public static readonly string Changelog = "# `NewControls` Changelog\n\n## Bugfixes\n- Repaired namespaces in classes\n\n## Changed Names\n- Changed namespace name from `Control` to `Controls`\n- Changed namespace name from `Dialog` to `Dialogs`\n\n## New Features\n- Added main class `NewControls` with `Ver` (Version) field, and `Changelog` field.\n- Changed inherit in `CmdLink` class from `Button` to `Btn` because `Btn` class contains `HasUACShield` property.\n";
    }
}
