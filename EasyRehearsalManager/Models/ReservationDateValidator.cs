using EasyRehearsalManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationDateValidator
    {
        private readonly EasyRehearsalManagerContext _context;

        public ReservationDateValidator(EasyRehearsalManagerContext context)
        {
            _context = context;
        }

        public ReservationDateError Validate(DateTime start, DateTime end, int roomId, string action, int reservationId)
        {
            if (end < start)
                return ReservationDateError.EndInvalid;

            if (end == start)
                return ReservationDateError.LengthInvalid;

            RehearsalRoom selectedRoom = _context.Rooms.FirstOrDefault(l => l.Id == roomId);

            if (selectedRoom == null)
                return ReservationDateError.None;

            switch (action)
            {
                case "create":
                    if (_context.Reservations.Where(l => l.RehearsalRoomId == selectedRoom.Id && l.End >= start)
                                        .ToList()
                                        .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    break;
                case "edit":
                    if (_context.Reservations.Where(l => l.Id != reservationId && l.RehearsalRoomId == selectedRoom.Id && l.End >= start)
                                    .ToList()
                                    .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    //itt azért nem kell nézni a reservationId-t, mert önmagával konfliktálhat az a foglalás,
                    //amit éppen módosítani akarok.
                    //pl. van 14-16-ig egy foglalásom és 14-15-re szeretném módosítani.
                    break;
            }

            return ReservationDateError.None;
        }
    }
}
