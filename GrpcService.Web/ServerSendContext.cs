using GrpcService.Web.Services;

namespace GrpcService.Web
{
    public class ServerSendContext
    {
        public static async void StartSend()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                //Console.WriteLine("Sending message to clients-start");
                RGVService.SendMessageToClients(DateTime.Now.ToString());
            }
        }
    }
}
