using System;
using System.Net;
using System.Windows.Documents;
using ZdravoCorp.Enums;
using ZdravoCorp.Serializer;

namespace ZdravoCorp.Model
{
    public class TimeSlot 
    {
        public DateTime Start { get; set; }
        public int Duration { get; set; }

        public TimeSlot()
        {

        }
        public TimeSlot(DateTime start, int duration)
        {
            Start = start;
            Duration = duration;
        }
        public bool OverlapsWith(TimeSlot timeSlot)
        {
            return Start < timeSlot.End() && End() > timeSlot.Start;
        }

        public DateTime End()
        {
            return Start.AddMinutes(Duration);
        }


    }


}