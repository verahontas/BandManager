using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;

namespace EasyRehearsalManager.Web.Models
{
    public interface IReservationService
    {
        IEnumerable<RehearsalStudio> Studios { get; }

        IEnumerable<RehearsalRoom> Rooms { get; }

        IEnumerable<Reservation> Reservations { get; }

        IEnumerable<Equipment> Equipments { get; }

        RehearsalStudio GetStudio(int? studioId);

        RehearsalStudio GetStudioByRoomId(int? roomId);

        RehearsalRoom GetRoom(int? roomId);

        Reservation GetReservation(int? reservationId);

        IEnumerable<Equipment> GetEquipmentsForStudio(int? studioId);

        bool AddStudio(RehearsalStudio studio);

        bool AddRoom(RehearsalRoom room);

        bool AddReservation(Reservation reservation);

        bool RemoveStudio(int? studioId);

        bool RemoveRoom(int? roomId);

        bool RemoveReservation(int? reservationId);

        bool UpdateStudio(RehearsalStudio studio);

        bool UpdateRoom(RehearsalRoom room);

        bool UpdateReservation(Reservation reservation);

        IEnumerable<RehearsalStudio> GetStudiosByOwner(int? ownerId);

        IEnumerable<RehearsalRoom> GetRoomsByOwnerId(int? ownerId);

        IEnumerable<Reservation> GetReservations(int userId, string role);

        IEnumerable<Reservation> GetReservationsByStudioId(int? studioId);

        ReservationViewModel NewReservation(int? roomId);

        bool ReservationExist(int? reservationId);

        bool RehearsalStudioExist(int? studioId);

        Task<bool> SaveReservationAsync(int? roomId, string userName, ReservationViewModel reservation);

        ReservationDateError ValidateReservation(DateTime start, DateTime end, string action, int reservationId, int? roomId);

        IEnumerable<DateTime> GetReservationDates(int roomId, int year, int month);

        Byte[] GetUserImage(int? userId);

        /// <summary>
        /// Increases the NumberOfRooms property of the studio.
        /// </summary>
        /// <param name="roomId"></param>
        bool NewRoomAdded(int? studioId);
    }
}
