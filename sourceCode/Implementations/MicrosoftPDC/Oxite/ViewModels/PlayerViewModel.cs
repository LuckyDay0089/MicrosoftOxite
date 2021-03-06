﻿//  --------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.codeplex.com/oxite/license
//  ---------------------------------
using Oxite.Models;

namespace Oxite.ViewModels
{
    public class PlayerViewModel
    {
        public PlayerViewModel(File media, File preview, string message, string bug)
        {
            Media = media;
            Preview = preview;
            Message = message;
            Bug = bug;
        }

        public File Media { get; private set; }
        public File Preview { get; private set; }
        public string Message { get; private set; }
        public string Bug { get; private set; }
    }
}
