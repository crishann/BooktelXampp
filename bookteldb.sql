-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 07, 2025 at 07:21 AM
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
  `BookingId` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `RoomId` int(11) NOT NULL,
  `CheckInDate` datetime NOT NULL,
  `CheckOutDate` datetime NOT NULL,
  `Guest` int(11) NOT NULL,
  `RoomType` varchar(100) NOT NULL,
  `TotalAmount` decimal(10,2) NOT NULL,
  `Status` varchar(50) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `rooms`
--

CREATE TABLE `rooms` (
  `Id` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Price` decimal(10,2) NOT NULL,
  `ImageUrl` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `rooms`
--

INSERT INTO `rooms` (`Id`, `Name`, `Price`, `ImageUrl`) VALUES
(1, 'Standard Room', 2350.00, '/img/room3.svg'),
(2, 'Double Room', 3.00, '/img/room4.svg'),
(3, 'Double Deluxe Room', 6927.00, '/img/room1.svg'),
(4, 'Presidential Suite', 50000.00, '/img/room2.svg');

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
(6, 'Admin', 'Admin', '09123456789', 'admin@gmail.com', '123', 'Admin', 1),
(15, 'scyhto', 'zucc', '09004124124', 'scyhtozucc@gmail.com', '$2a$11$jqPK7o4JUF3fWwTUB.VIB.ELHVk25reVOpgqU3gOOZbO6a6iu4hMa', 'Guest', 1),
(16, 'James', 'Reyes', '09004124124', 'jamesreyes2x@gmail.com', '$2a$11$wg5uWK5oD9iBM90Dtix/xeHyZgZrY5UpxJMx1y0d5.cxXwy4hQ.Su', 'Guest', 1),
(18, 'Admin2', 'Admin2', '09123456789', 'admin2@gmail.com', '$2a$11$ZuJeAdLi0f80xDbzwOXVHehA8gi83.Ts.O3a0y6mSeII9QFCZa2ky', 'Admin', 1),
(19, 'Housekeeping', 'Housekeeping', '09123456789', 'hk@gmail.com', '$2a$11$uQZDvAVrZcTLKpgzor6t5uAxu8fWkm4Ix745qyToz6T46HgegCw3y', 'housekeeping', 1);

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
  ADD PRIMARY KEY (`BookingId`),
  ADD KEY `FK_Bookings_Users` (`UserId`),
  ADD KEY `FK_Bookings_Rooms` (`RoomId`);

--
-- Indexes for table `rooms`
--
ALTER TABLE `rooms`
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
  MODIFY `BookingId` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `rooms`
--
ALTER TABLE `rooms`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `bookings`
--
ALTER TABLE `bookings`
  ADD CONSTRAINT `FK_Bookings_Rooms` FOREIGN KEY (`RoomId`) REFERENCES `rooms` (`Id`),
  ADD CONSTRAINT `FK_Bookings_Users` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
