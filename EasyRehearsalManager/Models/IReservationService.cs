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

        IEnumerable<ReservationEquipmentPair> ReservationEquipmentPairs { get; }

        RehearsalStudio GetStudio(int? studioId);

        RehearsalStudio GetStudioByRoomId(int? roomId);

        RehearsalRoom GetRoom(int? roomId);

        Reservation GetReservation(int? reservationId);

        Equipment GetEquipment(int? equipmentId);

        ReservationEquipmentPair GetReservationEquipmentPair(int? pairId);

        IEnumerable<Equipment> GetEquipmentsForStudio(int? studioId);

        bool AddStudio(RehearsalStudio studio);

        bool AddRoom(RehearsalRoom room);

        bool AddReservation(Reservation reservation);

        bool AddEquipment(Equipment equipment);

        bool RemoveStudio(int? studioId);

        bool RemoveRoom(int? roomId);

        bool RemoveReservation(int? reservationId);

        bool RemoveEquipment(int? equipmentId);

        bool UpdateStudio(RehearsalStudio studio);

        bool UpdateRoom(RehearsalRoom room);

        bool UpdateReservation(Reservation reservation);

        bool UpdateEquipment(Equipment equipment);

        bool UpdateReservationEquipmentTable(int reservationId, int studioId, Dictionary<string, bool> equipments);

        IEnumerable<RehearsalStudio> GetStudiosByOwner(int? ownerId);

        IEnumerable<RehearsalRoom> GetRoomsByOwnerId(int? ownerId);

        IEnumerable<Reservation> GetReservations(int userId, string role);

        IEnumerable<Reservation> GetReservationsByStudioId(int? studioId);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForStudio(int? studioId);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsByEquipment(string equipmentName);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForReservarion(int? reservationId);

        int? GetStudioIdByEquipment(int? equipmentId);

        ReservationViewModel NewReservation(int? roomId);

        bool ReservationExist(int? reservationId);

        bool RehearsalStudioExist(int? studioId);

        Task<bool> SaveReservationAsync(int? roomId, string userName, ReservationViewModel reservation);

        ReservationDateError ValidateReservation(DateTime start, DateTime end, string action, int reservationId, Dictionary<string, bool> equipments, int roomId);

        IEnumerable<DateTime> GetReservationDates(int roomId, int year, int month);

        List<string> GetEquipmentNamesForReservation(int? reservationId);

        Byte[] GetUserImage(int? userId);

        /// <summary>
        /// Increases the NumberOfRooms property of the studio.
        /// </summary>
        /// <param name="roomId"></param>
        bool NewRoomAdded(int? studioId);
    }
}
