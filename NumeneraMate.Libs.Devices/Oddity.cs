﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NumeneraMate.Libs.Devices
{
    public class Oddity
    {
        public string Description { get; set; }
        public string Source { get; set; }
        public override string ToString()
        {
            return $"Description: {Description}\nSource: {Source}\n";
        }
    }
}
