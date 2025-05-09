-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 09, 2025 at 11:04 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bookteldb`
--

-- --------------------------------------------------------

--
-- Table structure for table `bookings`
--

CREATE TABLE `bookings` (
  `Id` int(11) NOT NULL,
  `userid` int(11) NOT NULL,
  `room_id` int(11) DEFAULT NULL,
  `CheckInDate` date DEFAULT NULL,
  `CheckOutDate` date DEFAULT NULL,
  `Guest` int(11) DEFAULT NULL,
  `RoomType` varchar(50) DEFAULT NULL,
  `FullName` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `PhoneNumber` varchar(50) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `SpecialRequests` text DEFAULT NULL,
  `PaymentMethod` varchar(50) DEFAULT NULL,
  `PaymentStatus` text NOT NULL DEFAULT 'Pending',
  `TotalAmount` decimal(10,2) DEFAULT NULL,
  `Status` varchar(50) DEFAULT 'Pending',
  `CreatedAt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `bookings`
--

INSERT INTO `bookings` (`Id`, `userid`, `room_id`, `CheckInDate`, `CheckOutDate`, `Guest`, `RoomType`, `FullName`, `Email`, `PhoneNumber`, `Address`, `SpecialRequests`, `PaymentMethod`, `PaymentStatus`, `TotalAmount`, `Status`, `CreatedAt`) VALUES
(1, 3, NULL, '2025-05-08', '2025-05-10', 1, 'Standard', 'cristian torrejos', 'cristiantorrejos@gmail.com', '0912312312312', 'blk 30 lot 8 ', 'asda', 'Credit Card', 'Pending', 4000.00, 'Pending', '2025-05-07 02:43:04'),
(8, 22, NULL, '2025-05-10', '2025-05-17', 1, 'Suite', 'leclerc', 'leclerc@gmail.com', '0912312312312', 'USA', 'adasd', 'Credit Card', 'Pending', 24500.00, 'Pending', '2025-05-07 05:54:55'),
(9, 21, NULL, '2025-05-07', '2025-05-10', 2, 'Deluxe', 'james racal', 'lbjames@gmail.com', '0912312312312', 'mabolo', 'asdasdasdsad', 'PayPal', 'Pending', 8400.00, 'Pending', '2025-05-07 05:55:39');

-- --------------------------------------------------------

--
-- Table structure for table `contactus`
--

