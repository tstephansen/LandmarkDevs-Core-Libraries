namespace LandmarkDevs.Core.Prism
{
    /// <summary>
    ///     A static class containing the region name "MainRegion".
    /// </summary>
    public static class Regions
    {
        /// <summary>
        ///     Initializes static members of the <see cref="Regions"/> class.
        /// </summary>
        static Regions()
        {
            MainRegion = nameof(MainRegion);
        }
        /// <summary>
        ///     Gets or sets the main region.
        /// </summary>
        /// <value>The main region.</value>
        public static string MainRegion { get; set; }
    }
}