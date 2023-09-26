﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Dtos.AuthDto
{
    public class LoginUserDto
    {
        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(150, ErrorMessage = "Email Address must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Email { get; set; } = string.Empty;

        [PasswordPropertyText]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(150, ErrorMessage = "Password must be between {2} and {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}