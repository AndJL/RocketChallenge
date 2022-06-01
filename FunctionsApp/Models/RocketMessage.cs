using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace FunctionsApp.Models
{
    public class RocketMessage
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public Metadata Metadata { get; set; }
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Type { get; set; }
        public int LaunchSpeed { get; set; }
        public string Mission { get; set; }
        public int By { get; set; }
        public string Reason { get; set; }
        public string NewMission { get; set; }
    }

    public class Metadata
    {
        public string Channel { get; set; }
        public int MessageNumber { get; set; }
        public DateTime MessageTime { get; set; }
        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        RocketLaunched = 0,
        RocketSpeedIncreased,
        RocketSpeedDecreased,
        RocketExploded,
        RocketMissionChanged,
    }
}