﻿//  SPDX-FileCopyrightText: 2021 Pål Rune Sørensen Tuv <me@paaltuv.no>
//  SPDX-License-Identifier: MIT

using System;

namespace Snakk.Plugin.MyPlugin1.Routes.Channel.Post.List.Services
{
    public class Get : PluginFramework.Routes.Channel.Post.List.Services.IGet
    {
        public void ParseRequestData(object pluginData)
        {
            Console.WriteLine($"Hello from {GetType().FullName}.ParseRequestData()");
        }

        public void RunAfter()
        {
            Console.WriteLine($"Hello from {GetType().FullName}.RunAfter()");
        }

        public void RunBefore()
        {
            Console.WriteLine($"Hello from {GetType().FullName}.RunBefore()");
        }

        public void StuffResponseData(object pluginData)
        {
            Console.WriteLine($"Hello from {GetType().FullName}.StuffResponseData()");
        }
    }
}
