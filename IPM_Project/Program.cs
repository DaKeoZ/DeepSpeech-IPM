namespace IPM_Project
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // Calls the singleton IPMVocal
            IPMVocal ipmVocal = IPMVocal.GetInstance();
        }
    }
}