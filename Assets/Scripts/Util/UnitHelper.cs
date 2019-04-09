namespace Util
{
    public static class UnitHelper
    {
        public static float MS_SPEED_OF_LIGHT = 299792458f; //m/s
        public static float AU_H_SPEED_OF_LIGHT = 7.21436f; //AU/h
        public static float AU = 149597870.7f; //km
        public static float AUH_TO_KMS = 41555f; // 1 AU/h == 41555 km/s

        public static float KmToAu(float km)
        {
            return km / AU;
        }

        public static float AuToKm(float au)
        {
            return au * AU;
        }

        public static float KmsToAuh(float kms)
        {
            return kms / AUH_TO_KMS;
        }
        
        public static float AuhToKms(float auh)
        {
            return AUH_TO_KMS * auh;
        }
    }
}