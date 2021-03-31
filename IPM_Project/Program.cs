namespace IPM_Project
{
    internal static class Program
    {
        /// <summary>
        /// Main method
        /// Creates an instance of IPMVocal
        /// </summary>
        /// <param name="args">Arguments passed to the program</param>
        public static void Main(string[] args)
        {
            IPMVocal ipmVocal = IPMVocal.GetInstance();
        }
    }
}