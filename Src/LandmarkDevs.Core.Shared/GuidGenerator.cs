using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.NetworkInformation;

namespace LandmarkDevs.Core.Shared
{
    /// <summary>
    /// Class GuidGenerator.
    /// Code for this file was taken from https://github.com/fluentcassandra/fluentcassandra/tree/master/src
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class GuidGenerator
    {
        private static readonly Random Random;
        private static readonly object Lock = new object();

        private static DateTimeOffset _lastTimestampForNoDuplicatesGeneration = TimestampHelper.UtcNow();

        // number of bytes in uuid
        private const int ByteArraySize = 16;

        // multiplex variant info
        private const int VariantByte = 8;

        private const int VariantByteMask = 0x3f;
        private const int VariantByteShift = 0x80;

        // multiplex version info
        private const int VersionByte = 7;

        private const int VersionByteMask = 0x0f;
        private const int VersionByteShift = 4;

        // indexes within the uuid array for certain boundaries
        private const byte TimestampByte = 0;

        private const byte GuidClockSequenceByte = 8;
        private const byte NodeByte = 10;

        // offset to move from 1/1/0001, which is 0-time for .NET, to gregorian 0-time of 10/15/1582
        private static readonly DateTimeOffset GregorianCalendarStart = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);

        /// <summary>
        /// Gets or sets the unique identifier generation.
        /// </summary>
        /// <value>The unique identifier generation.</value>
        public static GuidGeneration GuidGeneration { get; set; }

        /// <summary>
        /// Gets or sets the node bytes.
        /// </summary>
        /// <value>The node bytes.</value>
        public static byte[] NodeBytes { get; set; }

        /// <summary>
        /// Gets or sets the clock sequence bytes.
        /// </summary>
        /// <value>The clock sequence bytes.</value>
        public static byte[] ClockSequenceBytes { get; set; }

        /// <summary>
        /// Initializes static members of the <see cref="GuidGenerator"/> class.
        /// </summary>
        static GuidGenerator()
        {
            Random = new Random();

            GuidGeneration = GuidGeneration.NoDuplicates;
            NodeBytes = GenerateNodeBytes();
            ClockSequenceBytes = GenerateClockSequenceBytes();
        }

        /// <summary>
        /// Generates a random value for the node.
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateNodeBytes()
        {
            var node = new byte[6];

            Random.NextBytes(node);
            return node;
        }

        /// <summary>
        /// Generates a node based on the first 6 bytes of an IP address.
        /// </summary>
        /// <param name="ip"></param>
        public static byte[] GenerateNodeBytes(IPAddress ip)
        {
            if (ip == null)
                throw new ArgumentNullException(nameof(ip));

            var bytes = ip.GetAddressBytes();

            if (bytes.Length < 6)
                throw new ArgumentOutOfRangeException(nameof(ip), @"The passed in IP address must contain at least 6 bytes.");

            var node = new byte[6];
            Array.Copy(bytes, node, 6);

            return node;
        }

        /// <summary>
        /// Generates a node based on the bytes of the MAC address.
        /// </summary>
        /// <param name="mac"></param>
        /// <remarks>The machines MAC address can be retrieved from <see cref="NetworkInterface.GetPhysicalAddress"/>.</remarks>
        public static byte[] GenerateNodeBytes(PhysicalAddress mac)
        {
            if (mac == null)
                throw new ArgumentNullException(nameof(mac));

            var node = mac.GetAddressBytes();

            return node;
        }

