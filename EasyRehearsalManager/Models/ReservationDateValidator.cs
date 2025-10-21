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
        
        public ReservationDateValidator(EasyRehearsalManagerContext context)
        {
            _context = context;
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
            #region Checking if start and end are valid
            if (end < start)
                return ReservationDateError.EndInvalid;

            if (end == start)
                return ReservationDateError.LengthInvalid;

            if (start < DateTime.Now)
                return ReservationDateError.StartInvalid;

            #endregion

            #region Checking studio's opening time

            RehearsalRoom currentRoom = _context.Rooms.Include(l => l.Reservations).Include(l => l.Studio).Include(l => l.Studio.Equipments).FirstOrDefault(l => l.Id == roomId);

            RehearsalStudio currentStudio = currentRoom.Studio;

            if (start.Hour < currentStudio.GetOpeningHour(start))
                return ReservationDateError.StartInvalid;

            if (end.Hour > currentStudio.GetClosingHour(end))
                return ReservationDateError.EndInvalid;

            #endregion

            #region Checking confliction

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

                

                //oldConflictingReservations: all reservations that conflict (date) with the current reservation
                //AND made to the same studio
                //AND there are equipments booked for it
                //(so after we collected them, we need to check whether the equipments have available pieces)
                List<int> oldConflictingReservations = new List<int>();

                foreach (var resEqPair in _context.ReservationEquipmentPairs) //iterate on reservations that already have some equipments booked
                {
                    if (resEqPair.StudioId == currentStudio.Id && equipments.ContainsKey(resEqPair.EquipmentName)) //if reservation is made in this studio and there is a kind of equipment booked, that we also want now
                    {
                        Reservation oldReservation = _context.Reservations.FirstOrDefault(l => l.Id == resEqPair.ReservationId); //then we search for this reservation
                        if (oldReservation.IsConflicting(start, end)) //if this is conflicting with our reservation, then we add
                        {
                            oldConflictingReservations.Add(oldReservation.Id); //in this we collect those reservations, that we have to check, which equipments are booked for these
                        }
                    }
                }

                int conflictingReservationsWithSameEquipments = 0;
                foreach (string eqName in equipments.Keys) //iterate on the equipments that we want to book now
                {
                    /*
                    * We have to calculate for each equipment, how many times are they booked.
                    * If this is greater or equal than the number of QuantityAvailable (how many items the studio have from this equipment)
                    * then we have to notify the user about this and we shouldn't approve the reservation.
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
