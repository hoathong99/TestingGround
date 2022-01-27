using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using MHPQ.Authorization.Roles;
using MHPQ.Authorization.Users;
using MHPQ.MultiTenancy;
using MHPQ.EntityDb;
using MHPQ.Friendships;
using MHPQ.Chat;
using MHPQ.Storage;
using MHPQ.RoomChats;


namespace MHPQ.EntityFrameworkCore
{
    public class MHPQDbContext : AbpZeroDbContext<Tenant, Role, User, MHPQDbContext>
    {
        #region DbSet

        #region Chathub
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<RoomChat> RoomChats { get; set; }
        public virtual DbSet<RoomUserChat> RoomUserChats { get; set; }
        public virtual DbSet<RoomMessage> RoomMessages { get; set; }
        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }
        #endregion

        #region City
        public virtual DbSet<ProblemSystem> ProblemSystems { get; set; }
        public virtual DbSet<CityNotification> CityNotifications { get; set; }
        public virtual DbSet<UserFeedback> UserFeedbacks { get; set; }
        public virtual DbSet<UserFeedbackComment> UserFeedbackComments { get; set; }
        public virtual DbSet<Citizen> Citizens { get; set; }
        public virtual DbSet<CityNotificationComment> CityNotificationComments { get; set; }

        //fee
        public virtual DbSet<UserBill> UserBills { get; set; }
        public virtual DbSet<BillMappingType> BillMappingTypes { get; set; }
        public virtual DbSet<BillViewSetting> BillViewSettings { get; set; }
        #endregion

        #region News
        public virtual DbSet<News> News { get; set; }
        #endregion

        #region PitchBooking
        public virtual DbSet<PitchBooking> PitchBooking { get; set; }
        #endregion
        #region Smarthome
     
        public virtual DbSet<HouseOwner> HouseOwners { get; set; }
        public virtual DbSet<House> Houses { get; set; }
        public virtual DbSet<SmartHome> SmartHomes { get; set; }
        public virtual DbSet<FloorSmartHome> FloorSmartHomes { get; set; }
        public virtual DbSet<RoomSmartHome> RoomSmartHomes { get; set; }
        public virtual DbSet<DeviceApi> DeviceApis { get; set; }
        public virtual DbSet<SmarthomeApi> SmarthomeApis { get; set; }
        public virtual DbSet<DeviceSomfy> DeviceSomfies { get; set; }
        public virtual DbSet<HomeDevice> HomeDevices { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<HomeServer> HomeServers { get; set; }
        public virtual DbSet<HomeServerSomfy> HomeServerSomfies { get; set; }
        public virtual DbSet<HomeGateway> HomeGateways { get; set; }
        public virtual DbSet<LightDevice> LightDevices { get; set; }
        public virtual DbSet<CurtainDeivce> CurtainDeivces { get; set; }
        public virtual DbSet<ConditionerDevice> ConditionerDevices { get; set; }
        public virtual DbSet<ConnectionDevice> ConnectionDevice { get; set; }
        public virtual DbSet<DoorEntryDevice> DoorEntryDevices { get; set; }
        public virtual DbSet<FireAlarmDevice> FireAlarmDevices { get; set; }
        public virtual DbSet<WatterDevice> WatterDevices { get; set; }
        public virtual DbSet<AirDevice> AirDevices { get; set; }
        public virtual DbSet<SwitchDevice> SwitchDevices { get; set; }
        public virtual DbSet<SecurityDevice> CamDevices { get; set; }
        public virtual DbSet<Theme> Themes { get; set; }
        public virtual DbSet<Site> Sites { get; set; }
        public virtual DbSet<SensorDevice> SensorDevices { get; set; }

        #region ThuePhong
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<RoomHotel> RoomHotel { get; set; }
        public virtual DbSet<BookingRoomHotel> BookingRoomHotels { get; set; }
        public virtual DbSet<GuestHotel> GuestHotels { get; set; }

        #endregion

        #region DichVu
        public virtual DbSet<UnlimitedSpaceServices> UnlimitedSpaceServices { get; set; }
        public virtual DbSet<LimitedSpaceServices> LimitedSpaceServices { get; set; }
        public virtual DbSet<GuestService> GuestServices { get; set; }
        public virtual DbSet<Hires> Hires { get; set; }

        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<ObjectPartner> ObjectPartners { get; set; }
        public virtual DbSet<ObjectType> ObjectTypes { get; set; }
        public virtual DbSet<BusinessNotify> BusinessNotifies { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<SetItems> SetItems { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<ItemViewSetting> ItemViewSettings { get; set; }
        #endregion

        #endregion

        #region CommunityService
        public virtual DbSet<FootballPitch> FootballPitch { get; set; }
        #endregion

        public virtual DbSet<WebPushEndPoint> WebPushEndPoints { get; set; }

        #endregion

        public MHPQDbContext(DbContextOptions<MHPQDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

           
            /* Configure your own tables/entities inside the ConfigureMPQ method */
            builder.Entity<RoomUserChat>()
                .HasKey(t => new { t.UserId, t.RoomChatId });


            builder.Entity<RoomUserChat>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.RoomUserChats)
                .HasForeignKey(pt => pt.UserId);

            builder.Entity<RoomUserChat>()
                .HasOne(pt => pt.RoomChat)
                .WithMany(t => t.RoomUserChats)
                .HasForeignKey(pt => pt.RoomChatId);


            //builder.ConfigureMPQ();
        }
    }
}