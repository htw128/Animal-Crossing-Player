namespace AK
{
    public class EVENTS
    {
        public static uint PLAY_CITY = 3366829419U;
        public static uint PLAY_MIDI = 1916619709U;
        public static uint SET_STATE_NONE = 4160240018U;
        public static uint SET_STATE_RAINY = 2716087331U;
        public static uint SET_STATE_SNOWY = 1986417150U;
        public static uint SET_STATE_SUNNY = 3776166869U;
    } // public class EVENTS

    public class STATES
    {
        public class DAY_AND_NIGHT
        {
            public static uint GROUP = 3284788872U;

            public class STATE
            {
                public static uint DAY = 311764537U;
                public static uint NIGHT = 1011622525U;
                public static uint NONE = 748895195U;
            } // public class STATE
        } // public class DAY_AND_NIGHT

        public class MUSEUM
        {
            public static uint GROUP = 1362148849U;

            public class STATE
            {
                public static uint ART = 865867148U;
                public static uint ENTRANCE = 2656882895U;
                public static uint FISH = 2695658327U;
                public static uint FOSSIL = 1812458901U;
                public static uint INSECT = 3534600765U;
                public static uint NONE = 748895195U;
            } // public class STATE
        } // public class MUSEUM

        public class ORIENTATIONS
        {
            public static uint GROUP = 1131084240U;

            public class STATE
            {
                public static uint NONE = 748895195U;
                public static uint STAGE_01 = 344719509U;
                public static uint STAGE_02 = 344719510U;
                public static uint STAGE_03 = 344719511U;
                public static uint STAGE_04 = 344719504U;
                public static uint STAGE_05 = 344719505U;
                public static uint STAGE_06 = 344719506U;
                public static uint STAGE_07 = 344719507U;
            } // public class STATE
        } // public class ORIENTATIONS

        public class WEATHER
        {
            public static uint GROUP = 317282339U;

            public class STATE
            {
                public static uint NONE = 748895195U;
                public static uint RAINY = 2599410036U;
                public static uint SNOWY = 3252420805U;
                public static uint SUNNY = 3569642402U;
            } // public class STATE
        } // public class WEATHER

    } // public class STATES

    public class SWITCHES
    {
        public class ISCLOSING
        {
            public static uint GROUP = 3760575214U;

            public class SWITCH
            {
                public static uint FALSE = 2452206122U;
                public static uint TRUE = 3053630529U;
            } // public class SWITCH
        } // public class ISCLOSING

        public class ISSPEAKER
        {
            public static uint GROUP = 377710816U;

            public class SWITCH
            {
                public static uint FALSE = 2452206122U;
                public static uint TRUE = 3053630529U;
            } // public class SWITCH
        } // public class ISSPEAKER

        public class ISUPGRADE
        {
            public static uint GROUP = 3928355299U;

            public class SWITCH
            {
                public static uint FALSE = 2452206122U;
                public static uint TRUE = 3053630529U;
            } // public class SWITCH
        } // public class ISUPGRADE

        public class MUSEUM_ROOM
        {
            public static uint GROUP = 1867804341U;

            public class SWITCH
            {
                public static uint FIRST = 998496889U;
                public static uint SECOND = 3476314365U;
                public static uint THIRD = 931160808U;
            } // public class SWITCH
        } // public class MUSEUM_ROOM

        public class NEWYEARSTATES
        {
            public static uint GROUP = 3948904236U;

            public class SWITCH
            {
                public static uint AM0 = 1117531557U;
                public static uint AM2 = 1117531559U;
                public static uint PM11 = 2531051618U;
                public static uint PM11_30 = 3043478152U;
                public static uint PM11_50 = 3144143898U;
                public static uint PM11_55 = 3144143903U;
            } // public class SWITCH
        } // public class NEWYEARSTATES

        public class TIME
        {
            public static uint GROUP = 2654366170U;

            public class SWITCH
            {
                public static uint AM_01 = 3883188347U;
                public static uint AM_02 = 3883188344U;
                public static uint AM_03 = 3883188345U;
                public static uint AM_04 = 3883188350U;
                public static uint AM_05 = 3883188351U;
                public static uint AM_06 = 3883188348U;
                public static uint AM_07 = 3883188349U;
                public static uint AM_08 = 3883188338U;
                public static uint AM_09 = 3883188339U;
                public static uint AM_10 = 3899965933U;
                public static uint AM_11 = 3899965932U;
                public static uint AM_12 = 3899965935U;
                public static uint PM_01 = 4100092806U;
                public static uint PM_02 = 4100092805U;
                public static uint PM_03 = 4100092804U;
                public static uint PM_04 = 4100092803U;
                public static uint PM_05 = 4100092802U;
                public static uint PM_06 = 4100092801U;
                public static uint PM_07 = 4100092800U;
                public static uint PM_08 = 4100092815U;
                public static uint PM_09 = 4100092814U;
                public static uint PM_10 = 4083315220U;
                public static uint PM_11 = 4083315221U;
                public static uint PM_12 = 4083315222U;
            } // public class SWITCH
        } // public class TIME

    } // public class SWITCHES

    public class GAME_PARAMETERS
    {
        public static uint DISTANCE_TO_RESIDENT = 1184364711U;
        public static uint TIME = 2654366170U;
        public static uint VOLUME = 2415836739U;
    } // public class GAME_PARAMETERS

    public class BANKS
    {
        public static uint INIT = 1355168291U;
        public static uint DEFAULT = 782826392U;
    } // public class BANKS

    public class BUSSES
    {
        public static uint MASTER_AUDIO_BUS = 3803692087U;
    } // public class BUSSES

    public class AUDIO_DEVICES
    {
        public static uint NO_OUTPUT = 2317455096U;
        public static uint STEREO = 3729966089U;
        public static uint SYSTEM = 3859886410U;
    } // public class AUDIO_DEVICES

}// namespace AK

