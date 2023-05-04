using Google.Protobuf;
using Grpc.Core;
using GrpcService.Web;
namespace GrpcService.Web.Services
{
    public class RGVService : RGVDeal.RGVDealBase
    {
        private readonly ILogger<RGVService> _logger;
        public RGVService(ILogger<RGVService> logger)
        {
            _logger = logger;
        }

        public override Task<RGVCommonReply> RGVMove(RGVMoveRequest request, ServerCallContext context)
        {
            return Task.FromResult(new RGVCommonReply
            {
                Message = "success:x= " + request.X + "y=" + request.Y + "z=" + request.Z,
                IsSuccess = true,
                Code = 0
            });

        }
        public override async Task RGVToMove(IAsyncStreamReader<RGVMoveRequest> requestStream, IServerStreamWriter<RGVCommonReply> responseStream, ServerCallContext context)
        {
            await requestStream.MoveNext();
            //var channel = MyStreamContext.Channels.FirstOrDefault(x => x.PlcID == requestStream.Current.PLCID);
            //if (channel == null)
            //{
            //    MyStreamContext.Channels.Add(new MyChannel { PlcID = requestStream.Current.PLCID, RequestStream = requestStream, ResponseStream = responseStream });
            //}
            //else
            //{
            //    channel.RequestStream = requestStream;
            //    channel.ResponseStream = responseStream;
            //}
            //if (requestStream.Current.ActionTypeIn == ActionType.ActionGateTake)
            //{
            //    MyStreamContext.ListenResults.Add(new ListenRgvResult { ListenEvent = ListenRgvEventEnum.TakeSuccess });
            //}
            //// 流式返回数据
            //await responseStream.WriteAsync(new RGVCommonReply
            //{
            //    Message = requestStream.Current.PLCID + ":" + DateTime.Now.ToString()
            //});

            //var i = 0;
            ////for (int i = 0; i < 6; i++)
            ////{

            ////}
            //while (true)
            //{
            //    //await                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          .MoveNext();
            //    // 打印次数
            //    //Console.WriteLine(i++);
            //    // 流式返回数据
            //    await responseStream.WriteAsync(new  RGVCommonReply 
            //    {
            //        Message =   DateTime.Now.ToString()
            //    });
            //    await Task.Delay(1000);
            //}


            // 监听客户端数据输入
            //while (await requestStream.MoveNext())
            //{

            //}
        }
        public override async Task GetRgvWarn(RGVMoveRequest request, IServerStreamWriter<RgvWarnReply> responseStream, ServerCallContext context)
        {
            var channel = MyStreamContext.WarnChannels.FirstOrDefault(x => x.PlcID == request.PLCID);
            if (channel != null)
            {
                MyStreamContext.Count++;
            }

            var newchannel = new WarnChannel { PlcID = request.PLCID, IsDisponse = false, Request = request, ResponseStream = responseStream };
            MyStreamContext.WarnChannels.Add(newchannel);

            Console.WriteLine($"PlcID={request.PLCID}开始线程{Thread.CurrentThread.ManagedThreadId}");
            int i = MyStreamContext.Count;
            while (i == MyStreamContext.Count)
            {


                //await                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          .MoveNext();
                // 打印次数
                //Console.WriteLine(i++);
                //// 流式返回数据
                //await responseStream.WriteAsync(new RgvWarnReply
                //{
                //    Message = DateTime.Now.ToString()
                //});

                await Task.Delay(1000);
            }
            Console.WriteLine($"PlcID={request.PLCID}释放线程{Thread.CurrentThread.ManagedThreadId}");
            if (channel != null)
            {

                MyStreamContext.WarnChannels.Remove(channel);
            }
        }

        public override async Task RGVTest(EmptyRequest request, IServerStreamWriter<MessageResponse> responseStream, ServerCallContext context)
        {
            try
            {
                //  Console.WriteLine($"Received message from client: {request.Message}");
                //  if (_sendResponseStream == null)
                //  {
                //      _sendResponseStream = responseStream;
                //  }
                //await    _sendResponseStream.WriteAsync(new MessageResponse { Message = DateTime.Now.ToString() });
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine( request.Message);
                    await responseStream.WriteAsync(new MessageResponse { Message = " server Data" + DateTime.Now });
                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RGVTest异常：" + ex.ToString());
            }

        }
        private static IServerStreamWriter<MessageResponse> _sendResponseStream;
        public static void SendMessageToClients(string message)
        {
            if (_sendResponseStream != null)
            {
                Console.WriteLine($"Sending message to clients-{message}");
                _sendResponseStream.WriteAsync(new MessageResponse { Message = message });
            }
            //Console.WriteLine("Sending message to clients-end.");
        }
    }
}
