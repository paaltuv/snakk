﻿//  SPDX-FileCopyrightText: 2021 Pål Rune Sørensen Tuv <me@paaltuv.no>
//  SPDX-License-Identifier: MIT

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snakk.DB
{
    [Table("SurveyQuestionAnswer")]
    public class SurveyQuestionAnswer
    {
        public long Id { get; set; }

        public DateTime CreatedUtc { get; set; }
    }
}
