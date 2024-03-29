﻿using VaultSharp.Core;

namespace VaultSharp.V1.AuthMethods.AWS
{
    internal class AWSAuthMethodProvider : IAWSAuthMethod
    {
        private readonly Polymath _polymath;

        public AWSAuthMethodProvider(Polymath polymath)
        {
            Checker.NotNull(polymath, "polymath");
            this._polymath = polymath;
        }
    }
}