﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SanPablo.Reclutador.Web.Core
{
    public class JsonResponse
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public object Data { get; set; }
    }
}