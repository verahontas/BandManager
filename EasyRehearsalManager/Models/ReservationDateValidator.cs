using EasyRehearsalManager.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyRehearsalManager.Web.Models
{
    public class ReservationDateValidator
    {
        private readonly EasyRehearsalManagerContext _context;
        //private readonly IReservationService _reservationService;

        public ReservationDateValidator(EasyRehearsalManagerContext context)
        {
            _context = context; //itt is inkább a reservationService-t kéne használni
            //_reservationService = reservationService;
        }

        public ReservationDateError Validate(DateTime start, DateTime end, int roomId, string action, int reservationId, Dictionary<string, bool> equipments)
        {
            if (end < start)
                return ReservationDateError.EndInvalid;

            if (end == start)
                return ReservationDateError.LengthInvalid;

            RehearsalRoom currentRoom = _context.Rooms.Include(l => l.Reservations).Include(l => l.Studio).Include(l => l.Studio.Equipments).FirstOrDefault(l => l.Id == roomId);

            RehearsalStudio currentStudio = currentRoom.Studio;

            List<int> oldConflictingReservations = new List<int>();
            
            foreach (var resEqPair in _context.ReservationEquipmentPairs) //végigmegyünk a már meglévő foglalásokon, amikhez eszközt is béreltek
            {
                if (resEqPair.StudioId == currentStudio.Id && equipments.ContainsKey(resEqPair.EquipmentName)) //ha ebbe a stúdióba szól a foglalás, és olyan eszközt béreltek hozzá, amit mi is akarunk
                {
                    Reservation oldReservation = _context.Reservations.FirstOrDefault(l => l.Id == resEqPair.ReservationId); //akkor megkeressük ezt a foglalást
                    if (oldReservation.IsConflicting(start, end)) //ha ez a foglalás konfliktál a mienkkel, akkor hozzáadjuk
                    {
                        oldConflictingReservations.Add(oldReservation.Id); //ebbe gyűjtjük azokat, amiknek meg kell nézni egyenként hogy milyen eszközt béreltek hozzá
                    }
                }
            }

            int conflictingReservationsWithSameEquipments = 0;
            foreach (string eqName in equipments.Keys) //végigmegyünk azokon az eszközökön, amiket most bérelni szeretnénk
            {
                /*
                * minden eszköznévre végig kell számolni, hogy hányszor lett kibérelve. 
                * ha ez annyi, mint amennyi a stúdióban rendelkezésre áll, akkor hibát kell dobni
                *
                */

                conflictingReservationsWithSameEquipments =
                            _context.ReservationEquipmentPairs
                            .Where(l => oldConflictingReservations.Contains(l.ReservationId) && l.EquipmentName == eqName).Count();

                int availableInStudio = _context.Studios
                                            .FirstOrDefault(l => l.Id == currentStudio.Id)
                                            .Equipments.FirstOrDefault(l => l.Name == eqName)
                                            .QuantityAvailable;

                if (conflictingReservationsWithSameEquipments >= availableInStudio)
                {
                    return ReservationDateError.EquipmentNotAvailable;
                }

                conflictingReservationsWithSameEquipments = 0;
            }

            /*
            itt kell csekkolni az adatbázist az equipment reservations pair táblában
             */
            //if (selectedRoom == null)
               // return ReservationDateError.None;

            //menjünk végig az adott stúdió összes termének összes foglalásán, válasszuk ki az aktuálissal konfliktálókat
            //majd ezekből gyűjtsük össze, hogy melyik eszközből hányat béreltek ki az adott időpontban
            //ha annyiszor van már kibérelve, ahány összesen elérhető a próbahelyen, akkor hibát kell dobni a foglalásra

            //RehearsalStudio studio = _reservationService.GetStudioByRoomId(roomId);
            /*
            List<Reservation> reservations = new List<Reservation>();

            foreach (var room in currentStudio.Rooms)
            {
                foreach (var reservation in _context.Reservations.Include(l => l.Equipments).Where(l => l.RehearsalRoomId == room.Id))
                {
                    if (reservation.IsConflicting(start, end)) //ebben benne lesz az aktuális foglalás is
                    {
                        reservations.Add(reservation);
                    }
                }
            }

            Dictionary<string, int> reservedEquipmentsInCurrentTime = new Dictionary<string, int>();

            foreach (var reservation in reservations)
            {
                foreach (var equipment in reservation.Equipments)
                {
                    if (!reservedEquipmentsInCurrentTime.ContainsKey(equipment.Name))
                    {
                        reservedEquipmentsInCurrentTime[equipment.Name] = 1;
                    }
                    else
                    {
                        reservedEquipmentsInCurrentTime[equipment.Name] += 1;
                    }
                }
            }

            foreach (var equipment in reservedEquipmentsInCurrentTime) //végigmegyek az adott időpontban már lefoglalt eszközökön
            {
                //és megnézem hogy az aktuális foglalásban van-e olyan, amit más máskorra is kibéreltek
                if (equipments.ContainsKey(equipment.Key) && equipments[equipment.Key] == true && equipment.Value > currentStudio.Equipments.FirstOrDefault(l => l.Name == equipment.Key).QuantityAvailable) //itt az kell hogy equipment.Value > összeselérhető, mert úgy gyűjtöttem a konfliktálókat, hogy az aktuális is benne van
                {
                    //és ha valamit már máskorra is kibéreltek annyiszor, ahány az eszközből rendelkezésre áll, akkor hibát dobunk
                    return ReservationDateError.EquipmentNotAvailable;
                }
            }
            */
            switch (action)
            {
                case "create":
                    if (_context.Reservations.Where(l => l.RehearsalRoomId == currentRoom.Id && l.End >= start)
                                        .ToList()
                                        .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    break;
                case "edit":
                    if (_context.Reservations.Where(l => l.Id != reservationId && l.RehearsalRoomId == currentRoom.Id && l.End >= start)
                                    .ToList()
                                    .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    //itt azért nem kell nézni a reservationId-t, mert önmagával konfliktálhat az a foglalás,
                    //amit éppen módosítani akarok.
                    //pl. van 14-16-ig egy foglalásom és 14-15-re szeretném módosítani, vagy csak a bérelt eszközöket akarom módosítani
                    break;
            }

            return ReservationDateError.None;
        }
    }
}
