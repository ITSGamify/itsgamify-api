﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace its.gamify.core.Models.Quizes
{
    public class QuizUpdateModel:QuizCreateModel
    {
        public Guid Id { get; set; }
    }
}
