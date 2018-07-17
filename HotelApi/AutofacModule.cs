using Application;
using Application.Interfaces;
using Autofac;
using Infrastructure;
using Infrastructure.BookingDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelApi
{
    public class AutofacModule : Module
    {
        private string hotelConnectionString;

        public AutofacModule(string hotelConnectionString)
        {
            this.hotelConnectionString = hotelConnectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HotelBookings>();
            builder.Register(c => new BookingRepository(this.hotelConnectionString)).As<IBookingRepo>();
            builder.Register(c => new BookingDao(this.hotelConnectionString)).As<IBookingDao>();
        }
    }
}
