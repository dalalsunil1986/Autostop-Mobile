﻿using Google.Maps.Common;
using Newtonsoft.Json;

namespace Google.Maps.Direction
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DirectionResponse : IServiceResponse
    {
        [JsonProperty("routes")]
        public DirectionRoute[] Routes { get; set; }

        [JsonProperty("geocoded_waypoints")]
        public GeocodedWaypoint[] Waypoints { get; set; }

        [JsonProperty("status")]
        public ServiceResponseStatus Status { get; set; }

        /// <summary>
        ///     More detailed information about the reasons behind the given status code, if other than OK.
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}