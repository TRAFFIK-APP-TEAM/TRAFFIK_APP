using System;

namespace TRAFFIK_APP.Configuration
{
    public static class Endpoints
    {
        // Toggle between local and Azure API
        /*#if ANDROID
                public const string BaseUrl = "http://10.0.2.2:5027"; // Android emulator localhost
        #elif IOS
                public const string BaseUrl = "http://localhost:5027"; // iOS simulator localhost
        #else
                public const string BaseUrl = "http://localhost:5027"; // Windows/other platforms
        #endif*/
        public const string BaseUrl = "https://traffikapi-a0bhabb4bag8g3g6.southafricanorth-01.azurewebsites.net"; // Azure API endpoint

        public static class Auth
        {
            public const string Login = "/api/Auth/Login"; // Authenticate user and return profile info
            public const string Register = "/api/Auth/Register"; // Register a new user account
            public const string Logout = "/api/Auth/Logout"; // Log out the current user
            public const string DeleteAccount = "/api/Auth/Delete/{id}"; // Delete user account by ID
        }

        public static class Booking
        {
            public const string GetAll = "/api/Bookings"; // Get all bookings in the system
            public const string Create = "/api/Bookings"; // Create a new booking
            public const string GetById = "/api/Bookings/{id}"; // Get booking details by ID
            public const string UpdateById = "/api/Bookings/{id}"; // Update booking by ID
            public const string DeleteById = "/api/Bookings/{id}"; // Delete booking by ID
            public const string AvailableSlots = "/api/Bookings/AvailableSlots"; // Get available time slots for a service
            public const string Confirm = "/api/Bookings/Confirm"; // Confirm booking after checking availability
            public const string GetByUser = "/api/Bookings/User/{userId}"; // Get bookings for a specific user
        }

        public static class BookingStages
        {
            public const string GetAll = $"{BaseUrl}/api/BookingStages"; // Get all booking stages
            public const string Create = $"{BaseUrl}/api/BookingStages"; // Create a new booking stage
            public const string GetById = $"{BaseUrl}/api/BookingStages/{{id}}"; // Get booking stage by ID
            public const string UpdateById = $"{BaseUrl}/api/BookingStages/{{id}}"; // Update booking stage by ID
            public const string DeleteById = $"{BaseUrl}/api/BookingStages/{{id}}"; // Delete booking stage by ID
            public const string UpdateStage = $"{BaseUrl}/api/BookingStages/UpdateStage"; // Update stage status using DTO
        }

        public static class SocialFeed
        {
            public const string GetInstagramPosts = $"{BaseUrl}/api/InstagramPost"; // Get Instagram feed posts
        }

        public static class Notification
        {
            public const string GetAll = $"{BaseUrl}/api/Notifications"; // Get all notifications
            public const string Create = $"{BaseUrl}/api/Notifications"; // Create a new notification
            public const string GetById = $"{BaseUrl}/api/Notifications/{{id}}"; // Get notification by ID
            public const string UpdateById = $"{BaseUrl}/api/Notifications/{{id}}"; // Update notification by ID
            public const string DeleteById = $"{BaseUrl}/api/Notifications/{{id}}"; // Delete notification by ID
        }

        public static class Payment
        {
            public const string GetAll = $"{BaseUrl}/api/Payments"; // Get all payment records
            public const string Create = $"{BaseUrl}/api/Payments"; // Create a new payment
            public const string GetById = $"{BaseUrl}/api/Payments/{{id}}"; // Get payment by ID
            public const string UpdateById = $"{BaseUrl}/api/Payments/{{id}}"; // Update payment by ID
            public const string DeleteById = $"{BaseUrl}/api/Payments/{{id}}"; // Delete payment by ID
        }

        public static class Review
        {
            public const string GetAll = $"{BaseUrl}/api/Review"; // Get all reviews
            public const string Create = $"{BaseUrl}/api/Review"; // Submit a new review
            public const string GetById = $"{BaseUrl}/api/Review/{{id}}"; // Get review by ID
            public const string UpdateById = $"{BaseUrl}/api/Review/{{id}}"; // Update review by ID
            public const string DeleteById = $"{BaseUrl}/api/Review/{{id}}"; // Delete review by ID
        }

