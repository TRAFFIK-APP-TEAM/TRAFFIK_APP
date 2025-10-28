using System;

namespace TRAFFIK_APP.Configuration
{
    public static class Endpoints
    {
//#if ANDROID
//                public const string BaseUrl = "http://10.0.2.2:5027";
//#elif IOS
//                public const string BaseUrl = "http://localhost:5027";
//#else
//        public const string BaseUrl = "http://localhost:5027";
//        #endif
        //public const string BaseUrl = "http://localhost:5027";

        public const string BaseUrl = "https://traffikapi-a0bhabb4bag8g3g6.southafricanorth-01.azurewebsites.net";
      
        public static class Auth
        {
            public const string Login = $"{BaseUrl}/api/Auth/Login"; // Authenticate user and return profile info
            public const string Register = $"{BaseUrl}/api/Auth/Register"; // Register a new user account
            public const string Logout = $"{BaseUrl}/api/Auth/Logout"; // Log out the current user
            public const string DeleteAccount = $"{BaseUrl}/api/Auth/Delete/{{id}}"; // Delete user account by ID
        }

        public static class Booking
        {
            public const string GetAll = $"{BaseUrl}/api/Bookings"; // Get all bookings in the system
            public const string Create = $"{BaseUrl}/api/Bookings"; // Create a new booking
            public const string GetById = $"{BaseUrl}/api/Bookings/{{id}}"; // Get booking details by ID
            public const string UpdateById = $"{BaseUrl}/api/Bookings/{{id}}"; // Update booking by ID
            public const string DeleteById = $"{BaseUrl}/api/Bookings/{{id}}"; // Delete booking by ID
            public const string AvailableSlots = $"{BaseUrl}/api/Bookings/AvailableSlots"; // Get available time slots for a service
            public const string Confirm = $"{BaseUrl}/api/Bookings/Confirm"; // Confirm booking after checking availability
            public const string GetByUser = $"{BaseUrl}/api/Bookings/User/{{userId}}"; // Get bookings for a specific user
            public const string GetStaffBookings = $"{BaseUrl}/api/Bookings/Staff"; // Get all bookings for staff with details
        }

        public static class BookingStages
        {
            public const string GetAll = $"{BaseUrl}/api/BookingStages"; // Get all booking stages
            public const string Create = $"{BaseUrl}/api/BookingStages"; // Create a new booking stage
            public const string GetById = $"{BaseUrl}/api/BookingStages/{{id}}"; // Get booking stage by ID
            public const string GetByBooking = $"{BaseUrl}/api/BookingStages/Booking/{{bookingId}}"; // Get stages for a booking
            public const string UpdateStage = $"{BaseUrl}/api/BookingStages/UpdateStage"; // Update booking stage
        }

        public static class InstagramPosts
        {
            public const string GetAll = $"{BaseUrl}/api/InstagramPost"; // Get all Instagram posts
            public const string GetById = $"{BaseUrl}/api/InstagramPost/{{id}}"; // Get specific post
            public const string Create = $"{BaseUrl}/api/InstagramPost"; // Create post
        }

        public static class Notification
        {
            public const string GetAll = $"{BaseUrl}/api/Notifications"; // Get all notifications
            public const string Create = $"{BaseUrl}/api/Notifications"; // Create a new notification
            public const string GetById = $"{BaseUrl}/api/Notifications/{{id}}"; // Get notification by ID
            public const string UpdateById = $"{BaseUrl}/api/Notifications/{{id}}"; // Update notification by ID
            public const string DeleteById = $"{BaseUrl}/api/Notifications/{{id}}"; // Delete notification by ID
            public const string GetByUser = $"{BaseUrl}/api/Notifications/User/{{userId}}"; // Get user's notifications
        }

        public static class Payment
        {
            public const string GetAll = $"{BaseUrl}/api/Payments"; // Get all payment records
            public const string Create = $"{BaseUrl}/api/Payments"; // Create a new payment
            public const string GetById = $"{BaseUrl}/api/Payments/{{id}}"; // Get payment by ID
            public const string UpdateById = $"{BaseUrl}/api/Payments/{{id}}"; // Update payment by ID
            public const string GetByBooking = $"{BaseUrl}/api/Payments/Booking/{{bookingId}}"; // Get payments for booking
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
            public const string GetByUser = $"{BaseUrl}/api/Reward/User/{{userId}}"; // Get user's rewards
            public const string GetBalance = $"{BaseUrl}/api/Reward/User/{{userId}}/balance"; // Get user's reward balance
        }

