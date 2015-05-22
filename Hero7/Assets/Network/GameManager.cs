using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using LitJson;
using Logic;
using MiscUtil.Conversion;
using ProtoBuf;
using RoyNet.GameServer.Entity;
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;

    private NetworkStream _stream;
    private TcpClient _client;
    private string _token;

    private readonly object _syncCmd = new object();
    private readonly Dictionary<int, ICommand> _commands = new Dictionary<int, ICommand>();
    public readonly ConcurrentQueue<Action> ActionQueue = new ConcurrentQueue<Action>();

    public static GameManager Instance
    {
        get { return _instance ?? (_instance = new GameManager()); }
    }

    public void RegisterCommand(ICommand command)
    {
        lock (_syncCmd)
        {
            _commands.Add(command.Name, command);
        }
    }

    public readonly List<Server> ServerList = new List<Server>();

    public void Load(JsonData jsonData)
    {
        ServerList.Clear();
        _token = jsonData["token"].ToString();
        var jsonList = jsonData["serverList"];
        for(int i=0; i<jsonList.Count; i++)
        {
            var j = jsonList[i];
            Server server = new Server()
            {
                DestID = (int)j["destID"],
                IP = j["iP"].ToString(),
                Name = j["name"].ToString(),
                Port = (int)j["port"]
            };
            ServerList.Add(server);
        }
    }

    /// <summary>
    /// 异步接收、线程池非UI线程
    /// </summary>
    /// <param name="stream"></param>
    void BeginRece(NetworkStream stream)
    {
        byte[] recedata = new byte[1024];
        stream.BeginRead(recedata, 0, recedata.Length, (a) =>
        {
            int receLength = stream.EndRead(a);
            if (receLength == 0)
            {
                //Log("服务器关闭了连接。可能是顶号。");
                Debug.Log("服务器关闭了连接。可能是顶号。");
                _client.Close();
            }
            else if (receLength == 1)
            {
                //Log("登录成功！");
                ActionQueue.Enqueue(() =>
                {
                    Application.LoadLevelAsync("mainScene");
                });
                BeginRece(stream);
            }
            else
            {
                var converter = EndianBitConverter.Big;
                int offset = 0;
                while (offset < receLength + 2)
                {
                    int length = converter.ToInt16(recedata, offset);
                    offset += 2;
                    int cmd = converter.ToInt32(recedata, offset);
                    offset += 4;
                    lock (_syncCmd)
                    {
                        ICommand recMsg;
                        if (_commands.TryGetValue(cmd, out recMsg))
                        {
                            using (var receMs = new MemoryStream())
                            {
                                receMs.Write(recedata, offset, length - 4);
                                receMs.Position = 0;
                                var package = recMsg.DeserializePackage(receMs);
                                ActionQueue.Enqueue(() =>
                                {
                                    recMsg.Execute(package);
                                });
                                //Debug.Log(package.Text);
                                offset += length;
                            }
                        }
                    }
                }
                BeginRece(stream);
            }
        }, null);
    }

    public void Enter(Server server)
    {
        if (_stream == null || !_stream.CanWrite)
        {
            _client = new TcpClient();
            _client.Connect(server.IP, server.Port);
            _stream = _client.GetStream();
            BeginRece(_stream);
        }

        var converter = EndianBitConverter.Big;
        byte[] body = Encoding.UTF8.GetBytes(_token);
        int length = body.Length;

        byte[] data = new byte[length + 10];
        int offset = 0;
        data[3] = 0x01; //cmd
        offset += 4;
        converter.CopyBytes((ushort)(length + 4), data, offset);
        offset += 2;
        converter.CopyBytes((int)CMD_Chat.Send, data, offset);
        offset += 4;
        Buffer.BlockCopy(body, 0, data, offset, length);
        _stream.Write(data, 0, data.Length);
    }

    public void Send(int cmd, object msg)
    {
        //if (_client.Connected && _stream.CanWrite)
        //{
            var converter = EndianBitConverter.Big;
            var msm = new MemoryStream();
            Serializer.Serialize(msm, msg);
            var body = msm.ToArray();
            int length = body.Length;

            byte[] data = new byte[length + 10];
            int offset = 0;
            data[3] = 0x02; //cmd
            offset += 4;
            converter.CopyBytes((ushort)(length + 4), data, offset);
            offset += 2;
            converter.CopyBytes(cmd, data, offset);
            offset += 4;
            Buffer.BlockCopy(body, 0, data, offset, length);
            _stream.Write(data, 0, data.Length);
        //}
    }
}

public class Server
    {
        public string Name { get; set; }

        public string IP { get; set; }

        public int Port { get; set; }

        public int DestID { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }