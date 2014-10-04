﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework;
using FC.Framework.Domain;
using DotPay.Domain.Events;
using DotPay.Domain.Exceptions;
using DotPay.Common;

namespace DotPay.Domain
{
    public class CNYAccount : Account
    {
        #region ctor
        protected CNYAccount() { }

        public CNYAccount(int userID) : base(userID) { }
        #endregion
    }
}