        public static class RewardCatalog
        {
            public const string GetAll = $"{BaseUrl}/api/RewardCatalog"; // Get all catalog items
            public const string RedeemItem = $"{BaseUrl}/api/RewardCatalog/redeem/{{itemId}}"; // Redeem a specific item
            public const string GetRedeemed = $"{BaseUrl}/api/RewardCatalog/user/{{userId}}/redeemed";
        }

        public static class ServiceCatalog
        {
            public const string GetAll = $"{BaseUrl}/api/ServiceCatalogs";
            public const string Create = $"{BaseUrl}/api/ServiceCatalogs";
            public const string GetById = $"{BaseUrl}/api/ServiceCatalogs/{{id}}";
            public const string UpdateById = $"{BaseUrl}/api/ServiceCatalogs/{{id}}";
            public const string DeleteById = $"{BaseUrl}/api/ServiceCatalogs/{{id}}";
            public const string GetForVehicle = $"{BaseUrl}/api/ServiceCatalogs/ForVehicle/{{licensePlate}}";
            public const string GetByVehicleType = $"{BaseUrl}/api/ServiceCatalogs/ByVehicleType/{{vehicleTypeId}}";
        }

        public static class ServiceHistory
        {
            public const string TrackWash = $"{BaseUrl}/api/ServiceHistory/TrackWash"; // Log a completed wash service
            public const string GetByVehicle = $"{BaseUrl}/api/ServiceHistory/Vehicle/{{licensePlate}}"; // Get service history for a vehicle
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
            public const string GetAll = $"{BaseUrl}/api/Users"; // Get all users in the system
            public const string Create = $"{BaseUrl}/api/Users"; // Create a new user
            public const string GetById = $"{BaseUrl}/api/Users/{{id}}"; // Get user details by ID
            public const string UpdateById = $"{BaseUrl}/api/Users/{{id}}"; // Update user info by ID
            public const string DeleteById = $"{BaseUrl}/api/Users/{{id}}"; // Delete user by ID
        }

        public static class Vehicle
        {
            public const string GetAll = $"{BaseUrl}/api/vehicle"; // Get all vehicles
            public const string GetByLicensePlate = $"{BaseUrl}/api/vehicle/{{licensePlate}}"; // Get specific vehicle
            public const string Create = $"{BaseUrl}/api/vehicle"; // Create a new vehicle
            public const string Update = $"{BaseUrl}/api/vehicle/{{licensePlate}}"; // Update vehicle
            public const string Delete = $"{BaseUrl}/api/vehicle/{{licensePlate}}"; // Delete vehicle
            public const string GetByUser = $"{BaseUrl}/api/vehicle/User/{{userId}}"; // Get user's vehicles
        }

        public static class VehicleType
        {
            public const string GetAll = $"{BaseUrl}/api/VehicleTypes"; // Get all vehicle types
            public const string GetById = $"{BaseUrl}/api/VehicleTypes/{{id}}"; // Get specific vehicle type
            public const string Create = $"{BaseUrl}/api/VehicleTypes"; // Create vehicle type
            public const string Update = $"{BaseUrl}/api/VehicleTypes/{{id}}"; // Update vehicle type
            public const string Delete = $"{BaseUrl}/api/VehicleTypes/{{id}}"; // Delete vehicle type
        }

        public static class RewardItem
        {
            public const string GetAll = $"{BaseUrl}/api/RewardItems"; // Get all reward items
            public const string GetById = $"{BaseUrl}/api/RewardItems/{{id}}"; // Get specific reward item
            public const string Create = $"{BaseUrl}/api/RewardItems"; // Create reward item
            public const string Update = $"{BaseUrl}/api/RewardItems/{{id}}"; // Update reward item
            public const string Delete = $"{BaseUrl}/api/RewardItems/{{id}}"; // Delete reward item
        }

    }
}
