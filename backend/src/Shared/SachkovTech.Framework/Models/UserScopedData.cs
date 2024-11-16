﻿using SachkovTech.Framework.Authorization;

namespace SachkovTech.Framework.Models;

public class UserScopedData
{
    public Guid UserId { get; set; }
    public List<string> Permissions { get; set; } = [];
    public List<string> Roles { get; set; } = [];
}