﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueOpexEmployeesManager.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
    }
}