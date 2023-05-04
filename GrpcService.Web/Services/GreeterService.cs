using Google.Protobuf;
using Grpc.Core;
using GrpcService.Web;

namespace GrpcService.Web.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
        public override async Task SayHello3(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            var i = 0;
            //for (int i = 0; i < 6; i++)
            //{

            //}
            while(true)
            {
                //await requestStream.MoveNext();
                // 打印次数
                Console.WriteLine(i++);
                // 流式返回数据
                await responseStream.WriteAsync(new HelloReply
                {
                    Message = i+ DateTime.Now.ToString()
                });
                await Task.Delay(1000);
            }
        

            // 监听客户端数据输入
            //while (await requestStream.MoveNext())
            //{

            //}
        }
    }
}