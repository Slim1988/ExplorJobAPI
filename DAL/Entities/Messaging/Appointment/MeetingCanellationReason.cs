using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExplorJobAPI.DAL.Entities.Messaging.Appointment
{
    public enum MeetingCanellationReason
    {
        InterlocutorAbsent = 0,
        LastMinCancellation = 1,
        Other = 2
    }
}
