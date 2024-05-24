using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SIPSorcery.Net;

namespace CloudGB.Web.WebRTC
{
    public class NegotiationHub: Hub
    {

        private readonly WebRTCService _webRTCService;

        public NegotiationHub([FromServices] WebRTCService webRTCService)
        {
            _webRTCService = webRTCService;
        }

        /// <summary>
        ///     新websocket连接建立，创建RTCPeerConnection，并发送SDPOffer
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            GameConnection conn = await _webRTCService.CreatePeerConnection();
            RTCSessionDescriptionInit sdpOffer = conn.PC.createOffer(null);
            await conn.PC.setLocalDescription(sdpOffer);
            await Clients.Caller.SendAsync("SDPOffer", conn.ID, sdpOffer.toJSON());
        }

        /// <summary>
        ///     接收客户端发送的SDPAnswer
        /// </summary>
        /// <param name="id">服务端分配给客户端连接的ID</param>
        /// <param name="message">SDPAnswer json</param>
        /// <returns></returns>
        public void SDPAnswer(string id, string message)
        {
            GameConnection? conn = _webRTCService.GetConnection(id);
            if (conn == null)
            {
                return;
            }
            RTCSessionDescriptionInit sdpAnswer;
            if (!RTCSessionDescriptionInit.TryParse(message, out sdpAnswer))
            {
                Console.WriteLine("invalid sdp format");
                return;
            }
            conn.PC.setRemoteDescription(sdpAnswer);
        }

        /// <summary>
        ///     接收客户端发送的ICE Candidate
        /// </summary>
        /// <param name="id">服务端分配给客户端连接的ID</param>
        /// <param name="message">ICECandidate JSON</param>
        public void ICECandidate(string id, string message)
        {

            GameConnection? conn = _webRTCService.GetConnection(id);
            if (conn == null)
            {
                return;
            }
            RTCIceCandidateInit candidate;
            if (!RTCIceCandidateInit.TryParse(message, out candidate))
            {
                Console.WriteLine("invalid ICE Candidate");
                return;
            }
            Console.WriteLine(candidate);
            conn.PC.addIceCandidate(candidate);
        }
    }
}
