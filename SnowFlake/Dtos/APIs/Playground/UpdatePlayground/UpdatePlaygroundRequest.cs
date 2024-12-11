namespace SnowFlake.Dtos.APIs.Playground.UpdatePlayground
{
    public class UpdatePlaygroundRequest
    {
        public string Id { get; set; }
        public string HostRoomCode { get; set; }
        public string PlayerRoomCode { get; set; }
        public string HostId { get; set; }
        public List<RoundEntity> Rounds { get; set; }
        public int NumberOfTeam { get; set; }
        public int MaxTeamMember { get; set; }
        public int TeamToken { get; set; }
    }
}
