﻿using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.Models.Account;

public class ForgotPasswordModel
{
    public string Email { get; set; } = string.Empty;
}
