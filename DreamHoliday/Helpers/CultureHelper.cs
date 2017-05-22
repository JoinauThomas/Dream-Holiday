using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamHoliday.Helpers
{
    public class CultureHelper
    {
        private static readonly List<string> _cultures = new List<string> {
            "fr", // first culture is the DEFAULT french NEUTRAL culture
            "en" // english NEUTRAL culture
        };
        /// <summary>
        /// Returns a valid NEUTRAL culture, if is not valid, returns default NEUTRAL culture "fr"
        /// </summary>
        /// <param name="name" />Culture's name (e.g. fr-BE)</param>
        public static string GetImplementedCulture(string name)
        {
            // make sure it's not null
            if (string.IsNullOrEmpty(name))
                return GetDefaultCulture();
            // get NEUTRAL culture
            var neutralName = GetNeutralCulture(name);
            // make sure it is a implemented culture
            if (!_cultures.Exists(c =>
            c.Equals(neutralName, StringComparison.InvariantCultureIgnoreCase)))
                return GetDefaultCulture(); // return Default culture if it is not implemented
            return neutralName;
        }
        /// <summary>
        /// Returns default culture
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return _cultures[0];
        }
        /// <summary>
        /// Return a NEUTRAL culture (e.g. fr from fr-BE)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetNeutralCulture(string name)
        {
            if (!name.Contains("-")) return name;
            // Read first part only. E.g. "en", "es"
            return name.Split('-')[0];
        }
    }
}