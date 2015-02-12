﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orleans.EventSourcing;
using Dotpay.Common;
using Dotpay.Actor.Interfaces;

namespace Dotpay.Actor.Events
{
    public class RippleToFinancialInstitutionInitialized : GrainEvent
    {
        public RippleToFinancialInstitutionInitialized(string invoiceId, TransferTargetInfo transferTargetInfo, decimal amount, decimal sendAmount, string memo)
        {
            this.InvoiceId = invoiceId;
            this.TransferTargetInfo = transferTargetInfo;
            this.Amount = amount;
            this.SendAmount = sendAmount;
            this.Memo = memo;
        }

        public string InvoiceId { get; set; }
        public TransferTargetInfo TransferTargetInfo { get; set; }
        public decimal Amount { get; set; }
        public decimal SendAmount { get; set; }
        public string Memo { get; set; }
    }

    public class RippleToFinancialInstitutionCompleted : GrainEvent
    {
        public RippleToFinancialInstitutionCompleted(string invoiceId, string txId,  decimal sendAmount )
        {
            this.InvoiceId = invoiceId;
            this.TxId = txId; 
            this.SendAmount = sendAmount; 
        }

        public string InvoiceId { get; set; }
        public string TxId { get; set; } 
        public decimal SendAmount { get; set; } 
    }

}
