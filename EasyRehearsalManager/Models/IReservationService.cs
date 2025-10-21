using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EasyRehearsalManager.Model;
using Microsoft.AspNetCore.Http;

namespace EasyRehearsalManager.Web.Models
{
    public interface IReservationService
    {
        #region Data sets

        IEnumerable<RehearsalStudio> Studios { get; }

        IEnumerable<RehearsalRoom> Rooms { get; }

        IEnumerable<Reservation> Reservations { get; }

        IEnumerable<Equipment> Equipments { get; }

        IEnumerable<ReservationEquipmentPair> ReservationEquipmentPairs { get; }

        IEnumerable<User> Users { get; }

        #endregion

        #region Operations with studios

        RehearsalStudio GetStudio(int? studioId);

        RehearsalStudio GetStudioByRoomId(int? roomId);

        IEnumerable<RehearsalStudio> GetStudiosByOwner(int? ownerId);

        int? GetStudioIdByEquipment(int? equipmentId);

        bool AddStudio(RehearsalStudio studio);

        bool RemoveStudio(int? studioId);

        bool UpdateStudio(RehearsalStudio studio);

        bool RehearsalStudioExist(int? studioId);

        byte[] GetStudioImage(int? imageId);

        byte[] GetLogo(int studioId);

        bool UploadImagesForStudio(int studioId, List<IFormFile> images);

        bool ChangeLogoForStudio(int studioId, IFormFile logo);

        List<int> GetImagesForStudio(int studioId);

        bool DeleteImagesForStudio(int studioId, List<int> images);

        #endregion

        #region Operations with rooms

        RehearsalRoom GetRoom(int? roomId);

        IEnumerable<RehearsalRoom> GetRoomsByOwnerId(int? ownerId);

        bool AddRoom(RehearsalRoom room);

        bool RemoveRoom(int? roomId);

        bool UpdateRoom(RehearsalRoom room);

        byte[] GetRoomImage(int? imageId);

        byte[] GetDefaultRoomImage(int? roomId);

        bool UploadImagesForRoom(int roomId, List<IFormFile> images);

        List<int> GetImagesForRoom(int roomId);

        bool DeleteImagesForRoom(int roomId, List<int> images);

        #endregion

        #region Operations with reservations

        Reservation GetReservation(int? reservationId);

        IEnumerable<Reservation> GetReservations(int userId, string role);

        IEnumerable<Reservation> GetReservationsByStudioId(int? studioId);

        bool RemoveReservation(int? reservationId);

        bool UpdateReservation(Reservation reservation);

        bool ReservationExist(int? reservationId);

        Task<bool> SaveReservationAsync(int? roomId, int userId, ReservationViewModel reservation);

        ReservationDateError ValidateReservation(DateTime start, DateTime end, string action, int reservationId, Dictionary<string, bool> equipments, int roomId);

        #endregion

        #region Operations with equipments

        Equipment GetEquipment(int? equipmentId);

        IEnumerable<Equipment> GetEquipmentsForStudio(int? studioId);

        bool AddEquipment(Equipment equipment);

        bool RemoveEquipment(int? equipmentId);

        bool UpdateEquipment(Equipment equipment);

        #endregion

        #region Operations with ReservationEquipmentPairs

        ReservationEquipmentPair GetReservationEquipmentPair(int? pairId);

        bool AddReservationEquipmentPair(int roomId, ReservationViewModel viewModel, int userId);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForStudio(int? studioId);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsByEquipment(string equipmentName);

        IEnumerable<ReservationEquipmentPair> GetReservationEquipmentPairsForReservarion(int? reservationId);

        bool UpdateReservationEquipmentTable(int reservationId, int studioId, Dictionary<string, bool> equipments);

        List<string> GetEquipmentNamesForReservation(int? reservationId);

        #endregion

        #region Operations with users

        byte[] GetUserImage(int? userId);

        bool UpdateProfilePicture(int userId, IFormFile image);

        #endregion
    }
}