        /// <summary>
        /// Generates a random clock sequence.
        /// </summary>
        public static byte[] GenerateClockSequenceBytes()
        {
            var bytes = new byte[2];
            Random.NextBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// In order to maintain a constant value we need to get a two byte hash from the DateTime.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateClockSequenceBytes(DateTime dt)
        {
            var utc = dt.ToUniversalTime();
            return GenerateClockSequenceBytes(utc.Ticks);
        }

        /// <summary>
        /// In order to maintain a constant value we need to get a two byte hash from the DateTime.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateClockSequenceBytes(DateTimeOffset dt)
        {
            var utc = dt.ToUniversalTime();
            return GenerateClockSequenceBytes(utc.Ticks);
        }

        /// <summary>
        /// In order to maintain a constant value we need to get a two byte hash from the DateTime.
        /// </summary>
        /// <param name="ticks">The ticks.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] GenerateClockSequenceBytes(long ticks)
        {
            var bytes = BitConverter.GetBytes(ticks);

            if (bytes.Length == 0)
                return new byte[] { 0x0, 0x0 };
            if (bytes.Length == 1)
                return new byte[] { 0x0, bytes[0] };
            return new[] { bytes[0], bytes[1] };
        }

        /// <summary>
        /// Gets the UUID version.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>GuidVersion.</returns>
        public static GuidVersion GetUuidVersion(this Guid guid)
        {
            var bytes = guid.ToByteArray();
            return (GuidVersion)((bytes[VersionByte] & 0xFF) >> VersionByteShift);
        }

        /// <summary>
        /// Gets the date time offset.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTimeOffset.</returns>
        public static DateTimeOffset GetDateTimeOffset(Guid guid)
        {
            var bytes = guid.ToByteArray();
            // reverse the version
            bytes[VersionByte] &= VersionByteMask;
            bytes[VersionByte] |= (byte)GuidVersion.TimeBased >> VersionByteShift;
            var timestampBytes = new byte[8];
            Array.Copy(bytes, TimestampByte, timestampBytes, 0, 8);
            var timestamp = BitConverter.ToInt64(timestampBytes, 0);
            var ticks = timestamp + GregorianCalendarStart.Ticks;
            return new DateTimeOffset(ticks, TimeSpan.Zero);
        }

        /// <summary>
        /// Gets the date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).DateTime;
        }

