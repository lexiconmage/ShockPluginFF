using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eorzap.Types
{
    public class DeathMode
    {
        public static Regex DeathModeDieOtherRegex = new Regex("^(.*) is defeated");
        public static Regex DeathModeLiveOtherRegex = new Regex("^(.*) is revived");

        public static ChatType.ChatTypes[] deathTypes = [
            ChatType.ChatTypes.DeathOther,
            ChatType.ChatTypes.DeathSelf,
            ChatType.ChatTypes.DeathSelf2,
            ChatType.ChatTypes.ReviveOther,
            ChatType.ChatTypes.ReviveSelf
        ];
    }
}
