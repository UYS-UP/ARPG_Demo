using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;


public class ProtoHelper
{
    public static byte[] ToBytes(object message)
    {
        return ((Google.Protobuf.IMessage)message).ToByteArray();
    }




    public static T ToObject<T>(byte[] bytes) where T : Google.Protobuf.IMessage,new()
    {
        var message = new T();// Activator.CreateInstance<T>();
        message.MergeFrom(bytes);
        return message;
    }

    public static T Clone<T>(object message) where T : Google.Protobuf.IMessage, new()
    {
       return ToObject<T>(ToBytes(message));
    }

}
