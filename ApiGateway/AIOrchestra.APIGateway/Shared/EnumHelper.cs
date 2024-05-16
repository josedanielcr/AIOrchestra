using System.ComponentModel;

namespace AIOrchestra.APIGateway.Helpers
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
