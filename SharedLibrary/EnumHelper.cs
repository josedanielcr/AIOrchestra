using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T e) where T : Enum
        {
            var type = e.GetType();
            var memberInfo = type.GetMember(e.ToString());
            if (memberInfo.Length == 0)
            {
                return string.Empty;
            }
            var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : e.ToString();
        }
    }
}
