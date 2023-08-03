using Avalonia.Collections;
using Emulator_PPU.Models;
using ReactiveUI;
using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Emulator_PPU.ViewModels
{
    public class RecieveMessage : ReactiveObject
    {
        private byte[]? _message;
        private string? _dateTime;

        public byte[]? message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public string? dateTime
        {
            get => _dateTime;
            set => this.RaiseAndSetIfChanged(ref _dateTime, value);
        }

        public RecieveMessage(byte[] data)
        {
            message = data;
            // dateTime = DateTime.Now.ToString();

            // dateTime = TimeOnly.FromDateTime(DateTime.Now).ToString();

            dateTime = DateTime.Now.ToLongTimeString();
        }
    }

    public class ViewModelBase : ReactiveObject
    {
        private PPU _ppu = new();
        UdpClient udpServer;
        private AvaloniaList<RecieveMessage>? _recieveMsg;
        private AvaloniaList<RecieveMessage>? _transmitMsg;

        public AvaloniaList<RecieveMessage>? recieveMsg
        {
            get => _recieveMsg;
            set => this.RaiseAndSetIfChanged(ref _recieveMsg, value);
        }
        public AvaloniaList<RecieveMessage>? transmitMsg
        {
            get => _transmitMsg;
            set => this.RaiseAndSetIfChanged(ref _transmitMsg, value);
        }


        public PPU ppu
        {
            get => _ppu;
            set => this.RaiseAndSetIfChanged(ref _ppu, value);
        }

        public ViewModelBase()
        {
            recieveMsg = new();
            transmitMsg = new();
            udpServer = new UdpClient(60000);
            UdpServer_tick();
        }

        async public Task UdpServer_tick()
        {
            while (true)
            {
                var result = await udpServer.ReceiveAsync();
                //var ip = result.RemoteEndPoint;
                // 192.168.10.2 // 5000

                byte[] arr = result.Buffer;
//                message = Encoding.UTF8.GetString(result.Buffer);
                if (result.Buffer != null)
                {
                    recieveMsg.Insert(0, new(arr));
                    if (recieveMsg.Count > 3600) recieveMsg.RemoveAt(recieveMsg.Count - 1);
                    //                    parseMsg(arr);
                    if (arr[0] == 24) // команда запроса статуса
                    {
                        byte[] arr_ans = new byte[10];
                        arr_ans[0] = 24;

                        short ppu_status = ppu.PPU_getStatus();

                        arr_ans[2] = (byte)ppu_status;
                        arr_ans[3] = (byte)(ppu_status >> 8);
                        byte[] channels_status = new byte[6];
                        for (int i = 0; i < 6; i++)
                        {
                            arr_ans[i+4] = ppu.Channel_getStatus(i);
                        }
                        transmitMsg.Insert(0, new(arr_ans));
                        if (transmitMsg.Count > 3600) transmitMsg.RemoveAt(transmitMsg.Count - 1);

                        IPEndPoint ip = result.RemoteEndPoint;
                        int bytes = udpServer.Send(arr_ans, ip);
                    }
                }
            }
        }
    }
}