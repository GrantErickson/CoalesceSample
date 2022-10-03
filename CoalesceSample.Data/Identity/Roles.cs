﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoalesceSample.Data;
public static class Roles
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string User = nameof(User);
    public static string[] AllRoles => typeof(Roles).GetFields().Select(x =>x.Name).ToArray();
}
