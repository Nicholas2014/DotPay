﻿//#define TAOBAODEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework;
using DFramework.Utilities;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using ConfigurationManagerWrapper = DFramework.ConfigurationManagerWrapper;
using Task = System.Threading.Tasks.Task;

namespace Dotpay.TaobaoMonitor
{
    internal class TaobaoUtils
    {
        private const string TaobaoSessionKey = "_taobao_session";
        private const string ApiKey = "23089573";
        private const string ApiSecret = "7b4cb6afdd716c3fb4a4d0dd98b8d593";
        private const string TaobaoRestUrl = "http://gw.api.taobao.com/router/rest";
        private static bool hasSession = false;
        private static DateTime? lastNoticeAt;
        private static ITopClient client = new DefaultTopClient(TaobaoRestUrl, ApiKey, ApiSecret);
        private static object noticeLocker = new object();

#if TAOBAODEBUG
        private static string _debugSession;
        private static List<Trade> DebugTrades = new List<Trade>();
        private static object locker = new object();
        private static long tidSeed = 885168847931951;
        private static int counter = 0;
#endif

#if TAOBAODEBUG
        public static void StartGenerateTaobaoTrade()
        {
            new Thread(() =>
            {
                while (true)
                {
                    var random = new Random();
                    var randomNum = random.Next(80, 120);

                    lock (locker)
                    {

                        var randomNumAmount = random.Next(1, 100);
                        var randomNumHasMessage = random.Next(1, 100);
                        //tid,status,total_fee,has_buyer_message,orders.title
                        ++counter;
                        var totalFee = (randomNumAmount * 10).ToString();
                        var buyerMessage = string.Empty;
                        if (randomNumHasMessage >= 10 && randomNumHasMessage < 20)
                            buyerMessage = "一个错误的用户留言";
                        else if (randomNumHasMessage >= 20 && randomNumHasMessage < 30)
                            buyerMessage = "r3iQifspCXQXrBsaTKy3vWw6zWTB3VWSTL";
                        else if (randomNumHasMessage >= 30)
                            buyerMessage = "rUSweLuRhP8xWd11FsnEjbDo8hJPcvnuLm";

                        var trade = new Trade()
                        {
                            Tid = tidSeed + counter,
                            Status = "WAIT_SELLER_SEND_GOODS",
                            BuyerNick = "test" + randomNumHasMessage,
                            TotalFee = totalFee,
                            PayTime = DateTime.Now.ToString(),
                            HasBuyerMessage = !string.IsNullOrEmpty(buyerMessage),
                            BuyerMessage = buyerMessage,
                            Orders = new List<Order>()
                            {
                               //tid,total_fee,buyer_message
                               new Order()
                               {
                                    TotalFee = totalFee
                               }
                            }
                        };

                        DebugTrades.Add(trade);
                        Log.Info("GenerateTaobaoTrade-->amount=" + trade.TotalFee + ",message=" + trade.BuyerMessage + ",tid=" + trade.Tid + ",hasmsg=" + !string.IsNullOrEmpty(buyerMessage));

                    }
                    Log.Debug("-->已生成{0}条淘宝交易,其中目前Dic中持有{1}条,已成功处理{2}", counter, DebugTrades.Count, counter - DebugTrades.Count);
                    Task.Delay(randomNum * 1000).Wait();
                }
            }).Start();

            Log.Info("AutoGenerateTaobaoTrade Startd");
        }
#endif


        public static string GetTaobaoSession()
        {
#if TAOBAODEBUG
            if (string.IsNullOrEmpty(_debugSession))
            {
                _debugSession = Guid.NewGuid().ToString();
                hasSession = true;
            }

            return _debugSession;
#else
            var taobaoSession = Cache.Get<string>(TaobaoSessionKey);

            if (string.IsNullOrWhiteSpace(taobaoSession)) hasSession = false;
            else hasSession = true;

            return taobaoSession;
#endif

        }

        /// <summary>
        /// 获取最近一个小时内，订单状态发生了变化的订单
        /// </summary>
        /// <param name="sessionKey"></param>
        /// <returns></returns>
        public static List<Trade> GetIncrementTaobaoTrade(string sessionKey)
        {
#if TAOBAODEBUG
            lock (locker)
            {
                return DebugTrades.Where(t => t.Status == "WAIT_SELLER_SEND_GOODS").ToList();
            }
#else
            TradesSoldIncrementGetRequest req = new TradesSoldIncrementGetRequest();
            req.Fields = "tid,status,buyer_nick,pay_time,total_fee,has_buyer_message,orders.title";

            DateTime start = DateTime.Now.AddHours(-1);
            req.StartModified = start;
            DateTime end = DateTime.Now;
            req.EndModified = end;
            req.Type = "fixed";
            req.ExtType = "service";
            req.PageNo = 1L;
            req.PageSize = 100L;
            req.UseHasNext = true;

            var response = client.Execute(req, sessionKey);

            if (response.IsError)
            {
                Log.Error("GetCompletePaymentOrder Error:" + response.ErrMsg + "--code=" + response.ErrCode);
            }

            return response.Trades;
#endif
        }

        public static Trade GetTradeFullInfo(long tid, string sessionKey)
        {
#if TAOBAODEBUG
            lock (locker)
            {
                return DebugTrades.SingleOrDefault(t => t.Tid == tid);
            }
#else
            TradeFullinfoGetRequest req = new TradeFullinfoGetRequest();
            req.Fields = "tid,total_fee,buyer_message";
            req.Tid = tid;
            TradeFullinfoGetResponse response = client.Execute(req, sessionKey);

            if (response.IsError)
            {
                Log.Error("GetTradeFullInfo Error:" + response.ErrMsg + "--code=" + response.ErrCode);
            }

            return response.Trade;
#endif
        }
        public static bool SendGoods(long tid, string sessionKey)
        {
#if TAOBAODEBUG
            lock (locker)
            {
                var exist = DebugTrades.SingleOrDefault(t => t.Tid == tid);
                if (exist != null)
                {
                    DebugTrades.Remove(exist);
                    Log.Info(tid + "淘宝订单发货完毕");
                }

                return true;
            }
#else
            LogisticsDummySendRequest req = new LogisticsDummySendRequest();
            req.Tid = tid;
            LogisticsDummySendResponse response = client.Execute(req, sessionKey);

            if (response.IsError)
            {
                Log.Error("SendGoods Error:" + response.ErrMsg + "--code=" + response.ErrCode);
            }
            else
            {
                Log.Info(tid + "淘宝订单发货完毕");
            }
            return !response.IsError;
#endif
        }


        public static void NoticeWebMaster()
        {
            lock (noticeLocker)
            {
                if (!hasSession && ((lastNoticeAt.HasValue && lastNoticeAt.Value.AddMinutes(10) > DateTime.Now) || !lastNoticeAt.HasValue))
                {
                    var mails = ConfigurationManagerWrapper.AppSettings["noticeMails"];
                    var mailList = mails.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    Log.Info("taobao session time out ,notice webmaster");
                    lastNoticeAt = DateTime.Now;

                    if (mailList.Any())
                    {
                        mailList.ForEach(m =>
                        {
                            EmailHelper.SendMailAsync(m, "taobao session 超时", "点击<a href='https://www.dotpay.co/taobao/login' >https://www.dotpay.co<a/>进行授权");
                        });
                    }
                }
            }
        }
    }
}