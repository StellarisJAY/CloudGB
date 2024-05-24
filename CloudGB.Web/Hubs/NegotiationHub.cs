using CloudGB.Web.Models;
using CloudGB.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SIPSorcery.Net;

namespace CloudGB.Web.Hubs
{
    public class NegotiationHub : Hub
    {
        private readonly RoomService _roomService;

        public NegotiationHub([FromServices] RoomService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>
        ///     新websocket连接建立，创建RTCPeerConnection，并发送SDPOffer
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            HttpContext? httpCtx = Context.GetHttpContext();
            if (httpCtx == null) { return; }
            string? token = (string?)(httpCtx.Items["access_token"]);
            string? roomId = (string?)(httpCtx.Items["room_id"]);
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(roomId))
            {
                return;
            }
            var rid = Convert.ToInt64(roomId);
            RoomSession? session = _roomService.GetRoomSession(rid);
            if (session == null)
            {
                Console.WriteLine("room not found");
                return;
            }
            GameConnection conn = await session.CreatePeerConnection();
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
        public void SDPAnswer(string roomId, string id, string message)
        {
            var rid = Convert.ToInt64(roomId);
            RoomSession? session = _roomService.GetRoomSession(rid);
            if (session == null)
            {
                Console.WriteLine("room not found");
                return;
            }
            GameConnection? conn = session.GetConnection(id);
            if (conn == null)
            {
                Console.WriteLine("connection not found");
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
        public void ICECandidate(string roomId, string id, string message)
        {
            var rid = Convert.ToInt64(roomId);
            RoomSession? session = _roomService.GetRoomSession(rid);
            if (session == null)
            {
                Console.WriteLine("room not found");
                return;
            }
            GameConnection? conn = session.GetConnection(id);
            if (conn == null)
            {
                Console.WriteLine("connection not found");
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
