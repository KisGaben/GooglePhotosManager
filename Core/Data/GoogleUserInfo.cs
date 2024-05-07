namespace Core.Data
{
    public class GoogleUserInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }

        public long TotalStorage { get; set; }
        public long UsedStorage { get; set; }
    }
}
