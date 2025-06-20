using FluentNotes.Utils.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace FluentNotes.Utils.Converters
{
    internal class EnumConverter<TEnum> : ValueConverter<TEnum, string>
        where TEnum : struct, Enum
    {
        public EnumConverter() : base(
            enumVal => enumVal.GetDescription(),
            strVal => EnumExtensions.FromDescription<TEnum>(strVal))
        {
        }
    }
}
