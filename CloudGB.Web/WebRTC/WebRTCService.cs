using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;

namespace CloudGB.Web.WebRTC
{
    public class WebRTCService
    {
        private readonly Dictionary<string, GameConnection> _connections = [];
        private readonly HashSet<string> _activeConns = [];
        private readonly object _connMutex = new();
        private readonly ILogger _logger;

        public readonly List<VideoFormat> SupportedVideoFormats = [
            new VideoFormat(VideoCodecsEnum.VP8, 96),
            ];
        public readonly List<AudioFormat> SupportedAudioFormats = [
            new AudioFormat(AudioCodecsEnum.OPUS, 97),
            ];

        public WebRTCService(ILogger<WebRTCService> logger)
        {
            _logger = logger;
        }

        public Task<GameConnection> CreatePeerConnection()
        {
            RTCPeerConnection pc = new();
            MediaStreamTrack videoTrack = new(SupportedVideoFormats);
            pc.addTrack(videoTrack);
            MediaStreamTrack audioTrack = new(SupportedAudioFormats);
            pc.addTrack(audioTrack);

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
            lock(_connMutex)
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
