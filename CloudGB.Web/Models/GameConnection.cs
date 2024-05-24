using SIPSorcery.Net;

namespace CloudGB.Web.Models
{
    public class GameConnection
    {
        public readonly string ID;
        public readonly RTCPeerConnection PC;

        public GameConnection(RTCPeerConnection pc)
        {
            PC = pc;
            ID = Guid.NewGuid().ToString();
        }

        public void OnDataChannel(RTCDataChannel channel)
        {

        }
    }
}
