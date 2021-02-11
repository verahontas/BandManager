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

        //teszthez kell parameterless ctor
        /*public ReservationDateValidator()
        {

        }*/

        public ReservationDateValidator(EasyRehearsalManagerContext context)
        {
            _context = context; //itt is inkább a reservationService-t kéne használni
            //_reservationService = reservationService;
        }

        /// <summary>
        /// Determines wheter the reservation that we want to create or modify
        /// conflicts with an existing reservation.
        /// Conflict can be caused by conflicting time period or booked equipments.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="roomId"></param>
        /// <param name="action"></param>
        /// <param name="reservationId"></param>
        /// <param name="equipments"></param>
        /// <returns></returns>
        public ReservationDateError Validate(DateTime start, DateTime end, int roomId, string action, int reservationId, Dictionary<string, bool> equipments)
        {
            #region Checking the reservation's date and time
            if (end < start)
                return ReservationDateError.EndInvalid;

            if (end == start)
                return ReservationDateError.LengthInvalid;

            switch (action)
            {
                case "create":
                    if (_context.Reservations.Where(l => l.RehearsalRoomId == roomId && l.End >= start)
                                        .ToList()
                                        .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    break;
                case "edit":
                    if (_context.Reservations.Where(l => l.Id != reservationId && l.RehearsalRoomId == roomId && l.End >= start)
                                    .ToList()
                                    .Any(l => l.IsConflicting(start, end)))
                        return ReservationDateError.Conflicting;
                    //if we edit an existing reservation, then we don't want to compare it to itself.
                    //e.g. we want to change the date from 2-4 pm. to 3-4 pm.
                    //or we just want to modify the equipments
                    //'l.End >= start' comparison is just to tighten the data set.
                    break;
            }
            #endregion

            #region Checking the equipments
            //we only have to check the equipments if any of them are selected
            if (equipments.ContainsValue(true))
            {
                //Determines whether all the selected equipments available at the selected time

                RehearsalRoom currentRoom = _context.Rooms.Include(l => l.Reservations).Include(l => l.Studio).Include(l => l.Studio.Equipments).FirstOrDefault(l => l.Id == roomId);

                RehearsalStudio currentStudio = currentRoom.Studio;

                //oldConflictingReservations: all reservations that conflict (date) with the current reservation
                //AND made to the same studio
                //AND there are equipments booked for it
                //(so after we collected them, we need to check whether the equipments have available pieces)
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

                    //make sure that the given equipments actually do exist...
                    //in theory the user can select only the given and existing equipments,
                    //but if in the equipments dictionary there is an invalid equipment name,
                    //then the following line would give a nullException

                    bool allExist = true;
                    foreach (var eq in equipments)
                    {
                        allExist = allExist && EquipmentExist(eq.Key, currentStudio.Id);
                    }

                    if (!allExist)
                        return ReservationDateError.EquipmentNotAvailable;

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
            }
            #endregion

            return ReservationDateError.None;
        }

        private bool EquipmentExist(string equipmentName, int studioId)
        {
            if (_context.Equipments.Where(l => l.Name == equipmentName && l.StudioId == studioId).Any())
            {
                return true;
            }

            return false;
        }
    }
}
