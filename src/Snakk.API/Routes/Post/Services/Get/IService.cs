﻿//  SPDX-FileCopyrightText: 2021 Pål Rune Sørensen Tuv <me@paaltuv.no>
//  SPDX-License-Identifier: MIT

using System.Threading.Tasks;

namespace Snakk.API.Routes.Thread.Services.Get
{
    public interface IService
    {
        Task<Dto.Routes.Thread.Get.ResponseDto> RunAsync(
            long id,
            object pluginData);
    }
}
