﻿using System;
using Dotpay.Common;
using Newtonsoft.Json;

namespace Dotpay.Front.ViewModel
{

    [Serializable]
    public class UserIdentity
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginAt { get; set; } 
        public DateTime? CreateAt { get; set; }
        public IdentityInfoViewModel IdentityInfo { get; set; }
        public bool IsActive { get; set; }
        public bool IsInitPaymentPassword { get; set; }
    }
    [Serializable]
    public class IdentityInfoViewModel
    {
        public string FullName { get; set; }
        public string IdNo { get; set; }
        public IdNoType IdType { get; set; }
    } 
    [Serializable]
    public class UserRegisterViewModel
    {
        public string Email { get; set; }
        public string LoginPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
    [Serializable]
    public class UserLoginViewModel
    {
        public string LoginName { get; set; }//login name 可能是邮箱、用户名或手机号
        public string Password { get; set; }
        public string Captcha { get; set; }
    }
}
