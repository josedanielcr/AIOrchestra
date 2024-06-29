using System.ComponentModel;

namespace CommonLibrary
{
    public enum Topics
    {
        [Description("user")]
        UserManagement,

        [Description("Unkown")]
        Unknown,

        [Description("cache")]
        Cache,

        [Description("gateway")]
        Gateway,

        [Description("ApiGatewayResponse")]
        ApiGatewayResponse,

        [Description("MusicRecommender")]
        MusicRecommender,

        [Description("PlaylistService")]
        PlaylistService
    }
}
