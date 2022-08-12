using AutoMapper;
using BookingApp.Rooms.DAL.Context;
using BookingApp.Rooms.DAL.Entities;
using BookingApp.Rooms.Domain.Dtos;
using BookingApp.Rooms.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Rooms.DAL.Repositories
{
    public class RoomsRepository : BaseCrudRepository<Room, RoomDto>, IRoomsRepository
    {
        public RoomsRepository(IDbContextFactory<RoomsContext> dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }
    }
}
