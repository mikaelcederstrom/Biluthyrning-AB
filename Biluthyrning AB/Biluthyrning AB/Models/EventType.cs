using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biluthyrning_AB.Models
{
    public enum EventType : int
    {
        CarBooked,
        CarReturned,
        CarWashed,
        CarServiced,
        CarAdded,
        CarRemoved,
        CustomerAdded,
        CustomerRemoved,
        MembershipUpgraded       
    }
}
