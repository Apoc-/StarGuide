namespace Util
{
    public static class UnitHelper
    {
        public static float MS_SPEED_OF_LIGHT = 299792458f; //m/s
        public static float AU_H_SPEED_OF_LIGHT = 7.21436f; //AU/h
        public static float AU = 149597870.7f; //km

        public static float KmToAu(float km)
        {
            return km / AU;
        }

        public static float AuToKm(float au)
        {
            return au * AU;
        }
    }
}