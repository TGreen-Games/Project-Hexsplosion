//
// Fingers Gestures
// (c) 2015 Digital Ruby, LLC
// Source code may be used for personal or commercial projects.
// Source code may NOT be redistributed or sold.
// 

using System;

namespace DigitalRubyShared
{
    public static class DeviceInfo
    {
        /// <summary>
        /// Convert centimeters to inches
        /// </summary>
        /// <returns>Centimeters converted to inches</returns>
        /// <param name="centimeters">Centimeters</param>
        public static float CentimetersToInches(float centimeters)
        {
            return centimeters * 0.393701f;
        }

        /// <summary>
        /// Pixels per inch
        /// </summary>
        /// <value>Pixels per inch</value>
        public static int PixelsPerInch { get; set; }

        /// <summary>
        /// Gets or sets the unit multiplier. For example, if you are specifying units in inches,
        /// you would want to set this to PixelsPerInch.
        /// </summary>
        /// <value>The unit multiplier.</value>
        public static int UnitMultiplier { get; set; }
    }
}

