﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC.Framework.Domain;
using FC.Framework;
using DotPay.MainDomain.Events;
using DotPay.Common;
using FC.Framework.Utilities;
using FC.Framework.Repository;
using DotPay.MainDomain.Exceptions;

namespace DotPay.MainDomain
{
    public class NXTWithdrawProcessLog : OperateLog
    {
        #region ctor
        protected NXTWithdrawProcessLog() { }

        public NXTWithdrawProcessLog(int withdrawID, int processorID, string memo) : base(withdrawID, string.Empty, memo, processorID, string.Empty) { }
        #endregion

    }
}