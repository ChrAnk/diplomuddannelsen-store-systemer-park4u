namespace Park4U
{
    public class ParkingRegistration
    {
        public string? ParkingSpaceId { get; set; }

        public DateTime ParkingStart { get; set; }

        public string? ParkingTimeMinutes { get; set; }

        public string? RegistrationNumber { get; set; }

        public string? EMailAddress { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class ParkingVerification
    {
        public string? ParkingSpaceId { get; set; }

        public string? RegistrationNumber { get; set; }

        public bool ParkingValid { get; set; }

        public int RemainingMinutes { get; set; }
    }

    public class ParkingDeletionConfirmation
    {
        public string? RegistrationNumber { get; set; }

        public int RecordsDeleted { get; set; }
    }
}