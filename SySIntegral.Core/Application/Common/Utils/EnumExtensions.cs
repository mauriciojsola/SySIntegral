using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace SySIntegral.Core.Application.Common.Utils
{
    public static class EnumExtensions
    {
        public static IDictionary<int, string> ToDictionary<T>()
        {
            var dictionary = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(T)))
            {
                var currentEnum = (T)Enum.Parse(typeof(T), value.ToString(CultureInfo.InvariantCulture));
                dictionary.Add(value, GetDescription(currentEnum as Enum));
            }
            return dictionary;
        }

        public static IEnumerable<T> GetAllValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ToEnum<T>(this string enumValue)
        {
            return (T)Enum.Parse(typeof(T), enumValue, true);
        }

        public static T ToEnum<T>(this int enumValue)
        {
            return enumValue.ToString().ToEnum<T>();
        }

        public static string GetDescription(this Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            if (fi == null) return string.Empty;

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes
                (typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}
