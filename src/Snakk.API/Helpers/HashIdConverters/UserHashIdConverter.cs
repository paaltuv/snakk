﻿//  SPDX-FileCopyrightText: 2021 Pål Rune Sørensen Tuv <me@paaltuv.no>
//  SPDX-License-Identifier: MIT

using Microsoft.Extensions.Configuration;

namespace Snakk.API.Helpers.HashIdConverters
{
    public interface IUserHashIdConverter : IBaseHashIdConverter
    {
    }

    public class UserHashIdConverter : BaseHashIdConverter, IUserHashIdConverter
    {
        public UserHashIdConverter(IConfiguration configuration)
            : base(configuration.GetValue<string>("UserHashidSecretSalt"))
        {
        }
    }
}
