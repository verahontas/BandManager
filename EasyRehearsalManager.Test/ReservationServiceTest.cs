using EasyRehearsalManager.Model;
using EasyRehearsalManager.Web.Controllers;
using EasyRehearsalManager.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace EasyRehearsalManager.Test
{
    public class ReservationServiceTest
    {
        private readonly List<RehearsalStudio> _studios;
        private readonly List<RehearsalRoom> _rooms;
        private readonly List<Reservation> _reservations;
        private readonly List<Equipment> _equipments;
        private readonly List<ReservationEquipmentPair> _reservationEquipmenrPairs;

        private Mock<DbSet<RehearsalStudio>> _studioMock;
        private Mock<DbSet<RehearsalRoom>> _roomMock;
        private Mock<DbSet<Reservation>> _reservationMock;
        private Mock<DbSet<Equipment>> _equipmentMock;
        private Mock<DbSet<ReservationEquipmentPair>> _reservationEquipmentMock;
        private Mock<EasyRehearsalManagerContext> _entityMock;

        private ReservationDateValidator _validator;
        private ReservationService _service;

        public ReservationServiceTest()
        {
            var studiosData = new List<RehearsalStudio>
            {
                new RehearsalStudio { Id = 1, UserId = 1, Name = "Studio1", Address = "Address1", 
                    Equipments = new List<Equipment>
                    {
                        new Equipment
                        { 
                            Id = 1, StudioId = 1, Name = "Equipment1", QuantityAvailable = 1 
                        },
                        new Equipment 
                        { 
                            Id = 3, StudioId = 1, Name = "Equipment3", QuantityAvailable = 3 
                        }
                    } 
                },
                new RehearsalStudio { Id = 2, UserId = 2, Name = "Studio2", Address = "Address2" }
            };

            _studios = studiosData.Select(studio => new RehearsalStudio
            {
                Id = studio.Id,
                UserId = studio.UserId,
                Name = studio.Name,
                Address = studio.Address,
                Equipments = studio.Equipments
            }).ToList();

            var roomsData = new List<RehearsalRoom>
            {
                new RehearsalRoom { Id = 1, Number = 1, Price = 1000, Size = 10, StudioId = 1, Studio = studiosData.FirstOrDefault(l => l.Id == 1) },
                new RehearsalRoom { Id = 2, Number = 2, Price = 2000, Size = 20, StudioId = 1, Studio = studiosData.FirstOrDefault(l => l.Id == 1) },
                new RehearsalRoom { Id = 3, Number = 3, Price = 3000, Size = 30, StudioId = 1, Studio = studiosData.FirstOrDefault(l => l.Id == 1) },
                new RehearsalRoom { Id = 4, Number = 4, Price = 4000, Size = 40, StudioId = 2, Studio = studiosData.FirstOrDefault(l => l.Id == 2) }
            };

            _rooms = roomsData.Select(room => new RehearsalRoom
            {
                Id = room.Id,
                Number = room.Number,
                Price = room.Price,
                Size = room.Size,
                StudioId = room.StudioId,
                Studio = room.Studio
            }).ToList();

            var reservationsData = new List<Reservation> { 
                new Reservation { Id = 1, Start = new DateTime(2021, 1, 1, 10, 0, 0), End = new DateTime(2021, 1, 1, 11, 0, 0), UserId = 1, RehearsalRoomId = 1, RehearsalRoom = roomsData.FirstOrDefault(l => l.Id == 1) },
                new Reservation { Id = 2, Start = new DateTime(2021, 1, 2, 10, 0, 0), End = new DateTime(2021, 1, 2, 11, 0, 0), UserId = 2, RehearsalRoomId = 2, RehearsalRoom = roomsData.FirstOrDefault(l => l.Id == 2) },
                new Reservation { Id = 3, Start = new DateTime(2021, 1, 3, 10, 0, 0), End = new DateTime(2021, 1, 3, 11, 0, 0), UserId = 3, RehearsalRoomId = 4, RehearsalRoom = roomsData.FirstOrDefault(l => l.Id == 4) },
            };

            _reservations = reservationsData.Select(reservation => new Reservation
            {
                Id = reservation.Id,
                Start = reservation.Start,
                End = reservation.End,
                UserId = reservation.UserId,
                RehearsalRoomId = reservation.RehearsalRoomId,
                RehearsalRoom = reservation.RehearsalRoom
            }).ToList();

            var equipmentsData = new List<Equipment> {
                new Equipment { Id = 1, StudioId = 1, Name = "Equipment1", QuantityAvailable = 1, Studio = studiosData.FirstOrDefault(l => l.Id == 1) },
                new Equipment { Id = 2, StudioId = 2, Name = "Equipment2", QuantityAvailable = 2, Studio = studiosData.FirstOrDefault(l => l.Id == 2) },
                new Equipment { Id = 3, StudioId = 1, Name = "Equipment3", QuantityAvailable = 3, Studio = studiosData.FirstOrDefault(l => l.Id == 1) },
            };

            _equipments = equipmentsData.Select(equipment => new Equipment
            {
                Id = equipment.Id,
                StudioId = equipment.StudioId,
                Name = equipment.Name,
                QuantityAvailable = equipment.QuantityAvailable,
                Studio = equipment.Studio
            }).ToList();

            var reservationEquipmentPairsData = new List<ReservationEquipmentPair>
            {
                new ReservationEquipmentPair { Id = 1, StudioId = 1, EquipmentId = 1, EquipmentName = "Equipment1", ReservationId = 1},
                new ReservationEquipmentPair { Id = 2, StudioId = 1, EquipmentId = 2, EquipmentName = "Equipment2", ReservationId = 2},
                new ReservationEquipmentPair { Id = 3, StudioId = 1, EquipmentId = 3, EquipmentName = "Equipment3", ReservationId = 3},
            };

            _reservationEquipmenrPairs = reservationEquipmentPairsData.Select(pair => new ReservationEquipmentPair
            {
                Id = pair.Id,
                StudioId = pair.StudioId,
                EquipmentId = pair.EquipmentId,
                EquipmentName = pair.EquipmentName,
                ReservationId = pair.ReservationId
            }).ToList();

            #region Initializing mocked tables

            IQueryable<RehearsalStudio> queryableStudioData = studiosData.AsQueryable();
            _studioMock = new Mock<DbSet<RehearsalStudio>>();
            _studioMock.As<IQueryable<RehearsalStudio>>().Setup(mock => mock.ElementType).Returns(() => queryableStudioData.ElementType);
            _studioMock.As<IQueryable<RehearsalStudio>>().Setup(mock => mock.Expression).Returns(() => queryableStudioData.Expression);
            _studioMock.As<IQueryable<RehearsalStudio>>().Setup(mock => mock.Provider).Returns(() => queryableStudioData.Provider);
            _studioMock.As<IQueryable<RehearsalStudio>>().Setup(mock => mock.GetEnumerator()).Returns(() => studiosData.GetEnumerator());

            IQueryable<RehearsalRoom> queryableRoomData = roomsData.AsQueryable();
            _roomMock = new Mock<DbSet<RehearsalRoom>>();
            _roomMock.As<IQueryable<RehearsalRoom>>().Setup(mock => mock.ElementType).Returns(() => queryableRoomData.ElementType);
            _roomMock.As<IQueryable<RehearsalRoom>>().Setup(mock => mock.Expression).Returns(() => queryableRoomData.Expression);
            _roomMock.As<IQueryable<RehearsalRoom>>().Setup(mock => mock.Provider).Returns(() => queryableRoomData.Provider);
            _roomMock.As<IQueryable<RehearsalRoom>>().Setup(mock => mock.GetEnumerator()).Returns(() => roomsData.GetEnumerator());

            IQueryable<Reservation> queryableReservationData = reservationsData.AsQueryable();
            _reservationMock = new Mock<DbSet<Reservation>>();
            _reservationMock.As<IQueryable<Reservation>>().Setup(mock => mock.ElementType).Returns(() => queryableReservationData.ElementType);
            _reservationMock.As<IQueryable<Reservation>>().Setup(mock => mock.Expression).Returns(() => queryableReservationData.Expression);
            _reservationMock.As<IQueryable<Reservation>>().Setup(mock => mock.Provider).Returns(() => queryableReservationData.Provider);
            _reservationMock.As<IQueryable<Reservation>>().Setup(mock => mock.GetEnumerator()).Returns(() => reservationsData.GetEnumerator());

            IQueryable<Equipment> queryableEquipmentData = equipmentsData.AsQueryable();
            _equipmentMock = new Mock<DbSet<Equipment>>();
            _equipmentMock.As<IQueryable<Equipment>>().Setup(mock => mock.ElementType).Returns(() => queryableEquipmentData.ElementType);
            _equipmentMock.As<IQueryable<Equipment>>().Setup(mock => mock.Expression).Returns(() => queryableEquipmentData.Expression);
            _equipmentMock.As<IQueryable<Equipment>>().Setup(mock => mock.Provider).Returns(() => queryableEquipmentData.Provider);
            _equipmentMock.As<IQueryable<Equipment>>().Setup(mock => mock.GetEnumerator()).Returns(() => equipmentsData.GetEnumerator());

            IQueryable<ReservationEquipmentPair> queryableReservationEquipmentPairsData = reservationEquipmentPairsData.AsQueryable();
            _reservationEquipmentMock = new Mock<DbSet<ReservationEquipmentPair>>();
            _reservationEquipmentMock.As<IQueryable<ReservationEquipmentPair>>().Setup(mock => mock.ElementType).Returns(() => queryableReservationEquipmentPairsData.ElementType);
            _reservationEquipmentMock.As<IQueryable<ReservationEquipmentPair>>().Setup(mock => mock.Expression).Returns(() => queryableReservationEquipmentPairsData.Expression);
            _reservationEquipmentMock.As<IQueryable<ReservationEquipmentPair>>().Setup(mock => mock.Provider).Returns(() => queryableReservationEquipmentPairsData.Provider);
            _reservationEquipmentMock.As<IQueryable<ReservationEquipmentPair>>().Setup(mock => mock.GetEnumerator()).Returns(() => reservationEquipmentPairsData.GetEnumerator());

            #endregion

            #region Set up operations

            #region Studios
            _studioMock.Setup(mock => mock.Add(It.IsAny<RehearsalStudio>())).Callback<RehearsalStudio>(studio =>
            {
                studiosData.Add(studio);
            });

            _studioMock.Setup(mock => mock.Update(It.IsAny<RehearsalStudio>())).Callback<RehearsalStudio>(studio =>
            {
                //the given studio has the new/updated data
                RehearsalStudio oldStudio = studiosData.FirstOrDefault(l => l.Id == studio.Id);
                studiosData.Remove(oldStudio);
                studiosData.Add(studio);
            });

            _studioMock.Setup(mock => mock.Remove(It.IsAny<RehearsalStudio>())).Callback<RehearsalStudio>(studio =>
            {
                studiosData.Remove(studio);
                //_studioMock.As<IQueryable<RehearsalStudio>>().Setup(mock => mock.GetEnumerator()).Returns(() => studiosData.GetEnumerator());
            });
            #endregion

            #region Rooms
            _roomMock.Setup(mock => mock.Add(It.IsAny<RehearsalRoom>())).Callback<RehearsalRoom>(room =>
            {
                roomsData.Add(room);
            });

            _roomMock.Setup(mock => mock.Update(It.IsAny<RehearsalRoom>())).Callback<RehearsalRoom>(room =>
            {
                RehearsalRoom oldRoom = roomsData.FirstOrDefault(l => l.Id == room.Id);
                roomsData.Remove(oldRoom);
                roomsData.Add(room);
            });

            _roomMock.Setup(mock => mock.Remove(It.IsAny<RehearsalRoom>())).Callback<RehearsalRoom>(room =>
            {
                roomsData.Remove(room);
            });
            #endregion

            #region Reservations
            _reservationMock.Setup(mock => mock.Add(It.IsAny<Reservation>())).Callback<Reservation>(reservation =>
            {
                reservationsData.Add(reservation);
            });

            _reservationMock.Setup(mock => mock.Update(It.IsAny<Reservation>())).Callback<Reservation>(reservation =>
            {
                Reservation oldReservation = reservationsData.FirstOrDefault(l => l.Id == reservation.Id);
                reservationsData.Remove(oldReservation);
                reservationsData.Add(reservation);
            });

            _reservationMock.Setup(mock => mock.Remove(It.IsAny<Reservation>())).Callback<Reservation>(reservation =>
            {
                reservationsData.Remove(reservation);
                //_reservationMock.As<IQueryable<Reservation>>().Setup(mock => mock.GetEnumerator()).Returns(() => reservationsData.GetEnumerator());
            });
            #endregion

            #region Equipments
            _equipmentMock.Setup(mock => mock.Add(It.IsAny<Equipment>())).Callback<Equipment>(equipment =>
            {
                equipmentsData.Add(equipment);
            });

            _equipmentMock.Setup(mock => mock.Update(It.IsAny<Equipment>())).Callback<Equipment>(equipment =>
            {
                Equipment oldEquipment = equipmentsData.FirstOrDefault(l => l.Id == equipment.Id);
                equipmentsData.Remove(oldEquipment);
                equipmentsData.Add(equipment);
            });

            _equipmentMock.Setup(mock => mock.Remove(It.IsAny<Equipment>())).Callback<Equipment>(room =>
            {
                equipmentsData.Remove(room);
            });
            #endregion

            #region ReservationEquipmentPairs
            _reservationEquipmentMock.Setup(mock => mock.Add(It.IsAny<ReservationEquipmentPair>())).Callback<ReservationEquipmentPair>(reservationEquipmentPair =>
            {
                reservationEquipmentPairsData.Add(reservationEquipmentPair);
            });

            _reservationEquipmentMock.Setup(mock => mock.Update(It.IsAny<ReservationEquipmentPair>())).Callback<ReservationEquipmentPair>(reservationEquipmentPair =>
            {
                ReservationEquipmentPair oldPair = reservationEquipmentPairsData.FirstOrDefault(l => l.Id == reservationEquipmentPair.Id);
                reservationEquipmentPairsData.Remove(oldPair);
                reservationEquipmentPairsData.Add(reservationEquipmentPair);
            });

            _reservationEquipmentMock.Setup(mock => mock.Remove(It.IsAny<ReservationEquipmentPair>())).Callback<ReservationEquipmentPair>(reservationEquipmentPair =>
            {
                reservationEquipmentPairsData.Remove(reservationEquipmentPair);
            });
            #endregion

            #endregion

            _entityMock = new Mock<EasyRehearsalManagerContext>();
            _entityMock.Setup<DbSet<RehearsalStudio>>(entity => entity.Studios).Returns(_studioMock.Object);
            _entityMock.Setup<DbSet<RehearsalRoom>>(entity => entity.Rooms).Returns(_roomMock.Object);
            _entityMock.Setup<DbSet<Equipment>>(entity => entity.Equipments).Returns(_equipmentMock.Object);
            _entityMock.Setup<DbSet<Reservation>>(entity => entity.Reservations).Returns(_reservationMock.Object);
            _entityMock.Setup<DbSet<ReservationEquipmentPair>>(entity => entity.ReservationEquipmentPairs).Returns(_reservationEquipmentMock.Object);

            _validator = new ReservationDateValidator(_entityMock.Object);
            _service = new ReservationService(_entityMock.Object, _validator);
        }

        #region Get data sets

        [Fact]
        public void GetStudiosDataSetTest()
        {
            var result = _service.Studios;

            Assert.Equal(_studios.FirstOrDefault(l => l.Id == 1).Name, result.FirstOrDefault(l => l.Id == 1).Name);
            Assert.Equal(_studios.FirstOrDefault(l => l.Id == 2).Address, result.FirstOrDefault(l => l.Id == 2).Address);
            Assert.Equal(_studios.Count(), result.Count());
        }

        [Fact]
        public void GetRoomsDataSetTest()
        {
            var result = _service.Rooms;

            var expected = _rooms.FirstOrDefault(l => l.Id == 1);
            var actual = result.FirstOrDefault(l => l.Id == 1);

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Number, actual.Number);
            Assert.Equal(expected.Price, actual.Price);
            Assert.Equal(expected.Size, actual.Size);
            Assert.Equal(expected.StudioId, actual.StudioId);
            Assert.Equal(_rooms.Count(), result.Count());
        }

        [Fact]
        public void GetReservationsDataSetTest()
        {
            var result = _service.Reservations;

            Assert.Equal(_reservations.Count(), result.Count());
        }

        [Fact]
        public void GetEquipmentsDataSetTest()
        {
            var result = _service.Equipments;

            Assert.Equal(_equipments.Count(), result.Count());
            Assert.Equal(_equipments.FirstOrDefault(l => l.Id == 1).Name, result.FirstOrDefault(l => l.Id == 1).Name);
        }

        [Fact]
        public void GetReservationEquipmentPairsDataSetTest()
        {
            var result = _service.ReservationEquipmentPairs;

            Assert.Equal(_reservationEquipmenrPairs.Count(), result.Count());
        }

        #endregion

        #region Studio operations

        [Fact]
        public void GetStudioTest()
        {
            var result = _service.GetStudio(1);

            Assert.Equal(1, result.Id);
            Assert.Equal("Studio1", result.Name);
            Assert.Equal("Address1", result.Address);
        }

        [Fact]
        public void GetStudioByRoomIdTest()
        {
            var result = _service.GetStudioByRoomId(2);

            Assert.Equal(1, result.Id);
            Assert.Equal("Studio1", result.Name);
            Assert.Equal("Address1", result.Address);
        }

        [Fact]
        public void GetStudioByOwnerTest()
        {
            var result = _service.GetStudiosByOwner(1);

            Assert.Single(result);
            Assert.Contains("Studio1", result.Where(l => l.UserId == 1).Select(l => l.Name));
        }

        [Fact]
        public void AddStudioTest()
        {
            RehearsalStudio studio = new RehearsalStudio
            {
                Id = 3,
                UserId = 2,
                Name = "Studio2",
                Address = "Address2"
            };

            var result = _service.AddStudio(studio);

            Assert.Equal(_studios.Count + 1, _entityMock.Object.Studios.Count());
        }

        [Fact]
        public void AddStudioTest_NullValue()
        {
            var result = _service.AddStudio(null);

            Assert.True(result);
            Assert.Equal(_studios.Count + 1, _entityMock.Object.Studios.Count());
        }

        [Fact]
        public void RemoveStudioTest()
        {
            var result = _service.RemoveStudio(1);

            Assert.Equal(_studios.Count - 1, _entityMock.Object.Studios.Count());
            Assert.Equal(_studios.Count - 1, _studioMock.Object.Count());
        }

        [Fact]
        public void RemoveStudioTest_NotExistingStudio()
        {
            var result = _service.RemoveStudio(3);

            Assert.True(result);
            Assert.Equal(_studios.Count, _studioMock.Object.Count());
        }
        
        [Fact]
        public void UpdateStudioTest()
        {
            RehearsalStudio studio = new RehearsalStudio
            {
                Id = 1,
                UserId = 2,
                Name = "Studio2",
                Address = "Address2"
            };

            var result = _service.UpdateStudio(studio);

            Assert.True(result);
            Assert.Equal(_studios.Count, _studioMock.Object.Count());
            Assert.Equal("Studio2", _entityMock.Object.Studios.Where(l => l.Id == 1).FirstOrDefault().Name);
            Assert.Equal("Studio2", _studioMock.Object.Where(l => l.Id == 1).FirstOrDefault().Name);
            Assert.Equal("Address2", _entityMock.Object.Studios.Where(l => l.Id == 1).FirstOrDefault().Address);
        }

        [Fact]
        public void GetStudioIdByEquipmentTest()
        {
            var result = _service.GetStudioIdByEquipment(2);

            Assert.Equal(2, result);
        }

        [Fact]
        public void RehearsalStudioExistTest()
        {
            var result = _service.RehearsalStudioExist(2);

            Assert.True(result);
        }

        #endregion

        #region Room operations

        [Fact]
        public void GetRoomTest()
        {
            var result = _service.GetRoom(3);

            Assert.Equal(3, result.Id);
            Assert.Equal(3000, result.Price);
            Assert.Equal(1, result.StudioId);
        }

        [Fact]
        public void AddRoomTest()
        {
            RehearsalRoom room = new RehearsalRoom
            {
                Id = 5,
                Number = 4,
                Size = 40,
                Price = 4000,
                StudioId = 1
            };

            var result = _service.AddRoom(room);

            Assert.True(result);
            Assert.Equal(_rooms.Count + 1, _roomMock.Object.Count());
            Assert.Equal(4000, _entityMock.Object.Rooms.FirstOrDefault(l => l.Id == 5).Price);
        }

        [Fact]
        public void RemoveRoomTest()
        {
            var result = _service.RemoveRoom(2);

            Assert.True(result);
            Assert.Equal(_rooms.Count - 1, _roomMock.Object.Count());
            Assert.Null(_entityMock.Object.Rooms.FirstOrDefault(l => l.Id == 2));
        }

        [Fact]
        public void UpdateRoomTest()
        {
            RehearsalRoom room = new RehearsalRoom
            {
                Id = 2,
                Number = 4,
                Price = 4000,
                Size = 20,
                StudioId = 1
            };

            var result = _service.UpdateRoom(room);

            Assert.True(result);
            Assert.Equal(4, _entityMock.Object.Rooms.FirstOrDefault(l => l.Id == 2).Number);
            Assert.Equal(4000, _entityMock.Object.Rooms.FirstOrDefault(l => l.Id == 2).Price);
            Assert.Equal(20, _entityMock.Object.Rooms.FirstOrDefault(l => l.Id == 2).Size);
        }

        [Fact]
        public void GetRoomsByOwnerIdTest()
        {
            var result = _service.GetRoomsByOwnerId(1);

            Assert.Equal(_rooms.Count() - 1, result.Count()); //all rooms belong to the Id=1 studio EXCEPT the roomId=4 room
            Assert.Contains<int>(_rooms.FirstOrDefault(l => l.Id == 1).Id, result.Select(l => l.Id));
            Assert.Contains<int>(_rooms.FirstOrDefault(l => l.Id == 2).Id, result.Select(l => l.Id));
            Assert.Contains<int>(_rooms.FirstOrDefault(l => l.Id == 3).Id, result.Select(l => l.Id));
            Assert.DoesNotContain<int>(_rooms.FirstOrDefault(l => l.Id == 4).Id, result.Select(l => l.Id));
        }

        #endregion

        #region Reservation operations

        [Fact]
        public void GetReservationTest()
        {
            var result = _service.GetReservation(3);

            Assert.Equal(_reservations.FirstOrDefault(l => l.Id == 3).Start, result.Start);
            Assert.Equal(_reservations.FirstOrDefault(l => l.Id == 3).End, result.End);
        }

        [Fact]
        public void RemoveReservationTest()
        {
            var result = _service.RemoveReservation(2);

            Assert.True(result);
            Assert.Equal(_reservations.Count - 1, _reservationMock.Object.Count());
            Assert.Null(_reservationMock.Object.FirstOrDefault(l => l.Id == 2));
        }

        [Fact]
        public void UpdateReservationTest()
        {
            Reservation reservation = new Reservation
            {
                Id = 2,
                Start = new DateTime(2021, 1, 2, 10, 0, 0),
                End = new DateTime(2021, 2, 2, 11, 0, 0)
            };

            var result = _service.UpdateReservation(reservation);

            Assert.True(result);
            Assert.Equal(_reservations.Count, _entityMock.Object.Reservations.Count());
            Assert.Equal(new DateTime(2021, 2, 2, 11, 0, 0), _reservationMock.Object.FirstOrDefault(l => l.Id == 2).End);
        }

        [Fact]
        public void GetReservationsTest_ForMusician()
        {
            var result = _service.GetReservations(1, "musician");

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault(l => l.UserId == 1).Id);
        }

        [Fact]
        public void GetReservationsTest_ForOwner()
        {
            var result = _service.GetReservations(1, "owner");

            Assert.Equal(2, result.Count());
            Assert.Contains(1, result.Select(l => l.Id));
            Assert.Contains(new DateTime(2021, 1, 1, 10, 0, 0), result.Select(l => l.Start));
            Assert.DoesNotContain(new DateTime(2021, 1, 3, 11, 0, 0), result.Select(l => l.End));
        }

        [Fact]
        public void GetReservationsByStudioIdTest()
        {
            var result = _service.GetReservationsByStudioId(2);

            Assert.Single(result);
            Assert.Equal(3, result.FirstOrDefault(l => l.RehearsalRoom.StudioId == 2).Id); //there's only one matching reservation
        }

        [Fact]
        public void ReservationExistTest_ValidParam()
        {
            var result = _service.ReservationExist(2);

            Assert.True(result);

            result = _service.ReservationExist(3);

            Assert.True(result);
        }

        [Fact]
        public void ReservationExistTest_InvalidParam()
        {
            var result = _service.ReservationExist(5);

            Assert.False(result);
        }

        /// <summary>
        /// Testing the function that validates a reservation.
        /// In this testcase the result should be ReservationDateError.None.
        /// The tested function doesn't save the reservation. It is only the validation.
        /// roomId=1 -> this room is in StudioId=1 -> this studio has "Equipment1" and "Equipment3"
        /// </summary>
        [Fact]
        public void ValidateReservaionTest_ResultOK()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment1", true }
            };
            //reservationId in parameter doesn't matter as it is not used when creating new reservation
            var result = _service.ValidateReservation(new DateTime(2021, 2, 1, 10, 0, 0), new DateTime(2021, 2, 1, 11, 0, 0), "create", 1, dictionary, 1);

            Assert.Equal(ReservationDateError.None, result);
        }

        [Fact]
        public void ValidateReservationTest_EndInvalid()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment1", true }
            };

            var result = _service.ValidateReservation(new DateTime(2021, 2, 1, 10, 0, 0), new DateTime(2021, 2, 1, 9, 0, 0), "create", 1, dictionary, 1);

            Assert.Equal(ReservationDateError.EndInvalid, result);
        }

        [Fact]
        public void ValidateReservationTest_LengthInvalid()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment1", true }
            };

            var result = _service.ValidateReservation(new DateTime(2021, 2, 1, 10, 0, 0), new DateTime(2021, 2, 1, 10, 0, 0), "create", 1, dictionary, 1);

            Assert.Equal(ReservationDateError.LengthInvalid, result);
        }

        [Fact]
        public void ValidateReservationTest_ConflictingWhenCreate()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

            //this is an existing reservation:
            //new Reservation { Id = 1, Start = new DateTime(2021, 1, 1, 10, 0, 0), End = new DateTime(2021, 1, 1, 11, 0, 0), UserId = 1, RehearsalRoom = roomsData.FirstOrDefault(l => l.Id == 1) }
            var result = _service.ValidateReservation(new DateTime(2021, 1, 1, 10, 0, 0), new DateTime(2021, 1, 1, 11, 0, 0), "create", 1, dictionary, 1);

            Assert.Equal(ReservationDateError.Conflicting, result);
        }

        /// <summary>
        /// We want to modify the reservation with ID 2
        /// to the same date and room as reservation ID 1.
        /// </summary>
        [Fact]
        public void ValidateReservationTest_ConflictingWhenEdit()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

            //this is an existing reservation:
            //new Reservation { Id = 1, Start = new DateTime(2021, 1, 1, 10, 0, 0), End = new DateTime(2021, 1, 1, 11, 0, 0), UserId = 1, RehearsalRoom = roomsData.FirstOrDefault(l => l.Id == 1) }
            var result = _service.ValidateReservation(new DateTime(2021, 1, 1, 10, 0, 0), new DateTime(2021, 1, 1, 11, 0, 0), "edit", 2, dictionary, 1);

            Assert.Equal(ReservationDateError.Conflicting, result);
        }

        /// <summary>
        /// The given equipment does not exist in that studio.
        /// </summary>
        [Fact]
        public void ValidateReservationTest_EquipmentNotAvailable_NotExistingEquipment()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "szinti", true }
            };

            var result = _service.ValidateReservation(new DateTime(2021, 3, 3, 10, 0, 0), new DateTime(2021, 3, 3, 11, 0, 0), "create", 1, dictionary, 1);

            Assert.Equal(ReservationDateError.EquipmentNotAvailable, result);
        }

        /// <summary>
        /// The given equipment is already reserved in that time and studio.
        /// The roomId is different from the reservation Id=1 unless we would get Conflicting error.
        /// </summary>
        [Fact]
        public void ValidateReservationTest_EquipmentNotAvailable_AllReservedAlready()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment1", true }
            };

            var result = _service.ValidateReservation(new DateTime(2021, 1, 1, 10, 0, 0), new DateTime(2021, 1, 1, 11, 0, 0), "create", 1, dictionary, 2);

            Assert.Equal(ReservationDateError.EquipmentNotAvailable, result);
        }

        #endregion

        #region Equipment operations

        [Fact]
        public void GetEquipmentTest()
        {
            var result = _service.GetEquipment(2);

            Assert.Equal(2, result.Id);
            Assert.Equal("Equipment2", result.Name);
            Assert.Equal(2, result.QuantityAvailable);
        }

        [Fact]
        public void GetEquipmentsForStudioTest()
        {
            var result = _service.GetEquipmentsForStudio(1);

            Assert.Equal(_equipments.Where(l => l.StudioId == 1).Count(), result.Count());
            Assert.Contains("Equipment1", result.Select(l => l.Name));
            Assert.Contains("Equipment3", result.Select(l => l.Name));
        }

        [Fact]
        public void GetEquipmentsForStudioTest_NotExistingStudio()
        {
            var result = _service.GetEquipmentsForStudio(4);

            Assert.Empty(result);
        }

        [Fact]
        public void AddEquipmentTest()
        {
            Equipment equipment = new Equipment
            {
                Name = "Equipment20",
                StudioId = 2,
                QuantityAvailable = 3
            };

            var result = _service.AddEquipment(equipment);

            Assert.True(result);
            Assert.Equal(_equipments.Count + 1, _entityMock.Object.Equipments.Count());
            Assert.Contains("Equipment20", _entityMock.Object.Equipments.Select(l => l.Name));
        }

        [Fact]
        public void RemoveEquipmentStudio()
        {
            var result = _service.RemoveEquipment(1);

            Assert.True(result);
            Assert.Equal(_equipments.Count - 1, _entityMock.Object.Equipments.Count());
            Assert.DoesNotContain("Equipment1", _entityMock.Object.Equipments.Select(l => l.Name));
        }

        [Fact]
        public void UpdateEquipment()
        {
            Equipment equipment = _equipments.FirstOrDefault(l => l.Id == 3);
            equipment.StudioId = 2;
            equipment.Name = "Equipment2021";
            equipment.QuantityAvailable = 2021;

            var result = _service.UpdateEquipment(equipment);

            Assert.Equal(_equipments.Count, _entityMock.Object.Equipments.Count());
            Assert.Equal("Equipment2021", _entityMock.Object.Equipments.FirstOrDefault(l => l.Id == 3).Name);
            Assert.Equal(2021, _entityMock.Object.Equipments.FirstOrDefault(l => l.Id == 3).QuantityAvailable);
            Assert.Equal(2, _entityMock.Object.Equipments.FirstOrDefault(l => l.Id == 3).StudioId);
        }

        #endregion

        #region ReservationEquipmentPairs operaions

        [Fact]
        public void GetReservationEqiupmentPairTest_NullValue()
        {
            var result = _service.GetReservationEquipmentPair(null);

            Assert.Null(result);
        }

        [Fact]
        public void GetReservationEqiupmentPairTest_CorrectValue()
        {
            var result = _service.GetReservationEquipmentPair(2);

            Assert.Equal(_reservationEquipmenrPairs.FirstOrDefault(l => l.Id == 2).EquipmentName, result.EquipmentName);
            Assert.Equal(_reservationEquipmenrPairs.FirstOrDefault(l => l.Id == 2).StudioId, result.StudioId);
            Assert.Equal(_reservationEquipmenrPairs.FirstOrDefault(l => l.Id == 2).ReservationId, result.ReservationId);
        }

        [Fact]
        public void AddReservationEquipmentPairTest()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment1", true }
            };
            
            ReservationViewModel viewModel = new ReservationViewModel
            {
                Equipments = dictionary,
                //BandName = "",
                Day = new DateTime(2021, 1, 1, 0, 0, 0),
                StartHour = 10,
                EndHour = 11
            };

            var result = _service.AddReservationEquipmentPair(1, viewModel, 1);

            Assert.True(result);
            Assert.Equal(_reservationEquipmenrPairs.Count + 1, _entityMock.Object.ReservationEquipmentPairs.Count());
        }

        /// <summary>
        /// Updating the id=3 reservation.
        /// This reservations already has Equipment3 to borrow
        /// but we add Equipment1 also to the reservation.
        /// Equipment1 is also in the same studio (id=1).
        /// So: we add an element to the reservation-equipment table
        /// </summary>
        [Fact]
        public void UpdateReservationEquipmentTableTest_ReservePlusOneEquipment()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment3", true },
                { "Equipment1", true }
            };

            var result = _service.UpdateReservationEquipmentTable(3, 1, dictionary);

            Assert.True(result);
            Assert.Equal(_reservationEquipmenrPairs.Count + 1, _entityMock.Object.ReservationEquipmentPairs.Count());
        }

        [Fact]
        public void UpdateReservationEquipmentTableTest_CancelOneReservedEquipment()
        {
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>
            {
                { "Equipment3", false }
            };

            var result = _service.UpdateReservationEquipmentTable(3, 1, dictionary);

            Assert.True(result);
            Assert.Equal(_reservationEquipmenrPairs.Count - 1, _entityMock.Object.ReservationEquipmentPairs.Count());
            Assert.DoesNotContain(3, _entityMock.Object.ReservationEquipmentPairs.Select(l => l.Id));
        }

        [Fact]
        public void GetReservationEquipmentPairsForStudioTest_StudioIdNull()
        {
            var result = _service.GetReservationEquipmentPairsForStudio(null);
            Assert.Null(result);
        }

        [Fact]
        public void GetReservationEquipmentPairsForStudioTest_MultipleResults()
        {
            var result = _service.GetReservationEquipmentPairsForStudio(1);

            Assert.Equal(_reservationEquipmenrPairs.Where(l => l.StudioId == 1).Count(), result.Count());
            Assert.Contains("Equipment2", result.Select(l => l.EquipmentName));
            Assert.Contains("Equipment3", result.Select(l => l.EquipmentName));
        }

        [Fact]
        public void GetReservationEquipmentPairsByEquipment_NotExistingEquipment()
        {
            var result = _service.GetReservationEquipmentPairsByEquipment("notexistingequipment");

            Assert.Empty(result);
        }

        [Fact]
        public void GetReservationEquipmentPairsByEquipment_ExistingEquipment()
        {
            var result = _service.GetReservationEquipmentPairsByEquipment("Equipment3");

            Assert.Single(result);
            Assert.Contains(3, result.Select(l => l.ReservationId));
            Assert.DoesNotContain(1, result.Select(l => l.ReservationId));
        }

        [Fact]
        public void GetReservationEquipmentPairsForReservarion_NotExistingReservation()
        {
            var result = _service.GetReservationEquipmentPairsForReservarion(6);

            Assert.Empty(result);
        }

        [Fact]
        public void GetReservationEquipmentPairsForReservarion_ExistingReservation()
        {
            var result = _service.GetReservationEquipmentPairsForReservarion(2);

            Assert.Single(result);
            Assert.Contains("Equipment2", result.Select(l => l.EquipmentName));
            Assert.DoesNotContain("Equipment1", result.Select(l => l.EquipmentName));
        }

        [Fact]
        public void GetEquipmentNamesForReservation_NotExistingReservation()
        {
            var result = _service.GetEquipmentNamesForReservation(6);

            Assert.Empty(result);
        }

        [Fact]
        public void GetEquipmentNamesForReservation_ExistingReservation()
        {
            var result = _service.GetEquipmentNamesForReservation(1);

            Assert.Single(result);
            Assert.Contains("Equipment1", result);
            Assert.DoesNotContain("Equipment2", result);
            Assert.DoesNotContain("Equipment3", result);
        }

        #endregion
    }
}
