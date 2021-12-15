using System;
using System.Collections.Generic;
using System.Linq;

namespace GameNight.Models.EnumUtils
{
    public static class EnumToList
    {
        public static IEnumerable<TEnum> ToList<TEnum>(this Type enumType) where TEnum : Enum
        {
            if(enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            return typeof(TEnum).GetEnumValues().Cast<TEnum>().ToList();
        }

        public static IEnumerable<TEnum> ToListOrDefault<TEnum>(this Type enumType) where TEnum : Enum
        {
            if (enumType == null)
            {
                return new List<TEnum>();
            }

            return enumType.ToList<TEnum>();

        }
    }
}