        public static class Reward
        {
            public const string GetAll = $"{BaseUrl}/api/Reward"; // Get all rewards
            public const string Create = $"{BaseUrl}/api/Reward"; // Create a new reward entry
            public const string GetById = $"{BaseUrl}/api/Reward/{{id}}"; // Get reward by ID
            public const string UpdateById = $"{BaseUrl}/api/Reward/{{id}}"; // Update reward by ID
            public const string DeleteById = $"{BaseUrl}/api/Reward/{{id}}"; // Delete reward by ID
            public const string GetBalance = $"{BaseUrl}/api/Reward/User/{{userId}}/balance"; // Get user's reward balance
            public const string Earn = $"{BaseUrl}/api/Reward/earn"; // Earn reward points
            public const string Redeem = $"{BaseUrl}/api/Reward/redeem"; // Redeem reward points
        }

        public static class RewardCatalog
        {
            public const string GetAll = $"{BaseUrl}/api/RewardCatalog"; // Get all catalog items
            public const string RedeemItem = $"{BaseUrl}/api/RewardCatalog/redeem/{{itemId}}"; // Redeem a specific item
            public const string GetRedeemed = $"{BaseUrl}/api/RewardCatalog/user/{{userId}}/redeemed";
            public const string MarkAsUsed = $"{BaseUrl}/api/RewardCatalog/user/{{userId}}/redeemed/{{itemId}}/use";
        }

        public static class ServiceCatalog
        {
            public const string GetAll = $"{BaseUrl}/api/ServiceCatalog"; // Get all services in catalog
            public const string Create = $"{BaseUrl}/api/ServiceCatalog"; // Add a new service
            public const string GetById = $"{BaseUrl}/api/ServiceCatalog/{{id}}"; // Get service by ID
            public const string UpdateById = $"{BaseUrl}/api/ServiceCatalog/{{id}}"; // Update service by ID
            public const string DeleteById = $"{BaseUrl}/api/ServiceCatalog/{{id}}"; // Delete service by ID
            public const string AvailableForVehicle = $"{BaseUrl}/api/ServiceCatalog/AvailableForVehicle/{{carModelId}}"; // Get services available for a vehicle
            public const string ByCarType = $"{BaseUrl}/api/ServiceCatalog/ByCarType/{{carTypeId}}"; // Get services for a car type
        }

        public static class ServiceHistory
        {
            public const string TrackWash = $"{BaseUrl}/api/ServiceHistory/TrackWash"; // Log a completed wash service
            public const string GetByVehicle = $"{BaseUrl}/api/ServiceHistory/Vehicle/{{vehicleId}}"; // Get service history for a vehicle
            public const string GetAll = $"{BaseUrl}/api/ServiceHistory/All"; // Get all service history records
        }

        public static class UserRole
        {
            public const string GetAll = $"{BaseUrl}/api/UserRole"; // Get all user roles
            public const string Create = $"{BaseUrl}/api/UserRole"; // Create a new user role
            public const string GetById = $"{BaseUrl}/api/UserRole/{{id}}"; // Get user role by ID
            public const string UpdateById = $"{BaseUrl}/api/UserRole/{{id}}"; // Update user role by ID
            public const string DeleteById = $"{BaseUrl}/api/UserRole/{{id}}"; // Delete user role by ID
        }

        public static class User
        {
            public const string GetAll = $"{BaseUrl}/api/User"; // Get all users in the system
            public const string Create = $"{BaseUrl}/api/User"; // Create a new user
            public const string GetById = $"{BaseUrl}/api/User/{{id}}"; // Get user details by ID
            public const string UpdateById = $"{BaseUrl}/api/User/{{id}}"; // Update user info by ID
            public const string DeleteById = $"{BaseUrl}/api/User/{{id}}"; // Delete user by ID
        }

        public static class Vehicle
        {
            public const string GetByUser = "/api/vehicle/user/{userId}"; // Get vehicles linked to a user
            public const string GetById = "/api/vehicle/{id}"; // Get vehicle by ID
            public const string Create = "/api/vehicle"; // Create a new vehicle
            public const string UpdateById = "/api/vehicle/{id}"; // Update vehicle by ID
            public const string DeleteById = "/api/vehicle/{id}"; // Delete vehicle by ID
            public const string GetAllVehicleTypes = "/api/vehicle/types"; // Get all vehicle types
        }
    }
}
