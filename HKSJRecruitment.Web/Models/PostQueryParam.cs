﻿using Common.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HKSJRecruitment.Web.Models
{
    public class PostQueryParam : QueryParam
    {
        public int DeptId { get; set; }
    }
}