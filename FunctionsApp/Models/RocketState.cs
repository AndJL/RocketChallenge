using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace FunctionsApp.Models
{
    public class RocketState
    {
        public ObjectId _id { get; set; }
        public string RocketId { get; set; }
        public int Speed { get; set; }
        public string Mission { get; set; }
        public string Type { get; set; }
        public string LastTransmissionMsg { get; set; }
        public int MessageNumber { get; set; }
        public DateTime Updated { get; set; }
        public List<RocketMessage> History { get; set; }
    }
}