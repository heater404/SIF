using Prism.Events;
using SIFP.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIFP.Core.Mvvm
{
    public class WatchLogEvent : PubSubEvent<LogModel> { }

    public class NoticeLogEvent : PubSubEvent<LogModel> { }

    public class ConnectCameraReplyEvent : PubSubEvent<ConnectCameraReply> { }

    public class ConfigCameraReplyEvent : PubSubEvent<ConfigCameraReply> { }

    public class StopStreamingReplyEvent : PubSubEvent<StopStreamingReply> { };

    public class CloseWaitingDialogEvent : PubSubEvent { }

    public class OpenPointCloudEvent : PubSubEvent<string> { }

    public class ClosePointCloudEvent : PubSubEvent { }
}
