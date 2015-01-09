﻿// Copyright (c) Microsoft Open Technologies, Inc.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This is a marker attribute that can be put on an interface to denote that only internal implementations
    /// of that interface should exist.
    /// </summary>
    internal sealed class InternalImplementationOnlyAttribute : Attribute
    {
    }
}