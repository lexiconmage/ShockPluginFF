using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eorzap.Types
{
    public static class ChatType
    {
        [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
        sealed class EnumOrderAttribute : Attribute
        {
            public int Order { get; }
            public EnumOrderAttribute(int order)
            {
                Order = order;
            }
        }

        public enum ChatTypes
        {
            [EnumOrder(0)]
            Tell_In = 0,

            [EnumOrder(1)]
            Tell = 17,

            [EnumOrder(2)]
            Say = 1,

            [EnumOrder(3)]
            Party = 2,

            [EnumOrder(4)]
            Alliance = 3,

            [EnumOrder(5)]
            Yell = 4,

            [EnumOrder(6)]
            Shout = 5,

            [EnumOrder(7)]
            FreeCompany = 6,

            [EnumOrder(8)]
            NoviceNetwork = 8,

            [EnumOrder(9)]
            CWL1 = 9,

            [EnumOrder(10)]
            CWL2 = 10,

            [EnumOrder(11)]
            CWL3 = 11,

            [EnumOrder(12)]
            CWL4 = 12,

            [EnumOrder(13)]
            CWL5 = 13,

            [EnumOrder(14)]
            CWL6 = 14,

            [EnumOrder(15)]
            CWL7 = 15,

            [EnumOrder(16)]
            CWL8 = 16,

            [EnumOrder(17)]
            LS1 = 19,

            [EnumOrder(18)]
            LS2 = 20,

            [EnumOrder(19)]
            LS3 = 21,

            [EnumOrder(20)]
            LS4 = 22,

            [EnumOrder(21)]
            LS5 = 23,

            [EnumOrder(22)]
            LS6 = 24,

            [EnumOrder(23)]
            LS7 = 25,
            [EnumOrder(24)]
            LS8 = 26,
        }

        public static IEnumerable<ChatTypes> GetOrderedChannels()
        {
            return Enum.GetValues(typeof(ChatType))
            .Cast<ChatTypes>()
            .Where(e => e != ChatTypes.Tell_In && e != ChatTypes.NoviceNetwork)
                    .OrderBy(e => GetOrder(e));
        }

        private static int GetOrder(ChatTypes channel)
        {
            // get the attribute of the channel
            var attribute = channel.GetType()
                .GetField(channel.ToString())
                ?.GetCustomAttributes(typeof(EnumOrderAttribute), false)
                .FirstOrDefault() as EnumOrderAttribute;
            // return the order of the channel, or if it doesnt have one, return the max value
            return attribute?.Order ?? int.MaxValue;
        }
    }
}