CREATE TABLE `contactus` (
  `Contid` int(11) NOT NULL,
  `name` text NOT NULL,
  `email` text NOT NULL,
  `message` varchar(2555) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `contactus`
--

INSERT INTO `contactus` (`Contid`, `name`, `email`, `message`) VALUES
(1, 'Cristian H. Torrejos', 'cristiantorrejos03@gmail.com', 'ako to si natoy');

-- --------------------------------------------------------

--
-- Table structure for table `housekeeping_logs`
--

CREATE TABLE `housekeeping_logs` (
  `id` int(11) NOT NULL,
  `room_id` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL,
  `task_description` text DEFAULT NULL,
  `status` enum('pending','in_progress','completed') DEFAULT 'pending',
  `logged_at` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `invoices`
--

CREATE TABLE `invoices` (
  `id` int(11) NOT NULL,
  `booking_id` int(11) DEFAULT NULL,
  `invoice_number` varchar(50) DEFAULT NULL,
  `issued_at` datetime DEFAULT current_timestamp(),
  `total_amount` decimal(10,2) DEFAULT NULL,
  `notes` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `payments`
--

CREATE TABLE `payments` (
  `id` int(11) NOT NULL,
  `booking_id` int(11) DEFAULT NULL,
  `amount` decimal(10,2) DEFAULT NULL,
  `method` enum('cash','credit_card','gcash','paypal') DEFAULT 'cash',
  `status` enum('pending','paid','failed') DEFAULT 'pending',
  `paid_at` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE `rooms` (
  `Id` int(11) NOT NULL,
  `room_number` varchar(10) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `status` enum('available','occupied','maintenance') DEFAULT 'available',
  `ImageUrl` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`Id`, `room_number`, `Name`, `Price`, `status`, `ImageUrl`) VALUES
(1, '101', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(2, '102', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(3, '103', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(4, '104', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(5, '105', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(6, '106', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(7, '107', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(8, '108', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(9, '109', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(10, '110', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(11, '111', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(12, '112', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(13, '113', 'Standard Room', 2350.00, 'available', '/img/room3.svg'),
(14, '114', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(15, '115', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(16, '116', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(17, '117', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(18, '118', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(19, '119', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(20, '120', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(21, '121', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(22, '122', 'Double Room', 5000.00, 'available', '/img/room4.svg'),
(23, '132', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(24, '133', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(25, '134', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(26, '135', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(27, '136', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(28, '137', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(29, '138', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(30, '139', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(31, '140', 'Double Deluxe Room', 6927.00, 'available', '/img/room1.svg'),
(32, '141', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(33, '142', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(34, '143', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(35, '144', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(36, '145', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(37, '146', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(38, '147', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(39, '148', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg'),
(40, '149', 'Presidential Suite', 50000.00, 'available', '/img/room2.svg');

-- --------------------------------------------------------

--
-- Table structure for table `roomtasks`
--

CREATE TABLE `roomtasks` (
  `Id` int(11) NOT NULL,
  `RoomNumber` int(11) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `AssignedDate` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `roomtasks`
--

INSERT INTO `roomtasks` (`Id`, `RoomNumber`, `Status`, `AssignedDate`) VALUES
(1, 101, 'Cleaned', '2025-04-07'),
(2, 102, 'Cleaned', '2025-04-06');

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `FirstName` longtext NOT NULL,
  `LastName` longtext NOT NULL,
  `ContactNumber` longtext NOT NULL,
  `Email` longtext NOT NULL,
  `Password` longtext NOT NULL,
  `Role` longtext NOT NULL,
  `IsEmailConfirmed` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `FirstName`, `LastName`, `ContactNumber`, `Email`, `Password`, `Role`, `IsEmailConfirmed`) VALUES
(2, 'John', 'Doe', '09123456789', 'johndoe@example.com', 'password123', 'Guest', 0),
(3, 'cristian', 'torrejos', '09232368502', 'cristian@gmail.com', '$2a$11$D5bZefZFg/x2DQQDwbrHAudeoTWs08wO/n4j6/PJ.5NX91ztuFgPe', 'Guest', 1),
(4, 'Xing', 'Xiang', '09123456789', 'Xing@gmail.com', 'Xing@2003', 'Guest', 0),
(5, 'test1', 'er', '09123456789', 'test1@gmail.com', 'test1@gmail.com', 'Guest', 0),
(15, 'scyhto', 'zucc', '09004124124', 'scyhtozucc@gmail.com', '$2a$11$jqPK7o4JUF3fWwTUB.VIB.ELHVk25reVOpgqU3gOOZbO6a6iu4hMa', 'Guest', 1),
(16, 'James', 'Reyes', '09004124124', 'jamesreyes2x@gmail.com', '$2a$11$wg5uWK5oD9iBM90Dtix/xeHyZgZrY5UpxJMx1y0d5.cxXwy4hQ.Su', 'Guest', 1),
(18, 'Admin2', 'Admin2', '09123456789', 'admin2@gmail.com', '$2a$11$ZuJeAdLi0f80xDbzwOXVHehA8gi83.Ts.O3a0y6mSeII9QFCZa2ky', 'admin', 1),
(19, 'Housekeeping', 'housekeeping', '09123456789', 'hk@gmail.com', '$2a$11$uQZDvAVrZcTLKpgzor6t5uAxu8fWkm4Ix745qyToz6T46HgegCw3y', 'housekeeping', 1),
(20, 'Admin', 'Admin', '09123456789', 'admin@gmail.com', '$2a$11$MG0DDKVv75/kHVqTccbIU.FnPqSMiro6GvbExmB47L1ql7qdRWrPm', 'Admin', 1),
(21, 'James', 'Raffy', '09123456789', 'lbjames@gmail.com', '$2a$11$3TRvGjd5qZP3Enpf9m447.tiPqPz.86H0UWv03Bz7hIYaR.RpHnHy', 'Guest', 1),
(22, 'Charles', 'Leclerc', '09123456789', 'leclerc@gmail.com', '$2a$11$Ku8G1NmqGCjZt/wSBKc8lOk6TNDzCPLkMWGInN/PzzUdiiKmmUfP6', 'Guest', 1),
(23, 'Admin', 'Reyes', '09123456789', 'admin22@gmail.com', '$2a$11$xcdR.UGOP8/8ugtgsbh2Outz9.S71r5FvkpSB.7Qv9TlsYw9AmN8.', 'Guest', 0),
(24, 'James', 'Reyes', '09123456789', 'test111@gmail.com', '$2a$11$6fAD3MVWYa3BP433IYJejOH5HwiN4dLyrSMZOc.FEvcKqklzo6iPG', 'Guest', 1),
(25, 'fd', 'fd', '09123456789', 'fd@gmail.com', '123', 'frontdesk', 1);

-- --------------------------------------------------------

--
-- Table structure for table `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20250313193929_InitialCreate', '8.0.13'),
('20250313195307_AddUserTable', '8.0.13'),
('20250313200131_FixUserModel', '8.0.13'),
('20250314165646_UpdateUsersTable', '8.0.13');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `bookings`
--
ALTER TABLE `bookings`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `contactus`
--
ALTER TABLE `contactus`
  ADD PRIMARY KEY (`Contid`);

--
-- Indexes for table `housekeeping_logs`
--
ALTER TABLE `housekeeping_logs`
  ADD PRIMARY KEY (`id`),
  ADD KEY `room_id` (`room_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Indexes for table `invoices`
--
ALTER TABLE `invoices`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `invoice_number` (`invoice_number`),
  ADD KEY `booking_id` (`booking_id`);

--
-- Indexes for table `payments`
--
ALTER TABLE `payments`
  ADD PRIMARY KEY (`id`),
  ADD KEY `booking_id` (`booking_id`);

--
-- Indexes for table `rooms`
--
ALTER TABLE `rooms`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `room_number` (`room_number`);

--
-- Indexes for table `roomtasks`
--
ALTER TABLE `roomtasks`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `bookings`
--
ALTER TABLE `bookings`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `contactus`
--
ALTER TABLE `contactus`
  MODIFY `Contid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `housekeeping_logs`
--
ALTER TABLE `housekeeping_logs`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `invoices`
--
ALTER TABLE `invoices`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `payments`
--
ALTER TABLE `payments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=41;

--
-- AUTO_INCREMENT for table `roomtasks`
--
ALTER TABLE `roomtasks`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=26;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `fk_room_id` FOREIGN KEY (`room_id`) REFERENCES `rooms` (`Id`);

--
-- Constraints for table `housekeeping_logs`
--
ALTER TABLE `housekeeping_logs`
  ADD CONSTRAINT `housekeeping_logs_ibfk_1` FOREIGN KEY (`room_id`) REFERENCES `rooms` (`Id`),
  ADD CONSTRAINT `housekeeping_logs_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `users` (`Id`);

--
-- Constraints for table `invoices`
--
ALTER TABLE `invoices`
  ADD CONSTRAINT `invoices_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`Id`);

--
-- Constraints for table `payments`
--
ALTER TABLE `payments`
  ADD CONSTRAINT `payments_ibfk_1` FOREIGN KEY (`booking_id`) REFERENCES `bookings` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
