﻿using System;
using System.Collections.Generic;

namespace Snakk.API.Extensions
{
    public static class IEnumerable
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (T item in @this)
            {
                action(item);
            }
        }
    }
}
