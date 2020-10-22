using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public enum ReservationDateError
    {
        None,
        StartInvalid,
        EndInvalid,
        LengthInvalid,
        Conflicting
    }
}
