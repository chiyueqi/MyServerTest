using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GrpcService.Web
{
    public class MyStreamContext
    {
        //public static List<IServerStreamWriter<RgvWarnReply>> WarnChannels = new List<IServerStreamWriter<RgvWarnReply>>();  
        public static List<WarnChannel> WarnChannels = new List<WarnChannel>();
        public static List<PlcInfos> TaskControl = new List<PlcInfos> { new PlcInfos { Id = 1, IsDispose = false }, new PlcInfos { Id = 2, IsDispose = false } };
        public static int Count = 0;
        public static System.Timers.Timer RGVTimer;
        //public static EventHandler<bool[]> PublishWarnEvent;
        public static List<ListenRgvResult> ListenResults { get; set; } = new List<ListenRgvResult>();
        public MyStreamContext()
        {
            RGVTimer = new System.Timers.Timer(5000);
            RGVTimer.Elapsed += RGVTimerHandler;
            RGVTimer.AutoReset = true;//是否重复执行

            //PublishWarnEvent +=
            RGVTimer.Start();
        }
        private void RGVTimerHandler(object sender, ElapsedEventArgs e)
        {
            bool[] result = new bool[2] { true, false };
            //发布报警信息
            PublishWarnEventHandler(result);


            ////Console.WriteLine(status[0]);
            //for (int i = 0; i < ListenResults.Count; i++)
            //{
            //    //    var dockConfig = ModbusTcpProvider.DockPlcConfigInfo.PortInfos.FirstOrDefault(x => x.Code == ListenResults[i].DockPortCode);

            //    if (ListenResults[i].ListenEvent == ListenRgvEventEnum.TakeSuccess)//如果监听伸出
            //    {
            //        //对个别请求，发送对应的结果通知
            //        if (DateTime.Now.Ticks % 3 == 0)
            //        {
            //            foreach (var item in WarnChannels)
            //            {
            //                if (item.PlcID == ListenResults[i].DockPortId)
            //                {
            //                    //if(item.RequestStream.Current..)
            //                    item.WriteAsync(new  RGVCommonReply 
            //                    { 
            //                        Message = item.PlcID + ":取货完成" + DateTime.Now.ToString()
            //                    });
            //                }

            //            }

            //        }
            //        //        if (status[dockConfig.MoveOutArriveListenAddress])//判断该对接口伸出是否到位
            //        //        {
            //        //            var n = new ListenResult { DockPortId = ListenResults[i].DockPortId, DockPortCode = ListenResults[i].DockPortCode, ListenEvent = ListenResults[i].ListenEvent, TaskId = ListenResults[i].TaskId };

            //        //            ListenResults.RemoveAt(i);
            //        //            i--;
            //        //            NotifyUI?.Invoke(null, n);
            //        //        }
            //    }
            //}


        }

        private void PublishWarnEventHandler(bool[] warnInfos)
        {
            Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < WarnChannels.Count; i++)
                    {
                        var replay = new RgvWarnReply
                        {
                            Message = DateTime.Now.ToString()
                        }
                        ;
                        replay.WarnInfos.Add(warnInfos);
                        WarnChannels[i].ResponseStream.WriteAsync(replay);

                    } 
                }
                catch (Exception ex)
                {
                    Console.WriteLine("推送warn异常：" + ex.ToString());
                }
            });
        }
    }
    public class PlcInfos
    {
        public int Id { get; set; }
        public bool IsDispose { get; set; }
    }
    public class MyChannel
    {
        public int PlcID { get; set; }
        public IAsyncStreamReader<RGVMoveRequest> RequestStream { get; set; }
        public IServerStreamWriter<RGVCommonReply> ResponseStream { get; set; }

    }
    public class WarnChannel
    {
        public int PlcID { get; set; }
        public RGVMoveRequest Request { get; set; }
        public IServerStreamWriter<RgvWarnReply> ResponseStream { get; set; }
        public bool IsDisponse { get; set; } = false;
    }
    public class ListenRgvResult
    {
        public int DockPortId { get; set; }
        public string DockPortCode { get; set; }
        public int TaskId { get; set; }

        public ListenRgvEventEnum ListenEvent { get; set; }
    }
    public enum ListenRgvEventEnum
    {
        PutSuccess = 0,
        TakeSuccess = 1,
        Fork_Arrived = 5,
        Fork_Origin = 6,
        Fork_Moving = 7,
        MoveArrived = 8,
        Moveing = 9,

    }
}
