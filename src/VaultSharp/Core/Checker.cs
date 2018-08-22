using System;
using System.Collections.Generic;

namespace VaultSharp.Core
{
    internal class Checker
    {
        public static void NotNull<T>(T value, string name) where T : class 
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void NotNull<T>(IList<T> value, string name)
        {
            if (value == null || value.Count == 0)
            {
                throw new ArgumentException("'" + name + "' value is empty or contains white-space characters only.", name);
            }
        }

        public static void NotNull(string value, string name)
        {
            NotNull<string>(value, name);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("'" + name + "' value is empty or contains white-space characters only.", name);
            }
        }
    }
}