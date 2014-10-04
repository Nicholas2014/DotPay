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
    public class STRAccount : Account
    {
        #region ctor
        protected STRAccount() { }

        public STRAccount(int userID) : base(userID) { }
        #endregion
    }
}