        /// <summary>
        /// Gets the local date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetLocalDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).LocalDateTime;
        }

        /// <summary>
        /// Gets the UTC date time.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns>DateTime.</returns>
        public static DateTime GetUtcDateTime(Guid guid)
        {
            return GetDateTimeOffset(guid).UtcDateTime;
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid()
        {
            switch (GuidGeneration)
            {
                case GuidGeneration.Fast:
                    return GenerateTimeBasedGuid(TimestampHelper.UtcNow(), ClockSequenceBytes, NodeBytes);
                default:
                    lock (Lock)
                    {
                        var ts = TimestampHelper.UtcNow();

                        if (ts <= _lastTimestampForNoDuplicatesGeneration)
                            ClockSequenceBytes = GenerateClockSequenceBytes();

                        _lastTimestampForNoDuplicatesGeneration = ts;

                        return GenerateTimeBasedGuid(ts, ClockSequenceBytes, NodeBytes);
                    }
            }
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), NodeBytes);
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), NodeBytes);
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="mac">The mac.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime, PhysicalAddress mac)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), GenerateNodeBytes(mac));
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="mac">The mac.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, PhysicalAddress mac)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), GenerateNodeBytes(mac));
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="ip">The ip.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime, IPAddress ip)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), GenerateNodeBytes(ip));
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="ip">The ip.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, IPAddress ip)
        {
            return GenerateTimeBasedGuid(dateTime, GenerateClockSequenceBytes(dateTime), GenerateNodeBytes(ip));
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="clockSequence">The clock sequence.</param>
        /// <param name="node">The node.</param>
        /// <returns>Guid.</returns>
        public static Guid GenerateTimeBasedGuid(DateTime dateTime, byte[] clockSequence, byte[] node)
        {
            return GenerateTimeBasedGuid(new DateTimeOffset(dateTime), clockSequence, node);
        }

        /// <summary>
        /// Generates a time based unique identifier.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="clockSequence">The clock sequence.</param>
        /// <param name="node">The node.</param>
        /// <returns>Guid.</returns>
        /// <exception cref="ArgumentNullException">
        /// clockSequence
        /// or
        /// node
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// clockSequence;The clockSequence must be 2 bytes.
        /// or
        /// node;The node must be 6 bytes.
        /// </exception>
        public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, byte[] clockSequence, byte[] node)
        {
            if (clockSequence == null)
                throw new ArgumentNullException(nameof(clockSequence));

            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (clockSequence.Length != 2)
                throw new ArgumentOutOfRangeException(nameof(clockSequence), @"The clockSequence must be 2 bytes.");

            if (node.Length != 6)
                throw new ArgumentOutOfRangeException(nameof(node), @"The node must be 6 bytes.");

            var ticks = (dateTime - GregorianCalendarStart).Ticks;
            var guid = new byte[ByteArraySize];
            var timestamp = BitConverter.GetBytes(ticks);

            // copy node
            Array.Copy(node, 0, guid, NodeByte, Math.Min(6, node.Length));

            // copy clock sequence
            Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

            // copy timestamp
            Array.Copy(timestamp, 0, guid, TimestampByte, Math.Min(8, timestamp.Length));

            // set the variant
            guid[VariantByte] &= VariantByteMask;
            guid[VariantByte] |= VariantByteShift;

            // set the version
            guid[VersionByte] &= VersionByteMask;
            guid[VersionByte] |= (byte)GuidVersion.TimeBased << VersionByteShift;

            return new Guid(guid);
        }
    }

    /// <summary>
    /// Class TimestampHelper.
    /// </summary>
    public static class TimestampHelper
    {
        /// <summary>
        /// The unix start
        /// </summary>
        public static readonly DateTimeOffset UnixStart;

        /// <summary>
        /// The maximum unix seconds
        /// </summary>
        public static readonly long MaxUnixSeconds;

        /// <summary>
        /// The maximum unix milliseconds
        /// </summary>
        public static readonly long MaxUnixMilliseconds;

        /// <summary>
        /// The maximum unix microseconds
        /// </summary>
        public static readonly long MaxUnixMicroseconds;

        /// <summary>
        /// The ticks in one microsecond
        /// </summary>
        public const long TicksInOneMicrosecond = 10L;

        /// <summary>
        /// The ticks in one millisecond
        /// </summary>
        public const long TicksInOneMillisecond = 10000L;

        /// <summary>
        /// The ticks in one second
        /// </summary>
        public const long TicksInOneSecond = 10000000L;

        /// <summary>
        /// Initializes static members of the <see cref="TimestampHelper"/> class.
        /// </summary>
        static TimestampHelper()
        {
            UnixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
            MaxUnixSeconds = Convert.ToInt64((DateTimeOffset.MaxValue - UnixStart).TotalSeconds);
            MaxUnixMilliseconds = Convert.ToInt64((DateTimeOffset.MaxValue - UnixStart).TotalMilliseconds);
            MaxUnixMicroseconds = Convert.ToInt64((DateTimeOffset.MaxValue - UnixStart).Ticks / TicksInOneMicrosecond);
        }

        /// Non-constant static fields should not be visible
        /// <summary>
        /// Allows for the use of alternative timestamp providers.
        /// </summary>
        [SuppressMessage("SonarLint", "S2223", Justification = "Needs to be visible")]
        public static Func<DateTimeOffset> UtcNow = () => DateTimePrecise.UtcNowOffset;

        /// <summary>
        /// Converts the <see cref="DateTimeOffset"/> to a cassandra timestamp.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns>System.Int64.</returns>
        public static long ToCassandraTimestamp(this DateTimeOffset dt)
        {
            // we are using the microsecond format from 1/1/1970 00:00:00 UTC same as the Cassandra server
            return (dt - UnixStart).Ticks / TicksInOneMicrosecond;
        }

        /// <summary>
        /// Converts from a cassandra timestamp to a <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <param name="ts">The ts.</param>
        /// <returns>DateTimeOffset.</returns>
        public static DateTimeOffset FromCassandraTimestamp(long ts)
        {
            // convert a timestamp in seconds to ticks
            // ** this should never happen, but it is in here for good measure **
            if (ts <= MaxUnixSeconds)
                ts *= TicksInOneSecond;

            // convert a timestamp in milliseconds to ticks
            if (ts <= MaxUnixMilliseconds)
                ts *= TicksInOneMillisecond;

            // convert a timestamp in microseconds to ticks
            if (ts <= MaxUnixMicroseconds)
                ts *= TicksInOneMicrosecond;

            return UnixStart.AddTicks(ts);
        }
    }

    /// <summary>
    /// Enum GuidVersion
    /// </summary>
    public enum GuidVersion
    {
        TimeBased = 0x01,
        Reserved = 0x02,
        NameBased = 0x03,
        Random = 0x04
    }

    /// <summary>
    /// Enum GuidGeneration
    /// </summary>
    public enum GuidGeneration
    {
        Fast,
        NoDuplicates
    }

    /// <summary>
    /// Class DateTimePrecise.
    /// </summary>
    public class DateTimePrecise
    {
        private static readonly DateTimePrecise Instance = new DateTimePrecise();
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <value>The current date and time.</value>
        public static DateTime Now => Instance.GetUtcNow().LocalDateTime;

        /// <summary>
        /// Gets the current UTC time.
        /// </summary>
        /// <value>The current UTC time.</value>
        public static DateTime UtcNow => Instance.GetUtcNow().UtcDateTime;

        /// <summary>
        /// Gets the current <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <value>The current <see cref="DateTimeOffset"/>.</value>
        public static DateTimeOffset NowOffset => Instance.GetUtcNow().ToLocalTime();

        /// <summary>
        /// Gets the UTC <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <value>The current UTC <see cref="DateTimeOffset"/>.</value>
        public static DateTimeOffset UtcNowOffset => Instance.GetUtcNow();

        private readonly double _divergentSeconds;
        private readonly double _syncSeconds;
        private readonly Stopwatch _stopwatch;
        private DateTimeOffset _baseTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePrecise"/> class.
        /// </summary>
        public DateTimePrecise() : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePrecise"/> class.
        /// </summary>
        /// <param name="divergentSeconds">The divergent seconds.</param>
        public DateTimePrecise(int divergentSeconds) : this(0, divergentSeconds)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimePrecise"/> class.
        /// </summary>
        /// <param name="syncSeconds">The synchronize seconds.</param>
        /// <param name="divergentSeconds">The divergent seconds.</param>
        public DateTimePrecise(int syncSeconds, int divergentSeconds)
        {
            _syncSeconds = syncSeconds;
            _divergentSeconds = divergentSeconds;

            _stopwatch = new Stopwatch();

            Syncronize();
        }

        private void Syncronize()
        {
            lock (_stopwatch)
            {
                _baseTime = DateTimeOffset.UtcNow;
                _stopwatch.Restart();
            }
        }

        /// <summary>
        /// Gets the UTC <see cref="DateTimeOffset"/>.
        /// </summary>
        /// <returns>DateTimeOffset.</returns>
        public DateTimeOffset GetUtcNow()
        {
            var now = DateTimeOffset.UtcNow;
            var elapsed = _stopwatch.Elapsed;

            if (elapsed.TotalSeconds > _syncSeconds)
            {
                Syncronize();

                // account for any time that has passed since the stopwatch was syncronized
                elapsed = _stopwatch.Elapsed;
            }

            /**
			 * The Stopwatch has many bugs associated with it, so when we are in doubt of the results
			 * we are going to default to DateTimeOffset.UtcNow
			 * http://stackoverflow.com/questions/1008345
			 **/

            // check for elapsed being less than zero
            if (elapsed < TimeSpan.Zero)
                return now;

            var preciseNow = _baseTime + elapsed;

            // make sure the two clocks don't diverge by more than defined seconds
            if (Math.Abs((preciseNow - now).TotalSeconds) > _divergentSeconds)
                return now;

            return _baseTime + elapsed;
        }
    }
}