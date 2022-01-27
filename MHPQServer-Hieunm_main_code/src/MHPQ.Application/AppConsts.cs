namespace MHPQ
{
    public class AppConsts
    {
        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "gsKxGZ012HLL3MI5";

        /// <summary>
        /// Default type device string
        /// </summary>
        public const string LightingDevice = "Smart Lighting";
        public const string CurtainDevice = "Smart Curtain";
        public const string AirDevice = "Smart Air";
        public const string ConditionerDevice = "Smart Conditioner";
        public const string ConnectionDevice = "Smart Connection";
        public const string FireAlarmDevice = "Smart Fire Alarm";
        public const string DoorEntryDevice = "Smart Door Entry";
        public const string SecurityDevice = "Smart Security";
        public const string WaterDevice = "Smart Water";
    }
    public class State_Object
    {
        public const int New = 1;
        public const int Active = 2;
        public const int Refuse = 3;
        public const int Disable = 4;
    }
}
