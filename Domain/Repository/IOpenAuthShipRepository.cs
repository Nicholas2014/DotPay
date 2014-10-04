﻿using FC.Framework.Repository;
using DotPay.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotPay.Domain.Repository
{
    public interface IOpenAuthShipRepository : IRepository
    {
        OpenAuthShip FindByOpenID(string openID, OpenAuthType openAuthType);
        OpenAuthShip FindByUserID(int userID);
    }
}
