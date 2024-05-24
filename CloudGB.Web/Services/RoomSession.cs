using CloudGB.Core;
using CloudGB.Web.Models;
using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;

namespace CloudGB.Web.Services
{
    public class RoomSession
    {
        private readonly Dictionary<string, GameConnection> _connections = [];
        private readonly HashSet<string> _activeConns = [];
        private readonly object _connMutex = new();

        public static readonly List<VideoFormat> SupportedVideoFormats = [
            new VideoFormat(VideoCodecsEnum.VP8, 96),
            ];
        public static readonly List<AudioFormat> SupportedAudioFormats = [
            new AudioFormat(AudioCodecsEnum.OPUS, 97),
            ];

        private IEmulator _emulator;

        public RoomSession()
        {
            _emulator = new InterpreterEmulator();
        }

        public Task<GameConnection> CreatePeerConnection()
        {
            RTCPeerConnection pc = new();
            MediaStreamTrack videoTrack = new(SupportedVideoFormats);
            pc.addTrack(videoTrack);

            GameConnection conn = new(pc);

            pc.onconnectionstatechange += (state) =>
            {
                Console.WriteLine($"pc state: {state}");
                switch (state)
                {
                    case RTCPeerConnectionState.connected:
                        lock (_connMutex)
                        {
                            _activeConns.Add(conn.ID);
                            if (_activeConns.Count == 0)
                            {
                                Monitor.Pulse(_connMutex);
                            }
                        }
                        break;
                    case RTCPeerConnectionState.failed:
                        lock (_connMutex)
                        {
                            _connections.Remove(conn.ID);
                        }
                        break;
                    case RTCPeerConnectionState.disconnected:
                        lock (_connMutex)
                        {
                            _activeConns.Remove(conn.ID);
                        }
                        break;
                }
            };
            pc.ondatachannel += conn.OnDataChannel;
            lock (_connMutex)
            {
                _connections.Add(conn.ID, conn);
            }
            return Task.FromResult(conn);
        }

        public GameConnection? GetConnection(string id)
        {
            return _connections[id];
        }
    }
}
