using System;

namespace Born.Extensions
{
    public static class Extensions
    {
        public static String GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }
    }
}