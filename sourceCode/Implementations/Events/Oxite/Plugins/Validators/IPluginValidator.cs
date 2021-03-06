﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using System;
using System.Collections.Generic;
using Oxite.Validation;

namespace Oxite.Plugins.Validators
{
    public interface IPluginValidator
    {
        IEnumerable<ValidationError> Validate(IDictionary<string, KeyValuePair<Type, object>> pluginProperties);
    }
}
