using System;

namespace VaultSharp.Infrastructure.Validation
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

        public static void NotNull(string value, string name)
        {
            NotNull<string>(value, name);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("value is empty or contains white-space characters only.", name);
            }
        }
    }
}