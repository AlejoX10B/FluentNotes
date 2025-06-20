using System;
using System.ComponentModel;
using System.Reflection;

namespace FluentNotes.Utils.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        public static T FromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute?.Description == description)
                    return (T)field.GetValue(null)!;
                

                if (field.Name == description)
                    return (T)field.GetValue(null)!;
            }

            throw new ArgumentException($"No se encontró la descripción '{description}' en el enum '{typeof(T).Name}'.");
        }
    }
}
